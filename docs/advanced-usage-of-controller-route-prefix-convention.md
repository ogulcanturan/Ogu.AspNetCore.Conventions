
# Advanced Usage of AddControllerRoutePrefixConvention

The `AddControllerRoutePrefixConvention` extension method allows you to register route prefixes on specific controllers. By default, it uses the strategy `RoutePrefixConventionStrategy.Add`, which appends a new prefix unless it already exists.

For more nuanced scenarios like replacing or combining routes, you can configure options via the `ControllerRoutePrefixOptions` parameter.

## Default Behavior: Add

Adds the route prefix `api/no-route` to `NoRouteController`.  
Uses default strategy: `RoutePrefixConventionStrategy.Add`

```csharp
opts.Conventions.AddControllerRoutePrefixConvention(typeof(NoRouteController), "api/no-route");
```

- Strategy: `RoutePrefixConventionStrategy.Add` (default)  
- Behavior: Skips route templates that already have the prefix.  
- Use Case: Introducing a new prefix without modifying existing ones.

## Controlling Application with ShouldApplyTo

```csharp
opts.Conventions.AddControllerRoutePrefixConvention(typeof(NoRouteController), "no-route", prefixOptions =>
{
    prefixOptions.ShouldApplyTo = (_, selector) => 
        !VersionChecker.HasVersionPrefix(selector.AttributeRouteModel!.Template);
});
```

This predicate determines whether the convention should apply to each route.  
For example, only apply the convention if the route does *not* already contain a version prefix like `v1`, `v2`, etc.

```csharp
public static class VersionChecker
{
    private static readonly Regex VersionRegex = new(@"\bv\d+\b", RegexOptions.IgnoreCase | RegexOptions.Compiled);

    public static bool HasVersionPrefix(string input)
    {
        return VersionRegex.IsMatch(input);
    }
}
```

## Strategy: Remove

Removes the `no-route` prefix from `NoRouteController`.

```csharp
opts.Conventions.AddControllerRoutePrefixConvention(typeof(NoRouteController), "no-route", prefixOptions =>
{
    prefixOptions.ConventionStrategy = RoutePrefixConventionStrategy.Remove;
});
```

- Use When: You want to strip certain route prefixes (e.g., deprecated paths).  
- Warning: Removing a prefix could leave the controller with no accessible route if no other template exists.

## Strategy: Combine

Combines `v1` with the existing route templates on `NoRouteController`, placing it to the right.

```csharp
opts.Conventions.AddControllerRoutePrefixConvention(typeof(NoRouteController), "v1", prefixOptions =>
{
    prefixOptions.ConventionStrategy = RoutePrefixConventionStrategy.Combine;
    prefixOptions.CombinationStrategy = RoutePrefixCombinationStrategy.Right;

    // Apply only if no version prefix is already present.
    prefixOptions.ShouldApplyTo = (_, selector) =>
        !VersionChecker.HasVersionPrefix(selector.AttributeRouteModel!.Template);
});
```

- Use When: You want to prepend or append route prefixes to existing routes.  
- CombinationStrategy:  
  - `Left` (default): `prefix/existing-template`  
  - `Right`: `existing-template/prefix`

## Summary

| Strategy                   | Purpose                                      | Notes                                                |
|---------------------------|----------------------------------------------|------------------------------------------------------|
| `Add` (default)           | Add new prefix unless already present        | Safe and non-invasive                                |
| `Remove`                  | Remove existing prefix                       | Use with caution                                     |
| `Combine`                 | Merge prefix with current routes             | Use `CombinationStrategy` to control position        |
| `ShouldApplyTo` predicate | Control logic per route selector             | Ideal for version-aware or conditional conventions   |

---

## Sample Application
A sample application demonstrating the usage of Ogu.AspNetCore.Conventions can be found [here](https://github.com/ogulcanturan/Ogu.AspNetCore.Conventions/tree/master/samples/).
