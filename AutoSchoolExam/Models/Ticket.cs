using System.Collections.Generic;

namespace AutoSchoolExam.Models
{
    /// <summary>
    /// Экзаменационный билет ПДД
    /// </summary>
    public class Ticket
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Связь: У одного билета много вопросов
        /// </summary>
        public List<Question> Questions { get; set; } = new();
    }
}