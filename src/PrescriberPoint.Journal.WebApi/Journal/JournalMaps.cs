using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using PrescriberPoint.Journal.Application.Journal.Commands;
using PrescriberPoint.Journal.Application.Journal.Queries;
using PrescriberPoint.Journal.WebApi.Journal.Models;
using PrescriberPoint.Journal.WebApi.Extensions;

namespace PrescriberPoint.Journal.WebApi.Journal;

public static class JournalMaps {
    public static void MapDataRoutes(this WebApplication app)
    {
        app.MapGet("/journal", async (
                ClaimsPrincipal user,
                [FromServices] IGetDataByUserIdQuery getDataByUserIdQuery) =>
            {
                var userId = user.GetUserId();
                var journals = await getDataByUserIdQuery.Handle(userId);
                var responses = journals.Select(d =>
                    new JournalResponse(d.Id, d.UserId, d.Patient, d.Note, d.CreatedAt, d.ModifiedAt));

                return Results.Ok(responses);
            })
            .Produces<List<JournalResponse>>()
            .WithOpenApi(op => new OpenApiOperation(op)
            {
                Summary = "Returns all journal entries for the current user",
                Description = "Requires authentication",
            })
            .RequireAuthorization();

        app.MapGet("/journal/{id}", async (
                [FromRoute]int id,
                ClaimsPrincipal user,
                [FromServices] IGetDataByDataIdQuery getDataByDataIdQuery) =>
            {
                var userId = user.GetUserId();

                var data = await getDataByDataIdQuery.Handle(id);
                if (data == null)
                {
                    return Results.NotFound();
                }

                if (data.UserId != userId)
                {
                    return Results.Forbid();
                }

                return Results.Ok(new JournalResponse(data.Id, data.UserId, data.Patient, data.Note, data.CreatedAt,
                    data.ModifiedAt));
            })
            .Produces<JournalResponse>()
            .WithOpenApi(op => new OpenApiOperation(op)
            {
                Summary = "Returns journal entry by id for the current user",
                Description = "Requires authentication",
            })
            .RequireAuthorization();

        app.MapDelete("/journal/{id}", async (
                int id,
                ClaimsPrincipal user,
                [FromServices] IDeleteDataCommand deleteDataCommand,
                [FromServices] IGetDataByDataIdQuery getDataByDataIdQuery) =>
            {
                var userId = user.GetUserId();

                var dataInfo = await getDataByDataIdQuery.Handle(id);

                if (dataInfo == null)
                {
                    return Results.NotFound();
                }

                if (dataInfo.UserId != userId)
                {
                    return Results.Forbid();
                }

                await deleteDataCommand.Handle(new DeleteJournalCommandParameters(id));

                return Results.Ok();
            })
            .WithOpenApi(op => new OpenApiOperation(op)
            {
                Summary = "Deletes journal entry by id for the current user",
                Description = "Requires authentication",
            })
            .RequireAuthorization();

        app.MapPost("/journal", async (
                ClaimsPrincipal user,
                [FromBody] CreateOrUpdateJournalRequest upsertDataRequest,
                [FromServices] ICreateDataCommand createDataCommand) =>
            {
                var userId = user.GetUserId();

                var data = await createDataCommand.Handle(new CreateDataCommandParameters(
                    userId,
                    upsertDataRequest.Patient,
                    upsertDataRequest.Note
                ));

                if (data == null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(data);
            })
            .Produces<JournalResponse>()
            .WithOpenApi(op => new OpenApiOperation(op)
            {
                Summary = "Creates a new journal entry for the current user",
                Description = "Requires authentication",
            })
            .RequireAuthorization();

        app.MapPut("/journal/{id}", async (
                int id,
                ClaimsPrincipal user,
                [FromBody] CreateOrUpdateJournalRequest upsertDataRequest,
                [FromServices] IUpdateDataCommand updateDataCommand,
                [FromServices] IGetDataByDataIdQuery getDataByDataIdQuery
            ) =>
            {
                var userId = user.GetUserId();

                var dataInfo = await getDataByDataIdQuery.Handle(id);

                if (dataInfo == null)
                {
                    return Results.NotFound();
                }

                if (dataInfo.UserId != userId)
                {
                    return Results.Forbid();
                }

                var data = await updateDataCommand.Handle(new UpdateDataCommandParameters(
                    id,
                    upsertDataRequest.Patient,
                    upsertDataRequest.Note
                ));

                if (data == null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(new JournalResponse(data.JournalId, data.UserId, data.Patient, data.Note,
                    data.CreatedAt,
                    data.ModifiedAt));
            })
            .WithOpenApi(op => new OpenApiOperation(op)
            {
                Summary = "Updates journal entry by id for the current user",
                Description = "Requires authentication",
            })
            .RequireAuthorization();
    }
}