using System.Collections.Generic;

namespace AutoSchoolExam.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        // Связь: У одного билета много вопросов
        public List<Question> Questions { get; set; } = new();
    }
}