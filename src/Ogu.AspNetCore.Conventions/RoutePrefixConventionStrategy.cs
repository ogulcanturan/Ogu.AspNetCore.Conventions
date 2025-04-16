namespace Ogu.AspNetCore.Conventions
{
    /// <summary>
    /// Defines the strategy to use when applying route handling logic
    /// in route conventions or registrations.
    /// </summary>
    public enum RoutePrefixConventionStrategy
    {
        /// <summary>
        /// Adds the new route alongside existing ones. May result in duplicates or conflicts. If conflicts occur, <see cref="ConflictingRoutesException"/> will be thrown.
        /// </summary>
        Add = 0,

        /// <summary>
        /// Removes the specified route prefix if it exists.
        /// </summary>
        Remove = 1,

        /// <summary>
        /// Combines the existing route with the new one. If conflicts occur, <see cref="ConflictingRoutesException"/> will be thrown.
        /// </summary>
        Combine = 2,
    }
}