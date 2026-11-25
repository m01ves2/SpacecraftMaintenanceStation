using Microsoft.AspNetCore.Mvc;
using SpacecraftMaintenanceStation.API.Mappers;
using SpacecraftMaintenanceStation.Application.Abstractions.Messaging;
using SpacecraftMaintenanceStation.Application.Bays.DTO;
using SpacecraftMaintenanceStation.Application.Bays.GetAll;
using SpacecraftMaintenanceStation.Application.Common.Models;

namespace SpacecraftMaintenanceStation.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaysController : ControllerBase
    {
        private readonly IQueryHandler<GetAllQuery, Result<IReadOnlyList<BayDTO>>> _getAllQueryHandler;

        public BaysController(IQueryHandler<GetAllQuery, Result<IReadOnlyList<BayDTO>>> getAllQueryHandler)
        {
            _getAllQueryHandler = getAllQueryHandler;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var result = await _getAllQueryHandler.Handle(new GetAllQuery(), cancellationToken);

            if (result.IsFailure) {
                return this.ToActionResult(result.Error!);
            }

            return Ok(result.Data);
        }

    }
}
