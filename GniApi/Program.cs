using GniApi.CustomExtensions;
using GniApi.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.



builder.Services.AddControllers(x =>
{
    x.Filters.Add<ValidateModelAttribute>();
});

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});



// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc($"v1", new OpenApiInfo
    {
        Title = "GNI APIs",
        Version = "v1",
        Description = "GNI Development APIs",
    });
    c.SchemaFilter<DefaultValueSchemaFilter>();

    //var apiKeyScheme = new OpenApiSecurityScheme
    //{
    //    Description = "API Key authentication",
    //    Name = "ApiKey",
    //    In = ParameterLocation.Header,
    //    Type = SecuritySchemeType.ApiKey
    //};

    //c.AddSecurityDefinition("ApiKey", apiKeyScheme);

    //c.AddSecurityRequirement(new OpenApiSecurityRequirement
    //{
    //    {
    //        new OpenApiSecurityScheme
    //        {
    //            Reference = new OpenApiReference
    //            {
    //                Type = ReferenceType.SecurityScheme,
    //                Id = "ApiKey"
    //            }
    //        },
    //        Array.Empty<string>()
    //    }
    //});
});


builder.Services.AddScoped<IOracleQueries, OracleQueries>();
builder.Services.AddScoped<HeaderCheckActionFilter>();
builder.Services.AddHttpClient();


var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}


app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.CustomExceptionHadler();


app.UseAuthorization();

app.MapControllers();

app.Run("http://0.0.0.0:7278");
