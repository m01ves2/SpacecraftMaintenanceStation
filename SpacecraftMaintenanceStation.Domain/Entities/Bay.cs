using SpacecraftMaintenanceStation.Domain.Exceptions;

namespace SpacecraftMaintenanceStation.Domain.Entities
{
    public class Bay
    {
        public const int MaxNameLength = 150;
        public int Id { get; private set; }
        public string Name { get; private set; } = string.Empty;

        private Bay()
        {
        }

        public Bay(string name)
        {
            ChangeName(name);
        }

        public void ChangeName(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) {
                throw new DomainValidationException("Name cannot be empty");
            }

            name = name.Trim();
            if (name.Length > MaxNameLength)
                throw new DomainValidationException($"Name cannot be longer than {MaxNameLength} chars");

            Name = name;            
        }
    }
}
