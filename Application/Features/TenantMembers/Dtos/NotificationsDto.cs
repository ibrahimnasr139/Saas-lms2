namespace Application.Features.TenantMembers.Dtos
{
    public sealed class NotificationsDto
    {
        public bool NewStudents { get; set; }
        public bool Assignments { get; set; }
        public bool LiveSession { get; set; }
        public bool WeeklyReport { get; set; }
        public bool ProfileVisibility { get; set; }
        public bool ShareProgress { get; set; }
    }
}
