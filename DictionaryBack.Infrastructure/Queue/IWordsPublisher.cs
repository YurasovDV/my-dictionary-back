using DictionaryBack.Common.DTOs.Query;
using System.Threading.Tasks;

namespace DictionaryBack.Common.Queue
{
    public interface IWordsPublisher
    {
        Task PublishChangedWord(WordDto wordDto);
    }
}
