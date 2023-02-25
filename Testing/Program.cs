using System;
using System.Runtime.InteropServices;

namespace Test
{
    internal static class Program
    {
        private static string ChechCondition(string settingValue)
        {
            int position = settingValue.IndexOf("=");
            string condition = settingValue.Substring(position + 1);
            return condition;
        }

        public static void Main(string[] args)
        {
            string setting = "StartMinimized=true";
            Console.WriteLine(setting);
            Console.WriteLine(ChechCondition(setting));
        }
    }
}