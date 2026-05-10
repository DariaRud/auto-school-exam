using System.Collections.Generic;

namespace AutoSchoolExam.Models
{
    public class Question
    {
        public int Id { get; set; }
        public int TicketId { get; set; } // Связь с билетом
        public string Text { get; set; } = string.Empty;

        // Связь: У одного вопроса много вариантов ответа
        public List<Option> Options { get; set; } = new();
    }
}