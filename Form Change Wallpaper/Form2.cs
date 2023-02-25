using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Form_Change_Wallpaper
{
    public partial class Form2 : Form
    {

        private void SetWallpaperChoice(int choice)
        {
            GlobalVars.UserChoice = choice;
            this.Close();
        }

        public Form2()
        {
            InitializeComponent();
        }

        private void pictureBox1_DoubleClick(object sender, EventArgs e)
        {
            SetWallpaperChoice(0);
        }

        private void pictureBox2_DoubleClick(object sender, EventArgs e)
        {
            SetWallpaperChoice(1);
        }

        private void pictureBox3_DoubleClick(object sender, EventArgs e)
        {
            SetWallpaperChoice(2);
        }

        private void pictureBox4_DoubleClick(object sender, EventArgs e)
        {
            SetWallpaperChoice(3);
        }
    }
}
