using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Plan.DTOs
{
    public sealed class PlanPricingResponse
    {
        public string Id { get; set; }
        public decimal Price { get; set; }
        public string Currency { get; set; }
        public string BillingCycle { get; set; }
        public decimal Discount { get; set; }
    }
}
