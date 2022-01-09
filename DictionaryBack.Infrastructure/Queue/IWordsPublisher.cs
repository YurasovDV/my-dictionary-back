using DictionaryBack.Common.Entities;
using System.Threading.Tasks;

namespace DictionaryBack.Common.Queue
{
    public interface IWordsPublisher
    {
        Task PublishChangedWord(Word word);
    }
}
