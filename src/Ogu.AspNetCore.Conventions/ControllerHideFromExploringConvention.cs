using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Ogu.AspNetCore.Conventions
{
    /// <summary>
    /// A convention that hides specified controllers from Api discovery tools (e.g., Swagger).
    /// </summary>
    public class ControllerHideFromExploringConvention : IControllerModelConvention
    {
        private readonly HashSet<Type> _controllerTypes;
        private readonly bool _inherit;

        /// <summary>
        /// Initializes a new instance of the <see cref="ControllerHideFromExploringConvention"/> class
        /// for a single controller type.
        /// </summary>
        /// <param name="controllerType">The controller type to hide from exploration.</param>
        /// <param name="inherit">Indicates whether inherited controllers should also be hidden. Default is <c>true</c>.</param>
        public ControllerHideFromExploringConvention(Type controllerType, bool inherit = true)
            : this(new Type[] { controllerType }, inherit) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ControllerHideFromExploringConvention"/> class
        /// for all controllers in a specified assembly.
        /// </summary>
        /// <param name="assembly">The assembly containing controllers to hide from exploration.</param>
        /// <param name="inherit">Indicates whether inherited controllers should also be hidden. Default is <c>true</c>.</param>
        public ControllerHideFromExploringConvention(Assembly assembly, bool inherit = true)
            : this(assembly.GetTypes().Where(type => type.GetCustomAttributes(typeof(ControllerAttribute)).Any()), inherit) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ControllerHideFromExploringConvention"/> class
        /// for multiple specified controller types.
        /// </summary>
        /// <param name="controllerTypes">The collection of controller types to hide from exploration.</param>
        /// <param name="inherit">Indicates whether inherited controllers should also be hidden. Default is <c>true</c>.</param>
        public ControllerHideFromExploringConvention(IEnumerable<Type> controllerTypes, bool inherit = true)
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
        }
    }
}