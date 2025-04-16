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
    /// <summary>
    /// A convention that applies authorization policies to specified controllers.
    /// </summary>
    public class ControllerAuthorizeConvention : IControllerModelConvention
    {
        private readonly HashSet<Type> _controllerTypes;
        private readonly ControllerAuthorizeConventionOptions _authorizeConventionOptions;
        private readonly bool _inherit;

        /// <summary>
        /// Initializes a new instance of the <see cref="ControllerAuthorizeConvention"/> class
        /// for a single controller type with optional authentication schemes, policies, and roles.
        /// </summary>
        /// <param name="controllerType">The controller type to apply authorization to.</param>
        /// <param name="authorizeConventionOptions">The authorize convention options.</param>
        /// <param name="inherit">Indicates whether inherited controllers should also receive authorization settings. Default is <c>true</c>.</param>
        public ControllerAuthorizeConvention(Type controllerType, ControllerAuthorizeConventionOptions authorizeConventionOptions = null, bool inherit = true)
            : this(new[] { controllerType }, authorizeConventionOptions, inherit) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ControllerAuthorizeConvention"/> class
        /// for all controllers in a specified assembly with optional authentication schemes, policies, and roles.
        /// </summary>
        /// <param name="assembly">The assembly containing controllers to apply authorization to.</param>
        /// <param name="authorizeConventionOptions">The authorize convention options.</param>

        /// <param name="inherit">Indicates whether inherited controllers should also receive authorization settings. Default is <c>true</c>.</param>
        public ControllerAuthorizeConvention(Assembly assembly, ControllerAuthorizeConventionOptions authorizeConventionOptions = null, bool inherit = true)
            : this(assembly.GetTypes().Where(type => type.GetCustomAttributes(typeof(ControllerAttribute)).Any()), authorizeConventionOptions, inherit) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ControllerAuthorizeConvention"/> class
        /// for multiple specified controller types with optional authentication schemes, policies, and roles.
        /// </summary>
        /// <param name="controllerTypes">The collection of controller types to apply authorization to.</param>
        /// <param name="authorizeConventionOptions">The authorize convention options.</param>
        /// <param name="inherit">Indicates whether inherited controllers should also receive authorization settings. Default is <c>true</c>.</param>
        public ControllerAuthorizeConvention(IEnumerable<Type> controllerTypes, ControllerAuthorizeConventionOptions authorizeConventionOptions = null, bool inherit = true)
        {
            _controllerTypes = new HashSet<Type>(controllerTypes);
            _authorizeConventionOptions = authorizeConventionOptions ?? new ControllerAuthorizeConventionOptions();
            _inherit = inherit;
            _inherit = inherit;
        }

        public void Apply(ControllerModel controller)
        {
            if ((!_inherit || _controllerTypes.Any(type => !type.IsAssignableFrom(controller.ControllerType))) && !_controllerTypes.Contains(controller.ControllerType) || !_authorizeConventionOptions.ShouldApplyTo(controller))
            {
                return;
            }

            var authFilter = new AuthorizeFilter(new IAuthorizeData[]
            {
                new AuthorizeAttribute
                {
                    Policy = _authorizeConventionOptions.Policy,
                    AuthenticationSchemes = _authorizeConventionOptions.AuthenticationSchemes,
                    Roles = _authorizeConventionOptions.Roles
                }
            });

            controller.Filters.Add(authFilter);
        }
    }
}