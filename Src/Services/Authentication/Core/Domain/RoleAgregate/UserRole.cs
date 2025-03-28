using BuildingBlocks.Domain;
using Domain.UserAgregate;
using Domain.UserAgregate.Role.Exception;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.RoleAgregate
{
   
        public class UserRole : Entity<int>
        {
            #region Constructor  
            public UserRole(int id, int userId, int roleId) : base(id)
            {
                GuardAgainstId(id);
                GuardAgainstUserId(userId);
                GuardAgainstRoleId(roleId);

                UserId = userId;
                RoleId = roleId;
            }

            protected UserRole(int id) : base(id) { } 
            #endregion

            #region Properties  
            public int UserId { get; private set; } 
            public User User { get; private set; } 
            public int RoleId { get; private set; }
            public Role Role { get; private set; } 
            #endregion

            #region Guard Against Methods  
            private static void GuardAgainstId(int id)
            {
                if (id <= 0) 
                    throw new UserRoleIdIsInvalidException();
            }

            private static void GuardAgainstUserId(int userId)
            {
                if (userId <= 0) 
                    throw new UserIdIsInvalidException();
            }

            private static void GuardAgainstRoleId(int roleId)
            {
                if (roleId <= 0)  
                    throw new RoleIdIsInvalidException();
            }
            #endregion
        }
    
}
