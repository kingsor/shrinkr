<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<PagedListViewModel<BadWord>>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Shrinkr : Control Panel
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="shrinkBox">
        <h2>Control Panel : Bad Words</h2>
        <div class="shrinkForm">
            <% Html.Telerik()
                   .TabStrip()
                   .Name("tabs")
                   .Items(tabs =>
                            {
                                tabs.AddSummary()
                                    .AddUrls()
                                    .AddUsers()
                                    .AddBannedIPs()
                                    .AddBannedDomains()
                                    .AddReservedAliases();

                                tabs.Add()
                                    .Text("Bad Words")
                                    .Selected(true)
                                    .Content(() =>
                                                 {%>
                                                     <table id="list" cellspacing="0" border="0">
                                                        <colgroup>
                                                            <col style="padding:0 2px 2px 0;text-align:left;width:200px;white-space:nowrap" />
                                                            <col style="padding:0 0 2px 0;text-align:left" />
                                                        </colgroup>
                                                        <thead>
                                                            <tr>
                                                                <td colspan="2" style="padding:0">
                                                                    <% using (Html.BeginForm("CreateBadWord", "ControlPanel", FormMethod.Post, new { id = "create" })){%>
                                                                        <div>
                                                                            <table cellspacing="0">
                                                                                <colgroup>
                                                                                    <col style="padding:0 2px 2px 0;text-align:left;width:200px" />
                                                                                    <col style="padding:0 0 2px 0;text-align:left" />
                                                                                </colgroup>
                                                                                <tbody>
                                                                                    <tr>
                                                                                        <td scope="col">
                                                                                            <%= Html.TextBox("expression", null, new { title = "Enter your expression...", maxlength = 440, @class = "smallTextBox", style = "width:160px" })%>
                                                                                            <%= Html.CustomValidationMessage("expression")%>
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
                                                                    <%= Html.List().Pager<BadWord>()%>
                                                                </td>
                                                            </tr>
                                                        </tfoot>
                                                        <tbody>
                                                            <% for (var i =0; i < Model.Items.Count; i++) { %>
                                                                <% var word = Model.Items[i]; %>
                                                                <tr>
                                                                    <td scope="col">
                                                                        <%= word.Expression %>
                                                                    </td>
                                                                    <td scope="col">
                                                                        <% using (Html.BeginForm("DeleteBadWord", "ControlPanel", FormMethod.Post, new { @class = "delete" })){%>
                                                                            <div>
                                                                                <input type="hidden" id="word-<%= i %>" name="id" value="<%= word.Id %>"/>
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
                            }
                        )
                  .Render();%>
        </div>
        <% Html.Telerik()
               .ScriptRegistrar()
               .Scripts(group => group.AddSharedGroup("controlPanelScripts"))
               .OnDocumentReady(() =>
                                      {%>
                                        badWord.init('<%= Url.InputValidationErrorIcon() %>', '<%= Url.Action("DeleteBadWord")%>');
                                      <%}
                               ); %>
    </div>
</asp:Content>