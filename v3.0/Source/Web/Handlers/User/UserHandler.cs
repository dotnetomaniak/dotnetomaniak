namespace Kigg.Web
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Imaging;
    using System.Drawing.Text;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Web;
    using DomainObjects;
    using Repository;
    using Infrastructure;
    using Jobs;

    public class UserHandler : BaseHandler
    {
        public UserHandler()
        {
            IoC.Inject(this);
        }

        public IAchievementRepository AchievementRepository { get; set; }

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

        public static Color GetColor(NameValueCollection queryString, string key, string defaultValue)
        {
            string hexValue = string.IsNullOrEmpty(queryString[key]) ? defaultValue : queryString[key];

            if (!hexValue.StartsWith("#", StringComparison.OrdinalIgnoreCase))
            {
                hexValue = "#" + hexValue;
            }

            try
            {
                return ColorTranslator.FromHtml(hexValue);
            }
            catch
            {
                return ColorTranslator.FromHtml(defaultValue);
            }
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

            var plaqueBadge = new PlaqueBadge();
            if (user.Achievements.Result.Any(x => x.Achievement.Id == plaqueBadge.Id) == false)
                AchievementRepository.Award(plaqueBadge.Id, new List<IUser> {user}.AsQueryable());

            Color borderColor = GetColor(request.QueryString, "borderColor", Colors.BorderColor);
            Color textBackColor = GetColor(request.QueryString, "textBackColor", Colors.TextBackColor);
            Color textForeColor = GetColor(request.QueryString, "textForeColor", Colors.TextForeColor);
            const int sideMargin = 15;

            var pktString = string.Format("• Punkty: {0}", user.CurrentScore);
            var badgesString = string.Format("• Odznaki: {0}", user.Achievements.Total);
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

                        using (Font f = new Font(new FontFamily("Consolas"), 12f, FontStyle.Bold))
                        {
                            var measureString = gdi.MeasureString(pktString, f, image.Width);
                            var positionX = 20;
                            var imageHeight = image.Height;
                            var positionY = imageHeight - 48f - 5;

                            try
                            {
                                string gravatarImage = user.GravatarUrl(48);

                                HttpWebRequest gravatarRequest = (HttpWebRequest)WebRequest.Create(gravatarImage);
                                using (var responseStream = gravatarRequest.GetResponse().GetResponseStream())
                                {
                                    Image userPicture = Image.FromStream(responseStream);
                                    gdi.DrawImage(userPicture, 10, positionY);
                                    positionX += 48;
                                }
                            }
                            catch (Exception)
                            {
                                //do nothing - we will draw only text                                
                            }

                            // text a bit higher then the picture
                            //positionY -= 5;
                            using (Brush b = new SolidBrush(textForeColor))
                            {
                                gdi.DrawString(pktString, f, b,
                                               positionX, positionY );
                                positionY += measureString.Height;
                                gdi.DrawString(badgesString, f, b, positionX, positionY);
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
