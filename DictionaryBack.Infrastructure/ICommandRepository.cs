using DictionaryBack.Domain;

namespace DictionaryBack.Infrastructure
{
    public interface ICommandRepository
    {
        int Add(Word tableRow);
    }
}
