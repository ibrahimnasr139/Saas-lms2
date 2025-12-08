using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Plan.DTOs
{
    public sealed class PlanResponse
    {

        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Slug { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime? updatedAt { get; set; }
        public IEnumerable<PlanPricingResponse> PlanPricings { get; set; }
        public IEnumerable<PlanFeatureResponse> PlanFeatures{ get; set; }
    }
}
