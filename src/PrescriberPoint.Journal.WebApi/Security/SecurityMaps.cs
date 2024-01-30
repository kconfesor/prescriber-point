using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using PrescriberPoint.Journal.Application.Security.Commands;

namespace PrescriberPoint.Journal.WebApi.Security;

public static class SecurityMaps {
    public static void MapSecurityRoutes(this WebApplication app)
    {
        app.MapPost("/security/signIn", async (
                [FromBody] CredentialRequest credentialRequest,
                [FromServices] ISignInUserCommand signInUserCommand) =>
            {
                var token = await signInUserCommand.Handle(new Credential(
                    credentialRequest.Username,
                    credentialRequest.Password
                ));

                if (token == null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(new TokenResponse(token.Token));
            })
            .WithOpenApi(op => new OpenApiOperation(op)
            {
                Summary = "Sign in user and returns a new token",
            });

        app.MapPost("/security/signUp", async (
            [FromBody] SignUpRequest signUpRequest,
            [FromServices] ISignUpUserCommand signUpUserCommand) => 
        {
            var user = await signUpUserCommand.Handle(new SignUpUserParameters(
                signUpRequest.Username,
                signUpRequest.Name,
                signUpRequest.Password
            ));

            if (user == null){
                return Results.NotFound();
            }
            return Results.Ok(new SignUpResponse(user.UserId, user.Username, user.Name)); 
        })
        .WithOpenApi(op => new OpenApiOperation(op)
        {
            Summary = "Sign up user and returns the new user",
        });
    }
}