#region Using

using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;

#endregion

namespace WebOptimizer.Handlers
{
	/// <summary>
	/// Handles combining and serving of multiple stylesheets.
	/// </summary>
	public class CombinedStylesheetHandler : BaseHandler
	{
		/// <summary>
		/// Regular expression used to find relative url references in the stylesheet.
		/// </summary>
		private static Regex urlFinder = new Regex(@"url\('?""?([^\)]+)'?""?\)", RegexOptions.Compiled | RegexOptions.IgnoreCase);

		/// <summary>
		/// Enables processing of HTTP Web requests by a custom HttpHandler that implements the <see cref="T:System.Web.IHttpHandler"/> interface.
		/// </summary>
		/// <param name="context">An <see cref="T:System.Web.HttpContext"/> object that provides references to the intrinsic server objects (for example, Request, Response, Session, and Server) used to service HTTP requests.</param>
		public override void ProcessRequest(HttpContext context)
		{
			if (!string.IsNullOrEmpty(context.Request.QueryString["path"]))
			{
				string[] relativeFiles = context.Request.QueryString["path"].Split(',');
				string[] absoluteFiles = new string[relativeFiles.Length];

				for (int i = 0; i < relativeFiles.Length; i++)
				{
					string file = relativeFiles[i];
					FileInfo absoluteFile = new FileInfo(context.Server.MapPath(file));

					if (absoluteFile.Extension.Equals(".css", StringComparison.OrdinalIgnoreCase))
					{
						WriteContent(context, absoluteFile.FullName, file);
						absoluteFiles[i] = absoluteFile.FullName;
					}
				}

				SetHeaders(context, absoluteFiles);
			}
		}

		/// <summary>
		/// Writes the content of the individual stylesheets to the response stream.
		/// </summary>
		/// <param name="context">The HTTP context.</param>
		/// <param name="file">The fully qualified path of the file on disk.</param>
		/// <param name="relative">The relative path of the file.</param>
		private static void WriteContent(HttpContext context, string file, string relative)
		{
			using (StreamReader reader = new StreamReader(file))
			{
				string body = reader.ReadToEnd();
				body = Processor.RemoveWhiteSpaceFromStylesheets(body);
				body = ConvertRelativeUrlReferences(relative, body);

				context.Response.Write(body);
			}
		}

		/// <summary>
		/// Converts relative URL's into absolute ones.
		/// </summary>
		/// <remarks>
		/// The reason for doing it is to ensure that image references will still work.
		/// </remarks>
		/// <param name="relative">The relative url of the file.</param>
		/// <param name="body">The body of the CSS text.</param>
		/// <returns>A css string with absolute url references.</returns>
		private static string ConvertRelativeUrlReferences(string relative, string body)
		{
			foreach (Match m in urlFinder.Matches(body))
			{
				string relativeUrl = m.Groups[1].Value;

				if (!relativeUrl.StartsWith("/", StringComparison.Ordinal))
				{
					string folder = relative.Substring(0, relative.LastIndexOf("/", StringComparison.Ordinal) + 1);
					string absoluteUrl = folder + relativeUrl;
					body = body.Replace(relativeUrl, absoluteUrl);
				}
			}

			return body;
		}

		/// <summary>
		/// This will make the browser and server keep the output
		/// in its cache and thereby improve performance.
		/// </summary>
		/// <param name="context">The HTTP context.</param>
		/// <param name="files">A list of the files ot add cache dependencies to.</param>
		private static void SetHeaders(HttpContext context, string[] files)
		{
			context.Response.ContentType = "text/css";
			context.Response.Cache.SetCacheability(HttpCacheability.Public);
			context.Response.Cache.SetExpires(DateTime.Now.AddYears(1));
			context.Response.AddFileDependencies(files);
			context.Response.Cache.VaryByParams["path"] = true;
			context.Response.Cache.SetValidUntilExpires(true);
		}
	}
}