using System;
using System.Configuration;
using Kigg.LinqToSql.Repository;
using StackExchange.Profiling;
using StackExchange.Profiling.Mvc;
using StackExchange.Profiling.Storage;

namespace Kigg.Web
{
    using System.Web;

    using Infrastructure;

    public class GlobalApplication : HttpApplication
    {
        public static void OnStart()
        {
            Bootstrapper.Run();
            Log.Info("Application Started");
        }

        public static void OnEnd()
        {
            Log.Warning("Application Ended");
            IoC.Reset();
        }

        protected void Application_BeginRequest()
        {
            // You can decide whether to profile here, or it can be done in ActionFilters, etc.
            // We're doing it here so profiling happens ASAP to account for as much time as possible.
            if (Request.IsLocal) // Example of conditional profiling, you could just call MiniProfiler.StartNew();
            {
                MiniProfiler.StartNew();
            }
        }

        protected void Application_EndRequest()
        {
            MiniProfiler.Current?.Stop(); // Be sure to stop the profiler!
        }

        protected void Application_Start()
        {
            InitializeDatabase();
            OnStart();
            MiniProfiler.Configure(new MiniProfilerOptions
                    {
                        // Sets up the route to use for MiniProfiler resources:
                        // Here, ~/profiler is used for things like /profiler/mini-profiler-includes.js
                        RouteBasePath = "~/profiler",

                        // Example of using SQLite storage instead
                        Storage = new MemoryCacheStorage(TimeSpan.FromMinutes(10)) ,

                        // Different RDBMS have different ways of declaring sql parameters - SQLite can understand inline sql parameters just fine.
                        // By default, sql parameters will be displayed.
                        //SqlFormatter = new StackExchange.Profiling.SqlFormatters.InlineFormatter(),

                        // These settings are optional and all have defaults, any matching setting specified in .RenderIncludes() will
                        // override the application-wide defaults specified here, for example if you had both:
                        //    PopupRenderPosition = RenderPosition.Right;
                        //    and in the page:
                        //    @MiniProfiler.Current.RenderIncludes(position: RenderPosition.Left)
                        // ...then the position would be on the left on that page, and on the right (the application default) for anywhere that doesn't
                        // specified position in the .RenderIncludes() call.
                        PopupRenderPosition = RenderPosition.Right, // defaults to left
                        PopupMaxTracesToShow = 10, // defaults to 15

                        // ResultsAuthorize (optional - open to all by default):
                        // because profiler results can contain sensitive data (e.g. sql queries with parameter values displayed), we
                        // can define a function that will authorize clients to see the JSON or full page results.
                        // we use it on http://stackoverflow.com to check that the request cookies belong to a valid developer.
                        ResultsAuthorize = request => request.IsLocal,

                        // ResultsListAuthorize (optional - open to all by default)
                        // the list of all sessions in the store is restricted by default, you must return true to allow it
                        ResultsListAuthorize = request =>
                        {
                            // you may implement this if you need to restrict visibility of profiling lists on a per request basis
                            return true; // all requests are legit in this example
                        },

                        // Stack trace settings
                        StackMaxLength = 256, // default is 120 characters

                        // (Optional) You can disable "Connection Open()", "Connection Close()" (and async variant) tracking.
                        // (defaults to true, and connection opening/closing is tracked)
                        TrackConnectionOpenClose = true
                    }
                    // Optional settings to control the stack trace output in the details pane, examples:
                    .ExcludeType("SessionFactory") // Ignore any class with the name of SessionFactory)
                    .ExcludeAssembly("NHibernate") // Ignore any assembly named NHibernate
                    .ExcludeMethod("Flush") // Ignore any method with the name of Flush
                    .AddViewPofiling() // Add MVC view profiling (you want this)
                // If using EntityFrameworkCore, here's where it'd go.
                // .AddEntityFramework()        // Extension method in the MiniProfiler.EntityFrameworkCore package
            );

            // If we're using EntityFramework 6, here's where it'd go.
            // This is in the MiniProfiler.EF6 NuGet package.
            // MiniProfilerEF6.Initialize();
        }

        private void InitializeDatabase()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["dotnetomaniak"].ConnectionString;
            Database database = new Database(connectionString);
            if (!database.DatabaseExists())
            {
                database.CreateDatabase();
            }
        }

        protected void Application_End()
        {
            OnEnd();
        }
    }
}