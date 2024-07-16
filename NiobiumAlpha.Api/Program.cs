using System.Reflection;
using FluentValidation;
using NiobiumAlpha.Api;
using NiobiumAlpha.Api.Endpoints;
using NiobiumAlpha.Api.Middlewares;
using NiobiumAlpha.Api.Services.CalculationService;
using NiobiumAlpha.Api.Swagger;

var builder = WebApplication.CreateBuilder(args);

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureSwagger();

// Services
builder.Services.AddSingleton<NiobiumCalculationService>();
builder.Services.AddSingleton<WolframCalculationService>();

builder.Services.AddSingleton<Func<string, ICalculationService>>(serviceProvider => key =>
    key switch
    {
        ApiKeyedServices.Calculation.Niobium => serviceProvider.GetService<NiobiumCalculationService>()!,
        ApiKeyedServices.Calculation.Wolfram => serviceProvider.GetService<WolframCalculationService>()!,
        _ => throw new NotImplementedException()
    });

builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly(), ServiceLifetime.Singleton);

// HttpClients
builder.Services.AddHttpClient("WolframAlpha", client =>
{
    client.BaseAddress = new Uri("https://api.wolframalpha.com");
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost", policyBuilder =>
        policyBuilder.WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors("AllowLocalhost");
}

app.UseHttpsRedirection();
app.UseCors("AllowLocalhost"); // Change when production available

app.UseMiddleware<ValidationMappingMiddleware>();

// Api endpoints mapping
app.MapApiEndpoints();

app.Run();
