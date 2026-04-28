using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheOneCRM.Application.Common
{
    public enum ErrorType
    {
        None,
        Validation,
        Unauthorized,
        Forbidden,
        NotFound,
        Conflict
    }
}
