using System;
using System.Web;

namespace WebOptimizer
{
	/// <summary>
	/// An interface for the optimization methods.
	/// </summary>
	/// <author>mads</author>
	public interface IOptimizer
	{
		/// <summary>
		/// Compresses the HttpContext's output stream using either Deflate or GZip.
		/// </summary>
		/// <param name="context">The current HTTP context to compress.</param>
		void Compress(HttpContext context);

		/// <summary>
		/// Removes whitespace from the specified string of HTML.
		/// </summary>
		/// <param name="html">The HTML string to remove white space from.</param>
		/// <returns>The specified HTML string stripped from all whitespace.</returns>
		string RemoveWhitespaceFromHtml(string html);

		/// <summary>
		/// Strips the whitespace from any .js file.
		/// </summary>
		/// <param name="body">The body text of which to remove white space from.</param>
		/// <returns>The specified body with no white space.</returns>
		string RemoveWhiteSpaceFromJavaScript(string body);

		/// <summary>
		/// Strips the whitespace from any .css file.
		/// </summary>
		/// <param name="body">The body/contents of the CSS file.</param>
		/// <returns>The body of the CSS file stripped from whitespace and comments.</returns>
		string RemoveWhiteSpaceFromStylesheets(string body);

		/// <summary>
		/// Writes the Last-Modified header and sets the conditional get headers.
		/// </summary>
		/// <param name="lastModified">The date of the last modification.</param>
		/// <param name="context">The HTTP context.</param>
		void SetConditionalGetHeaders(DateTime lastModified, HttpContext context);

		/// <summary>
		/// Sets the Etag for the conditional get headers.
		/// </summary>
		/// <param name="etag">The etag to send to the response.</param>
		/// <param name="context">The HTTP context.</param>
		void SetConditionalGetHeaders(string etag, HttpContext context);
	}
}
