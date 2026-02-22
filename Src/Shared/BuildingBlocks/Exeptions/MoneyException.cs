using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuildingBlocks.Domain;

namespace BuildingBlocks.Exeptions
{
    public class AmountIsInvalidException : DomainException
    {
        public AmountIsInvalidException(string message = "Amount is invalid.", string code = "0517008")
            : base(message, code) { }
    }

    public class CurrencyIsInvalidException : DomainException
    {
        public CurrencyIsInvalidException(string message = "Currency is invalid.", string code = "0517009")
            : base(message, code) { }
    }
}
