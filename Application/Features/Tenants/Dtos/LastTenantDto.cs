using Domain.Entites;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Tenants.Dtos
{
    public sealed class LastTenantDto
    {
        public int Id { get; set; }
        public string PlatformName { get; set; } = string.Empty;
        public string? Logo { get; set; }
        public List<LabelValueIdDto> Subjects { get; set; }
        public List<LabelValueIdDto> TeachingLevels { get; set; }
        public List<LabelValueIdDto> Grades { get; set; } 
        public string SubDomain { get; set; } = string.Empty;
    }
}
