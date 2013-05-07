#region Using

using System.Text;
using System.Web;

#endregion

namespace WebOptimizer.Filters
{
	/// <summary>
	/// A respons filter for handling conditional GETs using the Etag header.
	/// </summary>
	public class RemoveWhitespaceFilterStream : BaseFilterStream
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="RemoveWhitespaceFilterStream"/> class.
		/// </summary>
		/// <param name="context">The HTTP context.</param>
		public RemoveWhitespaceFilterStream(HttpContext context)
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
			html = Processor.RemoveWhitespaceFromHtml(html);

			byte[] outdata = System.Text.Encoding.Default.GetBytes(html);
			this.Sink.Write(outdata, 0, outdata.GetLength(0));
		}
	}
}