using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Web;
using Kigg.Repository;
using Kigg.Infrastructure;

namespace Kigg.Web
{
    public class UserHandler : BaseHandler
    {
        public UserHandler()
        {
            IoC.Inject(this);
        }

        public IUserRepository UserRepository
        {
            get;
            set;
        }

        public DefaultColors Colors
        {
            get;
            set;
        }

        public override void ProcessRequest(HttpContextBase context)
        {
            var request = context.Request;
            var userName = request.Params["userName"];
            if (string.IsNullOrEmpty(userName))
                return;
            var user = UserRepository.FindByUserName(userName);
            if (user == null)
                return;

            Color borderColor = GetColor(request.QueryString, "borderColor", Colors.BorderColor);
            Color textBackColor = GetColor(request.QueryString, "textBackColor", Colors.TextBackColor);
            Color textForeColor = GetColor(request.QueryString, "textForeColor", Colors.TextForeColor);
            const int sideMargin = 15;         

            var writeString = string.Format("Punkty: {0}", user.CurrentScore);
            using (MemoryStream ms = new MemoryStream())
            {
                using (Image image = new Bitmap(context.Server.MapPath("~/Assets/Images/badge2.png")))
                {
                    using (Graphics gdi = Graphics.FromImage(image))
                    {
                        gdi.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;
                        gdi.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        gdi.SmoothingMode = SmoothingMode.HighSpeed;
                        gdi.CompositingQuality = CompositingQuality.HighSpeed;

                        using (Font f = new Font(new FontFamily("Verdana"), 3.1f, FontStyle.Bold))
                        {
                            var measureString = gdi.MeasureString(writeString, f, image.Width);
                            var stringWidth = measureString.Width;
                            var imageWidth = image.Width;
                            var positionX = (imageWidth - stringWidth)/2;
                            var stringHeight = measureString.Height;
                            var imageHeight = image.Height;
                            var positionY = imageHeight - stringHeight;

                            using (Brush borderBrush = new SolidBrush(borderColor))
                            {
                                gdi.FillRectangle(borderBrush, sideMargin, positionY, imageWidth - 2*sideMargin, stringHeight);
                            }

                            const int borderWidth = 2;
                            using (Brush textBacgroundBrush = new SolidBrush(textBackColor))
                            {
                                gdi.FillRectangle(textBacgroundBrush, sideMargin + borderWidth, positionY + borderWidth,
                                                  (imageWidth - (sideMargin + borderWidth)*2),
                                                  (stringHeight - (borderWidth*2)));
                            }

                            using (Brush b = new SolidBrush(textForeColor))
                            {
                                gdi.DrawString(writeString, f, b,
                                               positionX, positionY );
                            }
                        }
                    }
                    image.Save(ms, ImageFormat.Png);
                }
                ms.WriteTo(context.Response.OutputStream);
            }
            context.Response.ContentType = "image/png";
            //context.Response.WriteFile("~/Assets/Images/badge2.png");
        }
    }
}
