using FlightScan.Application.Cqrs.Commands.Flight;
using FlightScan.Application.Cqrs.Queries.Flight;
using FlightScan.Core.Helpers;
using FlightScan.Core.Responses.Common;
using FlightScan.Core.Responses.Flights;
using FlightScan.Core.Specifications;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlightScan.Api.Controllers
{
    [Route("/api/flights")]
    [ApiController]
    [Authorize]
    public class FlightController : ControllerBase
    {
        private readonly IMediator _mediator;

        public FlightController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize(Roles = "Agent")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CreateResponse>> CreateFlightAsync([FromBody] CreateFlightCommand command)
        {
            var result = await _mediator.Send(command);
            return StatusCode(StatusCodes.Status201Created, result);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Pagination<FlightResponse>>> GetFlightsAsync([FromQuery] FlightSpecParams specParams)
        {
            var result = await _mediator.Send(new GetFlightsQuery { Params = specParams });
            return Ok(result);
        }

        [HttpPut("{id}/cancel")]
        [Authorize(Roles = "Administrator")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CancelFlightAsync(int id)
        {
            await _mediator.Send(new CancelFlightCommand { Id = id });
            return NoContent();
        }
    }
}
