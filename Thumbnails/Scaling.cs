using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thumbnails
{
    public static class Scaling
    {
        public static (int x, int y, int w, int h) Cover(int targetWidth, int targetHeight, int imageWidth, int imageHeight)
        {
            var sourceRatio = (double)imageWidth / imageHeight;
            var targetRatio = (double)targetWidth / targetHeight;

            if (sourceRatio > targetRatio)
            {
                var resultHeight = targetHeight;
                var resultWidth = (int)(imageWidth * ((double)targetHeight / imageHeight));
                var x = (int)((resultWidth - targetWidth) / 2.0);
                var y = 0;

                return (x, y, resultWidth, resultHeight);
            }
            else
            {
                var resultHeight = (int)(imageHeight * ((double)targetWidth / imageWidth));
                var resultWidth = targetWidth;
                var x = 0;
                var y = (int)((resultHeight - targetHeight) / 2.0);

                return (x, y, resultWidth, resultHeight);
            }
            
        }

        public static (int x, int y, int w, int h) MaxWidth(int targetWidth, int imageWidth, int imageHeight)
        { 
            var ratio = (double)imageHeight / imageWidth;
            var targetHeight = (int)(targetWidth * ratio);
            return (0, 0, targetWidth, targetHeight);
        }

    }
}
