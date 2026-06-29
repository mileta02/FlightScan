using FlightScan.Application.Cqrs.Commands.User;
using FlightScan.Application.Cqrs.Queries.User;
using FlightScan.Core.Helpers;
using FlightScan.Core.Responses.Common;
using FlightScan.Core.Responses.User;
using FlightScan.Core.Specifications;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlightScan.Api.Controllers
{
    [Route("/api/users")]
    [ApiController]
    [Authorize(Roles = "Administrator")]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;
        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Pagination<UserResponse>>> GetUsersAsync([FromQuery] UserSpecParams specParams)
        {
            var result = await _mediator.Send(new GetUsersQuery{ SpecParams = specParams });
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CreateResponse>> CreateUserAsync([FromBody] CreateUserCommand request)
        {
            var result = await _mediator.Send(request);
            return StatusCode(StatusCodes.Status201Created, result);
        }
    }
}
