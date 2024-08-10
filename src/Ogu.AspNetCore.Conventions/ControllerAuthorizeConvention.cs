using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Ogu.AspNetCore.Conventions
{
    public class ControllerAuthorizeConvention : IControllerModelConvention
    {
        private readonly HashSet<Type> _controllerTypes;
        private readonly string _policy;
        private readonly string _authenticationSchemes;
        private readonly string _roles;
        private readonly bool _inherit;

        public ControllerAuthorizeConvention(Type controllerType, string authenticationSchemes = null, string policy = null, string roles = null, bool inherit = true)
            : this(new Type[] { controllerType }, authenticationSchemes, policy, roles, inherit) { }

        public ControllerAuthorizeConvention(Assembly assembly, string authenticationSchemes = null, string policy = null, string roles = null, bool inherit = true)
            : this(assembly.GetTypes().Where(type => type.GetCustomAttribute(typeof(ControllerAttribute)) != null), authenticationSchemes, policy, roles, inherit) { }

        public ControllerAuthorizeConvention(IEnumerable<Type> controllerTypes, string authenticationSchemes = null, string policy = null, string roles = null, bool inherit = true)
        {
            _controllerTypes = new HashSet<Type>(controllerTypes);
            _authenticationSchemes = authenticationSchemes;
            _policy = policy;
            _roles = roles;
            _inherit = inherit;
            _inherit = inherit;
        }

        public void Apply(ControllerModel controller)
        {
            if ((!_inherit || _controllerTypes.Any(type => !type.IsAssignableFrom(controller.ControllerType))) && !_controllerTypes.Contains(controller.ControllerType))
            {
                return;
            }

            var authFilter = new AuthorizeFilter(new IAuthorizeData[]
            {
                new AuthorizeAttribute()
                {
                    Policy = _policy,
                    AuthenticationSchemes = _authenticationSchemes,
                    Roles = _roles
                }
            });

            controller.Filters.Add(authFilter);
        }
    }
}