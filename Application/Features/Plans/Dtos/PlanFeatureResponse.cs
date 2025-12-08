using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Plan.DTOs
{
    public sealed class PlanFeatureResponse
    {
        public string Id { get; set; }
        public string FeatureName { get; set; }
        public string FeatureDescription { get; set; }
        public string FeatureKey { get; set; }
        public int LimitValue { get; set; }
        public string LimitUnit { get; set; }
        public string? Note { get; set; }
    }
}
