using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Todo.Core.Entities.Models;

namespace Todo.Infrastructure.Configurations
{
    public class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.HasKey(a => a.Id);
            builder.HasIndex(a => a.Email).IsUnique();
            builder.Property(a => a.HashPassword).HasMaxLength(256).IsRequired();
            builder.Property(a => a.Token).HasMaxLength(256).IsRequired(false);
            builder.Property(a => a.TokenValidBefore).IsRequired(false);
            builder.Property(a => a.CreatedAt).IsRequired();

            builder.HasMany(e => e.TodoGroups)
                   .WithOne(e => e.Account)
                   .HasForeignKey(e => e.AccountId);
        }
    }
}