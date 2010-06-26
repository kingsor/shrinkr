<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<ShortUrlDTO>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Shrinkr : Control Panel
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="shrinkBox">
        <h2>Url:</h2>
        <div class="shrinkForm">
            <p>
                <label>Title:</label>
                <span style="float:left;width:700px"><a href="<%= Html.AttributeEncode(Model.Url) %>"><%= Html.Encode(Model.Title) %></a></span>
                <br class="clear"/>
            </p>
            <p>
                <label>Alias:</label> <a href="<%= Url.Visit(Model.Alias) %>"><%= Model.Alias %></a>
            </p>
            <p>
                <label>Created:</label> <%= Model.CreatedAt.ToString("G", Shrinkr.Culture.Current) %>
            </p>
            <p>
                <label>Visits:</label> <%= Model.Visits %>
            </p>
            <p>
                <label>User:</label> 
                <% if (string.IsNullOrWhiteSpace(Model.UserName)) {%>
                    n/a
                <% } %>
                <% else {%>
                    <a href="<%= Url.User(Model.UserId) %>"><%= Model.UserName %></a>
                <% } %>
            </p>
            <p>
                <label>Domain:</label> <a href="http://<%= Html.AttributeEncode(Model.Domain) %>"><%= Model.Domain %></a> <a class="command" id="blockDomain" href="javascript:void(0)">Block</a>
            </p>
            <p>
                <label>Ip Address:</label> <%= Model.IPAddress %> <a class="command" id="blockIp" href="javascript:void(0)">Block</a>
            </p>
            <p>
                <label>Spam:</label> <%= Model.SpamStatus.AsHtml("status") %>
            </p>
            <div style="padding: 4px 4px 4px 100px">
                <% using (Html.BeginForm("MarkUrlAsSpam", "ControlPanel", FormMethod.Post, new { id = "spam", style = !Model.SpamStatus.IsSpam() ? "display:inline" : "display:none" })) { %>
                    <div>
                        <input id="spamAlias" name="alias" type="hidden" value="<%= Model.Alias %>"/>
                        <input type="submit" value="Mark as Spam" class="smallButton"/>
                    </div>
                <% } %>
                <% using (Html.BeginForm("MarkUrlAsSafe", "ControlPanel", FormMethod.Post, new { id = "safe", style = !Model.SpamStatus.IsClean() ? "display:inline" : "display:none" })) { %>
                    <div>
                        <input id="safeAlias" name="alias" type="hidden" value="<%= Model.Alias %>"/>
                        <input type="submit" value="Mark as Safe" class="smallButton"/>
                    </div>
                <% } %>
            </div>
            <p>
                <a class="command" href="<%= Url.Urls(1) %>" style="margin-left:96px">« Back to Urls</a>
            </p>
        </div>
        <% Html.Telerik()
               .ScriptRegistrar()
               .Scripts(group => group.AddSharedGroup("controlPanelScripts"))
               .OnDocumentReady(() =>
                                      {%>
                                        shortUrl.init('Spam','<%= SpamStatus.Clean.AsDescriptiveText()%>', '<%= Model.Domain %>', '<%= Model.IPAddress %>','<%= Url.Action("CreateBannedDomain", "ControlPanel")%>', '<%= Url.Action("CreateBannedIPAddress", "ControlPanel")%>');
                                      <%}
                               ); %>
    </div>
</asp:Content>