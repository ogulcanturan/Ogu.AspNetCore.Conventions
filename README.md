# Ogu.AspNetCore.Conventions

[![.NET](https://github.com/ogulcanturan/Ogu.AspNetCore.Conventions/actions/workflows/dotnet.yml/badge.svg?branch=master)](https://github.com/ogulcanturan/Ogu.AspNetCore.Conventions/actions/workflows/dotnet.yml)
[![NuGet](https://img.shields.io/nuget/v/Ogu.AspNetCore.Conventions.svg?color=1ecf18)](https://nuget.org/packages/Ogu.AspNetCore.Conventions)
[![Nuget](https://img.shields.io/nuget/dt/Ogu.AspNetCore.Conventions.svg?logo=nuget)](https://nuget.org/packages/Ogu.AspNetCore.Conventions)

## Introduction

Ogu.AspNetCore.Conventions is a library which comprising three essential conventions that can be configured in the IMvcBuilder.

## Conventions

- **ControllerAuthorizeConvention:** Simplify authorization configuration by defining authorization rules within your IOC container. This convention allows you to enforce access control policies on controller actions without the need for explicit AuthorizeAttribute annotations.
- **ControllerDisableConvention:** Instead of removing entire controller assemblies, this convention lets you disable individual controllers.
- **ControllerRoutePrefixConvention:** This convention eliminates the need to clutter your controller classes with RouteAttribute annotations, allowing for cleaner and more flexible routing configurations.

## Installation

You can install the library via NuGet Package Manager:

```bash
dotnet add package Ogu.AspNetCore.Conventions
```

## Usage

**ControllerAuthorizeConvention:**
```csharp
public virtual void ConfigureServices(IServiceCollection services)
{
    ...
    services.AddControllers().AddMvcOptions(opts =>
    {
        // To configure with params => [Authorize(AuthenticationSchemes = "", Policy = "", Roles = "")]
        opts.Conventions.AddControllerAuthorizeConvention<LauncherController>(conventionOpts =>
        {
            conventionOpts.AuthenticationSchemes = "";
            conventionOpts.Policy = "";
            conventionOpts.Roles = "";
        });

        // to use default => [Authorize]
        opts.Conventions.AddControllerAuthorizeConvention<LauncherController>();
    });
    ...
}
```

**ControllerDisableConvention:**
```csharp
 public virtual void ConfigureServices(IServiceCollection services)
 {
    ...
    services.AddControllers().AddMvcOptions(opts =>
    {
        opts.Conventions.AddControllerDisableConvention<LauncherController>();
    });
    ...
 }
```

**ControllerRoutePrefixConvention:**
```csharp
public virtual void ConfigureServices(IServiceCollection services)
{
    ...
    services.AddControllers().AddMvcOptions(opts =>
    {
        opts.Conventions.AddControllerRoutePrefixConvention<LauncherController>("api/launcher");
    });
    ...
}
```
