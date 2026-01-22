using Application.Features.Plan.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using Application.Features.Plans.Dtos;
namespace Application.Features.Plans.Queries.GetAllPlans
{
    public sealed class GetAllPlansQuery : IRequest<PlansResponseDto>
    {
    }
}
