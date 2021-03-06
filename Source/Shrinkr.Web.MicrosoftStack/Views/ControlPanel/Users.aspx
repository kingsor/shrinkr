﻿<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<GridModel<UserDTO>>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Shrinkr : Control Panel
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="shrinkBox">
        <h2>Control Panel : Users</h2>
        <div class="shrinkForm">
            <% Html.Telerik()
                   .TabStrip()
                   .Name("tabs")
                   .Items(tabs =>
                            {
                                tabs.AddSummary()
                                    .AddUrls();

                                tabs.Add()
                                    .Text("Users")
                                    .Selected(true)
                                    .Content(() =>
                                                {%>
                                                    <% Html.Telerik()
                                                           .Grid(Model.Data)
                                                           .Name("users")
                                                           .PrefixUrlParameters(false)
                                                           .Columns(columns =>
                                                                    {
                                                                        columns.Bound(u => u.Name).Width(200);
                                                                        columns.Bound(u => u.CreatedAt).Width(120).Format("{0:MM/dd/yyyy}").Title("Created");
                                                                        columns.Bound(u => u.LastActivityAt).Format("{0:G}").Title("Last Activity");
                                                                        columns.Bound(u => u.ApiAccessAllowed).Width(80).Format("{0}").Title("Api");
                                                                        columns.Bound(u => u.HasExceededApiDailyLimit).Width(100).Format("{0}").Title("Exceeded");
                                                                        columns.Bound(u => u.Id).Width(80).Format("<a class=\"command\" href=\"User/{0}\">Details</a>").Encoded(false).Filterable(false).Sortable(false).Title("Action");
                                                                    })
                                                            .DataBinding(databinding => databinding.Ajax().Select("Users","ControlPanel").Enabled(true))
                                                            .Filterable()
                                                            .Pageable()
                                                            .Scrollable(scrolling => scrolling.Height(250))
                                                            .Sortable(sort => sort.SortMode(GridSortMode.MultipleColumn).OrderBy(orderby => orderby.Add(u => u.LastActivityAt).Descending()))
                                                            .Render(); %>
                                                <%}
                                            );

                                tabs.AddBannedIPs()
                                    .AddBannedDomains()
                                    .AddReservedAliases()
                                    .AddBadWords();
                            }
                        )
                  .Render();%>
        </div>
    </div>
</asp:Content>