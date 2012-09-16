namespace Kigg.Web
{
    using System;
    using System.Collections.Specialized;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Imaging;
    using System.Drawing.Text;
    using System.IO;
    using System.Web;

    using DomainObjects;
    using Infrastructure;
    using Repository;

    public class ImageHandler : BaseHandler
    {
        public ImageHandler()
        {
            IoC.Inject(this);
        }

        public int Width
        {
            get;
            set;
        }

        public int Height
        {
            get;
            set;
        }

        public int BorderWidth
        {
            get;
            set;
        }

        public string FontName
        {
            get;
            set;
        }

        public int FontSize
        {
            get;
            set;
        }

        public float NewStoryCacheDurationInMinutes
        {
            get;
            set;
        }

        public float ExpiredStoryCacheDurationInMinutes
        {
            get;
            set;
        }

        public DefaultColors Colors
        {
            get;
            set;
        }

        public IConfigurationSettings Settings
        {
            get;
            set;
        }

        public IStoryRepository StoryRepository
        {
            get;
            set;
        }


        public override void ProcessRequest(HttpContextBase context)
        {
            const int CountWidthBuffer = 6;

            HttpRequestBase request = context.Request;
            string url = request.QueryString["url"];

            Color borderColor = GetColor(request.QueryString, "borderColor", Colors.BorderColor);
            Color textBackColor = GetColor(request.QueryString, "textBackColor", Colors.TextBackColor);
            Color textForeColor = GetColor(request.QueryString, "textForeColor", Colors.TextForeColor);
            Color countBackColor = GetColor(request.QueryString, "countBackColor", Colors.CountBackColor);
            Color countForeColor = GetColor(request.QueryString, "countForeColor", Colors.CountForeColor);

            int width = GetInteger(request.QueryString, "width", Width);
            int height = GetInteger(request.QueryString, "height", Height);
            int borderWidth = GetInteger(request.QueryString, "borderWidth", BorderWidth);
            string fontName = string.IsNullOrEmpty(request.QueryString["fontName"]) ? FontName : request.QueryString["fontName"];
            int fontSize = GetInteger(request.QueryString, "fontSize", FontSize);

            HttpResponseBase response = context.Response;
            DateTime storyLastActivityAt = SystemTime.Now();

            using (MemoryStream ms = new MemoryStream())
            {
                using (Image image = new Bitmap(width, height, PixelFormat.Format32bppArgb))
                {
                    using (Graphics gdi = Graphics.FromImage(image))
                    {
                        gdi.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;
                        gdi.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        gdi.SmoothingMode = SmoothingMode.HighSpeed;
                        gdi.CompositingQuality = CompositingQuality.HighSpeed;

                        using (Brush borderBrush = new SolidBrush(borderColor))
                        {
                            gdi.FillRectangle(borderBrush, 0, 0, image.Width, image.Height);
                        }

                        using (Brush textBackgroundBrush = new SolidBrush(textBackColor))
                        {
                            gdi.FillRectangle(textBackgroundBrush, borderWidth, borderWidth, (image.Width - (borderWidth * 2)), (image.Height - (borderWidth * 2)));
                        }

                        int count = 0;

                        if (url.IsWebUrl())
                        {
                            IStory story = StoryRepository.FindByUrl(url);

                            if (story != null)
                            {
                                count = story.VoteCount;
                                storyLastActivityAt = story.LastActivityAt;
                            }
                        }

                        using (Font font = new Font(fontName, fontSize, FontStyle.Bold, GraphicsUnit.Pixel))
                        {
                            SizeF countSize = gdi.MeasureString(count.ToString(Constants.CurrentCulture), font);

                            float textWidth = (image.Width - (countSize.Width + CountWidthBuffer + (borderWidth * 2)));

                            using (Brush textForegroundBrush = new SolidBrush(textForeColor))
                            {
                                SizeF textSize = gdi.MeasureString(Settings.PromoteText, font);

                                float x = (((textWidth - textSize.Width) / 2) + borderWidth);
                                float y = ((image.Height - textSize.Height) / 2);

                                gdi.DrawString(Settings.PromoteText, font, textForegroundBrush, x, y);
                            }

                            using (Brush countBackgroundBrush = new SolidBrush(countBackColor))
                            {
                                gdi.FillRectangle(countBackgroundBrush, (textWidth + borderWidth), borderWidth, (countSize.Width + CountWidthBuffer), (image.Height - (borderWidth * 2)));
                            }

                            using (Brush countForegroundBrush = new SolidBrush(countForeColor))
                            {
                                float x = ((((countSize.Width + CountWidthBuffer) - countSize.Width) / 2) + borderWidth + textWidth);
                                float y = ((image.Height - countSize.Height) / 2);

                                gdi.DrawString(count.ToString(Constants.CurrentCulture), font, countForegroundBrush, x, y);
                            }
                        }
                    }

                    image.Save(ms, ImageFormat.Png);
                }

                ms.WriteTo(response.OutputStream);
                response.ContentType = "image/PNG";
            }

            bool doNotCache;

            if (!bool.TryParse(request.QueryString["noCache"], out doNotCache))
            {
                doNotCache = false;
            }

            if (doNotCache)
            {
                context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            }
            else
            {
                float durationInMinutes = ((SystemTime.Now() - storyLastActivityAt).TotalDays > Settings.MaximumAgeOfStoryInHoursToPublish) ? ExpiredStoryCacheDurationInMinutes : NewStoryCacheDurationInMinutes;

                if (durationInMinutes > 0)
                {
                    context.CacheResponseFor(TimeSpan.FromMinutes(durationInMinutes));
                }
            }
        }
    }
}