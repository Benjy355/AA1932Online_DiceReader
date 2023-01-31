using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AA_DiceReader
{
    internal class DiceImageProcessor
    {
        // Size of dice is 54x54 at my screen resolution, probably don't trust that number
        // BUT, if we find a single dice's size, we can use that for every other dice.

        // Colour of attacker dice is A41F16, colour of defender dice is 242422
        public static Color attackerDiceColour = Color.FromArgb(164, 31, 22);
        public static Color defenderDiceColour = Color.FromArgb(36, 36, 34);

        public static Collection<Rectangle> knownDice = new Collection<Rectangle>();

        // Colour of background is either 000000, or 030303
        // We know the background colour's max RGB values are 3, so if RGB < 4 we hit background
        public static Boolean isBackground(Color colour)
        {
            return (colour.R < 4 &&
                    colour.G < 4 &&
                    colour.B < 4);
        }

        // Returns DiceImageReference.NullReference when no dice image found
        protected static DiceImageReference findDicePicture(Bitmap img, Point start, DiceTeam diceTeam)
        {
            Color diceColor;
            if (diceTeam == DiceTeam.Attacker)
                diceColor = DiceImageProcessor.attackerDiceColour;
            else
                diceColor = DiceImageProcessor.defenderDiceColour;

            Boolean foundDice = false;
            int diceX = 0, diceY = 0;
            int diceWidth = 0, diceHeight = 0;
            Color mrPixel;

            // Loop through our Y as we search for the color value of our choice, once found stop both loops and start the 'find the height' loop
            for (int y = start.Y; y < img.Height &&
                !foundDice; y++)
            {
                for (int x = 0; x < img.Width; x++) // We used to do start.X here, but that was a bad idea.
                {
                    // We we have to make sure it doesn't intersect with a known dice
                    mrPixel = img.GetPixel(x, y);
                    Rectangle tempRec = new Rectangle(x, y, 1, 1);
                    Boolean skipThisDice = false;
                    foreach (Rectangle knownDie in knownDice)
                    {
                        if (knownDie.IntersectsWith(tempRec))
                        {
                            skipThisDice = true;
                            break;
                        }
                    }
                    if (!skipThisDice && 
                        mrPixel == diceColor)
                    { // OH BABY WE FOUND A DICE, (hopefully)
                        foundDice = true;
                        diceX = x;
                        diceY = y;
                        break;
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
            DiceImageReference diceRef = new DiceImageReference(diceImage, new Point(diceX, diceY), 0, diceTeam);

            return diceRef;
        }

        /*
         * THE PLAN:
         * GO THROUGH PIXEL BY PIXEL, FIND THE FIRST INSTANCE OF THE ATTACK COLOUR, ATTACKER ALWAYS ROLLS FIRST
         * WOW WE FOUND THE PIXEL, GO DOWN UNTIL WE FIND THE HEIGHT OF THE DICE; WIDTH = HEIGHT
         */

        // Go through picture, find all the dice for BOTH TEAMS, create collection containing all of them and return
        public static Collection<DiceImageReference> extractDiceRefsFromImage(Bitmap img, DiceTeam team)
        {
            Collection<DiceImageReference> finalCollection = new Collection<DiceImageReference>();

            Point newStart = new Point(0, 0);
            DiceImageReference lastResult = DiceImageReference.NullReference();
            Boolean firstRun = true;

            // NEED TO CHECK FOR TEAM
            // Loop through the picture until we stop finding dice
            while (firstRun == true ||
                !lastResult.isNull())
            {
                firstRun = false;
                // Adjust our starting position so we don't load the same dice picture over and over again
                newStart = lastResult.location;
                newStart.X += lastResult.size.Width;

                lastResult = DiceImageProcessor.findDicePicture(img, newStart, team);
                if (!lastResult.isNull())
                    finalCollection.Add(lastResult);
            }

            return finalCollection;
        }
    }
}
