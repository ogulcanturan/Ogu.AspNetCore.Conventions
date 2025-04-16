using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System;

namespace Ogu.AspNetCore.Conventions
{
    /// <summary>
    /// Represents options for configuring route prefixes in a controller.
    /// </summary>
    public class ControllerRoutePrefixOptions
    {
        /// <summary>
        /// Gets or sets the strategy for combining routes.
        /// Default is <see cref="RoutePrefixCombinationStrategy.Left"/>.
        /// </summary>
        /// <remarks>
        /// This only applicable when <see cref="ConventionStrategy"/> is set to <see cref="RoutePrefixConventionStrategy.Combine"/>. Available values:
        /// <list type="bullet">
        ///   <item>
        ///     <term><see cref="RoutePrefixCombinationStrategy.Left"/></term>
        ///     <description>Combines the route prefix to the left (default).</description>
        ///   </item>
        ///   <item>
        ///     <term><see cref="RoutePrefixCombinationStrategy.Right"/></term>
        ///     <description>Combines the route prefix to the right.</description>
        ///   </item>
        /// </list>
        /// </remarks>
        public RoutePrefixCombinationStrategy CombinationStrategy { get; set; }

        /// <summary>
        /// Gets or sets the strategy for the route handling.
        /// Default is <see cref="RoutePrefixConventionStrategy.Add"/>.
        /// </summary>
        public RoutePrefixConventionStrategy ConventionStrategy { get; set; }

        /// <summary>
        /// Gets or sets a predicate that determines whether the convention should apply
        /// to a given route template for a specific controller and selector.
        /// </summary>
        /// <remarks>
        /// The predicate receives the <see cref="ControllerModel"/> and <see cref="SelectorModel"/>. 
        /// Return <c>true</c> to apply the convention, or <c>false</c> to skip it.
        /// </remarks>
        public Func<ControllerModel, SelectorModel, bool> ShouldApplyTo { get; set; } = (_, __) => true;
    }
}