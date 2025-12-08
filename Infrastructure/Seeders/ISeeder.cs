using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Seeders
{
    public interface ISeeder
    {
        Task SeedAsync();
    }
}
