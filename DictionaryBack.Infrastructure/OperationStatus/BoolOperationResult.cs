namespace DictionaryBack.Common
{
    public class BoolOperationResult : OperationResult<NullObject>
    {
        public BoolOperationResult()
        {
            StatusCode = CommandStatus.Success;
        }        
        
        public BoolOperationResult(CommandStatus status, string error)
        {
            StatusCode = status;
            ErrorText = error;
        }
    }
}
