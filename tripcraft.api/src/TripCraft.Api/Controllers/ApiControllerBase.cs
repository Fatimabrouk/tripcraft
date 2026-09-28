using System.Security.Claims;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TripCraft.Api.Controllers;

[ApiController]
[Authorize]   // every controller inheriting this requires a valid JWT
public abstract class ApiControllerBase : ControllerBase
{
    /// <summary>The authenticated user's ID, read from the JWT "sub" claim.</summary>
    protected string UserId =>
        User.FindFirstValue(ClaimTypes.NameIdentifier)   // where ASP.NET maps "sub" by default
        ?? User.FindFirstValue("sub")                     // fallback if claim mapping is disabled
        ?? throw new InvalidOperationException("Authenticated request has no user id claim.");

    /// <summary>
    /// Runs a FluentValidation validator. Returns null when valid,
    /// or a standard 400 ValidationProblem result when not.
    /// </summary>
    protected async Task<ActionResult?> ValidateAsync<T>(
        IValidator<T> validator, T request, CancellationToken ct)
    {
        var result = await validator.ValidateAsync(request, ct);
        if (result.IsValid) return null;

        var errors = result.Errors
            .GroupBy(e => e.PropertyName)
            .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());

        return ValidationProblem(new ValidationProblemDetails(errors));
    }
}