using FitMeApp.Common;
using FitMeApp.Repository.EntityFramework.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FitMeApp.Repository.EntityFramework
{
    public class ApplicationDbContext: IdentityDbContext<User>
    {
        public DbSet<GymEntity> Gyms { get; set; }
        public DbSet<TrainerEntity> Trainers { get; set; }       
        public DbSet<GroupClassEntity> GroupClasses { get; set; }
        public DbSet<GroupClassGymTrainerEntity> GroupClassGymTrainer { get; set; }

        public DbSet<SubscriptionTypeEntity> SubscriptionType { get; set; }
        public DbSet<UserSubscriptionEntity> UserSubscription { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):
            base(options) 
        {
            //Database.EnsureCreated();
        }

     
    }
}
