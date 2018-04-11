using ImageService.Infrastructure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;


namespace ImageService.Modal
{

    public class ImageServiceModal : IImageServiceModal
    {
        #region Members
        private const int PICTURE_TAKING_TIME_PROP = 36867;
        private string m_outputFolder;            // The Output Folder
        private int m_thumbnailSize;              // The Size Of The Thumbnail Size
        #endregion

        public ImageServiceModal(string outputFolder, int thumbnailSize)
        {
            this.m_outputFolder = outputFolder;
            this.m_thumbnailSize = thumbnailSize;

            // create the output folder and make it hidden
            Directory.CreateDirectory(outputFolder);
            Directory.CreateDirectory(outputFolder + "\\Thumbnails");
            new FileInfo(outputFolder).Attributes |= FileAttributes.Hidden;
        }

        public string AddFile(string path, out bool result)
        {
            try
            {
                string targetDirectory;
                string targetPath;

                // get the time the picture was taken
                DateTime creationTime;
                try
                {
                    using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                    using (Image myImage = Image.FromStream(fs, false, false))
                    {
                        PropertyItem propItem = myImage.GetPropertyItem(PICTURE_TAKING_TIME_PROP);
                        /////////////////////////////////////////////////////////////////// CHECK REGEX
                        Regex r = new Regex(":");
                        string dateTaken = r.Replace(Encoding.UTF8.GetString(propItem.Value), "-", 2);
                        creationTime = DateTime.Parse(dateTaken);
                    }
                }
                catch
                {
                    creationTime = new FileInfo(path).CreationTime;
                }

                // create the directory for the image
                targetDirectory = m_outputFolder + "\\" + creationTime.Year.ToString("D4") + "\\" +
                    creationTime.Month.ToString("D2");
                Directory.CreateDirectory(targetDirectory);

                targetPath = targetDirectory + "\\" + creationTime.Day.ToString("D2") + "_" +
                    creationTime.ToString("HH-mm-ss") + " " + Path.GetFileName(path);
                File.Copy(path, targetPath);

                // creating thumbnail
                using (Image img = Image.FromFile(path))
                {
                    Size thumbnailSize = GetThumbnailSize(img);
                    Image thumbnail = img.GetThumbnailImage(thumbnailSize.Width, thumbnailSize.Height, null, IntPtr.Zero);
                    Directory.CreateDirectory(targetDirectory.Replace(m_outputFolder, m_outputFolder + "\\Thumbnails"));
                    thumbnail.Save(targetPath.Replace(m_outputFolder, m_outputFolder + "\\Thumbnails"));
                }

                result = true;
                return targetPath;
            }
            catch (Exception e)
            {
                result = false;
                return e.Message;
            }
        }

        private Size GetThumbnailSize(Image img)
        {
            // the factor of which the size of the img is going to change
            double factor;

            // determine the factor base on the larger dimension
            if (img.Width > img.Height)
            {
                factor = m_thumbnailSize / img.Width;
            }
            else
            {
                factor = m_thumbnailSize / img.Height;
            }

            return new Size((int)(img.Width * factor), (int)(img.Height * factor));
        }
    }
}
