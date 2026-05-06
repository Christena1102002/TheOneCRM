using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheOneCRM.Domain.Models.Entities;
using TheOneCRM.Domain.Specifications;

namespace TheOneCRM.Infrastructure.Specsification
{
    public class RefreshTokenByHashSpec : BaseSpecification<RefreshToken>
    {
        public RefreshTokenByHashSpec(string tokenHash)
            : base(t => t.TokenHash == tokenHash)
        {
        }
    }
}
