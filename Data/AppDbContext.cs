using Gestion_de_Tâches.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace Gestion_de_Tâches.Data
{
    public class AppDbContext : DbContext
    {
        // Constructeur avec options injectées
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // DbSets pour chaque modèle
        public DbSet<User> Users { get; set; }
        public DbSet<Gestion_de_Tâches.Models.Task> Tasks { get; set; }
        public DbSet<TaskHistory> TaskHistories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Relations et contraintes

            // Une tâche peut être assignée à un utilisateur (nullable)
            modelBuilder.Entity<Gestion_de_Tâches.Models.Task>()
                .HasOne(t => t.AssignedToUser)
                .WithMany() // Pas de collection inverse dans User ici
                .HasForeignKey(t => t.AssignedToUserId)
                .OnDelete(DeleteBehavior.SetNull);

            // Historique lié à une tâche
            modelBuilder.Entity<TaskHistory>()
                .HasOne(h => h.Task)
                .WithMany(t => t.History)
                .HasForeignKey(h => h.TaskId);
        }
    }
}
