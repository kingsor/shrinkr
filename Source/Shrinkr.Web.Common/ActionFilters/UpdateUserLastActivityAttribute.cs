namespace Shrinkr.Web
{
    using System;
    using System.Web.Mvc;

    using Services;

    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = false), CLSCompliant(false)]
    public class UpdateUserLastActivityAttribute : FilterAttribute, IResultFilter
    {
        public UpdateUserLastActivityAttribute(IUserService userService)
        {
            Check.Argument.IsNotNull(userService, "userService");

            UserService = userService;
        }

        public IUserService UserService
        {
            get;
            private set;
        }

        public void OnResultExecuting(ResultExecutingContext filterContext)
        {
            // Do nothing, just sleep.
        }

        public void OnResultExecuted(ResultExecutedContext filterContext)
        {
            Check.Argument.IsNotNull(filterContext, "filterContext");

            string userName = filterContext.HttpContext.User.Identity.IsAuthenticated ?
                              filterContext.HttpContext.User.Identity.Name :
                              null;

            if (!string.IsNullOrEmpty(userName))
            {
                UserService.UpdateLastActivity(userName);
            }
        }
    }
}