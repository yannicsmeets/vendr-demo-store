using Vendr.Umbraco;
using Vendr.Core.Events.Notification;
using Vendr.DemoStore.Events;
using Vendr.DemoStore.Web.Extractors;
using Vendr.Umbraco.Extractors;
using Vendr.Extensions;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Extensions;
using Umbraco.Cms.Core.Notifications;
using Vendr.Core.Calculators;
using Vendr.DemoStore.Calculators;

namespace Vendr.DemoStore.Composing
{
    [ComposeAfter(typeof(VendrComposer))]
    public class DemoStoreComposer : IComposer
    {
        public void Compose(IUmbracoBuilder builder)
        {
            // Replace the umbraco product name extractor with one that supports child variants
            builder.Services.AddUnique<IUmbracoProductNameExtractor, CompositeProductNameExtractor>();

            builder.Services.AddUnique<IShippingCalculator, CustomShippingCalculator>();

            // Register event handlers
            builder.WithNotificationEvent<OrderProductAddingNotification>()
                .RegisterHandler<OrderProductAddingHandler>();

            builder.WithNotificationEvent<OrderLineChangingNotification>()
                .RegisterHandler<OrderLineChangingHandler>();

            builder.WithNotificationEvent<OrderLineRemovingNotification>()
                .RegisterHandler<OrderLineRemovingHandler>();

            builder.WithNotificationEvent<OrderPaymentCountryRegionChangingNotification>()
                .RegisterHandler<OrderPaymentCountryRegionChangingHandler>();

            builder.WithNotificationEvent<OrderShippingCountryRegionChangingNotification>()
                .RegisterHandler<OrderShippingCountryRegionChangingHandler>();

            builder.WithNotificationEvent<OrderShippingMethodChangingNotification>()
                .RegisterHandler<OrderShippingMethodChangingHandler>();

            builder.AddNotificationHandler<UmbracoApplicationStartingNotification, TransformExamineValues>();
        }
    }
}
