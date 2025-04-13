using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SpeakEase.Domain;
using SpeakEase.Domain.Word;
using SpeakEase.Domain.Word.Enum;

namespace SpeakEase.Infrastructure.EntityFrameworkCore.ModelBuilders
{
    public static class DictionaryWordModelBuilder
    {
        public static ModelBuilder ConfigureModelDictionaryWord(this ModelBuilder builder)
        {

            builder.Entity<DictionaryWordEntity>(op =>
            {
                op.ToTable("dictionary_word");
                op.HasKey(p => p.Id);
                op.Property(p => p.Word).HasMaxLength(50).IsRequired();
                op.Property(p => p.Phonetic).HasMaxLength(100).IsRequired();
                op.Property(p => p.ChineseDefinition).HasMaxLength(255).IsRequired();
                op.Property(p => p.ExampleSentence).HasColumnType("text");
                op.Property(p => p.Level).HasConversion(new ValueConverter<WordLevel, int>(
                v => ((int)v),
                v => (WordLevel)v));
            })
           .BuilderCration<DictionaryWordEntity>()
           .BuilderModify<DictionaryWordEntity>();

            builder.Entity<WordExampleEntity>(op =>
            {
                op.ToTable("word_example");
                op.HasKey(p => p.Id);
                op.Property(p => p.Sentence).HasMaxLength(255).IsRequired();
                op.Property(p => p.Definition).HasMaxLength(255).IsRequired();
                op.Property(p => p.Description).HasColumnType("text");
            })
            .BuilderCration<WordExampleEntity>()
            .BuilderModify<WordExampleEntity>();
            return builder;
        }
    }
}
