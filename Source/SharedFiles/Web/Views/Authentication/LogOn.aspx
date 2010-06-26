<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Shrinkr : Log On
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="shrinkBox">
        <h2>Log on with your existing account:</h2>
        <div class="shrinkForm">
            <form class="openid" action="<%= ViewContext.HttpContext.Request.RawUrl %>" method="post">
                <div>
                    <ul>
                        <li class="openid" title="OpenID">
                            <img src="<%= Url.OpenIdIcon("openidW") %>" alt="openid" />
                            <span><strong>http://{your-openid-url}</strong></span>
                        </li>
                        <li class="direct" title="Google">
                            <img src="<%= Url.OpenIdIcon("googleW") %>" alt="google" /><span>https://www.google.com/accounts/o8/id</span>
                        </li>
                        <li class="direct" title="Yahoo">
                            <img src="<%= Url.OpenIdIcon("yahooW") %>" alt="yahoo" /><span>http://yahoo.com/</span>
                        </li>
                        <li class="username" title="AOL screen name">
                            <img src="<%= Url.OpenIdIcon("aolW") %>" alt="aol" /><span>http://openid.aol.com/<strong>username</strong></span>
                        </li>
                    </ul>
                    <br class="clear" />
                </div>
                <div>
                    <ul>
                        <li class="username" title="MyOpenID user name">
                            <img src="<%= Url.OpenIdIcon("myopenid") %>" alt="myopenid" /><span>http://<strong>username</strong>.myopenid.com/</span>
                        </li>
                        <li class="username" title="Flickr user name">
                            <img src="<%= Url.OpenIdIcon("flickr") %>" alt="flickr" /><span>http://flickr.com/<strong>username</strong>/</span>
                        </li>
                        <li class="username" title="Technorati user name">
                            <img src="<%= Url.OpenIdIcon("technorati") %>" alt="technorati" /><span>http://technorati.com/people/technorati/<strong>username</strong>/</span>
                        </li>
                        <li class="username" title="Wordpress blog name">
                            <img src="<%= Url.OpenIdIcon("wordpress") %>" alt="wordpress" /><span>http://<strong>username</strong>.wordpress.com</span>
                        </li>
                        <li class="username" title="Blogger blog name">
                            <img src="<%= Url.OpenIdIcon("blogger") %>" alt="blogger" /><span>http://<strong>username</strong>.blogspot.com/</span>
                        </li>
                        <li class="username" title="LiveJournal blog name">
                            <img src="<%= Url.OpenIdIcon("livejournal") %>" alt="livejournal" /><span>http://<strong>username</strong>.livejournal.com</span>
                        </li>
                        <li class="username" title="ClaimID user name">
                            <img src="<%= Url.OpenIdIcon("claimid") %>" alt="claimid" /><span>http://claimid.com/<strong>username</strong></span>
                        </li>
                        <li class="username" title="Vidoop user name">
                            <img src="<%= Url.OpenIdIcon("vidoop") %>" alt="vidoop" /><span>http://<strong>username</strong>.myvidoop.com/</span>
                        </li>
                        <li class="username" title="Verisign user name">
                            <img src="<%= Url.OpenIdIcon("verisign") %>" alt="verisign" /><span>http://<strong>username</strong>.pip.verisignlabs.com/</span>
                        </li>
                    </ul>
                    <br class="clear" />
                </div>
                <fieldset>
                    <label for="openid_username">
                        Enter your <span>Provider user name</span>
                    </label>
                    <div>
                        <span></span>
                        <input type="text" id="openid_username" name="openid_username" class="largeTextBox"/><span></span>
                        <%= Html.CustomValidationMessage("userName")%>
                        <input type="submit" value="Log on" class="largeButton"/>
                        <label><input id="openid_username_remember" name="openid_username_remember" type="checkbox" value="true"/>Keep me logged in on this computer</label>
                    </div>
                </fieldset>
                <fieldset>
                    <label for="openid_identifier">
                        Enter your <a class="openid_logo" href="http://openid.net">OpenID</a>
                    </label>
                    <div>
                        <input type="text" id="openid_identifier" name="openid_identifier" class="largeTextBox"/>
                        <%= Html.CustomValidationMessage("userName")%>
                        <input type="submit" value="Log on" class="largeButton"/>
                        <label><input id="openid_identifier_remember" name="openid_identifier_remember" type="checkbox" value="true"/>Keep me logged in on this computer</label>
                    </div>
                </fieldset>
                <%= Html.ValidationMessage("userName", new { style = "padding:0 10px" })%>
            </form>
        </div>
    </div>
    <% Html.Telerik()
           .ScriptRegistrar()
           .OnDocumentReady(() =>
                            {%>
                                $('form.openid').openid();
                            <%}
                           ); %>
</asp:Content>