using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System.Reflection;
using WorkWiseApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.ConfigureCorsPolicy();

builder.Services.ConfigureLoggerService();

builder.Services.ConfigureDBContext(builder.Configuration);

builder.Services.ConfigureRepositoryWrapper();

builder.Services.ConfigureAutoMapper();

builder.Services.ConfigureServiceWrapper(builder.Configuration);

builder.Services.AddControllers();

builder.Services.AddMvc(options =>
    {
        options.InputFormatters.RemoveType<Microsoft.AspNetCore.Mvc.Formatters.SystemTextJsonInputFormatter>();
        options.OutputFormatters.RemoveType<Microsoft.AspNetCore.Mvc.Formatters.SystemTextJsonOutputFormatter>();
    }).AddNewtonsoftJson(opts =>
    {
        opts.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        opts.SerializerSettings.Converters.Add(new StringEnumConverter(new CamelCaseNamingStrategy()));
    })
    .AddXmlSerializerFormatters();

builder.Services.ConfigureAuthentication(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Version = "v1",
            Title = "Workwise Application",
            Description = "Workwise is a comprehensive solution for elevated productivity that integrates intelligent task creation and goal setting using GenAI.",
            Contact = new OpenApiContact()
            {
                Name = "Pavithra S",
                Email = "pavithraselvaraj2510@gmail.com"
            }
        });
        var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
    }
);

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseCors("AllowAll");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
        options.SwaggerEndpoint("./v1/swagger.json", "WorkWise Api v1")
    );
}

app.ConfigureDbMigration();

app.UseHttpStatusCodeExceptionMiddleware();

// app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllers();
    });

app.Run();
