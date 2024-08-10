using External.Api.Controllers;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Ogu.AspNetCore.Conventions;
using Sample.Api.Controllers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddMvcOptions(opts =>
{
    // To hide all assembly's controllers - still can be accessible through the HTTP requests but won't be visible
    opts.Conventions.AddControllerHideFromExploringConvention(typeof(SecretController).Assembly);

    /* To disable controller - won't be visible and accessible through the HTTP requests */
    // opts.Conventions.AddControllerDisableConvention(typeof(VerySecretController));

    opts.Conventions.AddControllerRoutePrefixConvention(typeof(NoRouteController), "api/no-route");
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