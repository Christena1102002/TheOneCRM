using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheOneCRM.Domain.Models.Entities;
using TheOneCRM.Domain.Specifications;

namespace TheOneCRM.Infrastructure.Specsification.NewFolder
{
    public class OldRevokedTokensSpecification : BaseSpecification<RefreshToken>
    {
        public OldRevokedTokensSpecification(int retainDays = 90)
        {
            var threshold = DateTime.UtcNow.AddDays(-retainDays);

            Criteria = rt =>
                rt.IsRevoked &&
                rt.RevokedAt.HasValue &&
                rt.RevokedAt.Value < threshold;
        }
    }
}
