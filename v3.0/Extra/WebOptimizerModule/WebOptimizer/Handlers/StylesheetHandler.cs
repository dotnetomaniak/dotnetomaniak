using System;
using System.IO;
using System.Web;

namespace WebOptimizer.Handlers
{
	/// <summary>
	/// Minifies CSS files by removing all comments and whitespace.
	/// </summary>
	public class StylesheetHandler : BaseHandler
	{
		/// <summary>
		/// Enables processing of HTTP Web requests by a custom HttpHandler that implements the <see cref="T:System.Web.IHttpHandler"/> interface.
		/// </summary>
		/// <param name="context">An <see cref="T:System.Web.HttpContext"/> object that provides references to the intrinsic server objects (for example, Request, Response, Session, and Server) used to service HTTP requests.</param>
		public override void ProcessRequest(HttpContext context)
		{
			string fileName = context.Request.AppRelativeCurrentExecutionFilePath;
			FileInfo file = new FileInfo(context.Server.MapPath(fileName));

			if (file.Exists && file.Extension.Equals(".css", StringComparison.OrdinalIgnoreCase))
			{
				WriteContent(context, file.FullName);
				SetHeaders(context, file);
			}
			else
			{
				context.Response.StatusCode = 404;
			}
		}

		/// <summary>
		/// This will make the browser and server keep the output
		/// in its cache and thereby improve performance.
		/// </summary>
		/// <param name="context">The current HTTP context.</param>
		/// <param name="file">The CSS file that was minified.</param>
		private static void SetHeaders(HttpContext context, FileInfo file)
		{
			context.Response.ContentType = "text/css";
			context.Response.Cache.SetCacheability(HttpCacheability.Public);
			context.Response.Cache.SetExpires(DateTime.Now.AddYears(1));
			context.Response.AddFileDependency(file.FullName);
			context.Response.Cache.SetValidUntilExpires(true);
		}

		/// <summary>
		/// Writes the content of the individual stylesheets to the response stream.
		/// </summary>
		/// <param name="context">The current HTTP context.</param>
		/// <param name="file">The CSS file to minify.</param>
		private static void WriteContent(HttpContext context, string file)
		{
			using (StreamReader reader = new StreamReader(file))
			{
				string body = reader.ReadToEnd();
				body = Processor.RemoveWhiteSpaceFromStylesheets(body);
				context.Response.Write(body);
			}
		}
	}
}