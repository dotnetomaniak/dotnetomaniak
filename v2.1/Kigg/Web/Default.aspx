<%@ Page Language="C#"%>
<%-- Please do not delete this file. It is used to ensure that ASP.NET MVC is activated by IIS when a user makes a "/" request to the server.--%>
<%
    HttpContext.Current.RewritePath(Request.ApplicationPath);
    IHttpHandler httpHandler = new MvcHttpHandler();
    httpHandler.ProcessRequest(HttpContext.Current);
%>