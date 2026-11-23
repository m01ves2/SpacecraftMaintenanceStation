using SpacecraftMaintenanceStation.Application.Abstractions.Messaging;
using SpacecraftMaintenanceStation.Application.Abstractions.Persistence;
using SpacecraftMaintenanceStation.Application.Common.Models;
using SpacecraftMaintenanceStation.Domain.Entities;

namespace SpacecraftMaintenanceStation.Application.MaintenanceRequests.Add
{
    public class AddCommandHandler : ICommandHandler<AddCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork;
        public AddCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

        }
        public async Task<Result> Handle(AddCommand command, CancellationToken cancellationToken)
        {

            if(command.StartTime >= command.EndTime)
                return Result.Failure(new Error(ErrorCode.Validation, $"Incorrect time interval. EndTime({command.EndTime}) must be greater then  StartTime({command.StartTime})"));

            var nowTime = DateTimeOffset.Now;
            if (command.StartTime < nowTime || command.EndTime < nowTime)
                return Result.Failure(new Error(ErrorCode.Validation, $"Incorrect time interval. StartTime and EndTime must be greater then {nowTime}"));

            if(command.StartTime >= nowTime.AddDays(7) || command.EndTime >= nowTime.AddDays(7))
                return Result.Failure(new Error(ErrorCode.Validation, $"Incorrect time interval. StartTime  and EndTime must be less then {nowTime.AddDays(7)}"));

            var existMaintenanceRequestList = await _unitOfWork.MaintenanceRequests.GetByBayIdAsync(command.BayId, command.StartTime, command.EndTime, cancellationToken);
            //если в базе пока не появляются заявки со статусом Cancelled, достаточно такой проверки. в будущем нужно учитывать такие отмененные заявки!
            if (existMaintenanceRequestList.Any()) {
                return Result.Failure(new Error(ErrorCode.Conflict, $"Can not add new Maintenance Request. There are existing Maintenance Requests for time interval [{command.StartTime}, {command.EndTime})"));
            }

            var bay = await _unitOfWork.Bays.GetById(command.BayId, cancellationToken);

            if (bay == null) {
                return Result.Failure(new Error(ErrorCode.NotFound, $"Not found bay with bayId = {command.BayId}"));
            }

            var maintenanceRequest = new MaintenanceRequest(
                bay,
                command.SpaceCraftRegistrationNumber,
                command.CustomerName,
                command.ContactPhone,
                command.StartTime,
                command.EndTime,
                command.ProblemDescription);

            await _unitOfWork.MaintenanceRequests.AddAsync(maintenanceRequest, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
