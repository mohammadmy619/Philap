using BuildingBlocks.Domain;
using Domain.PermissionAgregate.Exception;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.PermissionAgregate
{
    public class Permission : AggregateRoot<Guid>
    {
        #region Constructor  
        public Permission( string name) 
        {
        
            GuardAgainstName(name);

            Name = name;
            
        }

        protected Permission()  { }
        #endregion

        #region Properties  

        public string Name { get; private set; }
        public ICollection<Guid> RoleIds { get; private set; }
        #endregion

        #region Guard Against Methods  
        private static void GuardAgainstId(Guid id)
        {
            if (id == Guid.Empty)
                throw new PermissionIdIsInvalidException();
        }

        private static void GuardAgainstName(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) 
                throw new PermissionNameIsNullException();
        }
        #endregion
    }
}
