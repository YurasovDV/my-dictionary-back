using System;
using System.Collections.Generic;

namespace DictionaryBack.Infrastructure
{
    public class OperationResult<T>
    {
        // TODO: use separate model for tests or generated client
        // internal OperationResult()
        // {
        // 
        // }


        private T data;

        public CommandStatus StatusCode { get; set; }

        public T Data
        {
            get
            {
                if (!this.IsSuccessful())
                {
                    throw new InvalidOperationException($"Attempt to access '{nameof(Data)}' property of non-successful {nameof(OperationResult<T>)}");
                }
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
