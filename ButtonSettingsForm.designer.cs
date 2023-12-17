
namespace Keys_Visualizer
{
    partial class ButtonSettingsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ButtonSettingsForm));
            this.lblName = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.txtText = new System.Windows.Forms.TextBox();
            this.lblText = new System.Windows.Forms.Label();
            this.txtLocationX = new System.Windows.Forms.TextBox();
            this.lblLocation = new System.Windows.Forms.Label();
            this.txtLocationY = new System.Windows.Forms.TextBox();
            this.txtSizeHeight = new System.Windows.Forms.TextBox();
            this.txtSizeWidth = new System.Windows.Forms.TextBox();
            this.lblSize = new System.Windows.Forms.Label();
            this.lblFontColor = new System.Windows.Forms.Label();
            this.btnFontColor = new System.Windows.Forms.Button();
            this.btnBackColor = new System.Windows.Forms.Button();
            this.lblBackColor = new System.Windows.Forms.Label();
            this.btnOnActiveColor = new System.Windows.Forms.Button();
            this.lblOnActiveColor = new System.Windows.Forms.Label();
            this.cdFont = new System.Windows.Forms.ColorDialog();
            this.cdBackColor = new System.Windows.Forms.ColorDialog();
            this.cdOnActiveColor = new System.Windows.Forms.ColorDialog();
            this.btnRemove = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(13, 11);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(38, 13);
            this.lblName.TabIndex = 0;
            this.lblName.Text = "Name:";
            // 
            // txtName
            // 
            this.txtName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtName.Location = new System.Drawing.Point(16, 28);
            this.txtName.Name = "txtName";
            this.txtName.ReadOnly = true;
            this.txtName.Size = new System.Drawing.Size(128, 20);
            this.txtName.TabIndex = 1;
            // 
            // txtText
            // 
            this.txtText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtText.Location = new System.Drawing.Point(16, 76);
            this.txtText.Name = "txtText";
            this.txtText.Size = new System.Drawing.Size(128, 20);
            this.txtText.TabIndex = 3;
            // 
            // lblText
            // 
            this.lblText.AutoSize = true;
            this.lblText.Location = new System.Drawing.Point(13, 59);
            this.lblText.Name = "lblText";
            this.lblText.Size = new System.Drawing.Size(31, 13);
            this.lblText.TabIndex = 2;
            this.lblText.Text = "Text:";
            // 
            // txtLocationX
            // 
            this.txtLocationX.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLocationX.Location = new System.Drawing.Point(16, 123);
            this.txtLocationX.Name = "txtLocationX";
            this.txtLocationX.Size = new System.Drawing.Size(66, 20);
            this.txtLocationX.TabIndex = 5;
            this.txtLocationX.TextChanged += new System.EventHandler(this.numberTextBox_TextChanged);
            // 
            // lblLocation
            // 
            this.lblLocation.AutoSize = true;
            this.lblLocation.Location = new System.Drawing.Point(13, 106);
            this.lblLocation.Name = "lblLocation";
            this.lblLocation.Size = new System.Drawing.Size(51, 13);
            this.lblLocation.TabIndex = 4;
            this.lblLocation.Text = "Location:";
            // 
            // txtLocationY
            // 
            this.txtLocationY.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLocationY.Location = new System.Drawing.Point(81, 123);
            this.txtLocationY.Name = "txtLocationY";
            this.txtLocationY.Size = new System.Drawing.Size(63, 20);
            this.txtLocationY.TabIndex = 6;
            this.txtLocationY.TextChanged += new System.EventHandler(this.numberTextBox_TextChanged);
            // 
            // txtSizeHeight
            // 
            this.txtSizeHeight.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSizeHeight.Location = new System.Drawing.Point(81, 169);
            this.txtSizeHeight.Name = "txtSizeHeight";
            this.txtSizeHeight.Size = new System.Drawing.Size(63, 20);
            this.txtSizeHeight.TabIndex = 9;
            this.txtSizeHeight.TextChanged += new System.EventHandler(this.numberTextBox_TextChanged);
            // 
            // txtSizeWidth
            // 
            this.txtSizeWidth.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSizeWidth.Location = new System.Drawing.Point(16, 169);
            this.txtSizeWidth.Name = "txtSizeWidth";
            this.txtSizeWidth.Size = new System.Drawing.Size(66, 20);
            this.txtSizeWidth.TabIndex = 8;
            this.txtSizeWidth.TextChanged += new System.EventHandler(this.numberTextBox_TextChanged);
            // 
            // lblSize
            // 
            this.lblSize.AutoSize = true;
            this.lblSize.Location = new System.Drawing.Point(13, 152);
            this.lblSize.Name = "lblSize";
            this.lblSize.Size = new System.Drawing.Size(30, 13);
            this.lblSize.TabIndex = 7;
            this.lblSize.Text = "Size:";
            // 
            // lblFontColor
            // 
            this.lblFontColor.AutoSize = true;
            this.lblFontColor.Location = new System.Drawing.Point(16, 208);
            this.lblFontColor.Name = "lblFontColor";
            this.lblFontColor.Size = new System.Drawing.Size(57, 13);
            this.lblFontColor.TabIndex = 10;
            this.lblFontColor.Text = "Font color:";
            // 
            // btnFontColor
            // 
            this.btnFontColor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFontColor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFontColor.Location = new System.Drawing.Point(16, 225);
            this.btnFontColor.Name = "btnFontColor";
            this.btnFontColor.Size = new System.Drawing.Size(128, 30);
            this.btnFontColor.TabIndex = 11;
            this.btnFontColor.Text = "Select";
            this.btnFontColor.UseVisualStyleBackColor = true;
            this.btnFontColor.Click += new System.EventHandler(this.btnFontColor_Click);
            // 
            // btnBackColor
            // 
            this.btnBackColor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBackColor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBackColor.Location = new System.Drawing.Point(16, 280);
            this.btnBackColor.Name = "btnBackColor";
            this.btnBackColor.Size = new System.Drawing.Size(128, 30);
            this.btnBackColor.TabIndex = 13;
            this.btnBackColor.Text = "Select";
            this.btnBackColor.UseVisualStyleBackColor = true;
            this.btnBackColor.Click += new System.EventHandler(this.btnBackColor_Click);
            // 
            // lblBackColor
            // 
            this.lblBackColor.AutoSize = true;
            this.lblBackColor.Location = new System.Drawing.Point(16, 263);
            this.lblBackColor.Name = "lblBackColor";
            this.lblBackColor.Size = new System.Drawing.Size(61, 13);
            this.lblBackColor.TabIndex = 12;
            this.lblBackColor.Text = "Back color:";
            // 
            // btnOnActiveColor
            // 
            this.btnOnActiveColor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOnActiveColor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOnActiveColor.Location = new System.Drawing.Point(16, 337);
            this.btnOnActiveColor.Name = "btnOnActiveColor";
            this.btnOnActiveColor.Size = new System.Drawing.Size(128, 30);
            this.btnOnActiveColor.TabIndex = 15;
            this.btnOnActiveColor.Text = "Select";
            this.btnOnActiveColor.UseVisualStyleBackColor = true;
            this.btnOnActiveColor.Click += new System.EventHandler(this.btnOnActiveColor_Click);
            // 
            // lblOnActiveColor
            // 
            this.lblOnActiveColor.AutoSize = true;
            this.lblOnActiveColor.Location = new System.Drawing.Point(16, 320);
            this.lblOnActiveColor.Name = "lblOnActiveColor";
            this.lblOnActiveColor.Size = new System.Drawing.Size(82, 13);
            this.lblOnActiveColor.TabIndex = 14;
            this.lblOnActiveColor.Text = "On active color:";
            // 
            // btnRemove
            // 
            this.btnRemove.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRemove.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRemove.Location = new System.Drawing.Point(16, 392);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(128, 30);
            this.btnRemove.TabIndex = 17;
            this.btnRemove.Text = "Remove";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // ButtonSettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gainsboro;
            this.ClientSize = new System.Drawing.Size(156, 435);
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.btnOnActiveColor);
            this.Controls.Add(this.lblOnActiveColor);
            this.Controls.Add(this.btnBackColor);
            this.Controls.Add(this.lblBackColor);
            this.Controls.Add(this.btnFontColor);
            this.Controls.Add(this.lblFontColor);
            this.Controls.Add(this.txtSizeHeight);
            this.Controls.Add(this.txtSizeWidth);
            this.Controls.Add(this.lblSize);
            this.Controls.Add(this.txtLocationY);
            this.Controls.Add(this.txtLocationX);
            this.Controls.Add(this.lblLocation);
            this.Controls.Add(this.txtText);
            this.Controls.Add(this.lblText);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.lblName);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(172, 474);
            this.MinimizeBox = false;
            this.Name = "ButtonSettingsForm";
            this.Text = "Button settingsForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ButtonSettingsForm_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.TextBox txtText;
        private System.Windows.Forms.Label lblText;
        private System.Windows.Forms.TextBox txtLocationX;
        private System.Windows.Forms.Label lblLocation;
        private System.Windows.Forms.TextBox txtLocationY;
        private System.Windows.Forms.TextBox txtSizeHeight;
        private System.Windows.Forms.TextBox txtSizeWidth;
        private System.Windows.Forms.Label lblSize;
        private System.Windows.Forms.Label lblFontColor;
        private System.Windows.Forms.Button btnFontColor;
        private System.Windows.Forms.Button btnBackColor;
        private System.Windows.Forms.Label lblBackColor;
        private System.Windows.Forms.Button btnOnActiveColor;
        private System.Windows.Forms.Label lblOnActiveColor;
        private System.Windows.Forms.ColorDialog cdFont;
        private System.Windows.Forms.ColorDialog cdBackColor;
        private System.Windows.Forms.ColorDialog cdOnActiveColor;
        private System.Windows.Forms.Button btnRemove;
    }
}