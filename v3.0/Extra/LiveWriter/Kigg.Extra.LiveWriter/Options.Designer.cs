namespace Kigg.Extra.LiveWriter
{
    partial class Options
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        #pragma warning disable 649
        private System.ComponentModel.IContainer components;
        #pragma warning restore 649

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
            this.btnOK = new System.Windows.Forms.Button();
            this.btnDefault = new System.Windows.Forms.Button();
            this.picPreview = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.countForeColor = new Kigg.Extra.LiveWriter.ColorControl();
            this.countBackColor = new Kigg.Extra.LiveWriter.ColorControl();
            this.shoutItForeColor = new Kigg.Extra.LiveWriter.ColorControl();
            this.shoutItBackColor = new Kigg.Extra.LiveWriter.ColorControl();
            this.borderColor = new Kigg.Extra.LiveWriter.ColorControl();
            ((System.ComponentModel.ISupportInitialize)(this.picPreview)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(252, 292);
            this.btnOK.Margin = new System.Windows.Forms.Padding(4);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(100, 28);
            this.btnOK.TabIndex = 6;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnDefault
            // 
            this.btnDefault.Location = new System.Drawing.Point(145, 292);
            this.btnDefault.Margin = new System.Windows.Forms.Padding(4);
            this.btnDefault.Name = "btnDefault";
            this.btnDefault.Size = new System.Drawing.Size(100, 28);
            this.btnDefault.TabIndex = 5;
            this.btnDefault.Text = "Default";
            this.btnDefault.UseVisualStyleBackColor = true;
            this.btnDefault.Click += new System.EventHandler(this.btnDefault_Click);
            // 
            // picPreview
            // 
            this.picPreview.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.picPreview.Location = new System.Drawing.Point(192, 131);
            this.picPreview.Name = "picPreview";
            this.picPreview.Size = new System.Drawing.Size(160, 44);
            this.picPreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.picPreview.TabIndex = 7;
            this.picPreview.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(192, 114);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 17);
            this.label1.TabIndex = 8;
            this.label1.Text = "Live Preview:";
            // 
            // countForeColor
            // 
            this.countForeColor.Caption = "Count Forecolor:";
            this.countForeColor.Location = new System.Drawing.Point(12, 230);
            this.countForeColor.Name = "countForeColor";
            this.countForeColor.Size = new System.Drawing.Size(180, 54);
            this.countForeColor.TabIndex = 4;
            this.countForeColor.Value = "";
            this.countForeColor.ColorChanged += new System.EventHandler(this.countForeColor_ColorChanged);
            // 
            // countBackColor
            // 
            this.countBackColor.Caption = "Count Backcolor:";
            this.countBackColor.Location = new System.Drawing.Point(12, 176);
            this.countBackColor.Name = "countBackColor";
            this.countBackColor.Size = new System.Drawing.Size(180, 54);
            this.countBackColor.TabIndex = 3;
            this.countBackColor.Value = "";
            this.countBackColor.ColorChanged += new System.EventHandler(this.countBackColor_ColorChanged);
            // 
            // shoutItForeColor
            // 
            this.shoutItForeColor.Caption = "Shout It Forecolor:";
            this.shoutItForeColor.Location = new System.Drawing.Point(12, 122);
            this.shoutItForeColor.Name = "shoutItForeColor";
            this.shoutItForeColor.Size = new System.Drawing.Size(180, 54);
            this.shoutItForeColor.TabIndex = 2;
            this.shoutItForeColor.Value = "";
            this.shoutItForeColor.ColorChanged += new System.EventHandler(this.shoutItForeColor_ColorChanged);
            // 
            // shoutItBackColor
            // 
            this.shoutItBackColor.Caption = "Shout It Backcolor:";
            this.shoutItBackColor.Location = new System.Drawing.Point(12, 68);
            this.shoutItBackColor.Name = "shoutItBackColor";
            this.shoutItBackColor.Size = new System.Drawing.Size(180, 54);
            this.shoutItBackColor.TabIndex = 1;
            this.shoutItBackColor.Value = "";
            this.shoutItBackColor.ColorChanged += new System.EventHandler(this.shoutItBackColor_ColorChanged);
            // 
            // borderColor
            // 
            this.borderColor.Caption = "Border Color:";
            this.borderColor.Location = new System.Drawing.Point(12, 14);
            this.borderColor.Name = "borderColor";
            this.borderColor.Size = new System.Drawing.Size(180, 54);
            this.borderColor.TabIndex = 0;
            this.borderColor.Value = "";
            this.borderColor.ColorChanged += new System.EventHandler(this.borderColor_ColorChanged);
            // 
            // Options
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(365, 334);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.picPreview);
            this.Controls.Add(this.btnDefault);
            this.Controls.Add(this.countForeColor);
            this.Controls.Add(this.countBackColor);
            this.Controls.Add(this.shoutItForeColor);
            this.Controls.Add(this.shoutItBackColor);
            this.Controls.Add(this.borderColor);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Options";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Options";
            this.Load += new System.EventHandler(this.Options_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picPreview)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ColorControl borderColor;
        private System.Windows.Forms.Button btnOK;
        private ColorControl shoutItBackColor;
        private ColorControl shoutItForeColor;
        private ColorControl countBackColor;
        private ColorControl countForeColor;
        private System.Windows.Forms.Button btnDefault;
        private System.Windows.Forms.PictureBox picPreview;
        private System.Windows.Forms.Label label1;
    }
}