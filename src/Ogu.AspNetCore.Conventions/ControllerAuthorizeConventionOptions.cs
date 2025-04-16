using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System;

namespace Ogu.AspNetCore.Conventions
{
    /// <summary>
    /// Represents options for configuring authorization settings on controllers.
    /// </summary>
    public class ControllerAuthorizeConventionOptions
    {
        /// <summary>
        /// Gets or sets the comma-delimited list of authentication schemes required for access.
        /// </summary>
        public string AuthenticationSchemes { get; set; }

        /// <summary>
        /// Gets or sets the authorization policy to apply.
        /// </summary>
        public string Policy { get; set; }

        /// <summary>
        /// Gets or sets the comma-delimited list of roles required for access.
        /// </summary>
        public string Roles { get; set; }

        /// <summary>
        /// Gets or sets a predicate that determines whether the convention should apply
        /// to a given auth properties for a specific controller.
        /// </summary>
        /// <remarks>
        /// The predicate receives the <see cref="ControllerModel"/>. 
        /// Return <c>true</c> to apply the convention, or <c>false</c> to skip it.
        /// </remarks>
        public Func<ControllerModel, bool> ShouldApplyTo { get; set; } = _ => true;
    }
}