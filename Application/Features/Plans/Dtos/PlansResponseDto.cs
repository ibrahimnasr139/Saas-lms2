using Application.Features.Plan.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Plans.Dtos
{
    public sealed class PlansResponseDto
    {
        public IEnumerable<PlanDto> Plans { get; init; } = [];
    }
}
