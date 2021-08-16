using DictionaryBack.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.Json;

namespace DictionaryBack.DAL.Configurations
{
    internal class WordConfiguration : IEntityTypeConfiguration<Word>
    {
        public void Configure(EntityTypeBuilder<Word> builder)
        {
            builder.HasKey(w => w.Term);

            builder.Property(w => w.Term).HasMaxLength(200);

            builder.Property(w => w.Topic).HasDefaultValue("user");

            builder.Property(w => w.Translation)
                .HasConversion(
                    // PG has array type but it won't work with mssql
                    translations => JsonSerializer.Serialize(translations, null),
                    dbValue => JsonSerializer.Deserialize<string[]>(dbValue, null))
                .IsRequired();
        }
    }
}
