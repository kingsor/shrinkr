namespace Shrinkr.Web.UnitTests
{
    using System.Web.Routing;
    using Microsoft.Practices.ServiceLocation;

    using Moq;

    public class RegisterRoutes : ConfigureRoutes
    {
        public void Execute()
        {
            var serviceLocator = new Mock<IServiceLocator>();

            serviceLocator.Setup(sl => sl.GetInstance<RouteCollection>()).Returns(RouteTable.Routes);

            Execute(serviceLocator.Object);
        }
    }
}