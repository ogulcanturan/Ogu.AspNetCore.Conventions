using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Ogu.AspNetCore.Conventions
{
    /// <summary>
    /// A convention that disables specified controllers, preventing them from being used in the application.
    /// </summary>
    public class ControllerDisableConvention : IControllerModelConvention
    {
        private readonly HashSet<Type> _controllerTypes;
        private readonly bool _inherit;

        /// <summary>
        /// Initializes a new instance of the <see cref="ControllerDisableConvention"/> class
        /// for a single controller type.
        /// </summary>
        /// <param name="controllerType">The controller type to disable.</param>
        /// <param name="inherit">Indicates whether inherited controllers should also be disabled. Default is <c>true</c>.</param>
        public ControllerDisableConvention(Type controllerType, bool inherit = true)
            : this(new Type[] { controllerType }, inherit) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ControllerDisableConvention"/> class
        /// for all controllers in a specified assembly.
        /// </summary>
        /// <param name="assembly">The assembly containing controllers to disable.</param>
        /// <param name="inherit">Indicates whether inherited controllers should also be disabled. Default is <c>true</c>.</param>
        public ControllerDisableConvention(Assembly assembly, bool inherit = true)
            : this(assembly.GetTypes().Where(type => type.GetCustomAttributes(typeof(ControllerAttribute)).Any()), inherit) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ControllerDisableConvention"/> class
        /// for multiple specified controller types.
        /// </summary>
        /// <param name="controllerTypes">The collection of controller types to disable.</param>
        /// <param name="inherit">Indicates whether inherited controllers should also be disabled. Default is <c>true</c>.</param>
        public ControllerDisableConvention(IEnumerable<Type> controllerTypes, bool inherit = true)
        {
            _controllerTypes = new HashSet<Type>(controllerTypes);
            _inherit = inherit;
        }

        public void Apply(ControllerModel controller)
        {
            if ((!_inherit || _controllerTypes.Any(type => !type.IsAssignableFrom(controller.ControllerType))) && !_controllerTypes.Contains(controller.ControllerType))
            {
                return;
            }

            controller.ApiExplorer.IsVisible = false;
            controller.Actions.Clear();
            controller.Selectors.Clear();
            controller.ControllerProperties.Clear();
            controller.Filters.Clear();
            controller.Properties.Clear();
        }
    }
}