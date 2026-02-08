using Application.Constants;
using Application.Contracts.Repositories;
using Application.Features.Courses.Dtos;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Courses.Queries.GetStatistics
{
    internal sealed class GetStatisticsQueryHandler : IRequestHandler<GetStatisticsQuery, StatisticsDto>
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HybridCache _hybridCache;
        public GetStatisticsQueryHandler(ICourseRepository courseRepository, IHttpContextAccessor httpContextAccessor, HybridCache hybridCache)
        {
            _courseRepository = courseRepository;
            _httpContextAccessor = httpContextAccessor;
            _hybridCache = hybridCache;
        }
        public async Task<StatisticsDto> Handle(GetStatisticsQuery request, CancellationToken cancellationToken)
        {
            var subdomain = _httpContextAccessor?.HttpContext?.Request.Cookies[AuthConstants.SubDomain];
            var cacheKey = $"{CacheKeysConstants.CourseStatisticsKey}_{subdomain}";
            var statistics = await _hybridCache.GetOrCreateAsync(
                cacheKey,
                async ct => await _courseRepository.GetCourseStatisticsAsync(subdomain!, cancellationToken),
                new HybridCacheEntryOptions
                {
                    Expiration = TimeSpan.FromMinutes(30)
                },
                cancellationToken: cancellationToken
            );
            return statistics;
        }
    }
}
