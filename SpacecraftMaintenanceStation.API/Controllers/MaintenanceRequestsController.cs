using Microsoft.AspNetCore.Mvc;
using SpacecraftMaintenanceStation.API.Mappers;
using SpacecraftMaintenanceStation.Application.Abstractions.Messaging;
using SpacecraftMaintenanceStation.Application.Common.Models;
using SpacecraftMaintenanceStation.Application.MaintenanceRequests.Add;
using SpacecraftMaintenanceStation.Application.MaintenanceRequests.DTO;
using SpacecraftMaintenanceStation.Application.MaintenanceRequests.GetByBayId;

namespace SpacecraftMaintenanceStation.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaintenanceRequestsController : ControllerBase
    {
        private readonly IQueryHandler<GetByBayIdQuery, Result<IReadOnlyList<MaintenanceRequestDTO>>> _getByBayIdQueryHandler;
        private readonly ICommandHandler<AddCommand, Result> _addCommandHandler;

        public MaintenanceRequestsController(IQueryHandler<GetByBayIdQuery, Result<IReadOnlyList<MaintenanceRequestDTO>>> getByBayIdQueryHandler,
                                             ICommandHandler<AddCommand, Result> addCommandHandler)
        {
            _getByBayIdQueryHandler = getByBayIdQueryHandler;
            _addCommandHandler = addCommandHandler;
        }

        [HttpGet]
        public async Task<IActionResult> GetByBayId([FromQuery] int bayId, DateTimeOffset startTime, DateTimeOffset endTime, CancellationToken cancellationToken)
        {
            var result = await _getByBayIdQueryHandler.Handle(new GetByBayIdQuery(bayId, startTime, endTime), cancellationToken );

            if (result.IsFailure) {
                return this.ToActionResult(result.Error!);
            }

            return Ok(result.Data);
        }


        [HttpPost]
        public async Task<IActionResult> Add(AddCommand request, CancellationToken cancellationToken)
        {
            var result = await _addCommandHandler.Handle(request, cancellationToken);

            if (result.IsFailure) {
                return this.ToActionResult(result.Error!);
            }

            return NoContent();
        }

    }
}
