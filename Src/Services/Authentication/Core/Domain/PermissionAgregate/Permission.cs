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

        private readonly List<AccessControl> _AccessControl = new List<AccessControl>();
        public IReadOnlyCollection<AccessControl> AccessControl => _AccessControl.AsReadOnly();
    


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
        private static void GuardAgainstAccessControlIsNull(AccessControl accessControl)
        {
            if (accessControl == null)
                throw new AccessControlIsNullException();
        }
        #endregion
        #region Method
        public void UpdatePermission(Guid Id,string name)
        {
            GuardAgainstName(name);
            GuardAgainstId(Id);
            this.Id = Id;
            Name = name;

        }

        #endregion

        #region AccessControl Methods
        //public void AddAccessControl(AccessControl accessControl)
        //{
        //    GuardAgainstAccessControlIsNull(accessControl);

        //    if (_AccessControl.Any(ac =>
        //        ac.Resource == accessControl.Resource &&
        //        ac.Action == accessControl.Action))
        //    {
        //        throw new DuplicateAccessControlException();
        //    }

        //    _AccessControl.Add(accessControl);
        //}

        public void UpdateAccessControl(AccessControl oldAccessControl, AccessControl newAccessControl)
        {
            GuardAgainstAccessControlIsNull(oldAccessControl);
            GuardAgainstAccessControlIsNull(newAccessControl);

            var existingControl = _AccessControl.FirstOrDefault(ac =>
                ac.Resource == oldAccessControl.Resource &&
                ac.Action == oldAccessControl.Action);

            if (existingControl == null)
                throw new AccessControlNotFoundException();

            _AccessControl.Remove(existingControl);
            _AccessControl.Add(newAccessControl);
        }

        public void RemoveAccessControl(AccessControl accessControl)
        {
            GuardAgainstAccessControlIsNull(accessControl);

            var existingControl = _AccessControl.FirstOrDefault(ac =>
                ac.Resource == accessControl.Resource &&
                ac.Action == accessControl.Action);

            if (existingControl == null)
                throw new AccessControlNotFoundException();

            _AccessControl.Remove(existingControl);
        }

        public AccessControl GetAccessControl(string resource, string action)
        {
            var accessControl = _AccessControl.FirstOrDefault(ac =>
                ac.Resource == resource &&
                ac.Action == action);

            if (accessControl == null)
                throw new AccessControlNotFoundException();

            return accessControl;
        }

        public bool HasAccessControl(string resource, string action)
        {
            return _AccessControl.Any(ac =>
                ac.Resource == resource &&
                ac.Action == action);
        }

      
        #endregion

      
    }
}
