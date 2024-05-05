using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System;

namespace Ogu.AspNetCore.Conventions
{
    public class ControllerRoutePrefixConvention : IControllerModelConvention
    {
        private readonly AttributeRouteModel _routePrefix;
        private readonly Type _controllerType;
        private readonly bool _inherit;

        public ControllerRoutePrefixConvention(string routePrefix, Type controllerType, bool inherit = true)
        {
            _routePrefix = new AttributeRouteModel(new RouteAttribute(routePrefix));
            _controllerType = controllerType;
            _inherit = inherit;
        }

        public void Apply(ControllerModel controller)
        {
            if ((!_inherit || !_controllerType.IsAssignableFrom(controller.ControllerType)) && _controllerType != controller.ControllerType)
            {
                return;
            }

            foreach (var selector in controller.Selectors)
            {
                selector.AttributeRouteModel = selector.AttributeRouteModel == null 
                    ? _routePrefix 
                    : AttributeRouteModel.CombineAttributeRouteModel(_routePrefix, selector.AttributeRouteModel);
            }
        }
    }
}