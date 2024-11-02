using EcomDataAccess;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authorization;
using EcommerceAppAPI.StartupConfig;

var builder = WebApplication.CreateBuilder(args);


builder.AddStandardServices();
builder.AddAuthServices();
builder.AddCustomServices();
builder.AddHealthCheckServices();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks(pattern: "/health").AllowAnonymous();


app.Run();
