namespace Shrinkr.Web
{
    using System.Web.Mvc;

    using DomainObjects;

    using MvcExtensions;

    public class ConfigureFilters : ConfigureFiltersBase
    {
        protected override void Configure(IFilterRegistry registry)
        {
            registry.Register<AuthenticationController, ShrinkrAuthorizeAttribute, UpdateUserLastActivityAttribute>(c => c.LogOff())
                    .Register<AuthenticationController, ShrinkrAuthorizeAttribute>(c => c.LogOff(null))
                    .Register<AuthenticationController, CompressAttribute>(attribute => { attribute.Order = int.MaxValue - 1; })
                    .Register<UserController, ShrinkrAuthorizeAttribute, UpdateUserLastActivityAttribute, CompressAttribute>()
                    .Register<ShortUrlController, UpdateUserLastActivityAttribute, CompressAttribute>()
                    .Register<ShortUrlController, ShrinkrAuthorizeAttribute>(c => c.List(null))
                    .Register<ControlPanelController, ShrinkrAuthorizeAttribute>(attribute => { attribute.AllowedRole = Role.Administrator; })
                    .Register<ControlPanelController, CompressAttribute>()
                    .Register<ElmahHandleErrorAttribute>(new TypeCatalogBuilder().Add(GetType().Assembly).Include(type => typeof(Controller).IsAssignableFrom(type)));
        }
    }
}