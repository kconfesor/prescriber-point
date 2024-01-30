using System.Reflection;
using System.Text;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PrescriberPoint.Journal.Application;
using PrescriberPoint.Journal.Persistence.Pgsql;
using PrescriberPoint.Journal.WebApi.Journal;
using PrescriberPoint.Journal.WebApi.Middlewares;
using PrescriberPoint.Journal.WebApi.Security;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option => {

    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });

    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

var jwtTokenSettings = builder.Configuration.GetSection("JwtTokenSettings");
builder.Services
    .AddOptions<JwtTokenSettings>()
    .Bind(jwtTokenSettings);

builder.AddServices();
// builder.AddMemoryPersistence();
builder.Services.AddPostgresPersistence(builder.Configuration);
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddScoped<CustomExceptionHandlerMiddleware>();

var validIssuer = jwtTokenSettings.GetValue<string>("ValidIssuer");
var validAudience = jwtTokenSettings.GetValue<string>("ValidAudience");
var symmetricSecurityKey = jwtTokenSettings.GetValue<string>("SymmetricSecurityKey");

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options=>{
     options.IncludeErrorDetails = true;
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ClockSkew = TimeSpan.Zero,
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = validIssuer,
            ValidAudience = validAudience,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(symmetricSecurityKey)
            ),
        };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
}

app.UseSwagger();
app.UseSwaggerUI();

var option = new RewriteOptions();
option.AddRedirect("^$", "swagger");
app.UseRewriter(option);

app.UseMiddleware<CustomExceptionHandlerMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();
app.MapDataRoutes();
app.MapSecurityRoutes();

app.Run();

public partial class Program
{ }