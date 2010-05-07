﻿<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<UserDTO>" %>
<script runat="server">
    string ApiAccessDropDownList()
    {
        var list = new List<SelectListItem>
                       {
                           new SelectListItem { Text = @"Not Allowed", Value = "-1", Selected = !Model.ApiAccessAllowed },
                           new SelectListItem { Text = @"250", Value = "250", Selected = (Model.ApiAccessAllowed && Model.ApiDailyLimit == 250) },
                           new SelectListItem { Text = @"500", Value = "500", Selected = (Model.ApiAccessAllowed && Model.ApiDailyLimit == 500) },
                           new SelectListItem { Text = @"1000", Value = "1000", Selected = (Model.ApiAccessAllowed && Model.ApiDailyLimit == 1000) },
                           new SelectListItem { Text = @"2500", Value = "2500", Selected = (Model.ApiAccessAllowed && Model.ApiDailyLimit == 2500) },
                           new SelectListItem { Text = @"Unlimited", Value = ApiSetting.InfiniteLimit.ToString(Shrinkr.Culture.Current) , Selected = (Model.ApiAccessAllowed && Model.ApiDailyLimit == ApiSetting.InfiniteLimit ) },
                       };

        return Html.DropDownList("dailyLimit", list, (string) null).ToString();
    }
</script>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Shrinkr : Control Panel
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="shrinkBox">
        <h2>User:</h2>
        <div class="shrinkForm">
            <% using (Html.BeginForm("UnlockUser", "ControlPanel", FormMethod.Post, new { id = "unlock", style = (Model.IsLockedOut ? "display:inline" : "display:none") + ";padding-bottom:4px" })) { %>
                <div>
                    <label style="float:left;width:100px">Name:</label> <%= Model.Name %>
                    <input type="hidden" id="unlockId" name="id" value="<%= Model.Id %>"/>
                    <input type="submit" value="Unlock" class="smallButton"/>
                </div>
            <% } %>
            <% using (Html.BeginForm("LockUser", "ControlPanel", FormMethod.Post, new { id = "lock", style = (!Model.IsLockedOut ? "display:inline" : "display:none") + ";padding-bottom:4px" })) { %>
                <div>
                    <label style="float:left;width:100px">Name:</label> <%= Model.Name %>
                    <input type="hidden" id="lockId" name="id" value="<%= Model.Id %>"/>
                    <input type="submit" value="Lock" class="smallButton"/>
                </div>
            <% } %>
            <p>
                <label>Email:</label>
                <% if (string.IsNullOrWhiteSpace(Model.Email)) {%>
                    n/a
                <% } %>
                <% else { %>
                    <a href="mailto:<%= Model.Email %>"><%= Model.Email %></a>
                <% } %>
            </p>
            <p>
                <label>Created:</label> <%= Model.CreatedAt.ToString("d MMM yyyy", Shrinkr.Culture.Current) %>
            </p>
            <p>
                <label>Last Activity:</label> <%= Model.LastActivityAt.ToString("G", Shrinkr.Culture.Current) %>
            </p>
            <% using (Html.BeginForm("UpdateUserRole", "ControlPanel", FormMethod.Post, new { id = "updateRole", style = "padding:4px" })) { %>
                <div>
                    <label for="role" style="float:left;width:100px">Role:</label>
                    <input type="hidden" id="roleId" name="id" value="<%= Model.Id %>"/>
                    <%= Html.DropDownList("role", Model.Role) %>
                    <input type="submit" value="Update" class="smallButton"/>
                </div>
            <% } %>
            <% using (Html.BeginForm("UpdateUserApiAccess", "ControlPanel", FormMethod.Post, new { id = "updateApiAccess", style = "padding:4px" })) { %>
                <div>
                    <label for="dailyLimit" style="float:left;width:100px">Api Access:</label>
                    <input type="hidden" id="apiId" name="id" value="<%= Model.Id %>"/>
                    <%= ApiAccessDropDownList() %>
                    <input type="submit" value="Update" class="smallButton"/>
                </div>
            <% } %>
            <p>
                <a class="command" href="<%= Url.Users(1) %>" style="margin-left:96px">« Back to Users</a>
            </p>
        </div>
        <% Html.Telerik()
               .ScriptRegistrar()
               .Scripts(group => group.AddSharedGroup("controlPanelScripts"))
               .OnDocumentReady(() =>
                                      {%>
                                        user.init();
                                      <%}
                               ); %>
    </div>
</asp:Content>