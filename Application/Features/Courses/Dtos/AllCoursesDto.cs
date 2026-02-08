using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Courses.Dtos
{
    public sealed class AllCoursesDto
    {
        public IEnumerable<CourseResponseDto> Data { get; set; } = [];
        public bool HasMore { get; set; }
        public int NextCursor { get; set; }
    }
}
