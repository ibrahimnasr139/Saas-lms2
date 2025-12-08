using Application.Features.Plan.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
namespace Application.Features.Plan.Queries
{
    public sealed class GetAllPlansQuery : IRequest<IEnumerable<PlanResponse>>
    {
    }
}
