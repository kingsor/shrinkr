<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CreateUrlViewModel>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Shrinkr
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="shrinkBox">
        <h2>Tired of long url? Try Shrinkr:</h2>
        <div class="shrinkForm">
            <% using (Html.BeginRouteForm("Default", FormMethod.Post, new { id = "create" })){ %>
                <% Html.RenderPartial("CreateForm"); %>
                <%= Html.CustomValidationSummary("Please correct the following errors and try again.", new { id = "errors" }) %>
                <% Html.RenderPartial("CreatedBox", Model); %>
            <% } %>
        </div>
    </div>
    <% Html.Telerik()
           .ScriptRegistrar()
           .OnDocumentReady(() =>
                                  {%>
                                    createShortUrl.init('<%= Url.InputValidationErrorIcon() %>');
                                  <%}
                           ); %>
</asp:Content>