using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Assemblies;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;

namespace Form_Change_Wallpaper
{
    public partial class Form1 : Form
    {
        private bool enabled = Properties.Settings.Default.Enabled;
        private bool startMinimized = Properties.Settings.Default.StartMinimized;
        private bool startWithWindows = Properties.Settings.Default.StartWithWindows;

        private int userChoise = GlobalVars.UserChoice;

        private static int sunriseTime = 7;
        private static int sunsetTime = 17;
        Wallpaper wallpaper = new Wallpaper();

        private int mouseX;
        private int mouseY;
        private bool canMove = false;

        Color btnEnabledColor = Color.FromArgb(241, 143, 1);
        Color btnColor = Color.FromArgb(39, 44, 78);
        Color uncheckedColor = Color.FromArgb(247, 247, 242);

        private static bool IsDay()
        {
            int hour = DateTime.Now.Hour;
            if (hour > sunriseTime && hour < sunsetTime)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void ChangeWallpaper()
        {
            if (IsDay() && Wallpaper.GetCurrentWallpaper() != wallpaper.Wallpapers[0, userChoise])
            {
                Wallpaper.SetWallpaper(wallpaper.Wallpapers[0, userChoise]);
                WallpaperToLabelText(wallpaper.Wallpapers[0, userChoise], "update");
            }
            else if (!IsDay() && Wallpaper.GetCurrentWallpaper() != wallpaper.Wallpapers[1, userChoise])
            {
                Wallpaper.SetWallpaper(wallpaper.Wallpapers[1, userChoise]);
                WallpaperToLabelText(wallpaper.Wallpapers[1, userChoise], "update");
            }
        }

        private void WallpaperToLabelText(string path, string choice)
        {
            if (choice == "current")
            {
                //coloredLabel.Text = "Current wallpaper is " + Path.GetFileName(path);
            }
            else if (choice == "update")
            {
                coloredLabel.Text = "Wallpaper:";
                uncoloredLabel.Text = Path.GetFileName(path);
            }
        }

        private void ShowTrayIcon()
        {
            if (this.WindowState == FormWindowState.Minimized && notifyIcon.Visible == false)
            {
                this.ShowInTaskbar = false;
                notifyIcon.Visible = true;
            }
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.btnEnable.Text = enabled ? "Active" : "Inactive";
            this.btnEnable.BackColor = enabled ? btnEnabledColor : btnColor;

            if (startMinimized)
            {
                this.checkBoxMinimize.Checked = true;
                this.WindowState = FormWindowState.Minimized;
                ShowTrayIcon();
            }

            if (startWithWindows)
            {
                this.checkBoxStart.Checked = true;
            }
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            ShowTrayIcon();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (enabled)
            {
                ChangeWallpaper();
            }
        }

        // Activate the app
        private void btnEnable_Click(object sender, EventArgs e)
        {
            if (!enabled)
            {
                Properties.Settings.Default.Enabled = true;
                enabled = true;
                this.timer.Enabled = true;

                this.btnEnable.Text = "Active";
                this.btnEnable.BackColor = btnEnabledColor;
            }
            else
            {
                Properties.Settings.Default.Enabled = false;
                enabled = false;
                this.timer.Enabled = false;

                this.btnEnable.Text = "Inactive";
                this.btnEnable.BackColor = btnColor;
            }
            Properties.Settings.Default.Save();
        }

        // Settings
        private void btnSettings_Click(object sender, EventArgs e)
        {
            using (Form2 f2 = new Form2())
            {
                f2.ShowDialog();
                userChoise = Properties.Settings.Default.Wallpaper;
            }
        }

        // Handle Minimize
        private void notifyIcon_MouseClick(object sender, MouseEventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            this.ShowInTaskbar = true;
            notifyIcon.Visible = false;
        }

        private void checkBoxMinimize_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxMinimize.CheckState == CheckState.Checked)
            {
                Properties.Settings.Default.StartMinimized = true;
                checkBoxMinimize.ForeColor = btnEnabledColor;
            }
            else
            {
                Properties.Settings.Default.StartMinimized = false;
                checkBoxMinimize.ForeColor = uncheckedColor;
            }
            Properties.Settings.Default.Save();
        }

        // Start with windows

        private void checkBoxStart_CheckedChanged(object sender, EventArgs e)
        {
            RegistryKey subkey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            if (checkBoxStart.CheckState == CheckState.Checked)
            {
                subkey.SetValue("Wallpaper Cycle", Application.ExecutablePath);
                Properties.Settings.Default.StartWithWindows = true;
                checkBoxStart.ForeColor = btnEnabledColor;
            }
            else
            {
                subkey.DeleteValue("Wallpaper Cycle", false);
                Properties.Settings.Default.StartWithWindows = false;
                checkBoxStart.ForeColor = uncheckedColor;
            }
            Properties.Settings.Default.Save();
        }

        // Drag the window
        private void controlPanel_MouseDown(object sender, MouseEventArgs e)
        {
            canMove = true;
            mouseX = e.X;
            mouseY = e.Y;
        }

        private void controlPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (canMove)
            {
                this.SetDesktopLocation(MousePosition.X - mouseX, MousePosition.Y - mouseY);
            }
        }

        private void controlPanel_MouseUp(object sender, MouseEventArgs e)
        {
            canMove = false;
        }

        // Form State Handler
        private void btnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}