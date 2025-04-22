using BuildingBlocks.Domain;
using Domain.PermissionAgregate.Exception;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.PermissionAgregate
{
    public class Permission : AggregateRoot<Guid>
    {
        #region Constructor  
        public Permission(string name)
        {

            GuardAgainstName(name);

            Name = name;

        }

        protected Permission() { }
        #endregion

        #region Properties  

        public string Name { get; private set; }
        public readonly List<Guid> _RoleIds  = new List<Guid>();
        public IReadOnlyCollection<Guid> RoleIds => _RoleIds.AsReadOnly();


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
        private static void GuardAgainstAccessControlsIsNull(AccessControl accessControl)
        {
            if (accessControl == null)
                throw new AccessControlIsNullException();
        }
        private static void GuardAgainstAccessControlIsNull(AccessControl accessControl)
        {
            if (accessControl == null)
                throw new AccessControlIsNullException();
        }
        #endregion
        #region Method
        public void UpdatePermission(Guid Id, string name)
        {
            GuardAgainstName(name);
            GuardAgainstId(Id);
            this.Id = Id;
            Name = name;

        }
        public void AddRoleIds(List<Guid> RoleIds)
        {

            if (RoleIds == null || RoleIds.Count == 0)
                throw new RoleIdNotValidsException();
            foreach (var role in RoleIds)
            {
                if (!_RoleIds.Contains(role))
                {
                    _RoleIds.Add(role);
                }
            }
        }
   

        public void AddAccessControl(AccessControl accessControl)
        {
            GuardAgainstAccessControlsIsNull(accessControl);

            if (_AccessControl.Any(ac =>
                ac.GetHashCode() == accessControl.GetHashCode()))
            {
                throw new DuplicateAccessControlException();
            }

            _AccessControl.AddRange(accessControl);
        }

        public void UpdateAccessControl(AccessControl newAccessControl)
        {
            if (!_AccessControl.Any(ac =>
                ac.GetHashCode() == newAccessControl.GetHashCode()))
            {
                throw new NotfoundAccessControlException();
            }
          var AccessControl= _AccessControl.Where(c=>c.GetHashCode() == newAccessControl.GetHashCode()).FirstOrDefault();

            _AccessControl.Remove(AccessControl);
            _AccessControl.Add(newAccessControl);

        }

        public void RemoveAccessControl(AccessControl accessControl)
        {
            GuardAgainstAccessControlIsNull(accessControl);

            var existingControl = _AccessControl.FirstOrDefault(ac =>
                ac.GetHashCode() == accessControl.GetHashCode());

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
