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
        public static int Width { get; set; } = 500;
        public static int Height { get; set; } = 400;

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
                //path = Rename(path);
                Resize(path);
            }
            catch (Exception ex)
            {

            }
        }

        private static string Rename(string path)
        {
            var dir = Path.GetDirectoryName(path);
            var ext = path[^8..];
            var name = "tana-a-honza-wedding-";
            var newPath = Path.Combine(dir, name + ext);
            File.Move(path, newPath);
            return newPath;
        }

        private static void Resize(string path)
        {
            try
            {
                using var sourceStream = File.OpenRead(path);
                using var skiaStream = new SKManagedStream(sourceStream, false);
                using var sourceImage = SKImage.FromEncodedData(skiaStream);
                using var sourceBitmap = SKBitmap.FromImage(sourceImage);


                var sourceRatio = (double)sourceBitmap.Width / sourceBitmap.Height;
                var targetRatio = (double)Width / Height;

                (int x, int y, int targetWidth, int targetHeight) =
                    Scaling.MaxWidth(Width, sourceBitmap.Width, sourceBitmap.Height);

                using var scaledBitmap = sourceBitmap.Resize(new SKImageInfo(targetWidth, targetHeight), SKFilterQuality.High);
                using var scaledImage = SKImage.FromBitmap(scaledBitmap);
                using var croppedImage = scaledImage.Subset(new SKRectI(x, y, targetWidth + x, targetHeight + y));
                using var data = croppedImage.Encode(SKEncodedImageFormat.Jpeg, 90);

                var outputPath =
                    Path.Combine(
                        Path.GetDirectoryName(path),
                        "thumbnails",
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
