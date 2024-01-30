using System.Net;
using FluentValidation;
using PrescriberPoint.Journal.Application.Common.Exceptions;

namespace PrescriberPoint.Journal.WebApi.Middlewares;

public class CustomExceptionHandlerMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try 
        {
            await next(context);
        }
        catch(ValidationException e) 
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            await context.Response.WriteAsJsonAsync(
                e.Errors
            );
        } 
        catch(BadRequestException e) 
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            await context.Response.WriteAsJsonAsync(
                new {Message = e.Message}
            );
        } 
        catch(Exception e) 
        {
            throw;
        }
    }
}