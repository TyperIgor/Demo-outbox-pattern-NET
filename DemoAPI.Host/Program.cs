using DemoProject.Domain.Interfaces;
using DemoProject.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDependencies(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/send-email", async (IEmailService _sender, string email, string subject, string body) =>
{ 
    await _sender.SendEmailAsync(email, subject);
    // Logic to send email
    return Results.Ok("Email sent successfully!");
 })
.WithName("Send-Email-notify");

app.UseAuthorization();

app.MapControllers();

app.Run();
