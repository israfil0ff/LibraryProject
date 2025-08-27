using Library.BLL.Helpers;
using Library.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.BLL.Exceptions
{
    public class AppException : Exception
    {
        public ErrorCode Code { get; }

        public AppException(ErrorCode code) : base(ErrorMessages.GetMessage(code))
        {
            Code = code;
        }

        public AppException(ErrorCode code, string message) : base(message)
        {
            Code = code;
        }
    }
}
