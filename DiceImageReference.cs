using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AA_DiceReader
{
    enum DiceTeam
    {
        Attacker,
        Defender,
        NULL
    }

    // Class used to hold the data DiceImageProcessing pulls from an image
    internal class DiceImageReference
    {
        public Size size; //Size of dice picture (probably always going to be 54x54
        public Point location; //X,Y of where the dice is on the main screenshot provided
        public Image image; //The picture of the dice
        public int diceValue; //1-6
        public DiceTeam diceTeam;

        public static Image nullImage = new Bitmap(1, 1);

        public DiceImageReference(Image diceImage, Point diceLocation, int diceValue, DiceTeam diceTeam)
        {
            this.image = diceImage;
            this.size = this.image.Size;
            this.location = diceLocation;
            this.diceValue = diceValue;
            this.diceTeam = diceTeam;
        }

        public Boolean isNull()
        {
            return (this.diceTeam == DiceTeam.NULL);
        }

        //Used for empty DiceImageReference
        protected DiceImageReference()
        {
            this.image = DiceImageReference.nullImage;
            this.diceTeam = DiceTeam.NULL;
        }

        public static DiceImageReference NullReference()
        {
            return new DiceImageReference();
        }
    }
}
