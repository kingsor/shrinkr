namespace Shrinkr.Web
{
    using Microsoft.Practices.ServiceLocation;

    using MvcExtensions;

    using Telerik.Web.Mvc;

    public class ConfigureAssets : BootstrapperTask
    {
        protected override TaskContinuation ExecuteCore(IServiceLocator serviceLocator)
        {
            WebAssetDefaultSettings.UseTelerikContentDeliveryNetwork = true;
            WebAssetDefaultSettings.Combined = true;

            SharedWebAssets.StyleSheets(
                                        group => group.AddGroup(
                                                    "appStyles",
                                                    styles => 
                                                        styles.Add("site.css")
                                                              .Add("openid.css")
                                                              .Add("form.css")
                                                              .Add("telerik.common.css")
                                                              .Add("telerik.forest.css")));

            SharedWebAssets.Scripts(
                                        group => group.AddGroup(
                                                    "publicScripts",
                                                    scripts => 
                                                        scripts.Add("jquery.validate.js")
                                                               .Add("jquery.form.js")
                                                               .Add("jquery.color.js")
                                                               .Add("jquery.watermark.js")
                                                               .Add("jquery.openid.js")
                                                               .Add("createShortUrl.js")
                                                               .Add("profile.js")));

            SharedWebAssets.Scripts(
                                        group => group.AddGroup(
                                                    "controlPanelScripts",
                                                    scripts =>
                                                        scripts.Add("administrativeItem.js")
                                                               .Add("bannedIpAddress.js")
                                                               .Add("bannedDomain.js")
                                                               .Add("reservedAlias.js")
                                                               .Add("badWord.js")
                                                               .Add("url.js")
                                                               .Add("user.js")));

            return TaskContinuation.Continue;
        }
    }
}