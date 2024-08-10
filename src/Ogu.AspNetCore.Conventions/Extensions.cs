using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Ogu.AspNetCore.Conventions
{
    public static class ConventionsExtensions
    {
        private static string GetInvalidControllerType(string controllerName) => $"{controllerName} does not inherit from Controller.";
        private const string InvalidControllerType = "One or more types do not inherit from Controller.";

        public static void AddControllerRoutePrefixConvention<TController>(
            this IList<IApplicationModelConvention> conventions,
            string routePrefix,
            Action<ControllerRoutePrefixOptions> configureOptions = null,
            bool inherit = true)
        {
            AddControllerRoutePrefixConvention(conventions, new[] { typeof(TController) }, routePrefix, configureOptions, inherit);
        }

        public static void AddControllerRoutePrefixConvention(
            this IList<IApplicationModelConvention> conventions,
            Type controllerType,
            string routePrefix,
            Action<ControllerRoutePrefixOptions> configureOptions = null,
            bool inherit = true)
        {
            AddControllerRoutePrefixConvention(conventions, new[] { controllerType }, routePrefix, configureOptions, inherit);
        }

        public static void AddControllerRoutePrefixConvention(this IList<IApplicationModelConvention> conventions, IEnumerable<Type> controllerTypes, string routePrefix, Action<ControllerRoutePrefixOptions> configureOptions = null, bool inherit = true)
        {
            if (conventions == null)
            {
                throw new ArgumentNullException(nameof(conventions));
            }

            var types = controllerTypes as Type[] ?? controllerTypes.ToArray();

            if (types.Any(type => type.GetCustomAttribute(typeof(ControllerAttribute)) == null))
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

        public static void AddControllerAuthorizeConvention<TController>(
            this IList<IApplicationModelConvention> conventions,
            Action<ControllerAuthorizeConventionOptions> configureOptions = null,
            bool inherit = true)
        {
            AddControllerAuthorizeConvention(conventions, typeof(TController), configureOptions, inherit);
        }

        public static void AddControllerAuthorizeConvention(this IList<IApplicationModelConvention> conventions, Type controllerType, Action<ControllerAuthorizeConventionOptions> configureOptions = null, bool inherit = true)
        {
            if (conventions == null)
            {
                throw new ArgumentNullException(nameof(conventions));
            }

            if (controllerType.GetCustomAttribute(typeof(ControllerAttribute)) == null)
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

        public static void AddControllerAuthorizeConvention(this IList<IApplicationModelConvention> conventions, IEnumerable<Type> controllerTypes, Action<ControllerAuthorizeConventionOptions> configureOptions = null, bool inherit = true)
        {
            if (conventions == null)
            {
                throw new ArgumentNullException(nameof(conventions));
            }

            var types = controllerTypes.ToArray();

            if (types.Any(type => type.GetCustomAttribute(typeof(ControllerAttribute)) == null))
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

        public static void AddControllerDisableConvention<TController>(this IList<IApplicationModelConvention> conventions, bool inherit = true)
        {
            AddControllerDisableConvention(conventions, typeof(TController), inherit);
        }

        public static void AddControllerDisableConvention(this IList<IApplicationModelConvention> conventions, Type controllerType, bool inherit = true)
        {
            if (conventions == null)
            {
                throw new ArgumentNullException(nameof(conventions));
            }

            if (controllerType.GetCustomAttribute(typeof(ControllerAttribute)) == null)
            {
                throw new NotSupportedException(GetInvalidControllerType(controllerType.Name));
            }

            conventions.Add(new ControllerDisableConvention(controllerType, inherit));
        }

        public static void AddControllerDisableConvention(this IList<IApplicationModelConvention> conventions, Assembly assembly, bool inherit = true)
        {
            if (conventions == null)
            {
                throw new ArgumentNullException(nameof(conventions));
            }

            conventions.Add(new ControllerDisableConvention(assembly, inherit));
        }

        public static void AddControllerDisableConvention(this IList<IApplicationModelConvention> conventions, IEnumerable<Type> controllerTypes, bool inherit = true)
        {
            if (conventions == null)
            {
                throw new ArgumentNullException(nameof(conventions));
            }

            var types = controllerTypes.ToArray();

            if (types.Any(type => type.GetCustomAttribute(typeof(ControllerAttribute)) == null))
            {
                throw new ArgumentException(InvalidControllerType);
            }

            conventions.Add(new ControllerDisableConvention(types, inherit));
        }

        public static void AddControllerHideFromExploringConvention<TController>(this IList<IApplicationModelConvention> conventions, bool inherit = true)
        {
            AddControllerHideFromExploringConvention(conventions, typeof(TController), inherit);
        }

        public static void AddControllerHideFromExploringConvention(this IList<IApplicationModelConvention> conventions, Type controllerType, bool inherit = true)
        {
            if (conventions == null)
            {
                throw new ArgumentNullException(nameof(conventions));
            }

            if (controllerType.GetCustomAttribute(typeof(ControllerAttribute)) == null)
            {
                throw new NotSupportedException(GetInvalidControllerType(controllerType.Name));
            }

            conventions.Add(new ControllerHideFromExploringConvention(controllerType, inherit));
        }

        public static void AddControllerHideFromExploringConvention(this IList<IApplicationModelConvention> conventions, IEnumerable<Type> controllerTypes, bool inherit = true)
        {
            if (conventions == null)
            {
                throw new ArgumentNullException(nameof(conventions));
            }

            conventions.Add(new ControllerHideFromExploringConvention(controllerTypes, inherit));
        }

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