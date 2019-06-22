using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Winform
{
    public partial class Form2 : Form
    {
        private bool reset;
            public bool Reset { get => reset; }
        public Form2(bool whiteWon)
        {
            string winner = whiteWon ? "White" : "Black";
            InitializeComponent();
            WinText.Text = $"Congratulations! {winner} player wins!";
            reset = false;
        }
            private void ResetButton_Click(object sender, EventArgs e)
        {
            reset = true;
            this.Close();
        }
    }
}
