using BuildingBlocks.Domain;
using Domain.Domain_Services;
using Domain.RoleAgregate;
using Domain.RoleAgregate.Exception;
using Domain.UserAgregate.Exception;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Domain.UserAgregate
{
    public class User : AggregateRoot<Guid>
    {
        #region Constructor  

        public User(string userName, string email, string passwordHash)
        {

            GuardAgainstUserName(userName);
            GuardAgainstEmail(email);
            GuardAgainstPasswordHash(passwordHash);
            UserName = userName;
            Email = email;
            PasswordHash = passwordHash;
            Roleds = new List<Guid>();

        }

        protected User(Guid id) { } // سازنده protected برای ORM  
        #endregion

        #region Properties  

        public string UserName { get; private set; }
        public string Email { get; private set; }
        public string PasswordHash { get; private set; }
        public ICollection<Guid> Roleds { get; private set; }
        #endregion
        #region Guard Against Methods  
        private void GuardAgainstId(Guid id)
        {
            if (id == Guid.Empty)
                throw new UserIdIsInvalidException();
        }

        private static void GuardAgainstUserName(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
                throw new UserNameIsNullException();
        }

        private static void GuardAgainstEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new EmailIsNullException();
            // می‌توانید بررسی الگوی ایمیل را نیز اضافه کنید  
        }

        private static void GuardAgainstPasswordHash(string passwordHash)
        {
            if (string.IsNullOrWhiteSpace(passwordHash))
                throw new PasswordHashIsNullException();
        }
        #endregion
        #region methods
       
        public void AddRolesToUser(List<Guid> rolesToAdd)
        {
            if (rolesToAdd == null || rolesToAdd.Count == 0)
                throw new RollIdNotValidsException();
            foreach (var role in rolesToAdd)
            {
                if (!Roleds.Contains(role))
                {
                    Roleds.Add(role);
                }
            }
        }

        public void UpdateUser(string userName, string email, string passwordHash)
        {
            GuardAgainstUserName(userName);
            GuardAgainstEmail(email);
            GuardAgainstPasswordHash(passwordHash);
            UserName = userName;
            Email = email;
            PasswordHash = passwordHash;
        }
        #endregion
    }
}
