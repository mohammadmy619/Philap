using BuildingBlocks.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Persons.Leader.Exception
{
    public class TitleIsNullException : DomainException
    {
        public TitleIsNullException(string message = "Title is required.", string code = "0156001")
            : base(message, code) { }
    }

    public class DepartmentIsNullException : DomainException
    {
        public DepartmentIsNullException(string message = "Department is required.", string code = "0156002")
            : base(message, code) { }
    }

    public class JoiningDateIsInvalidException : DomainException
    {
        public JoiningDateIsInvalidException(string message = "Joining date must be in the past.", string code = "0156003")
            : base(message, code) { }
    }

    public class SkillsAreEmptyException : DomainException
    {
        public SkillsAreEmptyException(string message = "At least one skill is required.", string code = "0156004")
            : base(message, code) { }
    }
}
