using AutoSchoolExam.Models;
using Microsoft.EntityFrameworkCore;

namespace AutoSchoolExam.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // таблицы
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Option> Options { get; set; }

        // Настройка связей 
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Связь Билет -> Вопросы (Каскадное удаление)
            modelBuilder.Entity<Ticket>()
                .HasMany(t => t.Questions)
                .WithOne()
                .HasForeignKey(q => q.TicketId)
                .OnDelete(DeleteBehavior.Cascade);

            // Связь Вопрос -> Ответы
            modelBuilder.Entity<Question>()
                .HasMany(q => q.Options)
                .WithOne()
                .HasForeignKey(o => o.QuestionId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}