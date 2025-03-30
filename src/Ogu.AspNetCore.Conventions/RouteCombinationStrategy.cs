namespace Ogu.AspNetCore.Conventions
{
    /// <summary>
    /// Defines the strategy for combining route prefixes.
    /// </summary>
    public enum RouteCombinationStrategy
    {
        /// <summary>
        /// Combines the route prefix to the left (default).
        /// </summary>
        Left,

        /// <summary>
        /// Combines the route prefix to the right.
        /// </summary>
        Right
    }
}