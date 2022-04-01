using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendr.Core.Calculators;
using Vendr.Core.Models;
using Vendr.Core.Services;

namespace Vendr.DemoStore.Calculators
{
    public class CustomShippingCalculator : ShippingCalculator
    {
        public CustomShippingCalculator(
            ITaxService taxService,
            IStoreService storeService
            ) : base(taxService, storeService)
        {
        }

        public override Price CalculateShippingMethodPrice(
            ShippingMethodReadOnly shippingMethod,
            Guid currencyId,
            Guid? countryId,
            Guid? regionId,
            TaxRate taxRate,
            ShippingCalculatorContext context)
        {
            var subtotalPrice = context.OrderCalculation?.SubtotalPrice.Value;
            if (subtotalPrice == null || subtotalPrice.WithTax == 0)
            {
                subtotalPrice = context.Order.SubtotalPrice.Value;
            }

            var shippingCost = subtotalPrice.WithTax > 10
                ? 5m
                : 10m;

            var shippingCostWithoutTax = shippingCost / (1 + taxRate.Value);
            var tax = shippingCost - shippingCostWithoutTax;

            var price = new Price(shippingCostWithoutTax, tax, currencyId);
            return price;
        }
    }
}
