namespace Kigg.Web
{
    using System.Security.Permissions;
    using System.Text;
    using System.Web;
    using System.Web.UI;

    using Infrastructure;

    //[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    //[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [ToolboxData("<{0}:reCAPTCHA runat=\"server\"></{0}:reCAPTCHA>")]
    public class reCAPTCHA : Control
    {
        private readonly reCAPTCHAValidator _validator;

        public reCAPTCHA() : this(IoC.Resolve<reCAPTCHAValidator>())
        {
        }

        public reCAPTCHA(reCAPTCHAValidator validator)
        {
            Check.Argument.IsNotNull(validator, "validator");

            _validator = validator;

            Theme = "white";
        }

        public string Theme
        {
            get;
            set;
        }

        protected override void Render(HtmlTextWriter writer)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Type, "text/javascript");
            writer.RenderBeginTag(HtmlTextWriterTag.Script);
                writer.WriteLine("var RecaptchaOptions = {");
                writer.WriteLine("theme : '{0}'".FormatWith(Theme ?? string.Empty));
                writer.WriteLine("};");
            writer.RenderEndTag();

            // <script> display
            writer.AddAttribute(HtmlTextWriterAttribute.Type, "text/javascript");
            writer.AddAttribute(HtmlTextWriterAttribute.Src, GenerateChallengeUrl(false), false);
            writer.RenderBeginTag(HtmlTextWriterTag.Script);
            writer.RenderEndTag();

            writer.RenderBeginTag(HtmlTextWriterTag.Noscript);

                writer.AddAttribute(HtmlTextWriterAttribute.Src, GenerateChallengeUrl(true), false);
                writer.AddAttribute(HtmlTextWriterAttribute.Width, "500");
                writer.AddAttribute(HtmlTextWriterAttribute.Height, "300");
                writer.AddAttribute("frameborder", "0");
                writer.RenderBeginTag(HtmlTextWriterTag.Iframe);
                writer.RenderEndTag();

                writer.RenderBeginTag(HtmlTextWriterTag.Br);
                writer.RenderEndTag();

                writer.AddAttribute(HtmlTextWriterAttribute.Name, _validator.ChallengeInputName);
                writer.AddAttribute(HtmlTextWriterAttribute.Rows, "3");
                writer.AddAttribute(HtmlTextWriterAttribute.Cols, "40");
                writer.RenderBeginTag(HtmlTextWriterTag.Textarea);
                writer.RenderEndTag();

                writer.AddAttribute(HtmlTextWriterAttribute.Name, _validator.ResponseInputName);
                writer.AddAttribute(HtmlTextWriterAttribute.Value, "manual_challenge");
                writer.AddAttribute(HtmlTextWriterAttribute.Type, "hidden");
                writer.RenderBeginTag(HtmlTextWriterTag.Input);
                writer.RenderEndTag();

            writer.RenderEndTag();

            base.Render(writer);
        }

        private string GenerateChallengeUrl(bool noScript)
        {
            StringBuilder urlBuilder = new StringBuilder();

            urlBuilder.Append((Context != null) && Context.Request.IsSecureConnection ? _validator.SecureHost : _validator.InsecureHost);

            urlBuilder.Append(noScript ? "/noscript?" : "/challenge?");
            urlBuilder.AppendFormat("k={0}", _validator.PublicKey);

            return urlBuilder.ToString();
        }
    }
}