namespace Kigg.Infrastructure.EnterpriseLibrary
{
    using System;
    using System.Diagnostics;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Web;

    using Microsoft.Practices.EnterpriseLibrary.Logging;

    public class WeblogEntry : LogEntry
    {
        internal const string Unavailable = "brak";

        private readonly string _namespaceName;
        private readonly string _className;
        private readonly string _methodSignature;

        private readonly HttpContextBase _context;

        public WeblogEntry(HttpContextBase context, string message, string category, TraceEventType severity, string namespaceName, string className, string methodSignature) : base(message, category, 1, 0, severity, null, null)
        {
            _context = context ?? ((HttpContext.Current != null) ? new HttpContextWrapper(HttpContext.Current) : null);

            _namespaceName = namespaceName;
            _className = className;
            _methodSignature = methodSignature;
        }

        public WeblogEntry(string message, string category, TraceEventType severity, string namespaceName, string className, string methodSignature) : this(null, message, category, severity, namespaceName, className, methodSignature)
        {
        }

        public string NamespaceName
        {
            [DebuggerStepThrough]
            get
            {
                return _namespaceName;
            }
        }

        public string ClassName
        {
            [DebuggerStepThrough]
            get
            {
                return _className;
            }
        }

        public string MethodSignature
        {
            [DebuggerStepThrough]
            get
            {
                return _methodSignature;
            }
        }

        public string CurrentUserName
        {
            get
            {
                if (IsHosted)
                {
                    if ((_context.User != null) && (_context.User.Identity != null))
                    {
                        return _context.User.Identity.IsAuthenticated ? _context.User.Identity.Name : "Niezalogowany użytkownik";
                    }
                }

                return Unavailable;
            }
        }

        public string CurrentUserIPAddress
        {
            get
            {
                if (IsRequestAvailable)
                {
                    string ip = _context.Request.UserHostAddress;

                    if (!string.IsNullOrEmpty(_context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"]))
                    {
                        ip += "->" + _context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                    }

                    return ip;
                }

                return Unavailable;
            }
        }

        public string CurrentUserAgent
        {
            get
            {
                return IsRequestAvailable ? _context.Request.UserAgent : Unavailable;
            }
        }

        public string CurrentUrl
        {
            get
            {
                return IsRequestAvailable ? _context.Request.RawUrl : Unavailable;
            }
        }

        public string CurrentUrlReferrer
        {
            get
            {
                if (IsRequestAvailable)
                {
                    if (_context.Request.UrlReferrer != null)
                    {
                        return _context.Request.UrlReferrer.ToString();
                    }
                }

                return Unavailable;
            }
        }

        internal bool IsRequestAvailable
        {
            [DebuggerStepThrough]
            get
            {
                Func<bool> isAvailable = () =>
                                         {
                                             try
                                             {
                                                 return _context.Request != null;
                                             }
                                             catch (HttpException)
                                             {
                                             }

                                             return false;
                                         };

                return IsHosted && isAvailable();
            }
        }

        private bool IsHosted
        {
            [DebuggerStepThrough]
            get
            {
                return _context != null;
            }
        }

        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.NoInlining)]
        internal static void GetMethodDetails(int frameToSkip, out string namespaceName, out string className, out string methodSignature)
        {
            StringBuilder output = new StringBuilder();
            MethodBase method = new StackFrame(frameToSkip, false).GetMethod();

            namespaceName = method.DeclaringType.Namespace;
            className = method.DeclaringType.Name;

            output.Append(method.Name);
            output.Append("(");

            ParameterInfo[] paramInfos = method.GetParameters();

            if (paramInfos.Length > 0)
            {
                output.Append("{0} {1}".FormatWith(paramInfos[0].ParameterType.Name, paramInfos[0].Name));

                if (paramInfos.Length > 1)
                {
                    for (int j = 1; j < paramInfos.Length; j++)
                    {
                        output.Append(", {0} {1}".FormatWith(paramInfos[j].ParameterType.Name, paramInfos[j].Name));
                    }
                }
            }

            output.Append(")");

            methodSignature = output.ToString();
        }
    }
}