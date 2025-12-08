using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Domain.Entites
{
    public sealed class PlanFeature
    {
        public Guid Id { get; set; } 
        public int LimitValue { get; set; }
        public string LimitUnit { get; set; } = string.Empty;
        public string? Note { get; set; }


        public Guid PlanId { get; set; } 
        public Plan Plan { get; set; } = null!;

        public Guid FeatureId { get; set; }
        public Feature Feature { get; set; } = null!;
    }
}
