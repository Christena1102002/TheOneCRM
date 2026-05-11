using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheOneCRM.Domain.Models.Entities;
using TheOneCRM.Domain.Specifications;

namespace TheOneCRM.Infrastructure.Specsification.NewFolder
{
    public class ExpiredNonRevokedTokensSpecification : BaseSpecification<RefreshToken>
    {
        public ExpiredNonRevokedTokensSpecification(int retainDays = 7)
        {
            var threshold = DateTime.UtcNow.AddDays(-retainDays);

            Criteria = rt =>
                rt.ExpiresAt < threshold &&
                !rt.IsRevoked;
        }
    }
}