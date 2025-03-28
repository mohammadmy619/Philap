using BuildingBlocks.Domain;
using Domain.UserAgregate.Role.Exception;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.RoleAgregate
{
    public class Role : Entity<int>
    {
        #region Constructor  
        public Role(int id, string name) : base(id)
        {
            GuardAgainstId(id);
            GuardAgainstName(name);

            Name = name;
            UserRoles = new List<UserRole>(); 
        }

        protected Role(int id) : base(id) { } 
        #endregion

        #region Properties  
        public string Name { get; private set; }  
        public ICollection<UserRole> UserRoles { get; private set; } 
        #endregion

        #region Guard Against Methods  
        private static void GuardAgainstId(int id)
        {
            if (id <= 0) 
                throw new RoleIdIsInvalidException();
        }

        private static void GuardAgainstName(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) 
                throw new RoleNameIsNullException();
        }
        #endregion
    }
}
