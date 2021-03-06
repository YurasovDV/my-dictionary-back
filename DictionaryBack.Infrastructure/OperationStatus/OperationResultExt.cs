namespace DictionaryBack.Common
{
    public static class OperationResultExt
    {
        public const string InnerErrorText =  "InnerErrorText";

        public static BoolOperationResult BoolSuccess()
        {
            return new BoolOperationResult();
        }

        public static BoolOperationResult BoolFail(CommandStatus status, string errorText)
        {
            return new BoolOperationResult(status, errorText);
        }        
        
        public static BoolOperationResult BoolFail(CommandStatus status, string errorText, string innerErrorText)
        {
            return new BoolOperationResult(status, errorText)
            {
                AdditionalData = new System.Collections.Generic.Dictionary<string, object>()
                {
                    { InnerErrorText, innerErrorText }
                }
            };
        }

        public static OperationResult<T> Success<T>(T data) => new OperationResult<T>()
        {
            StatusCode = 0,
            Data = data
        };

        public static OperationResult<T> Fail<T>(CommandStatus status, string error) => new OperationResult<T>()
        {
            StatusCode = status,
            ErrorText = error
        };

        public static OperationResult<T> Fail<T>(CommandStatus status, string error, string innerErrorText) => new OperationResult<T>()
        {
            StatusCode = status,
            ErrorText = error,
            AdditionalData = new System.Collections.Generic.Dictionary<string, object>()
            {
                { InnerErrorText, innerErrorText }
            }
        };

        public static bool IsSuccessful<T>(this OperationResult<T> res) => res.StatusCode == CommandStatus.Success;
    }
}
