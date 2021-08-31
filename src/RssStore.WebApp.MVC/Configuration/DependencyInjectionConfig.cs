using MediatR;
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
using RssStore.Core.DomainObjects.Messages.CommonMessages.Notifications;
using RssStore.Sales.Application.CommandHandlers;
using RssStore.Sales.Application.Commands;
using RssStore.Sales.Application.EventHandlers;
using RssStore.Sales.Application.Events;
using RssStore.Sales.Data;
using RssStore.Sales.Data.Repository;
using RssStore.Sales.Domain.Interfaces;

namespace RssStore.WebApp.MVC.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void ResolveDependencies(this IServiceCollection services)
        {
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


            //Sales
            //MediatR irá resolver o comando usando o handler
            services.AddScoped<IRequestHandler<AddOrderItemCommand, bool>, AddOrderItemCommandHandler>();
            //Context
            services.AddScoped<SalesContext>();
            //Repository
            services.AddScoped<IOrderRepository, OrderRepository>();
            //EventsHandler
            services.AddScoped<INotificationHandler<DraftOrderInitializedEvent>, OrderItemEventHandler>();
            services.AddScoped<INotificationHandler<OrderItemAddedEvent>, OrderItemEventHandler>();
            services.AddScoped<INotificationHandler<UpdatedOrderEvent>, OrderItemEventHandler>();
        }


    }
}
