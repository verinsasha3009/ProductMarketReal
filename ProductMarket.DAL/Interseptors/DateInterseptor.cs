using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using ProductMarket.Domain.Interfaces;

namespace ProductMarket.DAL.Interseptors
{
    public class DateInterseptor : SaveChangesInterceptor
    {
        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,InterceptionResult<int> result,
            CancellationToken token = new CancellationToken())
        {

            var dbContext = eventData.Context;
            if(dbContext == null )
            {
                return base.SavingChangesAsync(eventData, result, token);
            }
            var entries = dbContext.ChangeTracker.Entries<IDataTimeSourse>()
                .Where(p=>p.State == EntityState.Added||p.State == EntityState.Modified).ToList();
            foreach ( var entry in entries )
            {
                if( entry.State == EntityState.Added)
                {
                    entry.Property(p=>p.CreatedAt).CurrentValue = DateTime.UtcNow;
                }
                if( entry.State == EntityState.Modified)
                {
                    entry.Property(p=>p.UpdatedAt).CurrentValue = DateTime.UtcNow;
                }
            }
            return base.SavingChangesAsync(eventData, result, token);
        }
    }
}
