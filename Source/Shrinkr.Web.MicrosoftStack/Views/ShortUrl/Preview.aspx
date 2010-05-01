<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<ShortUrlDTO>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Shrinkr : <%= Model.Title %> - Preview
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="shrinkBox">
        <h2>Preview:</h2>
        <div class="shrinkForm">
            <div class="preview">
                <ul>
                    <li class="image">
                        <img alt="<%= Html.AttributeEncode(Model.Title) %>" src="<%= Html.AttributeEncode(Model.MediumThumbnail()) %>" style="width:280px;202px" />
                    </li>
                    <li class="info">
                        <ul>
                            <li>
                                <strong>Title:</strong> <%= Html.Encode(Model.Title) %>
                            </li>
                            <li>
                                <strong>Created:</strong> <%= Model.CreatedAt.ToString("d MMM yyyy", Shrinkr.Culture.Current)%>
                            </li>
                            <li>
                                <strong>Domain:</strong> <a href="http://<%= Html.Encode(Model.Domain)%>"><%= Html.Encode(Model.Domain) %></a>
                            </li>
                            <li>
                                <strong>Visits:</strong> <%= Model.Visits %>
                            </li>
                            <li>
                                <a class="command" style="font-size:16px" href="<%= Url.Visit(Model.Alias) %>">Continue »</a>
                            </li>
                        </ul>
                    </li>
                </ul>
                <div class="clear"></div>
            </div>
            <div>
                <% if (Model.SpamStatus.IsPhishing()) {%>
                    <p>
                        <span class="warningText">Warning- Suspected phishing page</span>
                        Visiting the above page may be a forgery or imitation of another website, designed to trick users into sharing
                        personal or financial information. Entering any personal information on this page may result in identity theft 
                        or other abuse. You can find out more about phishing from <a href="http://www.antiphishing.org">www.antiphishing.org</a> 
                        (Advisory provided by <a href="http://code.google.com/apis/safebrowsing/safebrowsing_faq.html#whyAdvisory">Google</a>).
                    </p>
                <% } %>
                <% if (Model.SpamStatus.IsMalware()) {%>
                    <p>
                        <span class="warningText">Warning- Visiting this web site may harm your computer</span>
                        This above page appears to contain malicious code that could be downloaded to your computer without your 
                        consent. You can learn more about harmful web content including viruses and other malicious code and 
                        how to protect your computer at <a href="http://stopBadware.org">StopBadware.org</a> 
                        (Advisory provided by <a href="http://code.google.com/apis/safebrowsing/safebrowsing_faq.html#whyAdvisory">Google</a>).
                    </p>
                <% } %>
            </div>
        </div>
    </div>
</asp:Content>