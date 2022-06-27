using SkiaSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Thumbnails
{
    public static class ThumbnailService
    {
        public static int Width { get; set; } = 300;
        public static int Height { get; set; } = 200;

        public static void ProcessStorageItems(IReadOnlyList<IStorageItem> files)
        {
            foreach (var file in files)
                ProcessStorageItem(file);
        }

        private static void ProcessStorageItem(IStorageItem file)
        {
            if (file.IsOfType(StorageItemTypes.Folder))
                foreach (var item in Directory.GetFiles(file.Path))
                    ProcessFile(item);
            else ProcessFile(file.Path);
        }

        private static void ProcessFile(string path)
        {
            try
            {
                using var sourceStream = File.OpenRead(path);
                //using var sourceBitmap = SKBitmap.Decode(sourceStream);
                using var skiaStream = new SKManagedStream(sourceStream, false);
                using var sourceImage = SKImage.FromEncodedData(skiaStream);
                using var sourceBitmap = SKBitmap.FromImage(sourceImage);


                var sourceRatio = (double)sourceBitmap.Width / sourceBitmap.Height;
                var targetRatio = (double)Width / Height;

                int targetHeight;
                int targetWidth;
                int x;
                int y;

                if (sourceRatio > targetRatio)
                {
                    targetHeight = Height;
                    targetWidth = (int)(sourceBitmap.Width * ((double)Height / sourceBitmap.Height));
                    x = (int)((targetWidth - Width) / 2.0);
                    y = 0;
                }
                else
                {
                    targetHeight = (int)(sourceBitmap.Height * ((double)Width / sourceBitmap.Width));
                    targetWidth = Width;
                    x = 0;
                    y = (int)((targetHeight - Height) / 2.0);
                }

                using var scaledBitmap = sourceBitmap.Resize(new SKImageInfo(targetWidth, targetHeight), SKFilterQuality.High);
                using var scaledImage = SKImage.FromBitmap(scaledBitmap);
                using var croppedImage = scaledImage.Subset(new SKRectI(x, y, Width + x, Height + y));
                using SKData data = croppedImage.Encode();

                var outputPath =
                    Path.Combine(
                        Path.GetDirectoryName(path),
                        Path.GetFileNameWithoutExtension(path) + "_thumb.jpg");
                using var outputStream = File.OpenWrite(outputPath);
                data.SaveTo(outputStream);
            }
            catch (Exception ex)
            {

            }
        }
    }
}
