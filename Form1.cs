using Microsoft.VisualBasic;
using System.Collections.ObjectModel;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace AA_DiceReader
{
    public partial class MainForm : Form
    {
        //private Collection<PictureBox> capturedImages_Controls = new Collection<PictureBox>(); Don't think we need this with imagesFlow.Controls

        private Collection<Image> allClipboardImages = new Collection<Image>();
        private Size maxImagePreviewSize = new Size(720, 480);
        private Collection<int> diceResults_attacker = new Collection<int>();
        private Collection<int> diceResults_defender = new Collection<int>();

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            clipboardCheckTimer.Enabled = true;
            imagesFlow.MaximumSize = new Size (int.MaxValue, maxImagePreviewSize.Height + 25);
        }

        private void clipboardCheckTimer_Tick(object sender, EventArgs e)
        {
            // Check to see if clipboard has an image in it, add it to our array and clear the clipboard
            if (!Clipboard.ContainsImage())
                return;

            allClipboardImages.Add(Clipboard.GetImage());
            Clipboard.Clear();

            // Add the newly grabbed image to our flow container
            PictureBox newPictureBox = new PictureBox();
            newPictureBox.Image = allClipboardImages.Last();
            newPictureBox.BorderStyle = BorderStyle.FixedSingle;

            // Sure wish autosize interacted with maximumsize the way I wanted it to
            if (newPictureBox.Image.Size.Width < maxImagePreviewSize.Width && 
                newPictureBox.Image.Size.Height < maxImagePreviewSize.Height)
            { // Our screenshot is smaller than our max size, just use autosize
                newPictureBox.SizeMode = PictureBoxSizeMode.AutoSize;
                newPictureBox.MaximumSize = maxImagePreviewSize;
            }
            else
            { // Our screenshot is a big boy, squish it
                newPictureBox.Size = maxImagePreviewSize;
                newPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            }

            imagesFlow.Controls.Add(newPictureBox);
        }

        private void runCheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            clipboardCheckTimer.Enabled = runCheckBox1.Checked;
        }

        private void scanDiceButton_Click(object sender, EventArgs e)
        {
            // Go through every image we have and find those dice...
            foreach (Control enumedControl in imagesFlow.Controls)
            {
                Bitmap img = (Bitmap)((PictureBox)enumedControl).Image;

                Collection<DiceImageReference> diceReferences = DiceImageProcessor.extractDiceRefsFromImage(img, DiceTeam.Attacker);

                //DiceImageReference newReference = DiceImageProcessor.findDicePicture(img, Point.Empty, DiceTeam.Attacker);
                foreach (DiceImageReference diceRef in diceReferences) {
                    PictureBox newPictureBox = new PictureBox();
                    newPictureBox.Image = diceRef.image;
                    newPictureBox.BorderStyle = BorderStyle.FixedSingle;
                    newPictureBox.SizeMode = PictureBoxSizeMode.AutoSize;

                    singleDiceImageFlow.Controls.Add(newPictureBox);
                }
            }
        }
    }
}