namespace AA_DiceReader
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.clipboardCheckTimer = new System.Windows.Forms.Timer(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.imagesFlow = new System.Windows.Forms.FlowLayoutPanel();
            this.runCheckBox1 = new System.Windows.Forms.CheckBox();
            this.scanDiceButton = new System.Windows.Forms.Button();
            this.singleDiceImageFlow = new System.Windows.Forms.FlowLayoutPanel();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // clipboardCheckTimer
            // 
            this.clipboardCheckTimer.Enabled = true;
            this.clipboardCheckTimer.Interval = 1000;
            this.clipboardCheckTimer.Tick += new System.EventHandler(this.clipboardCheckTimer_Tick);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(286, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "Note: Clipboard will be wiped on every image loaded";
            // 
            // imagesFlow
            // 
            this.imagesFlow.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.imagesFlow.AutoScroll = true;
            this.imagesFlow.BackColor = System.Drawing.SystemColors.ControlDark;
            this.imagesFlow.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.imagesFlow.Location = new System.Drawing.Point(12, 27);
            this.imagesFlow.Name = "imagesFlow";
            this.imagesFlow.Size = new System.Drawing.Size(557, 468);
            this.imagesFlow.TabIndex = 2;
            this.imagesFlow.WrapContents = false;
            // 
            // runCheckBox1
            // 
            this.runCheckBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.runCheckBox1.AutoSize = true;
            this.runCheckBox1.Checked = true;
            this.runCheckBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.runCheckBox1.Location = new System.Drawing.Point(12, 501);
            this.runCheckBox1.Name = "runCheckBox1";
            this.runCheckBox1.Size = new System.Drawing.Size(101, 19);
            this.runCheckBox1.TabIndex = 3;
            this.runCheckBox1.Text = "Pull Clipboard";
            this.runCheckBox1.UseVisualStyleBackColor = true;
            this.runCheckBox1.CheckedChanged += new System.EventHandler(this.runCheckBox1_CheckedChanged);
            // 
            // scanDiceButton
            // 
            this.scanDiceButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.scanDiceButton.Location = new System.Drawing.Point(12, 526);
            this.scanDiceButton.Name = "scanDiceButton";
            this.scanDiceButton.Size = new System.Drawing.Size(101, 23);
            this.scanDiceButton.TabIndex = 4;
            this.scanDiceButton.Text = "Scan Images";
            this.scanDiceButton.UseVisualStyleBackColor = true;
            this.scanDiceButton.Click += new System.EventHandler(this.scanDiceButton_Click);
            // 
            // singleDiceImageFlow
            // 
            this.singleDiceImageFlow.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.singleDiceImageFlow.AutoScroll = true;
            this.singleDiceImageFlow.BackColor = System.Drawing.SystemColors.ControlDark;
            this.singleDiceImageFlow.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.singleDiceImageFlow.Location = new System.Drawing.Point(575, 27);
            this.singleDiceImageFlow.Name = "singleDiceImageFlow";
            this.singleDiceImageFlow.Size = new System.Drawing.Size(510, 468);
            this.singleDiceImageFlow.TabIndex = 5;
            // 
            // toolTip1
            // 
            this.toolTip1.AutoPopDelay = 5000;
            this.toolTip1.InitialDelay = 100;
            this.toolTip1.ReshowDelay = 100;
            this.toolTip1.ShowAlways = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1097, 646);
            this.Controls.Add(this.singleDiceImageFlow);
            this.Controls.Add(this.scanDiceButton);
            this.Controls.Add(this.runCheckBox1);
            this.Controls.Add(this.imagesFlow);
            this.Controls.Add(this.label1);
            this.Name = "MainForm";
            this.Text = "A&A Dice Reader";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Timer clipboardCheckTimer;
        private Label label1;
        private FlowLayoutPanel imagesFlow;
        private CheckBox runCheckBox1;
        private Button scanDiceButton;
        private FlowLayoutPanel singleDiceImageFlow;
        private ToolTip toolTip1;
    }
}