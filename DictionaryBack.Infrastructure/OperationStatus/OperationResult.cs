using System.Collections.Generic;

namespace DictionaryBack.Common
{
    // TODO: use separate model for tests or generated client
    public class OperationResult<T>
    {


        private T data;

        public CommandStatus StatusCode { get; set; }

        public T Data
        {
            get
            {
                return data;
            }
            set
            {
                data = value;
            }
        }

        public string ErrorText { get; set; }

        public Dictionary<string, object> AdditionalData { get; set; }
    }
}
