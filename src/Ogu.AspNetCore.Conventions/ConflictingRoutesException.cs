using System;
using System.Collections.Generic;
using System.Linq;

namespace Ogu.AspNetCore.Conventions
{
    /// <summary>
    /// Exception thrown when conflicting routes are detected
    /// while route combination is disabled.
    /// </summary>
    /// <remarks>
    /// This exception is typically thrown when multiple controllers or actions result in the same route path.
    /// </remarks>
    public class ConflictingRoutesException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConflictingRoutesException"/> class
        /// with a predefined error message explaining the route conflict.
        /// </summary>
        public ConflictingRoutesException()
            : base($"Conflicting route(s) were detected.")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConflictingRoutesException"/> class
        /// with the specified conflicting route paths.
        /// </summary>
        /// <param name="conflictingRoutes">The route paths that caused the conflict.</param>
        public ConflictingRoutesException(IEnumerable<string> conflictingRoutes)
            : this(conflictingRoutes?.ToList() ?? new List<string>())
        {
        }

        private ConflictingRoutesException(List<string> conflictingRoutes)
            : base(GenerateMessage(conflictingRoutes))
        {
            ConflictingRoutes = conflictingRoutes;
        }

        /// <summary>
        /// Gets the list of route paths that caused the conflict.
        /// </summary>
        public IReadOnlyList<string> ConflictingRoutes { get; }

        private static string GenerateMessage(IEnumerable<string> routes)
        {
            var routeList = string.Join(", ", routes);

            return $"Conflicting route(s) were detected: {routeList}.";
        }
    }
}