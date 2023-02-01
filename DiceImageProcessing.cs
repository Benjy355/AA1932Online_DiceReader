using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AA_DiceReader
{
    internal static class DiceImageProcessor
    {
        // Colour of attacker dice is A41F16, colour of defender dice is 242422
        public static Color attackerDiceColour = Color.FromArgb(164, 31, 22);
        public static Color defenderDiceColour = Color.FromArgb(36, 36, 34);

        public static Collection<Point> possibleDiceWhiteDotLocations = new Collection<Point>();

        public static Collection<Rectangle> knownDice = new Collection<Rectangle>();

        // 3 and 5 are missing because they are just combos of other ones, ezpz
        static DiceImageProcessor()
        {
            possibleDiceWhiteDotLocations.Add(new Point(27, 27)); // 1
            possibleDiceWhiteDotLocations.Add(new Point(10, 10)); // 2
            possibleDiceWhiteDotLocations.Add(new Point(43, 43)); // 2
            possibleDiceWhiteDotLocations.Add(new Point(43, 10)); // 4
            possibleDiceWhiteDotLocations.Add(new Point(10, 43)); // 4
            possibleDiceWhiteDotLocations.Add(new Point(10, 26)); // 6
            possibleDiceWhiteDotLocations.Add(new Point(42, 26)); // 6
        }

        // Colour of background is either 000000, or 030303
        // We know the background colour's max RGB values are 3, so if RGB < 4 we hit background
        public static Boolean isBackground(Color colour)
        {
            return (colour.R < 4 &&
                    colour.G < 4 &&
                    colour.B < 4);
        }

        // Returns DiceImageReference.NullReference when no dice image found
        public static DiceImageReference findDicePicture(Bitmap img, Point start)
        {
            Boolean foundDice = false;
            int diceX = 0, diceY = 0;
            int diceWidth = 0, diceHeight = 0;
            Color mrPixel;
            DiceTeam diceTeam = DiceTeam.NULL;

            // Loop through our Y as we search for the color value of our choice, once found stop both loops and start the 'find the height' loop
            for (int y = start.Y; y < img.Height &&
                !foundDice; y++)
            {
                for (int x = 0; x < img.Width; x++) // We used to do start.X here, but that was a bad idea.
                {
                    // We we have to make sure it doesn't intersect with a known dice
                    mrPixel = img.GetPixel(x, y);
                    Boolean skipThisDice = false;
                    if (mrPixel == DiceImageProcessor.attackerDiceColour ||
                        mrPixel == DiceImageProcessor.defenderDiceColour)
                    { // OH BABY WE FOUND A DICE, (hopefully)
                        Rectangle tempRec = new Rectangle(x, y, 1, 1);
                        foreach (Rectangle knownDie in knownDice)
                        {
                            if (knownDie.IntersectsWith(tempRec))
                            {
                                skipThisDice = true;
                                break;
                            }
                        }
                        if (!skipThisDice)
                        {
                            foundDice = true;
                            diceX = x;
                            diceY = y;

                            if (mrPixel == DiceImageProcessor.attackerDiceColour) diceTeam = DiceTeam.Attacker; 
                            else
                            if (mrPixel == DiceImageProcessor.defenderDiceColour) diceTeam = DiceTeam.Defender;

                            break;
                        }
                    }
                }
            }

            if (!foundDice)
                return DiceImageReference.NullReference();

            // The "find the height" loop
            for (int y = diceY; y < img.Height; y++)
            {
                mrPixel = img.GetPixel(diceX, y);
                
                if (DiceImageProcessor.isBackground(mrPixel))
                { // our dice has ended
                    diceHeight = y - diceY;
                    diceWidth = diceHeight;
                    break;
                }
            }

            // Find the proper X, move to the centre of the dice to avoid the anti-aliasing, slide left to the background
            for (int x = diceX; x >= 0; x--)
            {
                int middleY = diceY + (diceHeight/2);
                mrPixel = img.GetPixel(x, middleY);

                if (isBackground(mrPixel))
                {
                    diceX = x + 1;
                    break;
                }
            }

            // We now have our X, Y, width and height :) RIP OUR IMAGE OUT
            Bitmap diceImage = new Bitmap(diceWidth, diceHeight);
            for (int x = 0; x < diceWidth; x++)
            {
                for (int y = 0; y < diceHeight; y++)
                {
                    diceImage.SetPixel(x, y, img.GetPixel(x + diceX, y + diceY));
                }
            }
            knownDice.Add(new Rectangle(diceX, diceY, diceWidth, diceHeight));
            DiceImageReference diceRef = 
                new DiceImageReference(diceImage, 
                new Point(diceX, diceY), 
                DiceImageProcessor.getDiceValueFromImg(diceImage), 
                diceTeam);

            return diceRef;
        }

        // Go through picture, find all the dice for BOTH TEAMS, create collection containing all of them and return
        public static Collection<DiceImageReference> extractDiceRefsFromImage(Bitmap img, Point firstStart)
        {
            Collection<DiceImageReference> finalCollection = new Collection<DiceImageReference>();

            Point newStart = firstStart;
            DiceImageReference lastResult = DiceImageReference.NullReference();
            Boolean firstRun = true;

            // NEED TO CHECK FOR TEAM SO WE DON'T HAVE TO RUN THIS SHIT TWICE ON OUR FORM1
            // Loop through the picture until we stop finding dice
            while (firstRun == true ||
                !lastResult.isNull())
            {
                firstRun = false;
                // Adjust our starting position so we don't load the same dice picture over and over again
                newStart = lastResult.location;
                newStart.X += lastResult.size.Width;

                lastResult = DiceImageProcessor.findDicePicture(img, newStart);
                if (!lastResult.isNull())
                    finalCollection.Add(lastResult);
            }

            return finalCollection;
        }
        public static Collection<DiceImageReference> extractDiceRefsFromImage(Bitmap img)
        {
            return extractDiceRefsFromImage(img, new Point(0, 0));
        }

        // Dice images in game files are vector graphics, so they *could* be any size, but things will always be relative.
        // Base size we work off of is 54x54
        // Returns int based on number of white pixels found in locations provided by possibleDiceWhiteDotLocations
        public static int getDiceValueFromImg(Bitmap img)
        {
            int count = 0;
            Decimal adjustment = (Decimal)54 / (Decimal)(img.Width); // images should be square

            foreach (Point possibleLocation in DiceImageProcessor.possibleDiceWhiteDotLocations)
            {
                int checkX = (int)(Math.Round(possibleLocation.X * adjustment));
                int checkY = (int)(Math.Round(possibleLocation.Y * adjustment));
                Color pixel = img.GetPixel(checkX, checkY);
                if (pixel.R == 255 &&
                    pixel.G == 255 &&
                    pixel.B == 255)
                    count++;
            }
            return count;
        }
    }
}
