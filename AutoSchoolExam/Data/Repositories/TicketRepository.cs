using AutoSchoolExam.Models;
using Microsoft.EntityFrameworkCore;

namespace AutoSchoolExam.Data.Repositories
{
    public class TicketRepository : ITicketRepository
    {
        private readonly AppDbContext _context;

        public TicketRepository(AppDbContext context)
        {
            _context = context;
        }

        // Сортируем билеты по номеру (1, 2, 3, 4, 5)
        public async Task<List<Ticket>> GetAllTicketsAsync()
        {
            return await _context.Tickets
                .OrderBy(t => t.Id)
                .ToListAsync();
        }

        // Перемешиваем вопросы и ответы внутри билета
        public async Task<Ticket?> GetTicketByIdAsync(int id)
        {
            var ticket = await _context.Tickets
                .Include(t => t.Questions)
                    .ThenInclude(q => q.Options)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (ticket != null)
            {
                Random random = new Random();

                // Перемешиваем вопросы
                ticket.Questions = ticket.Questions.OrderBy(x => random.Next()).ToList();

                // Перемешиваем варианты ответов в каждом вопросе
                foreach (var question in ticket.Questions)
                {
                    question.Options = question.Options.OrderBy(x => random.Next()).ToList();
                }
            }

            return ticket;
        }
    }
}