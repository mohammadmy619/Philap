using BuildingBlocks.Domain;
using Domain.RoleAgregate;
using Domain.RoleAgregate.Exception;
using Domain.Services;
using Domain.UserAgregate.Exception;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace Domain.UserAgregate
{
    public class User : AggregateRoot<Guid>
    {
        #region Constructor  
        private readonly IEmailService _emailService;
        public  User(string userName, string email, string passwordHash, IEmailService emailService)
        { 
            GuardAgainstUserName(userName);
            GuardAgainstEmail(email, emailService);
            GuardAgainstPasswordHash(passwordHash);
            UserName = userName;
            Email = email;
            PasswordHash = passwordHash;
            _emailService = emailService;
            _RoleIds = new List<Guid>();

        }

        protected User(Guid id) { } // سازنده protected برای ORM  
        #endregion

        #region Properties  

        public string UserName { get; private set; }
        public string Email { get; private set; }
        public string PasswordHash { get; private set; }

        public readonly List<Guid> _RoleIds = new List<Guid>();
        public IReadOnlyCollection<Guid> RoleIds => _RoleIds.AsReadOnly();
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
   
        private static void GuardAgainstEmail(string email, IEmailService emailService)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new EmailIsNullException();
           
            if (!emailService.CheckEmail(email))
            {
                throw new InvalidEmailException();
            }
          

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
                throw new RoleIdNotValidsException();
            foreach (var role in rolesToAdd)
            {
                if (!_RoleIds.Contains(role))
                {
                    _RoleIds.Add(role);
                }
            }
        }  

        public void UpdateUser(Guid id,string userName, string email, string passwordHash, IEmailService emailService)
        {
            GuardAgainstId(id);
            GuardAgainstUserName(userName);
            GuardAgainstEmail(email, emailService);
            GuardAgainstPasswordHash(passwordHash);
            Id = id;
            UserName = userName;
            Email = email;
            PasswordHash = passwordHash;
        }
        #endregion
    }
}
