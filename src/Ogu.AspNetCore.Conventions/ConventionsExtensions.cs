using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Ogu.AspNetCore.Conventions
{
    /// <summary>
    /// Provides extension methods for adding controller-related conventions to the application model.
    /// </summary>
    public static class ConventionsExtensions
    {
        private static string GetInvalidControllerType(string controllerName) => $"{controllerName} does not inherit from Controller.";

        private const string InvalidControllerType = "One or more types do not inherit from Controller.";

        /// <summary>
        /// Adds a route prefix convention for a specific controller type.
        /// </summary>
        /// <typeparam name="TController">The controller type.</typeparam>
        /// <param name="conventions">The list of application model conventions.</param>
        /// <param name="routePrefix">The route prefix to apply.</param>
        /// <param name="configureOptions">Optional configuration for route prefix behavior.</param>
        /// <param name="inherit">Determines whether inherited controllers should also receive the prefix.</param>
        public static void AddControllerRoutePrefixConvention<TController>(this IList<IApplicationModelConvention> conventions, string routePrefix, Action<ControllerRoutePrefixOptions> configureOptions = null, bool inherit = true)
        {
            AddControllerRoutePrefixConvention(conventions, new[] { typeof(TController) }, routePrefix, configureOptions, inherit);
        }

        /// <summary>
        /// Adds a controller route prefix convention for the specified controller type.
        /// </summary>
        /// <param name="conventions">The list of application model conventions.</param>
        /// <param name="controllerType">The type of the controller.</param>
        /// <param name="routePrefix">The route prefix to be added.</param>
        /// <param name="configureOptions">Optional action to configure route prefix options.</param>
        /// <param name="inherit">Indicates whether to inherit route prefix.</param>
        public static void AddControllerRoutePrefixConvention(this IList<IApplicationModelConvention> conventions, Type controllerType, string routePrefix, Action<ControllerRoutePrefixOptions> configureOptions = null, bool inherit = true)
        {
            AddControllerRoutePrefixConvention(conventions, new[] { controllerType }, routePrefix, configureOptions, inherit);
        }

        /// <summary>
        /// Adds a controller route prefix convention for the specified list of controller types.
        /// </summary>
        /// <param name="conventions">The list of application model conventions.</param>
        /// <param name="controllerTypes">The list of controller types.</param>
        /// <param name="routePrefix">The route prefix to be added.</param>
        /// <param name="configureOptions">Optional action to configure route prefix options.</param>
        /// <param name="inherit">Indicates whether to inherit route prefix.</param>
        public static void AddControllerRoutePrefixConvention(this IList<IApplicationModelConvention> conventions, IEnumerable<Type> controllerTypes, string routePrefix, Action<ControllerRoutePrefixOptions> configureOptions = null, bool inherit = true)
        {
            if (conventions == null)
            {
                throw new ArgumentNullException(nameof(conventions));
            }

            var types = controllerTypes as Type[] ?? controllerTypes.ToArray();

            if (types.Any(type => !type.GetCustomAttributes(typeof(ControllerAttribute)).Any()))
            {
                throw new ArgumentException(InvalidControllerType);
            }

            var options = new ControllerRoutePrefixOptions();
            configureOptions?.Invoke(options);

            conventions.Add(new ControllerRoutePrefixConvention(
                routePrefix,
                options.CombineRoutes,
                options.CombinationStrategy,
                types,
                inherit));
        }

        /// <summary>
        /// Adds a controller route prefix convention for the specified assembly.
        /// </summary>
        /// <param name="conventions">The list of application model conventions.</param>
        /// <param name="assembly">The assembly containing the controllers.</param>
        /// <param name="routePrefix">The route prefix to be added.</param>
        /// <param name="configureOptions">Optional action to configure route prefix options.</param>
        /// <param name="inherit">Indicates whether to inherit route prefix.</param>
        public static void AddControllerRoutePrefixConvention(this IList<IApplicationModelConvention> conventions, Assembly assembly, string routePrefix, Action<ControllerRoutePrefixOptions> configureOptions = null, bool inherit = true)
        {
            if (conventions == null)
            {
                throw new ArgumentNullException(nameof(conventions));
            }

            var options = new ControllerRoutePrefixOptions();
            configureOptions?.Invoke(options);

            conventions.Add(new ControllerRoutePrefixConvention(
                routePrefix,
                options.CombineRoutes,
                options.CombinationStrategy,
                assembly,
                inherit));
        }

        /// <summary>
        /// Adds a controller authorize convention for the specified controller type.
        /// </summary>
        /// <param name="conventions">The list of application model conventions.</param>
        /// <param name="configureOptions">Optional action to configure authorization convention options.</param>
        /// <param name="inherit">Indicates whether to inherit authorization settings.</param>
        public static void AddControllerAuthorizeConvention<TController>(
            this IList<IApplicationModelConvention> conventions,
            Action<ControllerAuthorizeConventionOptions> configureOptions = null,
            bool inherit = true)
        {
            AddControllerAuthorizeConvention(conventions, typeof(TController), configureOptions, inherit);
        }

        /// <summary>
        /// Adds a controller authorize convention for the specified controller type.
        /// </summary>
        /// <param name="conventions">The list of application model conventions.</param>
        /// <param name="controllerType">The type of the controller.</param>
        /// <param name="configureOptions">Optional action to configure authorization convention options.</param>
        /// <param name="inherit">Indicates whether to inherit authorization settings.</param>
        public static void AddControllerAuthorizeConvention(this IList<IApplicationModelConvention> conventions, Type controllerType, Action<ControllerAuthorizeConventionOptions> configureOptions = null, bool inherit = true)
        {
            if (conventions == null)
            {
                throw new ArgumentNullException(nameof(conventions));
            }

            if (!controllerType.GetCustomAttributes(typeof(ControllerAttribute)).Any())
            {
                throw new NotSupportedException(GetInvalidControllerType(controllerType.Name));
            }

            var options = new ControllerAuthorizeConventionOptions();
            configureOptions?.Invoke(options);

            conventions.Add(new ControllerAuthorizeConvention(
                controllerType,
                options.AuthenticationSchemes,
                options.Policy,
                options.Roles,
                inherit));
        }

        /// <summary>
        /// Adds a controller authorize convention for the specified list of controller types.
        /// </summary>
        /// <param name="conventions">The list of application model conventions.</param>
        /// <param name="controllerTypes">The list of controller types.</param>
        /// <param name="configureOptions">Optional action to configure authorization convention options.</param>
        /// <param name="inherit">Indicates whether to inherit authorization settings.</param>
        public static void AddControllerAuthorizeConvention(this IList<IApplicationModelConvention> conventions, IEnumerable<Type> controllerTypes, Action<ControllerAuthorizeConventionOptions> configureOptions = null, bool inherit = true)
        {
            if (conventions == null)
            {
                throw new ArgumentNullException(nameof(conventions));
            }

            var types = controllerTypes.ToArray();

            if (types.Any(type => !type.GetCustomAttributes(typeof(ControllerAttribute)).Any()))
            {
                throw new ArgumentException(InvalidControllerType);
            }

            var options = new ControllerAuthorizeConventionOptions();
            configureOptions?.Invoke(options);

            conventions.Add(new ControllerAuthorizeConvention(
                types,
                options.AuthenticationSchemes,
                options.Policy,
                options.Roles,
                inherit));
        }

        /// <summary>
        /// Adds a controller authorize convention for the controllers in the specified assembly.
        /// </summary>
        /// <param name="conventions">The list of application model conventions.</param>
        /// <param name="assembly">The assembly containing the controllers.</param>
        /// <param name="configureOptions">Optional action to configure authorization convention options.</param>
        /// <param name="inherit">Indicates whether to inherit authorization settings.</param>
        public static void AddControllerAuthorizeConvention(this IList<IApplicationModelConvention> conventions, Assembly assembly, Action<ControllerAuthorizeConventionOptions> configureOptions = null, bool inherit = true)
        {
            if (conventions == null)
            {
                throw new ArgumentNullException(nameof(conventions));
            }

            var options = new ControllerAuthorizeConventionOptions();
            configureOptions?.Invoke(options);

            conventions.Add(new ControllerAuthorizeConvention(
                assembly,
                options.AuthenticationSchemes,
                options.Policy,
                options.Roles,
                inherit));
        }

        /// <summary>
        /// Adds a controller disable convention for the specified controller type.
        /// </summary>
        /// <param name="conventions">The list of application model conventions.</param>
        /// <param name="inherit">Indicates whether to inherit disable convention settings.</param>
        public static void AddControllerDisableConvention<TController>(this IList<IApplicationModelConvention> conventions, bool inherit = true)
        {
            AddControllerDisableConvention(conventions, typeof(TController), inherit);
        }

        /// <summary>
        /// Adds a controller disable convention for the specified controller type.
        /// </summary>
        /// <param name="conventions">The list of application model conventions.</param>
        /// <param name="controllerType">The type of the controller.</param>
        /// <param name="inherit">Indicates whether to inherit disable convention settings.</param>
        public static void AddControllerDisableConvention(this IList<IApplicationModelConvention> conventions, Type controllerType, bool inherit = true)
        {
            if (conventions == null)
            {
                throw new ArgumentNullException(nameof(conventions));
            }

            if (!controllerType.GetCustomAttributes(typeof(ControllerAttribute)).Any())
            {
                throw new NotSupportedException(GetInvalidControllerType(controllerType.Name));
            }

            conventions.Add(new ControllerDisableConvention(controllerType, inherit));
        }

        /// <summary>
        /// Adds a controller disable convention for the controllers in the specified assembly.
        /// </summary>
        /// <param name="conventions">The list of application model conventions.</param>
        /// <param name="assembly">The assembly containing the controllers.</param>
        /// <param name="inherit">Indicates whether to inherit disable convention settings.</param>
        public static void AddControllerDisableConvention(this IList<IApplicationModelConvention> conventions, Assembly assembly, bool inherit = true)
        {
            if (conventions == null)
            {
                throw new ArgumentNullException(nameof(conventions));
            }

            conventions.Add(new ControllerDisableConvention(assembly, inherit));
        }

        /// <summary>
        /// Adds a controller disable convention for the specified list of controller types.
        /// </summary>
        /// <param name="conventions">The list of application model conventions.</param>
        /// <param name="controllerTypes">The list of controller types.</param>
        /// <param name="inherit">Indicates whether to inherit disable convention settings.</param>
        public static void AddControllerDisableConvention(this IList<IApplicationModelConvention> conventions, IEnumerable<Type> controllerTypes, bool inherit = true)
        {
            if (conventions == null)
            {
                throw new ArgumentNullException(nameof(conventions));
            }

            var types = controllerTypes.ToArray();

            if (types.Any(type => !type.GetCustomAttributes(typeof(ControllerAttribute)).Any()))
            {
                throw new ArgumentException(InvalidControllerType);
            }

            conventions.Add(new ControllerDisableConvention(types, inherit));
        }

        /// <summary>
        /// Adds a controller hide from exploring convention for the specified controller type.
        /// </summary>
        /// <param name="conventions">The list of application model conventions.</param>
        /// <param name="inherit">Indicates whether to inherit hide from exploring convention settings.</param>
        public static void AddControllerHideFromExploringConvention<TController>(this IList<IApplicationModelConvention> conventions, bool inherit = true)
        {
            AddControllerHideFromExploringConvention(conventions, typeof(TController), inherit);
        }

        /// <summary>
        /// Adds a controller hide from exploring convention for the specified controller type.
        /// </summary>
        /// <param name="conventions">The list of application model conventions.</param>
        /// <param name="controllerType">The type of the controller.</param>
        /// <param name="inherit">Indicates whether to inherit hide from exploring convention settings.</param>
        public static void AddControllerHideFromExploringConvention(this IList<IApplicationModelConvention> conventions, Type controllerType, bool inherit = true)
        {
            if (conventions == null)
            {
                throw new ArgumentNullException(nameof(conventions));
            }

            if (!controllerType.GetCustomAttributes(typeof(ControllerAttribute)).Any())
            {
                throw new NotSupportedException(GetInvalidControllerType(controllerType.Name));
            }

            conventions.Add(new ControllerHideFromExploringConvention(controllerType, inherit));
        }

        /// <summary>
        /// Adds a controller hide from exploring convention for the specified list of controller types.
        /// </summary>
        /// <param name="conventions">The list of application model conventions.</param>
        /// <param name="controllerTypes">The list of controller types.</param>
        /// <param name="inherit">Indicates whether to inherit hide from exploring convention settings.</param>
        public static void AddControllerHideFromExploringConvention(this IList<IApplicationModelConvention> conventions, IEnumerable<Type> controllerTypes, bool inherit = true)
        {
            if (conventions == null)
            {
                throw new ArgumentNullException(nameof(conventions));
            }

            conventions.Add(new ControllerHideFromExploringConvention(controllerTypes, inherit));
        }

        /// <summary>
        /// Adds a controller hide from exploring convention for the controllers in the specified assembly.
        /// </summary>
        /// <param name="conventions">The list of application model conventions.</param>
        /// <param name="assembly">The assembly containing the controllers.</param>
        /// <param name="inherit">Indicates whether to inherit hide from exploring convention settings.</param>
        public static void AddControllerHideFromExploringConvention(this IList<IApplicationModelConvention> conventions, Assembly assembly, bool inherit = true)
        {
            if (conventions == null)
            {
                throw new ArgumentNullException(nameof(conventions));
            }

            conventions.Add(new ControllerHideFromExploringConvention(assembly, inherit));
        }
    }
}