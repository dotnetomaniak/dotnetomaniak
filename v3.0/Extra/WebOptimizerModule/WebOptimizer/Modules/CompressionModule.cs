#region Using

using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;

#endregion

namespace WebOptimizer.Modules
{
	/// <summary>
	/// Compresses the output using standard gzip/deflate.
	/// </summary>
	public sealed class CompressionModule : IHttpModule
	{
		/// <summary>
		/// A regular expression for filtering content types.
		/// </summary>
		private static readonly Regex contentTypeFilter = new Regex("text/*|application.json", RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnoreCase);

		#region IHttpModule Members

		/// <summary>
		/// Disposes of the resources (other than memory) used by the module 
		/// that implements <see cref="T:System.Web.IHttpModule"></see>.
		/// </summary>
		void IHttpModule.Dispose()
		{
			// Nothing to dispose; 
		}

		/// <summary>
		/// Initializes a module and prepares it to handle requests.
		/// </summary>
		/// <param name="context">An <see cref="T:System.Web.HttpApplication"></see> 
		/// that provides access to the methods, properties, and events common to 
		/// all application objects within an ASP.NET application.
		/// </param>
		void IHttpModule.Init(HttpApplication context)
		{
			context.PreRequestHandlerExecute += new EventHandler(this.PreRequestHandlerExecute);
			context.Error += new EventHandler(this.ProcessError);
		}

		#endregion

		/// <summary>
		/// Check if the browser support compression
		/// </summary>
		/// <param name="context">The HTTP context.</param>
		/// <returns>A value indicating whether or not the client supports compression.</returns>
		private static bool IsCompressionSupported(HttpContext context)
		{
			if (context.Request.Browser == null)
			{
				return false;
			}

			if (context.Request.Params["SERVER_PROTOCOL"] != null && context.Request.Params["SERVER_PROTOCOL"].Contains("1.1"))
			{
				return true;
			}

			return false;
		}

		/// <summary>
		/// Handles the BeginRequest event of the context control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		private void PreRequestHandlerExecute(object sender, EventArgs e)
		{
			HttpContext context = ((HttpApplication)sender).Context;
			string contentType = context.Response.ContentType;

			if (context.Request.HttpMethod == "GET" && contentTypeFilter.IsMatch(contentType) && IsCompressionSupported(context))
			{
				context.Items["filter"] = context.Response.Filter;
				Processor.Compress(context);
			}
		}

		/// <summary>
		/// Handles the Error event of the context control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		private void ProcessError(object sender, EventArgs e)
		{
			HttpContext context = ((HttpApplication)sender).Context;
			if (context.Error != null)
			{
				context.Response.Filter = (Stream)context.Items["filter"];
			}
		}
	}
}