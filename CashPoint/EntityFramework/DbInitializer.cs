using CashPoint.Models;
using System.Linq;

namespace CashPoint.EntityFramework
{
    public class DbInitializer
    {
        public static void Initialize(CashPointContext context)
        {
            context.Database.EnsureCreated();

            // Look for any students.
            if (context.Customers.Any())
            {
                return;   // DB has been seeded
            }

            var customers = new Customer[]
            {
            new Customer{Name="Admin", Password="admin",AccountBalance=7654}
            };
            foreach (Customer s in customers)
            {
                context.Customers.Add(s);
            }
            context.SaveChanges();
        }
    }
}
