using GniApi.Helper;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.



builder.Services.AddControllers(x =>
{
    x.Filters.Add(new HeaderCheckActionFilter(builder.Configuration));
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

    var apiKeyScheme = new OpenApiSecurityScheme
    {
        Description = "API Key authentication",
        Name = "ApiKey",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey
    };

    var securityKeyScheme = new OpenApiSecurityScheme
    {
        Description = "Secret Key authentication",
        Name = "SecretKey",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey
    };
    c.AddSecurityDefinition("ApiKey", apiKeyScheme);
    c.AddSecurityDefinition("SecurityKey", securityKeyScheme);

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "ApiKey"
                }
            },

            Array.Empty<string>()
        }
    });
    //c.AddSecurityRequirement(new OpenApiSecurityRequirement
    //{
    //    {
    //        new OpenApiSecurityScheme
    //        {
    //            Reference = new OpenApiReference
    //            {
    //                Type = ReferenceType.Header,
    //                Id = "SecretKey"
    //            }
    //        },

    //        Array.Empty<string>()
    //    }
    //});
});

builder.Services.AddScoped<IOracleQueries, OracleQueries>();
builder.Services.AddScoped<HeaderCheckActionFilter>();

builder.Services.AddHttpClient("erpClient", c =>

{
    c.BaseAddress = new Uri("http://172.16.30.26:8083/api/v1");

    c.DefaultRequestHeaders.Add("x-api-key", "city_finance");
}
);


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


app.UseAuthorization();

app.MapControllers();

app.Run();
