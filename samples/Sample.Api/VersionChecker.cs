using System.Text.RegularExpressions;

namespace Sample.Api
{
    public static class VersionChecker
    {
        private static readonly Regex VersionRegex = new(@"\bv\d+\b", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        /// <summary>
        /// Checks if the route template starts with a version prefix, such as 'v1', 'v2', etc.
        /// </summary>
        /// <param name="input">The route template string to check for a version prefix.</param>
        /// <returns>
        /// Returns <c>true</c> if the input starts with a version prefix (e.g., 'v1', 'v2'); otherwise, <c>false</c>.
        /// </returns>
        public static bool HasVersionPrefix(string input)
        {
            return VersionRegex.IsMatch(input);
        }
    }
}