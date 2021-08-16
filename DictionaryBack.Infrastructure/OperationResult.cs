using System.Collections.Generic;

namespace DictionaryBack.Infrastructure
{
    public class OperationResult<T>
    {
        public CommandStatus StatusCode { get; set; }

        public T Data { get; set; }

        public string ErrorText { get; set; }

        public Dictionary<string, object> AdditionalData { get; set; }
    }
}
