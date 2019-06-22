namespace Winform
{
    partial class Form1
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
            this.Commit = new System.Windows.Forms.Button();
            this.Clutch = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Commit
            // 
            this.Commit.Location = new System.Drawing.Point(613, 350);
            this.Commit.Name = "Commit";
            this.Commit.Size = new System.Drawing.Size(75, 51);
            this.Commit.TabIndex = 0;
            this.Commit.Text = "Finish Turn";
            this.Commit.UseVisualStyleBackColor = true;
            this.Commit.Click += new System.EventHandler(this.Commit_Click);
            // 
            // Clutch
            // 
            this.Clutch.Location = new System.Drawing.Point(613, 246);
            this.Clutch.Name = "Clutch";
            this.Clutch.Size = new System.Drawing.Size(75, 51);
            this.Clutch.TabIndex = 1;
            this.Clutch.Text = "Clutch";
            this.Clutch.UseVisualStyleBackColor = true;
            this.Clutch.Click += new System.EventHandler(this.Clutch_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(775, 522);
            this.Controls.Add(this.Clutch);
            this.Controls.Add(this.Commit);
            this.Name = "Form1";
            this.Text = "Checkers";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button Commit;
        private System.Windows.Forms.Button Clutch;
    }
}

