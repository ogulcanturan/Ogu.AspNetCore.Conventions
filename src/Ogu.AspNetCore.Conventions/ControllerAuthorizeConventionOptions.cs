namespace Ogu.AspNetCore.Conventions
{
    /// <summary>
    /// Represents options for configuring authorization settings on controllers.
    /// </summary>
    public class ControllerAuthorizeConventionOptions
    {
        /// <summary>
        /// Gets or sets the authentication schemes required for access.
        /// </summary>
        public string AuthenticationSchemes { get; set; }

        /// <summary>
        /// Gets or sets the authorization policy to apply.
        /// </summary>
        public string Policy { get; set; }

        /// <summary>
        /// Gets or sets the roles required for access.
        /// </summary>
        public string Roles { get; set; }
    }
}