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
	public class MinifyInlineScriptsFilterStream : BaseFilterStream
	{
		/// <summary>
		/// A regular expression for finding scripts inlined in HTML.
		/// </summary>
		private static Regex regex = new Regex("<script[^>]*>([\\S\\s]*?)</script>", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Singleline);
	
		/// <summary>
		/// Initializes a new instance of the <see cref="MinifyInlineScriptsFilterStream"/> class.
		/// </summary>
		/// <param name="context">The HTTP context.</param>
		public MinifyInlineScriptsFilterStream(HttpContext context)
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

			var matches = regex.Matches(html);
			foreach (Match match in matches)
			{
				string style = match.Groups[1].Value;
				if (!string.IsNullOrEmpty(style))
				{
					html = html.Replace(style, Processor.RemoveWhiteSpaceFromJavaScript(style));
				}
			}

			byte[] outdata = System.Text.Encoding.Default.GetBytes(html);
			this.Sink.Write(outdata, 0, outdata.GetLength(0));
		}
	}
}