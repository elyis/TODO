using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Todo.Core.Entities.Models;

namespace Todo.Infrastructure.Configurations
{
    public class TodoGroupConfiguration : IEntityTypeConfiguration<TodoGroup>
    {
        public void Configure(EntityTypeBuilder<TodoGroup> builder)
        {
            builder.HasKey(tg => tg.Id);
            builder.HasIndex(tg => tg.Name);

            builder.HasMany(e => e.TodoItems)
                   .WithOne(e => e.TodoGroup)
                   .HasForeignKey(e => e.TodoGroupId);

            builder.HasOne(e => e.Account)
                   .WithMany(e => e.TodoGroups)
                   .HasForeignKey(e => e.AccountId);
        }
    }
}