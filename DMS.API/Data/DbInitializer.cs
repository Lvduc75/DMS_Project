using DMS.Models.Entities;

namespace DMS.API.Data
{
    public static class DbInitializer
    {
        public static void Initialize(DormManagementContext context)
        {
            context.Database.EnsureCreated();
        }
    }
} 