using System;
using System.ComponentModel.DataAnnotations;
using EBlumbit.Dto;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace EBlumbit.exceptions;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{

    private readonly ILogger<GlobalExceptionHandler> _logger = logger;

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var (status, code) = exception switch
        {
          DomainException d => (d.StatusCode, d.ErrorCode),  
          KeyNotFoundException => (400, "Recurso no encontrado"),
          ValidationException => (400, "Error de validacion"),
          InvalidOperationException => (400, "Error de consulta"),
          ArgumentException => (400, "Error de request data"),
          _=> (500, "Error interno de servidor")  
        };

        var problem = new ProblemDetails
        {
            Status = status,
            Title = code,
            Detail = exception.Message,
            Instance = httpContext.Request.Path
        };
        problem.Extensions["traceId"] = httpContext.TraceIdentifier;
        _logger.LogError("Logging Exception {exception}", exception);
        httpContext.Response.StatusCode = status;
        await httpContext.Response.WriteAsJsonAsync(problem);
        return true;
    }
}
