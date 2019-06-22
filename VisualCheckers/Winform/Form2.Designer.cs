namespace Winform
{
    partial class Form2
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
            this.ResetButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.WinText = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // ResetButton
            // 
            this.ResetButton.Location = new System.Drawing.Point(161, 123);
            this.ResetButton.Name = "ResetButton";
            this.ResetButton.Size = new System.Drawing.Size(70, 44);
            this.ResetButton.TabIndex = 1;
            this.ResetButton.Text = "Reset Game";
            this.ResetButton.UseVisualStyleBackColor = true;
            this.ResetButton.Click += new System.EventHandler(this.ResetButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 25.8F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(96, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(205, 51);
            this.label1.TabIndex = 3;
            this.label1.Text = "You Win!";
            // 
            // WinText
            // 
            this.WinText.AutoSize = true;
            this.WinText.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.WinText.Location = new System.Drawing.Point(45, 75);
            this.WinText.Name = "WinText";
            this.WinText.Size = new System.Drawing.Size(113, 25);
            this.WinText.TabIndex = 4;
            this.WinText.Text = "placeholder";
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(401, 220);
            this.Controls.Add(this.WinText);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ResetButton);
            this.Name = "Form2";
            this.Text = "Form2";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button ResetButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label WinText;
    }
}