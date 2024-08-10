namespace Ogu.AspNetCore.Conventions
{
    public class ControllerRoutePrefixOptions
    {
        public bool CombineRoutes { get; set; } = true;

        public RouteCombinationStrategy CombinationStrategy { get; set; }
    }

    public enum RouteCombinationStrategy
    {
        Left,
        Right
    }
}