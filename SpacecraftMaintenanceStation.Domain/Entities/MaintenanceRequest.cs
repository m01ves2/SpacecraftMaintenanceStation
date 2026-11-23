using SpacecraftMaintenanceStation.Domain.Exceptions;

namespace SpacecraftMaintenanceStation.Domain.Entities
{
    public enum MaintenanceStatus
    {
        Pending,
        InProgress,
        Finished,
        Cancelled
    }
    public class MaintenanceRequest
    {
        public const int MaxCustomerNameLength = 150;
        public const int MaxSpaceCraftRegistrationNumberLength = 50;
        public const int MaxProblemDescriptionLength = 1000;
        public const int MaxWorkDescriptionLength = 1000;
        public const int MaxContactPhoneLength = 20;

        public int Id {  get; private set; }
        public string? SpaceCraftRegistrationNumber { get; private set; }
        public string CustomerName { get; private set; } = string.Empty;
        public string ContactPhone {  get; private set; } = string.Empty;

        public DateTimeOffset StartTime { get; private set; }
        public DateTimeOffset EndTime { get; private set; }

        public string ProblemDescription {  get; private set; } = string.Empty;
        public string WorkDescription {  get; private set; } = string.Empty;

        public MaintenanceStatus Status { get; private set; } = MaintenanceStatus.Pending;
        private static readonly Dictionary<MaintenanceStatus, MaintenanceStatus []> AllowedTransitions = new()
        {
            [MaintenanceStatus.Pending] = new[] { MaintenanceStatus.InProgress, MaintenanceStatus.Cancelled },
            [MaintenanceStatus.InProgress] = new[] { MaintenanceStatus.Finished, MaintenanceStatus.Cancelled },
            [MaintenanceStatus.Finished] = Array.Empty<MaintenanceStatus>(),
            [MaintenanceStatus.Cancelled] = Array.Empty<MaintenanceStatus>()
        }; //это пример domain knowledge, которое существует только в коде!
        public bool CanBeCancelled => CanChangeStatusTo(MaintenanceStatus.Cancelled);

        public int BayId { get; private set; }
        public Bay Bay { get; private set; }

        private MaintenanceRequest() 
        { 
        }

        public MaintenanceRequest(Bay bay,
            string? spaceCraftRegistrationNumber, 
            string customerName, 
            string contactPhone, 
            DateTimeOffset startTime, 
            DateTimeOffset endTime, 
            string problemDescription)
        {
            ChangeBay(bay);

            ChangeSpaceCraftRegistrationNumber(spaceCraftRegistrationNumber);
            
            ChangeCustomerName(customerName);

            ChangeContactPhone(contactPhone);

            ChangeTime(startTime, endTime);

            ChangeProblemDescription(problemDescription);
        }

        public void ChangeBay(Bay bay)
        {
            if (bay == null)
                throw new DomainValidationException("Bay cannot be null");

            if(Status != MaintenanceStatus.Pending)
                throw new DomainValidationException("Bay cannot be changed when status is not `pending`");

            Bay = bay;
            BayId = bay.Id;
        }

        public void ChangeSpaceCraftRegistrationNumber(string? spaceCraftRegistrationNumber)
        {
            if (!string.IsNullOrWhiteSpace(spaceCraftRegistrationNumber)) {
                spaceCraftRegistrationNumber = spaceCraftRegistrationNumber.Trim();

                if(spaceCraftRegistrationNumber.Length > MaxSpaceCraftRegistrationNumberLength) {
                    throw new DomainValidationException($"Registration number cannot be longer than {MaxSpaceCraftRegistrationNumberLength} chars");
                }

                SpaceCraftRegistrationNumber = spaceCraftRegistrationNumber;
            }
            else {
                SpaceCraftRegistrationNumber = null;
            }
        }

        public void ChangeCustomerName(string customerName)
        {
            if ( string.IsNullOrWhiteSpace(customerName)) {
                throw new DomainValidationException("Name cannot be empty");
            }

            customerName = customerName.Trim();
            if (customerName.Length > MaxCustomerNameLength)
                throw new DomainValidationException($"Name cannot be longer than {MaxCustomerNameLength} chars");

            CustomerName = customerName;
        }

        public void ChangeContactPhone(string contactPhone)
        {
            if (string.IsNullOrWhiteSpace(contactPhone))
                throw new DomainValidationException("Contact phone cannot be empty");

            contactPhone = contactPhone.Trim();

            if(contactPhone.Length > MaxContactPhoneLength)
                throw new DomainValidationException($"Phone number cannot be longer than {MaxContactPhoneLength} chars");

            ContactPhone = contactPhone;
        }

        // TODO (Application):
        // - Reject bookings in the past.
        // - Limit booking horizon to 7 days.
        // - Check Bay availability for the requested interval.
        public void ChangeTime(DateTimeOffset startTime, DateTimeOffset endTime)
        {
            if (startTime >= endTime)
                throw new DomainValidationException("StartTime must be earlier than EndTime");

            if (Status != MaintenanceStatus.Pending)
                throw new DomainValidationException("Time cannot be changed when status is not `pending`");

            StartTime = startTime.ToUniversalTime(); //Любое корректное смещение преобразуется в UTC. В Domain сохраняются значения с Offset = 0.
                                                     //Это для дальнейшего использования в Postgresql. EF Core сможет записать их в timestamptz.
            EndTime = endTime.ToUniversalTime();
        }


        public void ChangeProblemDescription(string? problemDescription)
        {
            if (Status != MaintenanceStatus.Pending && Status != MaintenanceStatus.InProgress)
                throw new DomainValidationException("problem description cannot be changed when status is not `Pending` or `InProgress`");

            if (string.IsNullOrWhiteSpace(problemDescription)) {
                ProblemDescription = string.Empty;
                return;
            }


            problemDescription = problemDescription.Trim();

            if (problemDescription.Length > MaxProblemDescriptionLength)
                throw new DomainValidationException($"Problem description cannot exceed {MaxProblemDescriptionLength} characters");

            ProblemDescription = problemDescription;
        }

        public void ChangeWorkDescription(string? workDescription)
        {
            if (Status != MaintenanceStatus.InProgress)
                throw new DomainValidationException("Work description cannot be changed when status is not `InProgress`");

            if (string.IsNullOrWhiteSpace(workDescription)) {
                WorkDescription = string.Empty;
                return;
            }


            workDescription = workDescription.Trim();

            if (workDescription.Length > MaxWorkDescriptionLength)
                throw new DomainValidationException($"Work description cannot exceed {MaxWorkDescriptionLength} characters");

            WorkDescription = workDescription;
        }

        public void ChangeStatus(MaintenanceStatus newStatus)
        {
            if (!CanChangeStatusTo(newStatus)) {
                throw new DomainValidationException($"Cannot change status from {Status} to {newStatus}");
            }

            Status = newStatus;
        }

        public bool CanChangeStatusTo(MaintenanceStatus newStatus)
        {
            return AllowedTransitions.TryGetValue(Status, out var allowedStatuses)
                && allowedStatuses.Contains(newStatus);
        }

        public IReadOnlyCollection<MaintenanceStatus> GetAllowedStatuses()
        {
            return Array.AsReadOnly(AllowedTransitions[Status]);
        }

    }
}
