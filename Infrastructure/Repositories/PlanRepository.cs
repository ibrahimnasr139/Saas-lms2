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
            this._context = context;
            this._mapper = mapper;
        }
        public async Task<IEnumerable<PlanResponse>> GetAllPlansWithDetailsAsync(CancellationToken cancellationToken)
        {
            return await _context.Plans
                    .AsNoTracking()
                    .ProjectTo<PlanResponse>(_mapper.ConfigurationProvider) 
                    .ToListAsync(cancellationToken);
        }
    }
}
