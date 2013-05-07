#region Using directives

using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;

#endregion

[assembly: AssemblyTitle("ASP.NET Optimizer")]
[assembly: AssemblyDescription("A collection of handlers and modules for boosting both client- and server-side performance of ASP.NET")]
[assembly: AssemblyConfiguration("See readme.txt for information on how to configure the web.config")]
[assembly: AssemblyCompany("http://madskristensen.net")]
[assembly: AssemblyProduct("ASP.NET Optimizer")]
[assembly: AssemblyCopyright("Copyright @ Mads Kristensen 2009")]
[assembly: AssemblyTrademark("http://madskristensen.net")]
[assembly: AssemblyCulture("")]

[assembly: CLSCompliant(false)]
[assembly: ComVisible(false)]
[assembly: AllowPartiallyTrustedCallers]
[assembly: PermissionSet(SecurityAction.RequestMinimum, Name = "Nothing")]

[assembly: AssemblyVersion("1.0.0.0")]