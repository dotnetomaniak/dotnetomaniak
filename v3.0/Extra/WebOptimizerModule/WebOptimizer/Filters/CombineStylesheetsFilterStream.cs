#region Using

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

#endregion

namespace WebOptimizer.Filters
{
	/// <summary>
	/// A respons filter for handling conditional GETs using the Etag header.
	/// </summary>
	public class CombineStylesheetsFilterStream : BaseFilterStream
	{
		#region Private fields

		/// <summary>
		/// The link tag to inject into the page header.
		/// </summary>
		private const string LinkTag = "<link type=\"text/css\" rel=\"stylesheet\" href=\"/style.css?path={0}\" />";

		/// <summary>
		/// A regular expression for finding stylesheet references.
		/// </summary>
		private static Regex regex = new Regex("<link\\s[^>]*(href=\"([^\":]*.css)\"[^>]*)>", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Singleline);

		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="CombineStylesheetsFilterStream"/> class.
		/// </summary>
		/// <param name="context">The HTTP context.</param>
		public CombineStylesheetsFilterStream(HttpContext context)
		{
			this.Sink = context.Response.Filter;
		}

		/// <summary>
		/// Gets or sets a value indicating whether to ignore requests that are too large to be garbage collected efficiently.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if large response should be ignored; otherwise, <c>false</c>.
		/// </value>
		protected override bool IgnoreLargeResponse
		{
			get { return true; }
		}

		/// <summary>
		/// Process and manipulate the buffer.
		/// </summary>
		/// <param name="buffer">An array of bytes. This method copies <paramref name="count"/> bytes from <paramref name="buffer"/> to the current stream.</param>
		/// <param name="offset">The zero-based byte offset in <paramref name="buffer"/> at which to begin copying bytes to the current stream.</param>
		/// <param name="count">The number of bytes to be written to the current stream.</param>
		protected override void Process(byte[] buffer, int offset, int count)
		{
			string html = Encoding.Default.GetString(buffer, offset, count);
			int index = 0;
			List<string> list = new List<string>();
			string file = HttpContext.Current.Request.CurrentExecutionFilePath;
			file = file.Substring(0, file.LastIndexOf("/", StringComparison.Ordinal) + 1);
			var matches = regex.Matches(html);
			foreach (Match match in matches)
			{
				if (index == 0)
				{
					index = html.IndexOf(match.Value, StringComparison.Ordinal);
				}

				if (match.Groups[2].Value.StartsWith("/", StringComparison.Ordinal))
				{
					file = string.Empty;
				}

				list.Add(file + match.Groups[2].Value);
				html = html.Replace(match.Value, string.Empty);
			}

			if (list.Count > 0)
			{
				string parameters = string.Empty;

				foreach (string s in list)
				{
					parameters += HttpUtility.UrlEncode(s) + ",";
				}

				string path = string.Format(CultureInfo.InvariantCulture, LinkTag, parameters.Substring(0, parameters.Length - 1));
				html = html.Insert(index, path);
			}

			byte[] outdata = System.Text.Encoding.Default.GetBytes(html);
			this.Sink.Write(outdata, 0, outdata.GetLength(0));
		}
	}
}