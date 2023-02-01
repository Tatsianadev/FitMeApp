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
        public DbSet<TrainingEntity> Trainings { get; set; }
        public DbSet<TrainingTrainerEntity> TrainingTrainer { get; set; }

        public DbSet<SubscriptionEntity> Subscriptions { get; set; }
        public DbSet<GymSubscriptionEntity> GymSubscriptions { get; set; }
        public DbSet<UserSubscriptionEntity> UserSubscriptions { get; set; }

        public DbSet<PersonalTrainingPersonalTrainingEventEntity> PersonalTrainingEvents { get; set; }
        public DbSet<GymWorkHoursEntity> GymWorkHours { get; set; }
        public DbSet<TrainerWorkHoursEntity> TrainerWorkHours { get; set; }

        public DbSet<ChatMessageEntity> ChatMessages { get; set; }
        public  DbSet<ChatContactEntity> ChatContacts { get; set; }


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):
            base(options) 
        {
            //Database.EnsureCreated();
        }

     
    }
}
