<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Shrinkr : Log Off
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="shrinkBox">
        <h2>Are you sure you want to log off?</h2>
        <div class="shrinkForm">
            <% using (Html.BeginForm()){%>
                <div>
                    <span>You can log on anytime with your Open ID account.</span> <input type="submit" value="Log off" class="largeButton"/>
                </div>
            <% } %>
        </div>
    </div>
</asp:Content>