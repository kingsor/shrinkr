<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<ul class="form">
    <li>
        <%= Html.TextBox("url", null, new { title = "Enter your url...", maxlength = 2048, @class = "largeTextBox" })%>
        <%= Html.CustomValidationMessage("url")%>
    </li>
    <li>
        <%= Html.TextBox("alias", null, new { title = "alias (optional)", maxlength = 440, @class = "largeTextBox" })%>
        <%= Html.CustomValidationMessage("alias")%>
        <span class="tip">(alphanumeric only)</span>
    </li>
    <li>
        <%= Html.AntiForgeryToken() %>
        <input type="submit" value="Shrink" class="largeButton"/>
    </li>
</ul>
<div class="clear"></div>