using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Form_Change_Wallpaper
{
    internal class GlobalVars
    {
        private static int userChoice = Properties.Settings.Default.Wallpaper;

        public static int UserChoice
        {
            get { return userChoice; }
            set
            {
                Properties.Settings.Default.Wallpaper = value;
                Properties.Settings.Default.Save();
            }
        }
    }
}
