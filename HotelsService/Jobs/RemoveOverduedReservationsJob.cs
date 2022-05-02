using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Quartz;

namespace HotelsService.Jobs
{
    [DisallowConcurrentExecution]
    public class RemoveOverduedReservationsJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            using var db = new hotelsContext();
            var oldReservations = db.Events
                .Where(e => e.Creationtime < DateTime.Now.ToUniversalTime().AddMinutes(-1) && e.Type == "Reservation");
            db.Events.RemoveRange(oldReservations);
            db.SaveChanges();
            return Task.CompletedTask;
        }
    }
}