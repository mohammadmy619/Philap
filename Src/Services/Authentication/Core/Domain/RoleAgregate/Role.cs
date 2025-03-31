using BuildingBlocks.Domain;
using Domain.RoleAgregate.Exception;


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
    }
}
