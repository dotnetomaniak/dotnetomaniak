#region Using

using System;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using WebOptimizer.Filters;

#endregion

namespace WebOptimizer.Modules
{
	/// <summary>
	/// A module for handling conditional GET requests.
	/// </summary>
	public class MinifyInlineStylesModule : IHttpModule
	{
		#region IHttpModule Members

		/// <summary>
		/// Disposes of the resources (other than memory) used by the module that implements <see cref="T:System.Web.IHttpModule"/>.
		/// </summary>
		public void Dispose()
		{
			// Nothing to dispose.
		}

		/// <summary>
		/// Initializes a module and prepares it to handle requests.
		/// </summary>
		/// <param name="context">An <see cref="T:System.Web.HttpApplication"/> that provides access to the methods, properties, and events common to all application objects within an ASP.NET application</param>
		public void Init(HttpApplication context)
		{
			context.PostRequestHandlerExecute += new EventHandler(this.ProcessResponse);
		}

		#endregion

		/// <summary>
		/// Processes the response and sets a response filter for conditional GETs.
		/// </summary>
		/// <param name="sender">The HTTP application of the current request.</param>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		private void ProcessResponse(object sender, EventArgs e)
		{
			HttpContext context = ((HttpApplication)sender).Context;
			if (context.Request.HttpMethod == "GET" && context.Response.ContentType == "text/html")
			{
				context.Response.Filter = new MinifyInlineStylesFilterStream(context);
			}
		}
	}
}