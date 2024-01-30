using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using PrescriberPoint.Journal.Application.Journal.Commands;
using PrescriberPoint.Journal.Application.Journal.Queries;
using PrescriberPoint.Journal.Application.Security.Commands;
using PrescriberPoint.Journal.Application.Security.Validators;
using PrescriberPoint.Journal.Application.Users.Queries;

namespace PrescriberPoint.Journal.Application;

public static class BuilderExtensions
{
    public static void AddServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IValidator<Domain.Journal>, JournalValidator>();
        builder.Services.AddScoped<IValidator<SignUpUserParameters>, SignUpParametersValidator>();
       
        builder.Services.AddTransient<IGetDataByUserIdQuery, GetDataByUserIdQueryHandler>();
        builder.Services.AddTransient<IGetDataByDataIdQuery, GetDataByDataIdQueryHandler>();
        
        builder.Services.AddTransient<ICreateDataCommand, CreateDataCommandHandler>();
        builder.Services.AddTransient<IUpdateDataCommand, UpdateDataCommandHandler>();
        builder.Services.AddTransient<IDeleteDataCommand, DeleteDataCommandHandler>();
        builder.Services.AddTransient<ISignInUserCommand, SignInUserCommandHandler>();
        builder.Services.AddTransient<ISignUpUserCommand, SignUpUserCommandHandler>();

        builder.Services.AddTransient<IPasswordHashService, PasswordHashService>();
        builder.Services.AddTransient<ITokenService, TokenService>();   
    }
}
