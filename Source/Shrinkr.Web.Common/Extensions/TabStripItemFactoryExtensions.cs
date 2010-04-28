namespace Shrinkr.Web
{
    using Telerik.Web.Mvc.UI;

    public static class TabStripItemFactoryExtensions
    {
        public static TabStripItemFactory AddSummary(this TabStripItemFactory instance)
        {
            Check.Argument.IsNotNull(instance, "instance");

            instance.Add().Text("Summary").Action<ControlPanelController>(c => c.Summary());

            return instance;
        }

        public static TabStripItemFactory AddUrls(this TabStripItemFactory instance)
        {
            Check.Argument.IsNotNull(instance, "instance");

            instance.Add().Text("Urls").Action<ControlPanelController>(c => c.Urls(1, null, null));

            return instance;
        }

        public static TabStripItemFactory AddUsers(this TabStripItemFactory instance)
        {
            Check.Argument.IsNotNull(instance, "instance");

            instance.Add().Text("Users").Action<ControlPanelController>(c => c.Users(1, null, null));

            return instance;
        }

        public static TabStripItemFactory AddBannedIPs(this TabStripItemFactory instance)
        {
            Check.Argument.IsNotNull(instance, "instance");

            instance.Add().Text("Banned IPs").Action<ControlPanelController>(c => c.BannedIPAddresses(1));

            return instance;
        }

        public static TabStripItemFactory AddBannedDomains(this TabStripItemFactory instance)
        {
            Check.Argument.IsNotNull(instance, "instance");

            instance.Add().Text("Banned Domains").Action<ControlPanelController>(c => c.BannedDomains(1));

            return instance;
        }

        public static TabStripItemFactory AddReservedAliases(this TabStripItemFactory instance)
        {
            Check.Argument.IsNotNull(instance, "instance");

            instance.Add().Text("Reserved Aliases").Action<ControlPanelController>(c => c.ReservedAliases(1));

            return instance;
        }

        public static TabStripItemFactory AddBadWords(this TabStripItemFactory instance)
        {
            Check.Argument.IsNotNull(instance, "instance");

            instance.Add().Text("Bad Words").Action<ControlPanelController>(c => c.BadWords(1));

            return instance;
        }
    }
}