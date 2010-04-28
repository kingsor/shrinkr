<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<UserDTO>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Shrinkr : My Profile
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="shrinkBox">
        <h2>My Profile:</h2>
        <div class="shrinkForm">
            <p>
                <label>User Name:</label> <span class="largeTextBox"><%= Model.Name %></span>
            </p>
            <p>
                <label>Email:</label> <span class="largeTextBox"><%= string.IsNullOrWhiteSpace(Model.Email) ? "n/a" : Model.Email %></span>
            </p>
            <p>
                <label>Created:</label> <span class="largeTextBox"><%= Model.CreatedAt.ToString("d MMM yyyy", Shrinkr.Culture.Current) %></span>
            </p>
            <p>
                <label>Api Access:</label> <input type="checkbox" checked="<%= Model.ApiAccessAllowed ? "checked" : string.Empty %>" disabled="disabled" />
            </p>
            <% if (Model.ApiAccessAllowed) { %>
                <p>
                    <label>Daily Limit:</label> <span class="largeTextBox"><%= (Model.ApiDailyLimit == ApiSetting.InfiniteLimit) ? "Unlimited" : Model.ApiDailyLimit.GetValueOrDefault().ToString(Shrinkr.Culture.Current) %></span><% if (Model.HasExceededApiDailyLimit) {%><span class="warning">Has exceeded daily limit</span><%}%>
                </p>
                <p>
                    <label for="apiKey">Api Key:</label> <%= Html.TextBox("apiKey", Model.ApiKey, new { @class = "largeTextBox", style = "width:280px", @readonly = "readonly" })%>
                </p>
                <%= Html.CustomValidationSummary("Please correct the following errors and try again.", new { id = "errors", style = "padding-left:102px" })%>
                <% using (Html.BeginRouteForm("GenerateKey", FormMethod.Post, new { id = "generate", style = "padding-left:102px" })) { %>
                    <div>
                        <input type="submit" value="Reset Api Key" class="largeButton"/>
                    </div>
                <% } %>
            <% } %>
        </div>
        <% Html.Telerik()
               .ScriptRegistrar()
               .OnDocumentReady(() =>
                                      {%>
                                        profile.init('<%= Url.InputValidationErrorIcon() %>');
                                      <%}
                               ); %>
    </div>
</asp:Content>