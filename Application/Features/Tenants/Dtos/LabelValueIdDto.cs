using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Tenants.Dtos
{
    public sealed record LabelValueIdDto(int Id, string Value, string Label);
}
