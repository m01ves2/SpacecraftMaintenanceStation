using SpacecraftMaintenanceStation.Domain.Entities;
using SpacecraftMaintenanceStation.Domain.Exceptions;

namespace SpacecraftMaintenanceStation.Tests
{
    public class DomainUnitTests
    {
        [Fact]
        public void Constructor_ShouldSetPendingStatus()
        {
            var bay = new Bay("Bay #1");
            var startTime = DateTimeOffset.UtcNow.AddDays(1);
            var endTime = startTime.AddHours(2);

            var maintenanceRequest = new MaintenanceRequest(
                bay,
                "1-2-3-4",
                "Ivan",
                "123-45-67",
                startTime,
                endTime,
                "Engine warning appeared on control panel");
            Assert.Equal(MaintenanceStatus.Pending, maintenanceRequest.Status);
        }

        [Fact]
        public void Constructor_EndTimeShouldBeGreaterThenStartTime()
        {
            var bay = new Bay("Bay #1");
            var endTime = DateTimeOffset.UtcNow.AddDays(1);
            var startTime = endTime.AddHours(2);


            var createMaintenanceRequestAction = () =>
            {
                var mr = new MaintenanceRequest(
                bay,
                "1-2-3-4",
                "Ivan",
                "123-45-67",
                startTime,
                endTime,
                "Engine warning appeared on control panel");
            };

            DomainValidationException exception = Assert.Throws<DomainValidationException>(createMaintenanceRequestAction);
        }

        [Fact]
        public void ChangeStatus_FromFinishedToPending_ShouldThrow()
        {
            var bay = new Bay("Bay #1");
            var startTime = DateTimeOffset.UtcNow.AddDays(1);
            var endTime = startTime.AddHours(2);

            var maintenanceRequest = new MaintenanceRequest(
                bay,
                "1-2-3-4",
                "Ivan",
                "123-45-67",
                startTime,
                endTime,
                "Engine warning appeared on control panel");
            maintenanceRequest.ChangeStatus(MaintenanceStatus.InProgress);
            maintenanceRequest.ChangeStatus(MaintenanceStatus.Finished);

            var finishedToPendingWrongStatusChange = () =>
            {
                maintenanceRequest.ChangeStatus(MaintenanceStatus.Pending);
            };

            DomainValidationException exception = Assert.Throws<DomainValidationException>(finishedToPendingWrongStatusChange);
            Assert.Equal(MaintenanceStatus.Finished, maintenanceRequest.Status);
        }

        [Fact]
        public void ChangeBay_WhenInProgress_ShouldThrow()
        {
            var bay = new Bay("Bay #1");
            var startTime = DateTimeOffset.UtcNow.AddDays(1);
            var endTime = startTime.AddHours(2);

            var maintenanceRequest = new MaintenanceRequest(
                bay,
                "1-2-3-4",
                "Ivan",
                "123-45-67",
                startTime,
                endTime,
                "Engine warning appeared on control panel");
            maintenanceRequest.ChangeStatus(MaintenanceStatus.InProgress);

            var changeBayWhenInProgress = () =>
            {
                maintenanceRequest.ChangeBay(new Bay("Bay #2"));
            };

            DomainValidationException exception = Assert.Throws<DomainValidationException>(changeBayWhenInProgress);
        }

        [Fact]
        public void ChangeWorkDescription_WhenFinished_ShouldThrow()
        {
            var bay = new Bay("Bay #1");
            var startTime = DateTimeOffset.UtcNow.AddDays(1);
            var endTime = startTime.AddHours(2);

            var maintenanceRequest = new MaintenanceRequest(
                bay,
                "1-2-3-4",
                "Ivan",
                "123-45-67",
                startTime,
                endTime,
                "Engine warning appeared on control panel");
            maintenanceRequest.ChangeStatus(MaintenanceStatus.InProgress);
            maintenanceRequest.ChangeStatus(MaintenanceStatus.Finished);

            var changeWorkDescriptionWhenInProgress = () =>
            {
                maintenanceRequest.ChangeWorkDescription("Brakes should be repaired");
            };

            DomainValidationException exception = Assert.Throws<DomainValidationException>(changeWorkDescriptionWhenInProgress);
        }

        [Fact]
        public void ChangeTime_ShouldNormalizeToUtc()
        {
            var bay = new Bay("Bay #1");
            var startTime = DateTimeOffset.UtcNow.AddDays(1);
            var endTime = startTime.AddHours(5);

            var maintenanceRequest = new MaintenanceRequest(
                bay,
                "1-2-3-4",
                "Ivan",
                "123-45-67",
                startTime,
                endTime,
                "Engine warning appeared on control panel");

            var newStartTimeoffset = 3;

            var expectedStartTime = startTime.AddHours(3); //перенесли на 3 часа
            var newStartTime = expectedStartTime.ToOffset(TimeSpan.FromHours(3)); //добавили offset 3 часа

            // Создали MaintenanceRequest со временем UTC.
            maintenanceRequest.ChangeTime(newStartTime, endTime);


            Assert.Equal(startTime.AddHours(newStartTimeoffset), maintenanceRequest.StartTime);
            Assert.Equal(TimeSpan.Zero, maintenanceRequest.StartTime.Offset);
            Assert.Equal(startTime.Hour, maintenanceRequest.StartTime.Hour);
        }
    }
}
