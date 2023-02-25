using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Form_Change_Wallpaper
{
    internal class Wallpaper
    {
        private string[,] wallpapersMatrix = new string[3, 1];

        private readonly string assemblyPath = Path.GetFullPath(Assembly.GetExecutingAssembly().Location);

        public Wallpaper()
        {
            string dataPath = assemblyPath + @"..\..\..\..\data";
            string[] dataPathEntry = Directory.GetDirectories(dataPath);
            string[] DayEntryFiles = Directory.GetFiles(dataPathEntry[0]);
            string[] NightEntryFiles = Directory.GetFiles(dataPathEntry[1]);

            string[,] images =
            {
                    { DayEntryFiles[0], DayEntryFiles[1], DayEntryFiles[2], DayEntryFiles[3] },
                    { NightEntryFiles[0], NightEntryFiles[1], NightEntryFiles[2], NightEntryFiles[3] }
                };
            this.wallpapersMatrix = images;
        }

        public string[,] Wallpapers
        {
            get { return this.wallpapersMatrix; }
        }

        // Get current wallpaper
        private static byte[] SliceMe(byte[] source, int pos)
        {
            byte[] destFolder = new byte[source.Length - pos];
            Array.Copy(source, pos, destFolder, 0, destFolder.Length);
            return destFolder;
        }

        public static string GetCurrentWallpaper()
        {
            RegistryKey subkey = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop");
            byte[] path = (byte[])subkey.GetValue("TranscodedImageCache");

            String wallpaper = Encoding.Unicode.GetString(SliceMe(path, 24)).TrimEnd("\0".ToCharArray());
            return wallpaper;
        }

        // Set wallpaper with win32
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);

        private static readonly int SPI_SETDESKWALLPAPER = 0x14;
        private static readonly int SPIF_UPDATEINIFILE = 0x01;
        private static readonly int SPIF_SENDWININICHANGE = 0x02;

        public static void SetWallpaper(string wallpaperPath)
        {
            SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, wallpaperPath, SPIF_UPDATEINIFILE | SPIF_SENDWININICHANGE);
        }
    }
}
