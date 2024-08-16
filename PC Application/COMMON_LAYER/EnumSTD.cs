 
namespace COMMON
{

    /// <summary>
    /// 
    /// </summary>
    
    /// <summary>
    /// 
    /// </summary>
    public enum OperationResult
    {
        SaveSuccess,
        SaveError,
        UpdateSuccess,
        UpdateError,
        DeleteSuccess,
        DeleteError,
        ZeroStatus,
        Error,
        Duplicate,
        PartialDelete,
        FullDelete,
        DeleteRefference,
        ForeignKeyError,
        PrimaryKeyError,
        NotFound,
        Valid,
        Invalid,
        PrinterError,
        InsertError,
        InActiveUsers,
        ActiveUsers,
        CustomerCodeNotFound,
        ValidateError,
        StatusOne,
        StatusDone,
        UpdateRefference,
        QuantityExceeds,
        NotExist,
        CancelSuccess,
        CancelError,
        MRNBeforeQC,
        MRNAfterQC,
        PRNNotFound,
        PinterNotReady

    }
    /// <summary>
    /// 
    /// </summary>
    public enum ValidateType
    {
        IsNumeric,
        IsNumericOrDecimal,
        IsDateTime,
        IsDecimal,
        IsString
    }
    /// <summary>
    /// 
    /// </summary>
    public enum MsgType
    {
        Success,
        Error,
        Info,
        Confirm
    }
    /// <summary>
    /// 
    /// </summary>
    public enum MsgResult
    {
        DUPLICATE,
        YES,
        NO,
        OK,
        CANCEL,
        INVALID
    }
    public enum ValidationResults
    {
        Valid,
        Empty,
        InValid
    }
    public enum MessageResult
    {
        Yes,
        No
    }
}
