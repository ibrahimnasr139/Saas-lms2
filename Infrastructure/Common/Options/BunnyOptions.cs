using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Common.Options
{
    public sealed class BunnyOptions
    {
        public string AccessKey { get; set; } = string.Empty;
        public string StorageZoneName { get; set; } = string.Empty;
        public string CdnUrl { get; set; } = string.Empty;
        public string HostUrl { get; set; } = string.Empty;
        }
}
