<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<a href="<%= Url.Home() %>">New</a> /
<% if (Page.User.Identity.IsAuthenticated) {%>
    <a href="<%= Url.List(1) %>">My Urls</a> /
    <a href="<%= Url.Profile() %>">My Profile</a> /
    <a href="<%= Url.LogOff() %>">Log Off</a>
<%}
else {%>
    <a href="<%= Url.LogOn() %>">Log On</a>
<%}%>