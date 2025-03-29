using BuildingBlocks.Domain;
using Domain.RoleAgregate;
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
        public User(Guid id, string userName, string email, string passwordHash) : base(id)
        {
    
            GuardAgainstUserName(userName);
            GuardAgainstEmail(email);
            GuardAgainstPasswordHash(passwordHash);

            UserName = userName;
            Email = email;
            PasswordHash = passwordHash;

        }

        protected User(Guid id) : base(id) { } // سازنده protected برای ORM  
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
    }
}
