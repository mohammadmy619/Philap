using BuildingBlocks.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACL.PersonACL
{
    public interface IPersonACL: IAcl
    {

        /// <summary>
        /// بررسی می‌کند آیا Leader در ساب‌دامین مبدا وجود دارد و وضعیت Active دارد
        /// </summary>
        Task<bool> IsLeaderValidAsync(Guid leaderId, CancellationToken cancellationToken);



    }
}
