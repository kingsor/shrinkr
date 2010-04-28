<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<PagedListViewModel<BannedIPAddress>>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Shrinkr : Control Panel
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="shrinkBox">
        <h2>Control Panel : Banned IPs</h2>
        <div class="shrinkForm">
            <% Html.Telerik()
                   .TabStrip()
                   .Name("tabs")
                   .Items(tabs =>
                            {
                                tabs.AddSummary()
                                    .AddUrls()
                                    .AddUsers();

                                tabs.Add()
                                    .Text("Banned IPs")
                                    .Selected(true)
                                    .Content(() =>
                                                 {%>
                                                     <table id="list" cellspacing="0" border="0">
                                                        <colgroup>
                                                            <col style="padding:0 2px 2px 0;text-align:left;width:160px;white-space:nowrap" />
                                                            <col style="padding:0 0 2px 0;text-align:left" />
                                                        </colgroup>
                                                        <thead>
                                                            <tr>
                                                                <td colspan="2" style="padding:0">
                                                                    <% using (Html.BeginForm("CreateBannedIPAddress", "ControlPanel", FormMethod.Post, new { id = "create" })){%>
                                                                        <div>
                                                                            <table cellspacing="0">
                                                                                <colgroup>
                                                                                    <col style="padding:0 2px 2px 0;text-align:left;width:160px" />
                                                                                    <col style="padding:0 0 2px 0;text-align:left" />
                                                                                </colgroup>
                                                                                <tbody>
                                                                                    <tr>
                                                                                        <td scope="col">
                                                                                            <%= Html.TextBox("ipAddress", null, new { title = "Enter your ip...", maxlength = 15, @class = "smallTextBox" })%>
                                                                                            <%= Html.CustomValidationMessage("ipAddress")%>
                                                                                        </td>
                                                                                        <td scope="col">
                                                                                            <input type="submit" value="add" class="smallButton"/>
                                                                                        </td>
                                                                                    </tr>
                                                                                </tbody>
                                                                            </table>
                                                                        </div>
                                                                    <% } %>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2">
                                                                    <%= Html.CustomValidationSummary("Please correct the following errors and try again.", new { id = "errors" }) %>
                                                                </td>
                                                            </tr>
                                                        </thead>
                                                        <tfoot>
                                                            <tr>
                                                                <td colspan="2" style="text-align:center">
                                                                    <%= Html.List().Pager<BannedIPAddress>()%>
                                                                </td>
                                                            </tr>
                                                        </tfoot>
                                                        <tbody>
                                                            <% for (var i =0; i < Model.Items.Count; i++) { %>
                                                            <% var banned = Model.Items[i]; %>
                                                                <tr>
                                                                    <td scope="col">
                                                                        <%= banned.IPAddress %>
                                                                    </td>
                                                                    <td scope="col">
                                                                        <%using (Html.BeginForm("DeleteBannedIPAddress", "ControlPanel", FormMethod.Post, new { @class = "delete" })){%>
                                                                            <div>
                                                                                <input type="hidden" id="ipAddress-<%= i %>" name="id" value="<%= banned.Id %>"/>
                                                                                <input type="submit" value="remove" class="smallButton"/>
                                                                            </div>
                                                                        <% } %>
                                                                    </td>
                                                                </tr>
                                                            <% } %>
                                                        </tbody>
                                                     </table>
                                                 <%}
                                            );

                                tabs.AddBannedDomains()
                                    .AddReservedAliases()
                                    .AddBadWords();
                            }
                        )
                  .Render();%>
        </div>
        <% Html.Telerik()
               .ScriptRegistrar()
               .Scripts(group => group.AddSharedGroup("controlPanelScripts"))
               .OnDocumentReady(() =>
                                      {%>
                                        bannedIpAddress.init('<%= Url.InputValidationErrorIcon() %>', '<%= Url.Action("DeleteBannedIPAddress")%>');
                                      <%}
                               ); %>
    </div>
</asp:Content>