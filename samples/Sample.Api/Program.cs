using External.Api.Controllers;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Ogu.AspNetCore.Conventions;
using Sample.Api;
using Sample.Api.Controllers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddMvcOptions(opts =>
{
    // To hide all assembly's controllers - still can be accessible through the HTTP requests but won't be visible
    opts.Conventions.AddControllerHideFromExploringConvention(typeof(SecretController).Assembly);

    /* To disable controller - won't be visible and accessible through the HTTP requests */
    // opts.Conventions.AddControllerDisableConvention(typeof(VerySecretController));

    // Adding 'api/no-route^' to the `NoRouteController` - Default is `RoutePrefixConventionStrategy.Add`
    opts.Conventions.AddControllerRoutePrefixConvention(typeof(NoRouteController), "api/no-route");
    // Removing existing 'no-route' route prefix from the `NoRouteController`
    opts.Conventions.AddControllerRoutePrefixConvention(typeof(NoRouteController), "no-route", prefixOptions =>
    {
        prefixOptions.ConventionStrategy = RoutePrefixConventionStrategy.Remove;
    });
    // Combining v1 to the right -> api/no-route/v1
    opts.Conventions.AddControllerRoutePrefixConvention(typeof(NoRouteController), "v1", prefixOptions =>
    {
        prefixOptions.ConventionStrategy = RoutePrefixConventionStrategy.Combine;
        prefixOptions.CombinationStrategy = RoutePrefixCombinationStrategy.Right;

        // Check if the route template contains a version prefix (e.g., v1, v2). 
        // If no version prefix is found, the convention is applied.
        prefixOptions.ShouldApplyTo = (_, selector) => !VersionChecker.HasVersionPrefix(selector.AttributeRouteModel!.Template);
    });
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();