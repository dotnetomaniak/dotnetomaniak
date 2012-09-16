namespace Kigg.Infrastructure.EnterpriseLibrary
{
    using System;
    using System.Collections.Generic;
    using System.Web;

    public class UnityPerWebRequestLifetimeModule : DisposableResource, IHttpModule
    {
        private static readonly object Key = new object();

        private HttpContextBase _httpContext;

        public UnityPerWebRequestLifetimeModule(HttpContextBase httpContext)
        {
            _httpContext = httpContext;
        }

        public UnityPerWebRequestLifetimeModule()
        {
        }

        internal IDictionary<UnityPerWebRequestLifetimeManager, object> Instances
        {
            get
            {
                _httpContext = (HttpContext.Current != null) ? new HttpContextWrapper(HttpContext.Current) : _httpContext;

                return (_httpContext == null) ? null : GetInstances(_httpContext);
            }
        }

        public void Init(HttpApplication context)
        {
            context.EndRequest += (sender, e) => RemoveAllInstances();
        }

        internal static IDictionary<UnityPerWebRequestLifetimeManager, object> GetInstances(HttpContextBase httpContext)
        {
            IDictionary<UnityPerWebRequestLifetimeManager, object> instances;

            if (httpContext.Items.Contains(Key))
            {
                instances = (IDictionary<UnityPerWebRequestLifetimeManager, object>) httpContext.Items[Key];
            }
            else
            {
                lock (httpContext.Items)
                {
                    if (httpContext.Items.Contains(Key))
                    {
                        instances = (IDictionary<UnityPerWebRequestLifetimeManager, object>) httpContext.Items[Key];
                    }
                    else
                    {
                        instances = new Dictionary<UnityPerWebRequestLifetimeManager, object>();
                        httpContext.Items.Add(Key, instances);
                    }
                }
            }

            return instances;
        }

        internal void RemoveAllInstances()
        {
            IDictionary<UnityPerWebRequestLifetimeManager, object> instances = Instances;

            if (!instances.IsNullOrEmpty())
            {
                foreach (KeyValuePair<UnityPerWebRequestLifetimeManager, object> entry in instances)
                {
                    IDisposable disposable = entry.Value as IDisposable;

                    if (disposable != null)
                    {
                        disposable.Dispose();
                    }
                }

                instances.Clear();
            }
        }
    }
}