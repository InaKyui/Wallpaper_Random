namespace WallpaperRandom
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.InteropServices;
    using Microsoft.Win32;

    /// <summary>
    /// Wallpaper random application.
    /// </summary>
    class WallpaperRandom
    {
        [DllImport("user32.dll", EntryPoint = "SystemParametersInfo")]
        public static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);

        public static bool ChangeWallpaper(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                return false;
            }
            if (File.Exists(filePath) == false)
            {
                return false;
            }

            var nResult = SystemParametersInfo(20, 1, filePath, 0x1 | 0x2);
            if (nResult == 0)
            {
                return false;
            }
            else
            {
                RegistryKey hk = Registry.CurrentUser;
                RegistryKey run = hk.CreateSubKey(@"Control Panel\Desktop\");
                run.SetValue("Wallpaper", filePath);

                return true;
            }
        }

        public static void Main(string[] args)
        {
            try
            {
                var wallpaperFolder = AppDomain.CurrentDomain.BaseDirectory;
                var dir = new DirectoryInfo(wallpaperFolder);
                var files = dir.GetFiles();
                var images = new List<string>();
                var image_ext = new List<string>() { ".jpg", ".png" };
                for (int i = 0; i < files.Length; i++)
                {
                    if (image_ext.Contains(files[i].Extension))
                    {
                        images.Add(files[i].FullName);
                    }
                }
                if (images.Count == 0)
                {
                    Console.WriteLine("Current path: " + wallpaperFolder);
                    Console.WriteLine("Please check image count.");
                    Console.ReadKey();
                }
                else
                {
                    var random = new Random();
                    var image = images[random.Next(0, images.Count)];
                    ChangeWallpaper(image);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Console.WriteLine(ex.Message);
                Console.ReadKey();
            }
        }
    }
}

