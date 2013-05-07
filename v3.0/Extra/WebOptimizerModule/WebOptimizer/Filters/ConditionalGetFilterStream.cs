#region Using

using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Web;
using WebOptimizer;

#endregion

namespace WebOptimizer.Filters
{
	/// <summary>
	/// A respons filter for handling conditional GETs using the Etag header.
	/// </summary>
	public class ConditionalGetFilterStream : BaseFilterStream
	{
		#region Private fields

		/// <summary>
		/// The HTTP context in which this filter should be applied.
		/// </summary>
		private readonly HttpContext context;

		/// <summary>
		/// The hash algorithm used to calculate the hash code of the response.
		/// </summary>
		private static readonly HashAlgorithm hashAlgorithm = new MD5CryptoServiceProvider();

		/// <summary>
		/// Used for mutexing when accessing the hashAlgorithm
		/// </summary>
		private static readonly object syncRoot = new object();

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="ConditionalGetFilterStream"/> class.
		/// </summary>
		/// <param name="context">The HTTP context.</param>
		public ConditionalGetFilterStream(HttpContext context)
		{
			this.Sink = context.Response.Filter;
			this.context = context;
		}

		#endregion

		/// <summary>
		/// Gets or sets a value indicating whether to ignore requests that are too large to be garbage collected efficiently.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if large response should be ignored; otherwise, <c>false</c>.
		/// </value>
		protected override bool IgnoreLargeResponse
		{
			get { return false; }
		}

		/// <summary>
		/// Process and manipulate the buffer.
		/// </summary>
		/// <param name="buffer">An array of bytes. This method copies <paramref name="count"/> bytes from <paramref name="buffer"/> to the current stream.</param>
		/// <param name="offset">The zero-based byte offset in <paramref name="buffer"/> at which to begin copying bytes to the current stream.</param>
		/// <param name="count">The number of bytes to be written to the current stream.</param>
		protected override void Process(byte[] buffer, int offset, int count)
		{
			if (this.context.Items["optimizer.conditionalget"] == null)
			{
				try
				{
					lock (syncRoot)
					{
						string etag = Convert.ToBase64String(hashAlgorithm.ComputeHash(buffer, offset, count));
						Processor.SetConditionalGetHeaders(etag, this.context);
						this.context.Items.Add("optimizer.conditionalget", 1);
					}
				}
				catch (HttpException)
				{
					// The response have been manually flushed and no headers can be added to the response.
				}
			}

			this.Sink.Write(buffer, offset, count);
		}
	}
}