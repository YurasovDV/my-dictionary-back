using DictionaryBack.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DictionaryBack.DAL.Configurations
{
    internal class TopicConfiguration : IEntityTypeConfiguration<Topic>
    {
        public void Configure(EntityTypeBuilder<Topic> builder)
        {
            builder.ToTable("topics");

            builder.HasKey(t => t.Id);

            builder.Property(t => t.Id).HasColumnName("id");

            builder.Property(t => t.Name).HasMaxLength(200).HasColumnName("name");

            builder.Property(c => c.Name).HasColumnType("citext");

            builder.Property(t => t.IsDeleted).HasDefaultValue(false).HasColumnName("is_deleted");

            // several users?
            builder.HasIndex(t => t.Name).IsUnique();
        }
    }
}
