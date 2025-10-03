using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace RegionFlagIcons
{

    public class ThumbnailProcessor
    {
        public static bool DeleteOldFile = true;

        public static void Main()
        {
            //string flagPath = @"C:\Users\Konsi\Documents\CS2-Modding\CS2-Region-Flag-Icons\.ail\flags\Arizona.png";
            //string[] imagePaths = { @"C:\Users\Konsi\Documents\CS2-Modding\CS2-Region-Flag-Icons\.ail\ee_thumbnails\EE Commercial Low.png" };
            //string folderPath = @"C:\Users\Konsi\Documents\CS2-Modding\CS2-Region-Flag-Icons\.ail\ee_thumbnails";
            //ApplyFlagToFolder(flagPath, folderPath);
        }

        public static void ApplyFlagToFolder(string flagPath, string folderPath, string extension = ".png")
        {
            string[] imagePaths = Directory.GetFiles(folderPath, "*" + extension);
            ApplyFlagToImages(flagPath, imagePaths);
        }

        public static void ApplyFlagToImages(string flagPath, string[] imagePaths)
        {
            foreach (string imagePath in imagePaths)
            {
                ApplyFlagToImage(flagPath, imagePath);
            }
        }

        public static void ApplyFlagToImage(string flagPath, string imagePath)
        {
            try
            {
                // Position of the flag
                Point location = new Point(90, 91);
                Rectangle rectangleLocation = new Rectangle(location.X - 2, location.Y - 2, 35, 35);

                // Colors for the border
                Color fillColor = ColorTranslator.FromHtml("#001626");
                Color lineColor = ColorTranslator.FromHtml("#00c1ff");

                // Load flag
                // Open file
                string backupPath = imagePath + ".old";
                using (Image flag = Image.FromFile(flagPath))
                {
                    //LogInfo($"Opening {imagePath}...");
                    File.Copy(imagePath, backupPath, true);
                    try
                    {
                        File.Decrypt(imagePath);
                    }
                    catch (Exception)
                    {
                        // Do nothing
                    }

                    using (Bitmap thumb = new Bitmap(backupPath))
                    using (Graphics g = Graphics.FromImage(thumb))
                    {
                        //LogInfo($"Drawing border at {rectangleLocation}...");

                        // Draw a border
                        using (Brush fillBrush = new SolidBrush(fillColor))
                        using (Pen outlinePen = new Pen(lineColor, 2))
                        {
                            g.FillRectangle(fillBrush, rectangleLocation);
                            g.DrawRectangle(outlinePen, rectangleLocation);
                        }

                        // Stamp the flag
                        g.DrawImage(flag, location);

                        // Save file
                        thumb.Save(imagePath, ImageFormat.Png);
                        //LogInfo("File saved!");
                    }
                }

                if (DeleteOldFile)
                {
                    File.Delete(backupPath);
                }
            }
            catch (Exception x)
            {
                LogWarn($"Failed to apply flag to image {imagePath}: {x.Message}");
            }
        }

        private static void LogInfo(string message)
        {
            if (Mod.log != null)
            {
                Mod.log.Info(message);
            }
            else
            {
                Console.WriteLine("[INFO] " + message);
            }
        }

        private static void LogWarn(string message)
        {
            if (Mod.log != null)
            {
                Mod.log.Warn(message);
            }
            else
            {
                Console.WriteLine("[WARN] " + message);
            }
        }
    }
}