<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<System.Web.Mvc.HandleErrorInfo>" %>
<script runat="server">
    string GetTitle()
    {
        var exception = Model.Exception as HttpException ?? Model.Exception.InnerException as HttpException;

        var title = "Error";

        if (exception != null)
        {
            title = exception.GetHttpCode().ToString(Shrinkr.Culture.Current);
        }

        return title;
    }
</script>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Shrinkr : Error
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="shrinkBox">
        <h2><%= GetTitle() %>:</h2>
        <div class="shrinkForm">
            <p class="warningText">
                Something unholy just happened, we are unable to process your request at this time. You can click
                on the following link to go back to homepage.
            </p>
            <p>
                <a class="command" href="<%= Url.Home() %>">« Continue</a>
            </p>
        </div>
    </div>
</asp:Content>