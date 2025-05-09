using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Application.Permission.GetAccess
{
    public record class GetAccessControlQuery(Guid permissionId):IRequest<List<GetAccessControlResponse>>;
 
}
