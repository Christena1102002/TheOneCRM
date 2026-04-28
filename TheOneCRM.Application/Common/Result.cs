using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheOneCRM.Application.Common
{
    public class Result
    {
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
        //public T Data { get; set; }
        public IEnumerable<string> Errors { get; set; }
        public ErrorType ErrorType { get; set; } = ErrorType.None;
        public static Result Success(/*T data,*/string message) => new Result { IsSuccess = true, /*Data = data*/ Message = message };
        public static Result Failure(string message, IEnumerable<string> errors = null) => new Result { IsSuccess = false, Message = message, Errors = errors, /*Data = default*/ };
    }
}
    

