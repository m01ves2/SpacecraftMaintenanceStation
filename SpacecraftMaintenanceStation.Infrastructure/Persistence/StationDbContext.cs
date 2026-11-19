using Microsoft.EntityFrameworkCore;
using SpacecraftMaintenanceStation.Domain.Entities;

namespace SpacecraftMaintenanceStation.Infrastructure.Persistence
{
    public class StationDbContext : DbContext
    {
        // тут возможны два эквивалентных варианта:
        //1.Первый вариант — свойство с get; set;
        public DbSet<Bay> Bays { get; set; } //Компилятор создаёт для него скрытое поле, в котором хранится ссылка на DbSet<Bay>.
                                             //При создании DbContext EF Core обнаруживает такие свойства и инициализирует их.
                                             //и потом можно писать:
                                             //var bays = await context.Bays.ToListAsync();

        //2.Второй вариант — вычисляемое свойство
        //public DbSet<Bay> Bays  => Set<Bay>(); //При каждом обращении к Bays вызывается метод DbContext.Set<TEntity>().

        // ни один из способов не создаёт таблицу непосредственно.
        // Наличие DbSet<Bay> помогает EF Core включить Bay в модель, а уже конфигурация модели и миграции определяют схему базы.
        public DbSet<MaintenanceRequest> MaintenanceRequests { get; set; }

        public StationDbContext(DbContextOptions<StationDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(StationDbContext).Assembly);
        }


        // Explicit interface implementation avoids hiding DbContext.SaveChangesAsync,
        // which returns Task<int>, while IUnitOfWork requires Task.
        //async Task IUnitOfWork.SaveChangesAsync(CancellationToken cancellationToken)
        //{
        //    await base.SaveChangesAsync(cancellationToken);
        //}
    }
}
