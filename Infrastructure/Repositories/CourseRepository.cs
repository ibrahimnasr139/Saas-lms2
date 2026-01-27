using Application.Features.Courses.Dtos;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories
{
    internal sealed class CourseRepository : ICourseRepository
    {
        private readonly AppDbContext _dbContext;
        public CourseRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<StatisticsDto> GetCourseStatisticsAsync(string tenantSubdomain, CancellationToken cancellationToken)
        {
            var response =  await _dbContext.Tenants
                .Where(t => t.SubDomain == tenantSubdomain)
                .Select(t => new StatisticsDto
                {
                    TotalCourses = t.Courses.Count(),
                    ActiveCourses = t.Courses.Count(c => c.CourseStatus == CourseStatus.Published),
                    TotalStudentsEnrolled = t.Courses.SelectMany(e => e.Enrollments).Select(x => x.StudentId).Distinct().Count()
                })
                .FirstOrDefaultAsync(cancellationToken);
            if (response != null)
            {
                response.DraftCourses = response.TotalCourses - response.ActiveCourses;
            }
            return response!;
        }
    }
}
