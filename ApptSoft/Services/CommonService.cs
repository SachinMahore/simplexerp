using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;

namespace ApptSoft.Services
{
    public class CommonService
    {

        public byte[] CompressImageTo400Kb(Stream inputStream, int targetKb)
        {
            using (var loadedImage = Image.FromStream(inputStream))
            {
                // Resize logic (optional)
                int width = loadedImage.Width;
                int height = loadedImage.Height;

                int maxWidth = 1200;
                int newWidth = width;
                int newHeight = height;

                if (width > maxWidth)
                {
                    newWidth = maxWidth;
                    newHeight = (int)((float)height * newWidth / width);
                }

                using (var resizedImage = new Bitmap(loadedImage, new Size(newWidth, newHeight)))
                {
                    var jpegEncoder = ImageCodecInfo.GetImageDecoders()
                                        .First(c => c.FormatID == ImageFormat.Jpeg.Guid);

                    var encoderParams = new EncoderParameters(1);
                    MemoryStream ms = new MemoryStream();
                    long quality = 90;

                    while (quality >= 10)
                    {
                        ms.SetLength(0);
                        encoderParams.Param[0] = new EncoderParameter(Encoder.Quality, quality);
                        resizedImage.Save(ms, jpegEncoder, encoderParams);

                        if (ms.Length <= targetKb * 1024)
                            break;

                        quality -= 5;
                    }

                    return ms.ToArray();
                }
            }
        }


    }
}