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

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageServiceModal"/> class.
        /// </summary>
        /// <param name="outputFolder">The output folder.</param>
        /// <param name="thumbnailSize">Size of the thumbnail.</param>
        public ImageServiceModal(string outputFolder, int thumbnailSize)
        {
            this.m_outputFolder = outputFolder;
            this.m_thumbnailSize = thumbnailSize;

            // create the output folder and make it hidden
            Directory.CreateDirectory(outputFolder);
            Directory.CreateDirectory(outputFolder + "\\Thumbnails");
            new FileInfo(outputFolder).Attributes |= FileAttributes.Hidden;
        }

        /// <summary>
        /// The Function Addes A file to the backup folder
        /// </summary>
        /// <param name="path">The Path of the Image</param>
        /// <param name="result">The result of the command</param>
        /// <returns>
        /// the path to the new file if the command was successful, error otherwise.
        /// </returns>
        public string AddFile(string path, out bool result)
        {
            try
            {
                // get the time the picture was taken
                DateTime creationTime = GetCreationTime(path);
                string targetDirectory = m_outputFolder + "\\" + creationTime.Year.ToString("D4") + "\\" +
                    creationTime.Month.ToString("D2");
                string fileName = creationTime.Day.ToString("D2") + "_" + creationTime.ToString("HH-mm-ss")
                    + " " + Path.GetFileName(path);
                string targetPath = Path.Combine(targetDirectory, fileName);

                for (int i = 1; File.Exists(targetPath); i++)
                {
                    fileName = creationTime.Day.ToString("D2") + "_" + creationTime.ToString("HH-mm-ss")
                        + " " + Path.GetFileNameWithoutExtension(path) + " (" + i + ")" + Path.GetExtension(path);
                    targetPath = Path.Combine(targetDirectory, fileName);
                }

                // create the directory for the image
                Directory.CreateDirectory(targetDirectory);

                // creating thumbnail
                using (Image img = Image.FromFile(path))
                {
                    Size thumbnailSize = GetThumbnailSize(img);
                    Image thumbnail = img.GetThumbnailImage(thumbnailSize.Width, thumbnailSize.Height, null, IntPtr.Zero);
                    Directory.CreateDirectory(targetDirectory.Replace(m_outputFolder, m_outputFolder + "\\Thumbnails"));
                    thumbnail.Save(targetPath.Replace(m_outputFolder, m_outputFolder + "\\Thumbnails"));
                }

                File.Move(path, targetPath);

                result = true;
                return targetPath;
            }
            catch (Exception e)
            {
                result = false;
                return e.Message;
            }
        }

        /// <summary>
        /// Gets the creation time of the photo in the path
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns> the creation time of the photo </returns>
        private static DateTime GetCreationTime(string path)
        {
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

            return creationTime;
        }

        /// <summary>
        /// Gets the size of the thumbnail.
        /// </summary>
        /// <param name="img">The img.</param>
        /// <returns> the size of the thumbnail </returns>
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
