using DictionaryBack.Common.DTOs.Command;
using System.Threading.Tasks;

namespace DictionaryBack.Common.Queue
{
    public interface IRepetitionResultsPublisher
    {
        Task PublishResult(WordRepetitionResult[] wordsRepetitionResults);
    }
}
