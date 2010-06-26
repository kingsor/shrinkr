<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<PagedListViewModel<ShortUrlDTO>>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Shrinkr : My Urls
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="shrinkBox">
        <h2>My Urls:</h2>
        <div class="shrinkForm">
            <ul class="list">
                <li>
                    <ul class="item">
                        <li class="preview">Preview</li>
                        <li class="visits">Visits</li>
                        <li class="title">Title</li>
                        <li class="added">Created</li>
                    </ul>
                    <div class="clear"></div>
                </li>
                <% if (Model.Items.Any()){ %>
                    <% foreach (ShortUrlDTO shortUrl in Model.Items){ %>
                    <li>
                        <ul class="item">
                            <li class="preview"><a href="<%= Url.Preview(shortUrl.Alias) %>"><img alt="<%= Html.AttributeEncode(shortUrl.Title) %>" src="<%= Html.AttributeEncode(shortUrl.SmallThumbnail()) %>" style="width:150px;height:108px"/></a></li>
                            <li class="visits"><%= shortUrl.Visits %></li>
                            <li class="title"><a href="<%= Url.Visit(shortUrl.Alias) %>"><%= Html.Encode(shortUrl.Title) %></a></li>
                            <li class="added"><%= shortUrl.CreatedAt.ToString("G", Shrinkr.Culture.Current) %></li>
                        </ul>
                        <div class="clear"></div>
                    </li>
                    <% } %>
                <% } %>
                <% else{ %>
                    <li>
                        <ul class="item">
                            <li>No url exists.</li>
                        </ul>
                        <div class="clear"></div>
                    </li>
                <% } %>
            </ul>
            <%= Html.List().Pager<ShortUrlDTO>() %>
        </div>
    </div>
</asp:Content>