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

            builder.Property(w => w.Term).HasColumnType("citext").HasColumnName("term");

            builder.Property(w => w.IsDeleted).HasDefaultValue(false).HasColumnName("is_deleted");

            builder.Property(w => w.Status).HasColumnName("status").HasConversion<int>().HasDefaultValue(WordStatus.Added);

            builder.Property(w => w.LastRepetition).IsRequired(false).HasColumnName("last_repetition");

            builder.Property(w => w.TopicId).HasColumnName("topic_id");

            builder.HasMany(w => w.Translations).WithOne().HasForeignKey(t => t.Term);

            builder.HasOne(w => w.Topic).WithMany().HasForeignKey(w => w.TopicId);

            builder.Navigation(w => w.Translations).AutoInclude();

            builder.Navigation(w => w.Topic).AutoInclude();

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
