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
        private readonly AttributeRouteModel[] _routePrefixes;
        private readonly ControllerRoutePrefixConventionOptions _routePrefixConventionOptions;
        private readonly HashSet<Type> _controllerTypes;
        private readonly bool _inherit;

        /// <summary>
        /// Initializes a new instance of the <see cref="ControllerRoutePrefixConvention"/> class
        /// with a specified route prefix, combination strategy, and a single controller type.
        /// </summary>
        /// <param name="routePrefix">The route prefix to apply.</param>
        /// <param name="routePrefixConventionOptions">The route prefix options.</param>
        /// <param name="controllerType">The controller type to apply the route prefix to.</param>
        /// <param name="inherit">Indicates whether inherited controllers should also receive the prefix. Default is <c>true</c>.</param>
        public ControllerRoutePrefixConvention(string routePrefix, ControllerRoutePrefixConventionOptions routePrefixConventionOptions, Type controllerType, bool inherit = true)
            : this(routePrefix, routePrefixConventionOptions, new[] { controllerType }, inherit) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ControllerRoutePrefixConvention"/> class
        /// with a specified route prefix, combination strategy, and a single controller type.
        /// </summary>
        /// <param name="routePrefixes">A collection of route prefixes to apply.</param>
        /// <param name="routePrefixConventionOptions">The route prefix options.</param>
        /// <param name="controllerType">The controller type to apply the route prefix to.</param>
        /// <param name="inherit">Indicates whether inherited controllers should also receive the prefix. Default is <c>true</c>.</param>
        public ControllerRoutePrefixConvention(IEnumerable<string> routePrefixes, ControllerRoutePrefixConventionOptions routePrefixConventionOptions, Type controllerType, bool inherit = true)
            : this(routePrefixes, routePrefixConventionOptions, new[] { controllerType }, inherit) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ControllerRoutePrefixConvention"/> class
        /// with a specified route prefix, combination strategy, and all controllers in an assembly.
        /// </summary>
        /// <param name="routePrefix">The route prefix to apply.</param>
        /// <param name="routePrefixConventionOptions">The route prefix options.</param>
        /// <param name="assembly">The assembly containing controllers to apply the route prefix to.</param>
        /// <param name="inherit">Indicates whether inherited controllers should also receive the prefix. Default is <c>true</c>.</param>
        public ControllerRoutePrefixConvention(string routePrefix, ControllerRoutePrefixConventionOptions routePrefixConventionOptions, Assembly assembly, bool inherit = true)
            : this(routePrefix, routePrefixConventionOptions, assembly.GetTypes().Where(type => type.GetCustomAttributes(typeof(ControllerAttribute)).Any()), inherit) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ControllerRoutePrefixConvention"/> class
        /// with a specified route prefix, combination strategy, and all controllers in an assembly.
        /// </summary>
        /// <param name="routePrefixes">A collection of route prefixes to apply.</param>
        /// <param name="routePrefixConventionOptions">The route prefix options.</param>
        /// <param name="assembly">The assembly containing controllers to apply the route prefix to.</param>
        /// <param name="inherit">Indicates whether inherited controllers should also receive the prefix. Default is <c>true</c>.</param>
        public ControllerRoutePrefixConvention(IEnumerable<string> routePrefixes, ControllerRoutePrefixConventionOptions routePrefixConventionOptions, Assembly assembly, bool inherit = true)
            : this(routePrefixes, routePrefixConventionOptions, assembly.GetTypes().Where(type => type.GetCustomAttributes(typeof(ControllerAttribute)).Any()), inherit) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ControllerRoutePrefixConvention"/> class
        /// with a specified route prefix, combination strategy, and multiple controller types.
        /// </summary>
        /// <param name="routePrefix">The route prefix to apply.</param>
        /// <param name="routePrefixConventionOptions">The route prefix options.</param>
        /// <param name="controllerTypes">The collection of controller types to apply the route prefix to.</param>
        /// <param name="inherit">Indicates whether inherited controllers should also receive the prefix. Default is <c>true</c>.</param>
        public ControllerRoutePrefixConvention(string routePrefix, ControllerRoutePrefixConventionOptions routePrefixConventionOptions, IEnumerable<Type> controllerTypes,
            bool inherit = true) : this(new[] { routePrefix },
            routePrefixConventionOptions, controllerTypes, inherit) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ControllerRoutePrefixConvention"/> class
        /// with multiple route prefixes and controller types.
        /// </summary>
        /// <param name="routePrefixes">A collection of route prefixes to apply.</param>
        /// <param name="routePrefixConventionOptions">The route prefix options.</param>
        /// <param name="controllerTypes">The controller types to apply the route prefixes to.</param>
        /// <param name="inherit">Whether to include inherited controllers. Default is <c>true</c>.</param>
        public ControllerRoutePrefixConvention(IEnumerable<string> routePrefixes, ControllerRoutePrefixConventionOptions routePrefixConventionOptions, IEnumerable<Type> controllerTypes, bool inherit = true)
        {
            _routePrefixes = routePrefixes
                .Select(prefix => new AttributeRouteModel(new RouteAttribute(prefix)))
                .ToArray();

            _routePrefixConventionOptions = routePrefixConventionOptions;

            _controllerTypes = new HashSet<Type>(controllerTypes);
            _inherit = inherit;
        }

        public void Apply(ControllerModel controller)
        {
            if ((!_inherit || _controllerTypes.Any(type => !type.IsAssignableFrom(controller.ControllerType))) && !_controllerTypes.Contains(controller.ControllerType))
            {
                return;
            }

            switch (_routePrefixConventionOptions.ConventionStrategy)
            {
                case RoutePrefixConventionStrategy.Add:

                    ApplyAddStrategy(controller);

                    break;

                case RoutePrefixConventionStrategy.Combine:

                    ApplyCombineStrategy(controller);

                    break;

                case RoutePrefixConventionStrategy.Remove:

                    ApplyRemoveStrategy(controller);

                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void ApplyAddStrategy(ControllerModel controller)
        {
            var existingTemplates = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            var duplicates = controller.Selectors
                .Where(s => s.AttributeRouteModel != null)
                .Select(s =>
                {
                    existingTemplates.Add(s.AttributeRouteModel.Template);

                    return s.AttributeRouteModel.Template;
                })
                .GroupBy(x => x, StringComparer.OrdinalIgnoreCase)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .ToList();

            if (duplicates.Count > 0)
            {
                throw new ConflictingRoutesException(duplicates);
            }

            foreach (var routePrefix in _routePrefixes.Where(r => !existingTemplates.Contains(r.Template)))
            {
                var selector = new SelectorModel
                {
                    AttributeRouteModel = routePrefix
                };

                if (_routePrefixConventionOptions.ShouldApplyTo(controller, selector))
                {
                    controller.Selectors.Add(selector);
                }
            }
        }

        private void ApplyRemoveStrategy(ControllerModel controller)
        {
            var distinctPrefixes = new HashSet<string>(_routePrefixes.Select(r => r.Template), StringComparer.OrdinalIgnoreCase);

            for (var i = controller.Selectors.Count - 1; i >= 0; i--)
            {
                var selector = controller.Selectors[i];
                
                if (selector.AttributeRouteModel != null && distinctPrefixes.Contains(selector.AttributeRouteModel.Template) && _routePrefixConventionOptions.ShouldApplyTo(controller, selector))
                {
                    controller.Selectors.RemoveAt(i);
                }
            }
        }

        private void ApplyCombineStrategy(ControllerModel controller)
        {
            var templates = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            HashSet<string> duplicates = null;

            foreach (var routePrefix in _routePrefixes)
            {
                foreach (var selector in controller.Selectors.Where(sel => sel.AttributeRouteModel != null && _routePrefixConventionOptions.ShouldApplyTo(controller, sel)))
                {
                    selector.AttributeRouteModel =
                        _routePrefixConventionOptions.CombinationStrategy == RoutePrefixCombinationStrategy.Left
                            ? AttributeRouteModel.CombineAttributeRouteModel(routePrefix, selector.AttributeRouteModel)
                            : AttributeRouteModel.CombineAttributeRouteModel(selector.AttributeRouteModel, routePrefix);

                    if (templates.Add(selector.AttributeRouteModel.Template))
                    {
                        continue;
                    }

                    duplicates = duplicates ?? new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                    duplicates.Add(selector.AttributeRouteModel.Template);
                }
            }

            if (duplicates?.Count > 0)
            {
                throw new ConflictingRoutesException(duplicates);
            }
        }
    }
}