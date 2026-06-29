using FlightScan.Application.Cqrs.Commands.Reservation;
using FlightScan.Application.Cqrs.Queries.Reservation;
using FlightScan.Core.Helpers;
using FlightScan.Core.Responses.Common;
using FlightScan.Core.Responses.Reservations;
using FlightScan.Core.Specifications;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlightScan.Api.Controllers
{
    [Route("/api/reservations")]
    [ApiController]
    [Authorize]
    public class ReservationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ReservationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize(Roles = "Visitor")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CreateResponse>> CreateReservationAsync(CreateReservationCommand command)
        {
            command.UserId = int.Parse(User.FindFirst("UserId")!.Value);
            var result = await _mediator.Send(command);
            return StatusCode(StatusCodes.Status201Created, result);
        }

        [HttpGet("my")]
        [Authorize(Roles = "Visitor")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Pagination<ReservationResponse>>> GetMyReservationsAsync([FromQuery] ReservationSpecParams specParams)
        {
            var userId = int.Parse(User.FindFirst("UserId")!.Value);
            var result = await _mediator.Send(new GetMyReservationsQuery { UserId = userId, Params = specParams });
            return Ok(result);
        }

        [HttpGet("pending")]
        [Authorize(Roles = "Agent")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Pagination<PendingReservationResponse>>> GetPendingReservationsAsync([FromQuery] ReservationSpecParams specParams)
        {
            var result = await _mediator.Send(new GetPendingReservationsQuery { Params = specParams });
            return Ok(result);
        }

        [HttpPut("{id}/approve")]
        [Authorize(Roles = "Agent")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ApproveReservationAsync(int id)
        {
            await _mediator.Send(new ApproveReservationCommand { Id = id });
            return NoContent();
        }
    }
}
