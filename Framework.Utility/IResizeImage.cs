using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Framework.Utility
{
    public interface IResizeImage
    {
        byte[] ResizeImageByHeight(string filePath, int height);

        byte[] ResizeImageByWidth(string filePath, int width);

        void ResizeImageByHeightAndSave(string filePath, int height, string pathToSave);

        void ResizeImageByWidthAndSave(string filePath, int width, string pathToSave);
    }

    public class ResizeImageService : IResizeImage
    {
        public byte[] ResizeImageByHeight(string filePath, int height)
        {
            var img = new Bitmap(filePath);
            if (height == 0)
            {
                height = img.Height;
            }
            var scaleRate = (height * 1.0) / (img.Height * 1.0);
            var width = img.Width * scaleRate;
            var resizedImg = new Bitmap((int)width, height);
            var outStream = new MemoryStream();
            var graphic = Graphics.FromImage(resizedImg);
            graphic.Clear(Color.White);
            graphic.DrawImage(img, new Rectangle(0, 0, (int)width, height), new Rectangle(0, 0, img.Width, img.Height), GraphicsUnit.Pixel);
            resizedImg.Save(outStream, GetImageFormat(filePath));
            graphic.Dispose();
            resizedImg.Dispose();
            img.Dispose();
            var byteResult = outStream.ToArray();
            outStream.Dispose();
            return byteResult;
        }

        public void ResizeImageByHeightAndSave(string filePath, int height, string pathToSave)
        {
            var data = ResizeImageByHeight(filePath, height);
            if (File.Exists(pathToSave))
            {
                File.Delete(pathToSave);
            }
            File.WriteAllBytes(pathToSave, data);
        }

        public byte[] ResizeImageByWidth(string filePath, int width)
        {
            var img = new Bitmap(filePath);
            if (width == 0)
            {
                width = img.Width;
            }
            if (width > img.Width)
            {
                width = img.Width;
            }
            var scaleRate = (width * 1.0) / (img.Width * 1.0);
            var height = img.Height * scaleRate;
            var resizedImg = new Bitmap(width, (int)height);
            var outStream = new MemoryStream();
            var graphic = Graphics.FromImage(resizedImg);
            graphic.Clear(Color.White);
            graphic.DrawImage(img, new Rectangle(0, 0, width, (int)height), new Rectangle(0, 0, img.Width, img.Height), GraphicsUnit.Pixel);
            resizedImg.Save(outStream, GetImageFormat(filePath));
            graphic.Dispose();
            resizedImg.Dispose();
            img.Dispose();
            var byteResult = outStream.ToArray();
            outStream.Dispose();
            return byteResult;
        }

        public void ResizeImageByWidthAndSave(string filePath, int width, string pathToSave)
        {
            var data = ResizeImageByWidth(filePath, width);
            if (File.Exists(pathToSave))
            {
                File.Delete(pathToSave);
            }
            File.WriteAllBytes(pathToSave, data);
        }

        private ImageFormat GetImageFormat(string path)
        {
            switch (Path.GetExtension(path))
            {
                case ".bmp": return ImageFormat.Bmp;
                case ".gif": return ImageFormat.Gif;
                case ".jpg": return ImageFormat.Jpeg;
                case ".png": return ImageFormat.Png;
            }
            return ImageFormat.Jpeg;
        }
    }
}
