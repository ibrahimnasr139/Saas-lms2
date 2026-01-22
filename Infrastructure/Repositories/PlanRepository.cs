using Application.Features.Plan.DTOs;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories
{
    internal sealed class PlanRepository : IPlanRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public PlanRepository(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<IEnumerable<PlanDto>> GetAllPlansWithDetailsAsync(CancellationToken cancellationToken)
        {
            return await _context.Plans
                    .Where(p => p.Slug != "free-trial")
                    .AsNoTracking()
                    .ProjectTo<PlanDto>(_mapper.ConfigurationProvider) 
                    .ToListAsync(cancellationToken);
        }

        public async Task<Guid> GetFreePlanPricingIdAsync(CancellationToken cancellationToken)
        {
            var planPricing = await _context.PlanPricings.FirstOrDefaultAsync(p => p.Price == 0);
            return planPricing!.Id;
        }
    }
}
