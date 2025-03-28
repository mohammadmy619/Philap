using BuildingBlocks.Domain;
using Domain.UserAgregate.Permission.Exception;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.PermissionAgregate
{
    public class Permission : Entity<int>
    {
        #region Constructor  
        public Permission(int id, string name) : base(id)
        {
            GuardAgainstId(id);
            GuardAgainstName(name);

            Name = name;
            Roles = new List<RoleAgregate.Role>(); 
        }

        protected Permission(int id) : base(id) { } 
        #endregion

        #region Properties  
        public string Name { get; private set; }
        public ICollection<RoleAgregate.Role> Roles { get; private set; }
        #endregion

        #region Guard Against Methods  
        private static void GuardAgainstId(int id)
        {
            if (id <= 0) 
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
