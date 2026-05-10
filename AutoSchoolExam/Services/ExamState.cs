using AutoSchoolExam.Models;

namespace AutoSchoolExam.Services
{
    /// <summary>
    /// Сервис для хранения состояния текущего экзамена (Scoped)
    /// </summary>
    public class ExamState
    {
        public Ticket? CurrentTicket { get; set; }
        public int CurrentQuestionIndex { get; set; } = 0;
        public int ErrorCount { get; set; } = 0;
        public string? LastErrorMessage { get; set; }

        /// <summary>Экзамен завершен: прошли все вопросы или превышен лимит ошибок</summary>
        public bool IsExamFinished => CurrentQuestionIndex >= 20 || ErrorCount > 2;

        /// <summary>Экзамен сдан: ошибок <= 2 и все вопросы пройдены</summary>
        public bool IsPassed => ErrorCount <= 2 && CurrentQuestionIndex >= 20;

        /// <summary>Сброс состояния для нового экзамена</summary>
        public void Reset()
        {
            CurrentTicket = null;
            CurrentQuestionIndex = 0;
            ErrorCount = 0;
            LastErrorMessage = null;
        }
    }
}