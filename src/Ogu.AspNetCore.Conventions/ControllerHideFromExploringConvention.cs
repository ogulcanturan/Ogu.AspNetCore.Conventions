using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Ogu.AspNetCore.Conventions
{
    public class ControllerHideFromExploringConvention : IControllerModelConvention
    {
        private readonly HashSet<Type> _controllerTypes;
        private readonly bool _inherit;

        public ControllerHideFromExploringConvention(Type controllerType, bool inherit = true)
            : this(new Type[] { controllerType }, inherit) { }

        public ControllerHideFromExploringConvention(Assembly assembly, bool inherit = true)
            : this(assembly.GetTypes().Where(type => type.GetCustomAttribute(typeof(ControllerAttribute)) != null), inherit) { }

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