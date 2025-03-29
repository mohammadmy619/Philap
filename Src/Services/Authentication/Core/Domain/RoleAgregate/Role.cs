using BuildingBlocks.Domain;
using Domain.RoleAgregate.Exception;


namespace Domain.RoleAgregate
{
    public class Role : AggregateRoot<Guid>
    {
        #region Constructor  
        public Role(Guid id, string name) : base(id)
        {
           
            GuardAgainstName(name);

            Name = name;
       
        }

        protected Role(Guid id) : base(id) { }
        #endregion

        #region Properties  
        public string Name { get; private set; }  
        public ICollection<Guid> UserIds { get; private set; } 
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
