using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Ogu.AspNetCore.Conventions
{
    public static class Extensions
    {
        public static void AddControllerRoutePrefixConvention<TController>(this IList<IApplicationModelConvention> conventions, string routePrefix, bool inherit = true)
        {
            AddControllerRoutePrefixConvention(conventions, typeof(TController), routePrefix, inherit);
        }

        public static void AddControllerRoutePrefixConvention(this IList<IApplicationModelConvention> conventions, Type controllerType, string routePrefix, bool inherit = true)
        {
            if (conventions == null)
            {
                throw new ArgumentNullException(nameof(conventions));
            }

            _ = controllerType.GetCustomAttribute(typeof(ControllerAttribute)) ?? throw new NotSupportedException($"{controllerType.Name} does not inherit from Controller.");

            conventions.Add(new ControllerRoutePrefixConvention(routePrefix, controllerType, inherit));
        }

        public static void AddControllerAuthorizeConvention<TController>(this IList<IApplicationModelConvention> conventions, bool inherit = true)
        {
            AddControllerAuthorizeConvention(conventions, typeof(TController), null, inherit);
        }

        public static void AddControllerAuthorizeConvention<TController>(this IList<IApplicationModelConvention> conventions, Action<ControllerAuthorizeConventionOptions> opts, bool inherit = true)
        {
            AddControllerAuthorizeConvention(conventions, typeof(TController), opts, inherit);           
        }

        public static void AddControllerAuthorizeConvention(this IList<IApplicationModelConvention> conventions, Type controllerType, bool inherit = true)
        {
            AddControllerAuthorizeConvention(conventions, controllerType, null, inherit);
        }

        public static void AddControllerAuthorizeConvention(this IList<IApplicationModelConvention> conventions, Type controllerType,
            Action<ControllerAuthorizeConventionOptions> opts, bool inherit = true)
        {
            if (conventions == null)
            {
                throw new ArgumentNullException(nameof(conventions));
            }

            _ = controllerType.GetCustomAttribute(typeof(ControllerAttribute)) ?? throw new NotSupportedException($"{controllerType.Name} does not inherit from Controller.");

            var authorizeConventionOpts = new ControllerAuthorizeConventionOptions();

            opts?.Invoke(authorizeConventionOpts);

            conventions.Add(new ControllerAuthorizeConvention(controllerType, authorizeConventionOpts.AuthenticationSchemes, authorizeConventionOpts.Policy, authorizeConventionOpts.Roles, inherit));
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

            _ = controllerType.GetCustomAttribute(typeof(ControllerAttribute)) ?? throw new NotSupportedException($"{controllerType.Name} does not inherit from Controller.");

            conventions.Add(new ControllerDisableConvention(controllerType, inherit));
        }
    }
}