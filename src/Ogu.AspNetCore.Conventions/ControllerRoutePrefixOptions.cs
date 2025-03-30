namespace Ogu.AspNetCore.Conventions
{
    /// <summary>
    /// Represents options for configuring route prefixes in a controller.
    /// </summary>
    public class ControllerRoutePrefixOptions
    {
        /// <summary>
        /// Gets or sets a value indicating whether routes should be combined.
        /// Default is <c>true</c>.
        /// </summary>
        public bool CombineRoutes { get; set; } = true;

        /// <summary>
        /// Gets or sets the strategy for combining routes.
        /// Default is <see cref="RouteCombinationStrategy.Left"/>.
        /// </summary>
        /// <remarks>
        /// Available values:
        /// <list type="bullet">
        ///   <item>
        ///     <term><see cref="RouteCombinationStrategy.Left"/></term>
        ///     <description>Combines the route prefix to the left (default).</description>
        ///   </item>
        ///   <item>
        ///     <term><see cref="RouteCombinationStrategy.Right"/></term>
        ///     <description>Combines the route prefix to the right.</description>
        ///   </item>
        /// </list>
        /// </remarks>
        public RouteCombinationStrategy CombinationStrategy { get; set; }
    }
}