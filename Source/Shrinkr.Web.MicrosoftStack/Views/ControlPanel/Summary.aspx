<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IDictionary<TimeSpan, SystemHealthDTO>>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Shrinkr : Control Panel
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="shrinkBox">
        <h2>Control Panel : Summary</h2>
        <div class="shrinkForm">
            <% Html.Telerik()
                   .TabStrip()
                   .Name("tabs")
                   .Items(tabs =>
                            {
                                tabs.Add()
                                    .Text("Summary")
                                    .Selected(true)
                                    .Content(() =>
                                                {%>
                                                    <% foreach(var item in Model){%>
                                                        <% var duration = item.Key; %>
                                                        <% var status = item.Value; %>
                                                        <div style="padding:16px;float:left">
                                                            <h3 style="margin:2px;padding:0;text-decoration:underline">Last <%= duration %></h3>
                                                            <ul style="margin:0;padding:0px">
                                                                <li style="list-style: none none;padding:2px">Url Created : <%= status.UrlCreated %></li>
                                                                <li style="list-style: none none;padding:2px">Url Visited : <%= status.UrlVisited %></li>
                                                                <li style="list-style: none none;padding:2px">User Created : <%= status.UserCreated %></li>
                                                                <li style="list-style: none none;padding:2px">User Visited : <%= status.UserVisited %></li>
                                                            </ul>
                                                        </div>
                                                    <% }%>
                                                    <div class="clear"></div>
                                                <%}
                                            );

                                tabs.AddUrls()
                                    .AddUsers()
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