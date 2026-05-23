using BuildingBlocks.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlocks.Exeptions
{
    public class NotFoundException : DomainException
    {
        public NotFoundException(string message = "NotFoundException", string code = "00000")
            : base(message, code) { }
    }
}
