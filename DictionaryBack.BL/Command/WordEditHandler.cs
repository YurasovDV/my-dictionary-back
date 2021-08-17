using DictionaryBack.BL.Command.Models;
using DictionaryBack.BL.Query.Models;
using DictionaryBack.DAL;
using DictionaryBack.Domain;
using DictionaryBack.Infrastructure;
using System;
using System.Threading.Tasks;

namespace DictionaryBack.BL.Command
{
    public interface IWordEditHandler
    {
        Task<OperationResult<WordDto>> Edit(WordEditModel request);
    }

    public class WordEditHandler : BaseCommand, IWordEditHandler
    {
        public WordEditHandler(DictionaryContext dictionaryContext) : base(dictionaryContext) { }

        public async Task<OperationResult<WordDto>> Edit(WordEditModel request)
        {
            // i think i hate dapper
            try
            {
                // can be rewritten for one database trip, if we receive original values from client
                var existingWord = await DictionaryContext.Words.FindAsync(request.Term);
                if (existingWord != null)
                {
                    // for plain properties: DictionaryContext.Entry(existingWord).CurrentValues.SetValues(request);

                    EditInternal(existingWord, request);

                    await DictionaryContext.SaveChangesAsync();
                    return OperationResultExt.Success(Mapper.Map(existingWord));
                }
                return OperationResultExt.Fail<WordDto>(CommandStatus.WordNotFound, "Word does not exist");
            }
            catch (Exception ex)
            {
                return OperationResultExt.Fail<WordDto>(CommandStatus.InternalError, "Internal error", ex.Message);
            }
        }

        private void EditInternal(Word existingWord, WordEditModel request)
        {
            if (request.Topic != existingWord?.Topic.Name)
            {
                existingWord.Topic = FindTopic(request);
            }

            if (existingWord.Status != request.Status)
            {
                existingWord.Status = request.Status;
            }


            var translationsAdded = [];
            var translationsRemoved = [];

            existingWord.Translations.Remove(translationsRemoved)
            existingWord.Translations.Add(translationsAdded)
        }

        private Topic FindTopic(WordEditModel request)
        {
            
        }
    }
}
