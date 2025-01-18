using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Todo.Core.Entities.Models;

namespace Todo.Infrastructure.Configurations
{
    public class RequestLogConfiguration : IEntityTypeConfiguration<RequestLog>
    {
        public void Configure(EntityTypeBuilder<RequestLog> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.IpAddress).IsRequired();
            builder.Property(x => x.Method).IsRequired();
            builder.Property(x => x.Path).IsRequired();
            builder.Property(x => x.CreatedAt).IsRequired();
        }
    }
}