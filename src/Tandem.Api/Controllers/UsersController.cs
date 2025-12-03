using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Tandem.Application.Commands;
using Tandem.Application.DTOs;
using Tandem.Application.Queries;

namespace Tandem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    [HttpPost]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<UserResponse>> CreateUser([FromBody] CreateUserRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var command = new CreateUserCommand(request);
            var result = await _mediator.Send(command, cancellationToken);
            return CreatedAtAction(nameof(GetUserByEmail), new { emailAddress = result.EmailAddress }, result);
        }
        catch (ValidationException ex)
        {
            var errors = ex.Errors.Select(e => new { e.PropertyName, e.ErrorMessage });
            return BadRequest(new { errors });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { error = ex.Message });
        }
        catch (Exception)
        {
            return BadRequest(new { error = "Invalid request data." });
        }
    }

    [HttpGet]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UserResponse>> GetUserByEmail([FromQuery] string emailAddress, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(emailAddress))
        {
            return BadRequest(new { error = "EmailAddress query parameter is required." });
        }

        var query = new GetUserByEmailQuery(emailAddress);
        var result = await _mediator.Send(query, cancellationToken);

        if (result == null)
        {
            return NotFound(new { error = $"User with email address '{emailAddress}' not found." });
        }

        return Ok(result);
    }

    [HttpPut("{emailAddress}")]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UserResponse>> UpdateUser(string emailAddress, [FromBody] UpdateUserRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var command = new UpdateUserCommand(emailAddress, request);
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(result);
        }
        catch (ValidationException ex)
        {
            var errors = ex.Errors.Select(e => new { e.PropertyName, e.ErrorMessage });
            return BadRequest(new { errors });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (Exception)
        {
            return BadRequest(new { error = "Invalid request data." });
        }
    }
}
