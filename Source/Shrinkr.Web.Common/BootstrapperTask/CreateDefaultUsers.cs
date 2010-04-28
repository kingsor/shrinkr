namespace Shrinkr.Web
{
    using System.Collections.Generic;

    using DomainObjects;
    using Infrastructure;

    using Microsoft.Practices.ServiceLocation;
    using MvcExtensions;

    using Repositories;

    public class CreateDefaultUsers : BootstrapperTask
    {
        protected override TaskContinuation ExecuteCore(IServiceLocator serviceLocator)
        {
            Check.Argument.IsNotNull(serviceLocator, "serviceLocator");

            IUserRepository userRepository = serviceLocator.GetInstance<IUserRepository>();
            IUnitOfWork unitOfWork = serviceLocator.GetInstance<IUnitOfWork>();
            IEnumerable<User> users = serviceLocator.GetInstance<Settings>().DefaultUsers;

            bool shouldCommit = false;

            foreach (User user in users)
            {
                if (userRepository.GetByName(user.Name) == null)
                {
                    user.AllowApiAccess(ApiSetting.InfiniteLimit);

                    userRepository.Add(user);
                    shouldCommit = true;
                }
            }

            if (shouldCommit)
            {
                unitOfWork.Commit();
            }

            return TaskContinuation.Continue;
        }
    }
}