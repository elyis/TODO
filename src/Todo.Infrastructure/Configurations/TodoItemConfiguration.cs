using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Todo.Core.Entities.Models;

namespace Todo.Infrastructure.Configurations
{
    public class TodoItemConfiguration : IEntityTypeConfiguration<TodoItem>
    {
        public void Configure(EntityTypeBuilder<TodoItem> builder)
        {
            builder.HasKey(ti => ti.Id);
            builder.Property(ti => ti.Name).HasMaxLength(256).IsRequired();
            builder.Property(ti => ti.Description).HasMaxLength(1024).IsRequired();
            builder.Property(ti => ti.IsDone).IsRequired();
            builder.Property(ti => ti.CreatedAt).IsRequired();

            builder.HasOne(e => e.TodoGroup)
                   .WithMany(e => e.TodoItems)
                   .HasForeignKey(e => e.TodoGroupId);
        }
    }
}