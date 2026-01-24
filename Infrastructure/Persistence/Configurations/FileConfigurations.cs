using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Text.Json;

namespace Infrastructure.Persistence.Configurations
{
    internal class FileConfigurations : IEntityTypeConfiguration<Domain.Entites.File>
    {
        public void Configure(EntityTypeBuilder<Domain.Entites.File> builder)
        {
            var converter = new ValueConverter<Dictionary<string, string>, string>(
           v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null!),
           v => JsonSerializer.Deserialize<Dictionary<string, string>>(v, (JsonSerializerOptions)null!)!
       );
            builder.Property(f => f.Size)
                   .HasMaxLength(1100000000)
                   .IsRequired();
            builder.Property(f => f.StorageProvider)
                   .HasMaxLength(100)
                   .IsRequired();
            builder.Property(f => f.Metadata)
                .HasConversion(converter!)
                .HasColumnType("jsonb");
            builder.HasOne(f => f.Tenant)
                   .WithMany()
                   .HasForeignKey(f => f.TenantId)
                   .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(f => f.UploadedBy)
                   .WithMany()
                   .HasForeignKey(f => f.UploadedById)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
