using test_app.Controllers;
using test_app.Models;

namespace test_app.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ProjectContext context)
        {
            if (context.locations.Any())
            {
                return;
            }

            List<Location> locations = new List<Location>
            {
                new Location {id = 1, city = "Warszawa"},
                new Location {id = 2, city = "Kraków"},
                new Location {id = 3, city = "Katowice"}
            };

            List<Desk> desks = new List<Desk>
            {
                new Desk {id = 1, isAvailable = true, location = locations[0]},
                new Desk {id = 2, isAvailable = false, location = locations[0]},
                new Desk {id = 3, isAvailable = true, location = locations[1]}
            };

            context.locations.AddRange(locations);
            context.desks.AddRange(desks);
            context.SaveChanges();
        }
    }
}