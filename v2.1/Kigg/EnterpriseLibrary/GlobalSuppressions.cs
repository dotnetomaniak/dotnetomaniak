// This file is used by Code Analysis to maintain SuppressMessage 
// attributes that are applied to this project. 
// Project-level suppressions either have no target or are given 
// a specific target and scoped to a namespace, type, member, etc. 
//
// To add a suppression to this file, right-click the message in the 
// Error List, point to "Suppress Message(s)", and click 
// "In Project Suppression File". 
// You do not need to add suppressions to this file manually. 

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Kigg", Scope = "namespace", Target = "Kigg.Infrastructure.EnterpriseLibrary")]
[assembly: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Kigg")]
[assembly: SuppressMessage("Microsoft.Naming", "CA1724:TypeNamesShouldNotMatchNamespaces", Scope = "type", Target = "Kigg.Infrastructure.EnterpriseLibrary.Cache")]
[assembly: SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Scope = "member", Target = "Kigg.Infrastructure.EnterpriseLibrary.Cache.#Get`1(System.String)")]
[assembly: SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "2#", Scope = "member", Target = "Kigg.Infrastructure.EnterpriseLibrary.IExceptionPolicy.#HandleException(System.Exception,System.String,System.Exception&)")]
[assembly: SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Scope = "member", Target = "Kigg.Infrastructure.EnterpriseLibrary.UnityDependencyResolver.#Resolve`1()")]
[assembly: SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Scope = "member", Target = "Kigg.Infrastructure.EnterpriseLibrary.UnityDependencyResolver.#Resolve`1(System.String)")]
[assembly: SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Scope = "member", Target = "Kigg.Infrastructure.EnterpriseLibrary.UnityDependencyResolver.#Resolve`1(System.Type)")]
[assembly: SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Scope = "member", Target = "Kigg.Infrastructure.EnterpriseLibrary.UnityDependencyResolver.#Resolve`1(System.Type,System.String)")]
[assembly: SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Scope = "member", Target = "Kigg.Infrastructure.EnterpriseLibrary.UnityDependencyResolver.#ResolveAll`1()")]
[assembly: SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings", Scope = "member", Target = "Kigg.Infrastructure.EnterpriseLibrary.WeblogEntry.#CurrentUrl")]
[assembly: SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings", Scope = "member", Target = "Kigg.Infrastructure.EnterpriseLibrary.WeblogEntry.#CurrentUrlReferrer")]
[assembly: SuppressMessage("Microsoft.Portability", "CA1903:UseOnlyApiFromTargetedFramework", MessageId = "System.Web.Abstractions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35")]