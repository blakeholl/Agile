using System.Net;
using System.Runtime.Remoting.Messaging;
using System.Threading.Tasks;
using Agile.Common.Cqrs;
using Agile.Common.Cqrs.Implementation.Persistence;
using Agile.Common.Cqrs.Persistence;
using Agile.Planning.Domain.CommandHandlers;
using Agile.Planning.Domain.CommandHandlers.Products;
using Agile.Planning.Domain.CommandHandlers.Stories;
using Agile.Planning.Domain.Commands;
using Agile.Planning.Domain.Commands.Products;
using Agile.Planning.Domain.Commands.Stories;
using EventStore.ClientAPI;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(Agile.Web.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(Agile.Web.App_Start.NinjectWebCommon), "Stop")]

namespace Agile.Web.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<IEventStoreConnection>()
                .ToMethod(x =>
                {
                    var connection = EventStoreConnection.Create(new IPEndPoint(IPAddress.Loopback, 1113));
                    connection.ConnectAsync().Wait();
                    return connection;
                })
                .InSingletonScope();

            kernel.Bind<IRepository>()
                .To<GetEventStoreRepository>()
                .InRequestScope();

            kernel.Bind<ICommandHandler<AddStoryCommand>>()
                .To<AddStoryComandHandler>()
                .InRequestScope();

            kernel.Bind<ICommandHandler<ChangeStoryTitleCommand>>()
                .To<ChangeStoryTitleCommandHandler>()
                .InRequestScope();

            kernel.Bind<ICommandHandler<DeleteStoryCommand>>()
                .To<DeleteStoryCommandHandler>()
                .InRequestScope();

            kernel.Bind<ICommandHandler<AddProductCommand>>()
                .To<AddProductCommandHandler>()
                .InRequestScope();
        }        
    }
}
