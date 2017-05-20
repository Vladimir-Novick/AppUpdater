namespace SGCombo.Net.Update.Downloader
{
    partial class UpdaterMainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        public System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UpdaterMainForm));
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.LabelFormTile = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.StatusMessages = new System.Windows.Forms.TextBox();
            this.processLineControl = new System.Windows.Forms.Label();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.pictureBoxLogo = new System.Windows.Forms.PictureBox();
            this.BoxLoad = new System.Windows.Forms.PictureBox();
            this.buttonUnistall = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BoxLoad)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Image = ((System.Drawing.Image)(resources.GetObject("btnOK.Image")));
            this.btnOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOK.Location = new System.Drawing.Point(144, 326);
            this.btnOK.Margin = new System.Windows.Forms.Padding(10, 3, 3, 3);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(121, 23);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "Start Upgrade";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Image = ((System.Drawing.Image)(resources.GetObject("btnCancel.Image")));
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(516, 326);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(121, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Close";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // LabelFormTile
            // 
            this.LabelFormTile.BackColor = System.Drawing.Color.Transparent;
            this.LabelFormTile.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelFormTile.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.LabelFormTile.Location = new System.Drawing.Point(153, 40);
            this.LabelFormTile.Name = "LabelFormTile";
            this.LabelFormTile.Size = new System.Drawing.Size(421, 23);
            this.LabelFormTile.TabIndex = 5;
            this.LabelFormTile.Text = "SGCombo.com Update Manager V6";
            this.LabelFormTile.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.ForeColor = System.Drawing.Color.DimGray;
            this.label3.Location = new System.Drawing.Point(184, 351);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(415, 24);
            this.label3.TabIndex = 26;
            this.label3.Text = "Copyright (C) 2011 Vladimir Novick . All rights reserved.";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(164, 63);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 13);
            this.label1.TabIndex = 29;
            this.label1.Text = "Status Messages:";
            // 
            // StatusMessages
            // 
            this.StatusMessages.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.StatusMessages.ForeColor = System.Drawing.Color.White;
            this.StatusMessages.Location = new System.Drawing.Point(144, 82);
            this.StatusMessages.Margin = new System.Windows.Forms.Padding(6, 3, 3, 3);
            this.StatusMessages.Multiline = true;
            this.StatusMessages.Name = "StatusMessages";
            this.StatusMessages.ReadOnly = true;
            this.StatusMessages.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.StatusMessages.Size = new System.Drawing.Size(501, 202);
            this.StatusMessages.TabIndex = 31;
            // 
            // processLineControl
            // 
            this.processLineControl.BackColor = System.Drawing.Color.Transparent;
            this.processLineControl.Location = new System.Drawing.Point(167, 287);
            this.processLineControl.Name = "processLineControl";
            this.processLineControl.Size = new System.Drawing.Size(470, 23);
            this.processLineControl.TabIndex = 32;
            this.processLineControl.Text = ">>";
            // 
            // linkLabel1
            // 
            this.linkLabel1.ActiveLinkColor = System.Drawing.Color.DarkRed;
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.BackColor = System.Drawing.Color.Transparent;
            this.linkLabel1.LinkColor = System.Drawing.Color.Navy;
            this.linkLabel1.Location = new System.Drawing.Point(484, 63);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(131, 13);
            this.linkLabel1.TabIndex = 34;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "http://www.sgcombo.com";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // pictureBoxLogo
            // 
            this.pictureBoxLogo.BackColor = System.Drawing.Color.Transparent;
            this.pictureBoxLogo.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxLogo.Image")));
            this.pictureBoxLogo.Location = new System.Drawing.Point(9, 40);
            this.pictureBoxLogo.Name = "pictureBoxLogo";
            this.pictureBoxLogo.Size = new System.Drawing.Size(119, 128);
            this.pictureBoxLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBoxLogo.TabIndex = 27;
            this.pictureBoxLogo.TabStop = false;
            // 
            // BoxLoad
            // 
            this.BoxLoad.BackColor = System.Drawing.Color.Transparent;
            this.BoxLoad.Image = global::SGCombo.Net.Update.Downloader.Properties.Resources.inline_loader;
            this.BoxLoad.Location = new System.Drawing.Point(144, 290);
            this.BoxLoad.Name = "BoxLoad";
            this.BoxLoad.Size = new System.Drawing.Size(19, 11);
            this.BoxLoad.TabIndex = 35;
            this.BoxLoad.TabStop = false;
            this.BoxLoad.Visible = false;
            // 
            // buttonUnistall
            // 
            this.buttonUnistall.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonUnistall.Image = ((System.Drawing.Image)(resources.GetObject("buttonUnistall.Image")));
            this.buttonUnistall.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonUnistall.Location = new System.Drawing.Point(397, 326);
            this.buttonUnistall.Margin = new System.Windows.Forms.Padding(10, 3, 3, 3);
            this.buttonUnistall.Name = "buttonUnistall";
            this.buttonUnistall.Size = new System.Drawing.Size(100, 23);
            this.buttonUnistall.TabIndex = 36;
            this.buttonUnistall.Text = "Unistall";
            this.buttonUnistall.UseVisualStyleBackColor = true;
            this.buttonUnistall.Click += new System.EventHandler(this.buttonUnistall_Click);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Image = ((System.Drawing.Image)(resources.GetObject("button1.Image")));
            this.button1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button1.Location = new System.Drawing.Point(288, 326);
            this.button1.Margin = new System.Windows.Forms.Padding(10, 3, 3, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(96, 23);
            this.button1.TabIndex = 37;
            this.button1.Text = "Install";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // UpdaterMainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(657, 387);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.buttonUnistall);
            this.Controls.Add(this.BoxLoad);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.processLineControl);
            this.Controls.Add(this.StatusMessages);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBoxLogo);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.LabelFormTile);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "UpdaterMainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SGCombo.com Update Manager ";
            this.Activated += new System.EventHandler(this.UpdaterMainForm_Activated);
            this.Load += new System.EventHandler(this.UpdaterMainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BoxLoad)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.Label LabelFormTile;
        private System.Windows.Forms.TextBox StatusMessages;
        private System.Windows.Forms.Label processLineControl;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.PictureBox pictureBoxLogo;
        private System.Windows.Forms.PictureBox BoxLoad;
        private System.Windows.Forms.Button buttonUnistall;
        private System.Windows.Forms.Button button1;

    }
}

