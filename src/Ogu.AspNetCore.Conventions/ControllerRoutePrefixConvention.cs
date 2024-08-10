using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Ogu.AspNetCore.Conventions
{
    public class ControllerRoutePrefixConvention : IControllerModelConvention
    {
        private readonly AttributeRouteModel _routePrefix;
        private readonly bool _combineRoutes;
        private readonly RouteCombinationStrategy _combinationStrategy;
        private readonly HashSet<Type> _controllerTypes;
        private readonly bool _inherit;

        public ControllerRoutePrefixConvention(string routePrefix, bool combineRoutes, RouteCombinationStrategy combinationStrategy, Type controllerType, bool inherit = true) 
            : this(routePrefix, combineRoutes, combinationStrategy, new Type[] { controllerType }, inherit) { }

        public ControllerRoutePrefixConvention(string routePrefix, bool combineRoutes, RouteCombinationStrategy combinationStrategy, Assembly assembly, bool inherit = true)
            : this(routePrefix, combineRoutes, combinationStrategy, assembly.GetTypes().Where(type => type.GetCustomAttribute(typeof(ControllerAttribute)) != null), inherit) { }

        public ControllerRoutePrefixConvention(string routePrefix, bool combineRoutes, RouteCombinationStrategy combinationStrategy, IEnumerable<Type> controllerTypes, bool inherit = true)
        {
            _routePrefix = new AttributeRouteModel(new RouteAttribute(routePrefix));
            _combineRoutes = combineRoutes;
            _combinationStrategy = combinationStrategy;
            _controllerTypes = new HashSet<Type>(controllerTypes);
            _inherit = inherit;
        }

        public void Apply(ControllerModel controller)
        {
            if ((!_inherit || _controllerTypes.Any(type => !type.IsAssignableFrom(controller.ControllerType))) && !_controllerTypes.Contains(controller.ControllerType))
            {
                return;
            }

            foreach (var selector in controller.Selectors)
            {
                selector.AttributeRouteModel = selector.AttributeRouteModel == null || !_combineRoutes
                    ? _routePrefix 
                    : _combinationStrategy == RouteCombinationStrategy.Left 
                        ? AttributeRouteModel.CombineAttributeRouteModel(_routePrefix, selector.AttributeRouteModel)
                        : AttributeRouteModel.CombineAttributeRouteModel(selector.AttributeRouteModel, _routePrefix);
            }
        }
    }
}