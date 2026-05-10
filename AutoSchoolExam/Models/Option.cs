namespace AutoSchoolExam.Models
{
    public class Option
    {
        public int Id { get; set; }
        public int QuestionId { get; set; } // Связь с вопросом
        public string Text { get; set; } = string.Empty;
        public bool IsCorrect { get; set; } // Правильный ли ответ?
    }
}