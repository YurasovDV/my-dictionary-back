using DictionaryBack.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DictionaryBack.DAL.Configurations
{
    internal class WordConfiguration : IEntityTypeConfiguration<Word>
    {
        public void Configure(EntityTypeBuilder<Word> builder)
        {
            builder.ToTable("words");

            builder.HasKey(w => w.Term);

            builder.Property(w => w.Term).HasMaxLength(200).HasColumnName("term");

            builder.Property(w => w.Topic).HasDefaultValue("user").HasColumnName("topic");

            builder.Property(w => w.IsDeleted).HasDefaultValue(false).HasColumnName("is_deleted");

            // big thank you dapper

            // builder.Property(w => w.Translations)
            //     .HasConversion(
            //         // PG has array type but it won't work with mssql
            //         translations => JsonSerializer.Serialize(translations, null),
            //         dbValue => JsonSerializer.Deserialize<string[]>(dbValue, null))
            //     .IsRequired()
            //     .HasColumnName("translations");             
        }
    }
}
