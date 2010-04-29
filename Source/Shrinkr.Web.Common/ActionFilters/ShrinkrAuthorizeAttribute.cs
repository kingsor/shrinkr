namespace Shrinkr.Web
{
    using System;
    using System.Security.Principal;
    using System.Web.Mvc;

    using DataTransferObjects;
    using DomainObjects;
    using MvcExtensions;
    using Services;

    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = false), CLSCompliant(false)]
    public class ShrinkrAuthorizeAttribute : ExtendedAuthorizeAttribute
    {
        public ShrinkrAuthorizeAttribute(IUserService userService)
        {
            Check.Argument.IsNotNull(userService, "userService");

            UserService = userService;
        }

        public IUserService UserService
        {
            get;
            private set;
        }

        public Role? AllowedRole
        {
            get;
            set;
        }

        public override bool IsAuthorized(AuthorizationContext filterContext)
        {
            Check.Argument.IsNotNull(filterContext, "filterContext");

            IPrincipal principal = filterContext.HttpContext.User;

            if (principal.Identity.IsAuthenticated)
            {
                UserDTO user = UserService.GetByName(principal.Identity.Name);

                if ((user != null) && !user.IsLockedOut)
                {
                    if (AllowedRole.HasValue)
                    {
                        return user.Role == AllowedRole.Value;
                    }

                    return true;
                }
            }

            return false;
        }
    }
}