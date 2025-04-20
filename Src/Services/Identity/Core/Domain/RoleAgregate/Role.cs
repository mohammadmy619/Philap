using BuildingBlocks.Domain;
using Domain.RoleAgregate.Exception;
using Domain.UserAgregate.Exception;
using System.Xml.Linq;


namespace Domain.RoleAgregate
{
    public class Role : AggregateRoot<Guid>
    {
        #region Constructor  
        public Role(string name) 
        {
           
            GuardAgainstName(name);

            Name = name;
       
        }

        protected Role() { }
        #endregion

        #region Properties  
        public string Name { get; private set; }  
        public ICollection<Guid> PermissionIds { get; private set; } 
        #endregion

        #region Guard Against Methods  
        private static void GuardAgainstId(Guid id)
        {
            if (id == Guid.Empty)
                throw new RoleIdIsInvalidException();
        }

        private static void GuardAgainstName(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) 
                throw new RoleNameIsNullException();
        }
        #endregion

        #region Methods
        public void AddPermissionIdsToRole(List<Guid> PermissionToAdd)
        {
            if (PermissionToAdd == null || PermissionToAdd.Count == 0)
                throw new Exception.PermissionIdNotValidsException();
            PermissionIds.Clear();
            foreach (var role in PermissionToAdd)
            {
                if (!PermissionIds.Contains(role))
                {
                    PermissionIds.Add(role);
                }
            }
        }

        public void UpdateRole(Guid id,string name)
        {
            GuardAgainstName(name);
            Id = id;
            Name = name;
        }
        #endregion
    }
}
