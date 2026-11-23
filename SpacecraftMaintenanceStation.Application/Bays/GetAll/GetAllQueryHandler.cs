using SpacecraftMaintenanceStation.Application.Abstractions.Messaging;
using SpacecraftMaintenanceStation.Application.Abstractions.Persistence;
using SpacecraftMaintenanceStation.Application.Bays.DTO;
using SpacecraftMaintenanceStation.Application.Common.Models;

namespace SpacecraftMaintenanceStation.Application.Bays.GetAll
{
    public sealed class GetAllQueryHandler : IQueryHandler<GetAllQuery, Result<IReadOnlyList<BayDTO>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<IReadOnlyList<BayDTO>>> Handle(GetAllQuery query, CancellationToken cancellationToken) //GetCategoriesQuery query -"намерение выполнить этот сценарий"
        {
            var bays = await _unitOfWork.Bays.GetAllAsync(cancellationToken);

            var resultData = bays.Select(x => new BayDTO(x.Id, x.Name)).ToList();

            return Result<IReadOnlyList<BayDTO>>.Success(resultData);
        }
    }
}
