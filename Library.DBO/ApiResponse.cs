using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.DBO
{
    public class ApiResponse
    {
        public bool Success { get; set; }
        public object? Data { get; set; }
        public string? Message { get; set; }

        public static ApiResponse SuccessResponse(object? data = null, string? message = null)
            => new ApiResponse { Success = true, Data = data, Message = message };

        public static ApiResponse FailResponse(string message, object? data = null)
            => new ApiResponse { Success = false, Data = data, Message = message };
    }
}
