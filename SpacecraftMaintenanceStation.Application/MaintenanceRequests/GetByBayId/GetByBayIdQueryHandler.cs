using SpacecraftMaintenanceStation.Application.Abstractions.Messaging;
using SpacecraftMaintenanceStation.Application.Abstractions.Persistence;
using SpacecraftMaintenanceStation.Application.Common.Models;
using SpacecraftMaintenanceStation.Application.MaintenanceRequests.DTO;

namespace SpacecraftMaintenanceStation.Application.MaintenanceRequests.GetByBayId
{
    public class GetByBayIdQueryHandler : IQueryHandler<GetByBayIdQuery, Result<IReadOnlyList<MaintenanceRequestDTO>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetByBayIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<IReadOnlyList<MaintenanceRequestDTO>>> Handle(GetByBayIdQuery query, CancellationToken cancellationToken)
        {
            if(query.StartTime >= query.EndTime)
                return Result<IReadOnlyList<MaintenanceRequestDTO>>.Failure(
                    new Error(ErrorCode.Validation, $"Incorrect time interval. StartTime({query.StartTime}) must be less then EndTime ({query.EndTime})"));

            var maintenanceRequests = await _unitOfWork.MaintenanceRequests.GetByBayIdAsync(query.BayId, query.StartTime, query.EndTime, cancellationToken);

            var resultData = maintenanceRequests.Select(x => 
                new MaintenanceRequestDTO(x.BayId,
                                          x.SpaceCraftRegistrationNumber,
                                          x.CustomerName,
                                          x.ContactPhone,
                                          x.StartTime,
                                          x.EndTime,
                                          x.ProblemDescription,
                                          x.WorkDescription)
                ).ToList();

            return Result<IReadOnlyList<MaintenanceRequestDTO>>.Success(resultData);

        }
    }
}
