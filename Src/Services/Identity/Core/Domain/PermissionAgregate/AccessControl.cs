using BuildingBlocks.Domain;
using Domain.PermissionAgregate.Exception;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.PermissionAgregate
{
    public class AccessControl: ValueObject<AccessControl>
    {
        public AccessControl(string resource, string action)
        {
            GuardAgainstResource(resource);
            GuardAgainstAction(action);

            Resource = resource;
            Action = action;
        }

        public string Resource { get; private set; }
        public string Action { get; private set; }

        #region Guard
        private static void GuardAgainstResource(string resource)
        {
            if (string.IsNullOrWhiteSpace(resource))
                throw new AccessControlResourceInvalidException();
        }

        private static void GuardAgainstAction(string action)
        {
            if (string.IsNullOrWhiteSpace(action))
                throw new AccessControlActionInvalidException();
        }
        #endregion
        #region method
        public override IEnumerable<object> GetEqualityComponents()
        {
            yield return Resource;
            yield return Action;

        }
        #endregion
    }
}
