using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Authorization;
using System;

namespace Ogu.AspNetCore.Conventions
{
    public class ControllerAuthorizeConvention : IControllerModelConvention
    {
        private readonly string _policy;
        private readonly string _authenticationSchemes;
        private readonly string _roles;
        private readonly Type _controllerType;
        private readonly bool _inherit;

        public ControllerAuthorizeConvention(Type controllerType, string authenticationSchemes = null, string policy = null, string roles = null, bool inherit = true)
        {
            _controllerType = controllerType;
            _authenticationSchemes = authenticationSchemes;
            _policy = policy;
            _roles = roles;
            _inherit = inherit;
        }

        public void Apply(ControllerModel controller)
        {
            if ((!_inherit || !_controllerType.IsAssignableFrom(controller.ControllerType)) && _controllerType != controller.ControllerType)
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