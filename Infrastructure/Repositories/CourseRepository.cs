using Application.Features.Courses.Dtos;
using AutoMapper;
using Domain.Enums;
using Infrastructure.Constants;
namespace Infrastructure.Repositories
{
    internal sealed class CourseRepository : ICourseRepository
    {
        private readonly AppDbContext _dbContext;
        public CourseRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<AllCoursesDto> GetAllCoursesAsync(string subDomain, string? q, int? gradeId, int? subjectId, string? sortDate, string? sortStudents, string? sortCompletion, int? cursor, CancellationToken cancellationToken)
        {
            var query = _dbContext.Courses.Where(c => c.Tenant.SubDomain == subDomain).AsNoTracking();
            if (!string.IsNullOrEmpty(q))
            {
                query = query.Where(c => c.Title.Contains(q) || c.Description.Contains(q));
            }
            if (gradeId.HasValue)
            {
                query = query.Where(c => c.GradeId == gradeId.Value);
            }
            if (subjectId.HasValue)
            {
                query = query.Where(c => c.SubjectId == subjectId.Value);
            }
            if (cursor.HasValue)
            {
                query = query.Where(c => c.Id > cursor.Value);
            }
            var studentCountQuery = _dbContext.Enrollments.AsNoTracking().Where(e => e.Course.Tenant.SubDomain == subDomain)
                .GroupBy(e => e.CourseId)
                .Select(g => new { CourseId = g.Key, StudentCount = g.Select(e => e.StudentId).Count().ToString()})
                .DefaultIfEmpty();
            var courseProgressQuery = _dbContext.CourseProgresses.AsNoTracking().Where(p => p.Course.Tenant.SubDomain == subDomain)
                .GroupBy(e => e.CourseId)
                .Select(g => new { CourseId = g.Key, CompletionRate = g.Where(p => p.TotalLessons > 0).Average(p => (double)p.CompletedLessons / p.TotalLessons).ToString()});
            var queryWithCounts = query.LeftJoin(studentCountQuery, c => c.Id, sc => sc.CourseId, (c, sc) => new { Course = c, StudentCount = sc != null ? sc.StudentCount : null! })
                .LeftJoin(courseProgressQuery, c => c.Course.Id, cp => cp.CourseId, (c, cp) => new { c.Course, c.StudentCount, CompletionRate = cp != null ? cp.CompletionRate: null! });
            if (!string.IsNullOrEmpty(sortDate))
            {
                queryWithCounts = sortDate == SortDirections.Ascending
                    ? queryWithCounts.OrderBy(c => c.Course.CreatedAt)
                    : queryWithCounts.OrderByDescending(c => c.Course.CreatedAt);
            }
            else if (!string.IsNullOrEmpty(sortStudents))
            {
                queryWithCounts = sortStudents == SortDirections.Ascending
                    ? queryWithCounts.OrderBy(c => c.StudentCount)
                    : queryWithCounts.OrderByDescending(c => c.StudentCount);
            }
            else if (!string.IsNullOrEmpty(sortCompletion))
            {
                queryWithCounts = sortCompletion == SortDirections.Ascending
                    ? queryWithCounts.OrderBy(c => c.CompletionRate)
                    : queryWithCounts.OrderByDescending(c => c.CompletionRate);
            }
            else
            {
                queryWithCounts = queryWithCounts.OrderBy(c => c.Course.Id);
            }
            var courses = await queryWithCounts
                .Take(PaginationLimits.CoursesPageSize + 1)
                .Select(a => new CourseResponseDto
                {
                    Id = a.Course.Id,
                    Title = a.Course.Title,
                    Description = a.Course.Description,
                    Grade = a.Course.Grade.Label,
                    Subject = a.Course.Subject.Label,
                    Status = a.Course.CourseStatus,
                    CreatedAt = a.Course.CreatedAt,
                    StudentsCount = a.StudentCount != null ? int.Parse(a.StudentCount) : 0,
                    CompletionRate = a.CompletionRate != null ? double.Parse(a.CompletionRate) : 0.0,
                })
                .ToListAsync(cancellationToken);
            var hasMore = courses.Count > PaginationLimits.CoursesPageSize;
            if (hasMore)
            {
                courses.RemoveAt(courses.Count - 1);
            }
            var nextCursor = courses.LastOrDefault()?.Id;
            return new AllCoursesDto
            {
                Data = courses,
                HasMore = hasMore,
                NextCursor = nextCursor ?? 0
            };
        }

        public async Task<StatisticsDto> GetCourseStatisticsAsync(string tenantSubdomain, CancellationToken cancellationToken)
        {
            var response = await _dbContext.Tenants
                .Where(t => t.SubDomain == tenantSubdomain)
                .Select(t => new StatisticsDto
                {
                    TotalCourses = t.Courses.Count(),
                    ActiveCourses = t.Courses.Count(c => c.CourseStatus == CourseStatus.Published),
                    TotalStudentsEnrolled = t.Courses.SelectMany(e => e.Enrollments).Select(x => x.StudentId).Distinct().Count(),

                })
                .FirstOrDefaultAsync(cancellationToken);
            if (response != null)
            {
                response.DraftCourses = response.TotalCourses - response.ActiveCourses;
                var averages = await _dbContext.Courses.Where(c => c.Tenant.SubDomain == tenantSubdomain)
                    .SelectMany(c => c.CourseProgresses).Where(p => p.TotalLessons > 0)
                    .Select(p => (double)p.CompletedLessons / p.TotalLessons).ToListAsync(cancellationToken);
                response.AverageCompletionRate = averages.Count > 0 ? averages.Average() : 0.0;
            }
            return response!;
        }
    }
}
