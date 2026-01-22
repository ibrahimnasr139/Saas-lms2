using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Plan.DTOs
{
    public sealed class PlanDto
    {

        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public DateTime createdAt { get; set; }
        public DateTime? updatedAt { get; set; }
        public IEnumerable<PlanPricingResponse> PlanPricing { get; set; } = [];
        public IEnumerable<PlanFeatureResponse> PlanFeatures { get; set; } = [];
    }
}
