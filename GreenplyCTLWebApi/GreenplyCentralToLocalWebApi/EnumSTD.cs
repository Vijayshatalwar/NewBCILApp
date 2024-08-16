using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenplyCentralToLocalWebApi
{
    public enum OperationResult
    {
        SaveSuccess,
        SaveError,
        UpdateSuccess,
        UpdateError,
        DeleteSuccess,
        DeleteError,
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
        Success,
        PartialImport,
        ImportSuccess,
        NotImport,
        InvalidLength

    }
    public enum MessageType
    {
        Success, Error, Warning, Alert, Exception
    }
}
