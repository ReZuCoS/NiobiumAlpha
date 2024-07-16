using System.Reflection;
using Microsoft.OpenApi.Models;

namespace NiobiumAlpha.Api.Swagger;

public static class SwaggerExtensions
{
	public static void ConfigureSwagger(this IServiceCollection services)
	{
		services.AddSwaggerGen(options => {
			options.SwaggerDoc("v1", new OpenApiInfo
			{
				Version = "v1",
				Title = "NiobiumAlpha Api"
			});

			var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
			var filePath = Path.Combine(AppContext.BaseDirectory, xmlFilename);

			if (File.Exists(filePath)) {
				options.IncludeXmlComments(filePath);
			}
		});
	}
}