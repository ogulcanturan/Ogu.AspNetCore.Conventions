namespace Ogu.AspNetCore.Conventions
{
    public class ControllerAuthorizeConventionOptions
    {
        public string AuthenticationSchemes { get; set; }

        public string Policy { get; set; }

        public string Roles { get; set; }
    }
}