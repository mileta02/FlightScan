using FlightScan.Application.Cqrs.Commands.Auth;
using FlightScan.Core.Responses.Auth;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FlightScan.Api.Controllers
{
    [Route("/api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<LoginResponse>> LoginAsync(LoginCommand command) {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
