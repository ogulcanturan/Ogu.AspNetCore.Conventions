using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Ogu.AspNetCore.Conventions
{
    /// <summary>
    /// A convention that applies a route prefix to selected controllers.
    /// </summary>
    public class ControllerRoutePrefixConvention : IControllerModelConvention
    {
        private readonly AttributeRouteModel _routePrefix;
        private readonly bool _combineRoutes;
        private readonly RouteCombinationStrategy _combinationStrategy;
        private readonly HashSet<Type> _controllerTypes;
        private readonly bool _inherit;

        /// <summary>
        /// Initializes a new instance of the <see cref="ControllerRoutePrefixConvention"/> class
        /// with a specified route prefix, combination strategy, and a single controller type.
        /// </summary>
        /// <param name="routePrefix">The route prefix to apply.</param>
        /// <param name="combineRoutes">Indicates whether routes should be combined.</param>
        /// <param name="combinationStrategy">The strategy for combining routes.</param>
        /// <param name="controllerType">The controller type to apply the route prefix to.</param>
        /// <param name="inherit">Indicates whether inherited controllers should also receive the prefix. Default is <c>true</c>.</param>
        public ControllerRoutePrefixConvention(string routePrefix, bool combineRoutes, RouteCombinationStrategy combinationStrategy, Type controllerType, bool inherit = true) 
            : this(routePrefix, combineRoutes, combinationStrategy, new Type[] { controllerType }, inherit) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ControllerRoutePrefixConvention"/> class
        /// with a specified route prefix, combination strategy, and all controllers in an assembly.
        /// </summary>
        /// <param name="routePrefix">The route prefix to apply.</param>
        /// <param name="combineRoutes">Indicates whether routes should be combined.</param>
        /// <param name="combinationStrategy">The strategy for combining routes.</param>
        /// <param name="assembly">The assembly containing controllers to apply the route prefix to.</param>
        /// <param name="inherit">Indicates whether inherited controllers should also receive the prefix. Default is <c>true</c>.</param>
        public ControllerRoutePrefixConvention(string routePrefix, bool combineRoutes, RouteCombinationStrategy combinationStrategy, Assembly assembly, bool inherit = true)
            : this(routePrefix, combineRoutes, combinationStrategy, assembly.GetTypes().Where(type => type.GetCustomAttributes(typeof(ControllerAttribute)).Any()), inherit) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ControllerRoutePrefixConvention"/> class
        /// with a specified route prefix, combination strategy, and multiple controller types.
        /// </summary>
        /// <param name="routePrefix">The route prefix to apply.</param>
        /// <param name="combineRoutes">Indicates whether routes should be combined.</param>
        /// <param name="combinationStrategy">The strategy for combining routes.</param>
        /// <param name="controllerTypes">The collection of controller types to apply the route prefix to.</param>
        /// <param name="inherit">Indicates whether inherited controllers should also receive the prefix. Default is <c>true</c>.</param>
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