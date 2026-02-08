using Application.Features.Courses.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Courses.Queries.GetAll
{
    public sealed record GetAllQuery(string? Q, int? GradeId, int? SubjectId, string? SortDate, string? SortStudents, string? SortCompletion, int? Cursor)
        : IRequest<AllCoursesDto>;
}
