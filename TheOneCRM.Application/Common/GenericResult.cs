using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheOneCRM.Application.Common
{
    public class GenericResult<T> : Result
    {
        public T Data { get; private set; }

        public static GenericResult<T> Success(T data, string message = "")
            => new GenericResult<T>
            {
                IsSuccess = true,
                Message = message,
                Data = data
            };

        public static new GenericResult<T> Failure(string message)
            => new GenericResult<T>
            {
                IsSuccess = false,
                Message = message,
                Data = default
            };
    }
}
