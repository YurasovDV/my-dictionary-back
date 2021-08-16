using DictionaryBack.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DictionaryBack.DAL.Configurations
{
    internal class TranslationConfiguration : IEntityTypeConfiguration<Translation>
    {
        public void Configure(EntityTypeBuilder<Translation> builder)
        {
            builder.ToTable("translations");

            builder.HasKey(w => new { w.TermId, w.Meaning });

            builder.Property(w => w.Meaning).HasMaxLength(200).HasColumnName("meaning");

            builder.Property(w => w.IsDeleted).HasDefaultValue(false).HasColumnName("is_deleted");
        }
    }
}
