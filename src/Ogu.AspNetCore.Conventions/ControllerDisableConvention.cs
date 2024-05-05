using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System;

namespace Ogu.AspNetCore.Conventions
{
    public class ControllerDisableConvention : IControllerModelConvention
    {
        private readonly Type _controllerType;
        private readonly bool _inherit;
        public ControllerDisableConvention(Type controllerType, bool inherit = true)
        {
            _controllerType = controllerType;
            _inherit = inherit;
        }

        public void Apply(ControllerModel controller)
        {
            if ((!_inherit || !_controllerType.IsAssignableFrom(controller.ControllerType)) && _controllerType != controller.ControllerType)
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