using Hangfire;
using Serilog;
using Infrastructure.Extensions;
using Application.Extensions;
using Api.Extensions;
using Application.Constants;
using Microsoft.OpenApi;
using Infrastructure.Seeders;

var builder = WebApplication.CreateBuilder(args);

builder.AddApplicationServices();
builder.AddPresentationServices();
builder.AddInfrastructureServices(builder.Configuration);




var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var planSeeder = scope.ServiceProvider.GetRequiredService<ISeeder>();
    await planSeeder.SeedAsync();
}

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi();
   app.UseHangfireDashboard();
}
app.UseSerilogRequestLogging(); 

app.UseHttpsRedirection();
app.UseCors();
app.UseHealthChecks("/health");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
