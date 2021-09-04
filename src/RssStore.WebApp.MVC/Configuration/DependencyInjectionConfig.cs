using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RssStore.Catalog.Application.ApplicationServices;
using RssStore.Catalog.Application.Interfaces;
using RssStore.Catalog.Data;
using RssStore.Catalog.Data.Repository;
using RssStore.Catalog.Domain.EventHandlers;
using RssStore.Catalog.Domain.Events;
using RssStore.Catalog.Domain.Interfaces;
using RssStore.Catalog.Domain.Services;
using RssStore.Core.Communication.Mediator;
using RssStore.Core.DomainObjects.Messages.CommonMessages.IntegrationEvents;
using RssStore.Core.DomainObjects.Messages.CommonMessages.Notifications;
using RssStore.Payment.AntiCorruption;
using RssStore.Payment.AntiCorruption.Interfaces;
using RssStore.Payment.Business.Interfaces;
using RssStore.Payment.Business.Services;
using RssStore.Payment.Data.Context;
using RssStore.Payment.Data.Repositories;
using RssStore.Sales.Application.CommandHandlers;
using RssStore.Sales.Application.Commands;
using RssStore.Sales.Application.EventHandlers;
using RssStore.Sales.Application.Events;
using RssStore.Sales.Application.Queries;
using RssStore.Sales.Application.Queries.Interfaces;
using RssStore.Sales.Data;
using RssStore.Sales.Data.Repository;
using RssStore.Sales.Domain.Interfaces;
using RssStore.WebApp.MVC.Data;

namespace RssStore.WebApp.MVC.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void ResolveDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
               options.UseSqlServer(
                   configuration.GetConnectionString("DefaultConnection")));

            services.AddDbContext<CatalogDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection")));

            services.AddDbContext<SalesContext>(options =>
               options.UseSqlServer(
                   configuration.GetConnectionString("DefaultConnection")));

            services.AddDbContext<PaymentContext>(options =>
               options.UseSqlServer(
                   configuration.GetConnectionString("DefaultConnection")));

            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();

            //Event Bus
            services.AddScoped<IMediatorHandler, MediatorHandler>();

            services.AddScoped<INotificationHandler<DomainNotification>, DomainNotificationHandler>();

            //Catolog
            //Repository
            services.AddScoped<IProductRepository, ProductRepository>();
            //Application
            services.AddScoped<IProductAppService, ProductAppService>();
            //Domain
            services.AddScoped<IStockService, StockService>();
            //Context
            services.AddScoped<CatalogDbContext>();

            //MediatR Handler para evento de estoque
            services.AddScoped<INotificationHandler<LowProductStockEvent>, ProductStockEventHandler>();
            services.AddScoped<INotificationHandler<IniciatedOrderEvent>, ProductStockEventHandler>();
            services.AddScoped<INotificationHandler<OrderProcessCancelledEvent>, ProductStockEventHandler>();

            //Sales
            //MediatR irá resolver o comando usando o handler
            services.AddScoped<IRequestHandler<AddOrderItemCommand, bool>, OrderCommandHandler>();
            services.AddScoped<IRequestHandler<ApplyVoucherOrderCommand, bool>, OrderCommandHandler>();
            services.AddScoped<IRequestHandler<RemoveOrderItemCommand, bool>, OrderCommandHandler>();
            services.AddScoped<IRequestHandler<UpdateOrderItemCommand, bool>, OrderCommandHandler>();
            services.AddScoped<IRequestHandler<IniciateOrderCommand, bool>, OrderCommandHandler>();
            services.AddScoped<IRequestHandler<FinishOrderCommand, bool>, OrderCommandHandler>();
            services.AddScoped<IRequestHandler<CancelOrderProcessReturnStockCommand, bool>, OrderCommandHandler>();
            services.AddScoped<IRequestHandler<CancelOrderProcessCommand, bool>, OrderCommandHandler>();

            //Context
            services.AddScoped<SalesContext>();
            //Repository
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IOrderQueries, OrderQueries>();
            //EventsHandler
            services.AddScoped<INotificationHandler<DraftOrderInitializedEvent>, OrderEventHandler>();
            services.AddScoped<INotificationHandler<OrderItemAddedEvent>, OrderEventHandler>();
            services.AddScoped<INotificationHandler<UpdatedOrderEvent>, OrderEventHandler>();
            services.AddScoped<INotificationHandler<RejectedOrderStockEvent>, OrderEventHandler>();
            services.AddScoped<INotificationHandler<AuthorizedPaymentEvent>, OrderEventHandler>();
            services.AddScoped<INotificationHandler<UnauthorizedPaymentEvent>, OrderEventHandler>();

            //Payment
            //Context
            services.AddScoped<PaymentContext>();
            //Repository
            services.AddScoped<IPaymentRepository, PaymentRepository>();
            //Services
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<ICreditCardPaymentFacade, CreditCardPaymentFacade>();
            //ExternalServices
            services.AddScoped<IPayPalGateway, PayPalGateway>();
            services.AddScoped<IConfigurationManager, ConfigurationManager>();
        }
    }
}
