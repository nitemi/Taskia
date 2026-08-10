using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Taskia.Domain.Exceptions;

namespace Taskia.Api.Middleware;

public class GlobalExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;

    public GlobalExceptionHandlingMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/problem+json";

        var problemDetails = exception switch
        {
            EntityNotFoundException notFoundEx => new ProblemDetails
            {
                Title = "Resource Not Found",
                Status = (int)HttpStatusCode.NotFound,
                Detail = notFoundEx.Message,
                Type = "https://taskia.api/errors/entity-not-found",
                Extensions = { ["code"] = notFoundEx.Code }
            },
            DomainException domainEx => new ProblemDetails
            {
                Title = "Domain Rule Violation",
                Status = (int)HttpStatusCode.BadRequest,
                Detail = domainEx.Message,
                Type = "https://taskia.api/errors/domain-error",
                Extensions = { ["code"] = domainEx.Code }
            },
            _ => new ProblemDetails
            {
                Title = "Internal Server Error",
                Status = (int)HttpStatusCode.InternalServerError,
                Detail = "An unexpected error occurred. Please try again later.",
                Type = "https://taskia.api/errors/internal-server-error"
            }
        };

        context.Response.StatusCode = problemDetails.Status ?? (int)HttpStatusCode.InternalServerError;

        var json = JsonSerializer.Serialize(problemDetails, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        return context.Response.WriteAsync(json);
    }
}
