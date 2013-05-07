using System;
using System.Web;

namespace WebOptimizer
{
	/// <summary>
	/// Optimizes various different aspects of an ASP.NET website.
	/// </summary>
	/// <author>mads</author>
	public class Optimizer : WebOptimizer.IOptimizer
	{
		/// <summary>
		/// Writes the Last-Modified header and sets the conditional get headers.
		/// </summary>
		/// <param name="lastModified">The date of the last modification.</param>
		/// <param name="context">The HTTP context.</param>
		/// <author>mads</author>
		public void SetConditionalGetHeaders(DateTime lastModified, HttpContext context)
		{
			Processor.SetConditionalGetHeaders(lastModified, context);
		}

		/// <summary>
		/// Sets the Etag for the conditional get headers.
		/// </summary>
		/// <param name="etag">The etag to send to the response.</param>
		/// <param name="context">The HTTP context.</param>
		/// <author>mads</author>
		public void SetConditionalGetHeaders(string etag, HttpContext context)
		{
			Processor.SetConditionalGetHeaders(etag, context);
		}

		/// <summary>
		/// Removes whitespace from the specified string of HTML.
		/// </summary>
		/// <param name="html">The HTML string to remove white space from.</param>
		/// <returns>
		/// The specified HTML string stripped from all whitespace.
		/// </returns>
		/// <author>mads</author>
		public string RemoveWhitespaceFromHtml(string html)
		{
			return Processor.RemoveWhitespaceFromHtml(html);
		}

		/// <summary>
		/// Strips the whitespace from any .js file.
		/// </summary>
		/// <param name="body">The body text of which to remove white space from.</param>
		/// <returns>The specified body with no white space.</returns>
		/// <author>mads</author>
		public string RemoveWhiteSpaceFromJavaScript(string body)
		{
			return Processor.RemoveWhiteSpaceFromJavaScript(body);
		}

		/// <summary>
		/// Strips the whitespace from any .css file.
		/// </summary>
		/// <param name="body">The body/contents of the CSS file.</param>
		/// <returns>
		/// The body of the CSS file stripped from whitespace and comments.
		/// </returns>
		/// <author>mads</author>
		public string RemoveWhiteSpaceFromStylesheets(string body)
		{
			return Processor.RemoveWhiteSpaceFromStylesheets(body);
		}

		/// <summary>
		/// Compresses the HttpContext's output stream using either Deflate or GZip.
		/// </summary>
		/// <param name="context">The current HTTP context to compress.</param>
		/// <author>mads</author>
		public void Compress(HttpContext context)
		{
			Processor.Compress(context);
		}
	}
}
