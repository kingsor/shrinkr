<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<GridModel<ShortUrlDTO>>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Shrinkr : Control Panel
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="shrinkBox">
        <h2>Control Panel : Urls</h2>
        <div class="shrinkForm">
            <% Html.Telerik()
                   .TabStrip()
                   .Name("tabs")
                   .Items(tabs =>
                            {
                                tabs.AddSummary();

                                tabs.Add()
                                    .Text("Urls")
                                    .Selected(true)
                                    .Content(() =>
                                                {%>
                                                    <% Html.Telerik()
                                                           .Grid(Model.Data)
                                                           .Name("urls")
                                                           .PrefixUrlParameters(false)
                                                           .Columns(columns =>
                                                                    {
                                                                        columns.Bound(u => u.Domain).Format("<a href=\"http://{0}\">{0}</a>").Encoded(false);
                                                                        columns.Bound(u => u.Title).Width(200);
                                                                        columns.Bound(u => u.Alias).Width(80).Format("<a href=\"" + Url.Content("~/") + "{0}\">{0}</a>").Encoded(false);
                                                                        columns.Bound(u => u.CreatedAt).Format("{0:G}").Title("Created");
                                                                        columns.Bound(u => u.Alias).Width(80).Format("<a class=\"command\" href=\"Url/{0}\">Details</a>").Encoded(false).Filterable(false).Sortable(false).Title("Action");
                                                                    })
                                                            .DataBinding(databinding => databinding.Ajax().Enabled(true))
                                                            .Filterable()
                                                            .Pageable()
                                                            .Scrollable(scrolling => scrolling.Height(250))
                                                            .Sortable(sort => sort.SortMode(GridSortMode.MultipleColumn).OrderBy(orderby => orderby.Add(u => u.CreatedAt).Descending()))
                                                            .Render(); %>
                                                <%}
                                            );

                                tabs.AddUsers()
                                    .AddBannedIPs()
                                    .AddBannedDomains()
                                    .AddReservedAliases()
                                    .AddBadWords();
                            }
                        )
                  .Render();%>
        </div>
    </div>
</asp:Content>