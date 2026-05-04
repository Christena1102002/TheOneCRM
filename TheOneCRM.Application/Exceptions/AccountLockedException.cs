using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheOneCRM.Application.Exceptions
{
    public class AccountLockedException : Exception
    {
        public AccountLockedException(string message) : base(message) { }
    }
}
