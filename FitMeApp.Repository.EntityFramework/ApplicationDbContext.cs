using FitMeApp.Contracts;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FitMeApp.Repository
{
    public class ApplicationDbContext: IdentityDbContext<User>
    {
        //public DbSet<GymEntity> Gyms { get; set; }
        //public DbSet<TrainerEntity> Trainers { get; set; }
        //public DbSet<TrainerGymEntity> TrainerGym { get; set; }
        //public DbSet<GroupTrainingEntity> GroupTraining { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):
            base(options) 
        {
            Database.EnsureCreated();
        }

     
    }
}
