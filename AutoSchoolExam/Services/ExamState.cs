using AutoSchoolExam.Models;

namespace AutoSchoolExam.Services
{
    public class ExamState
    {
        public Ticket? CurrentTicket { get; set; }
        public int CurrentQuestionIndex { get; set; } = 0;
        public int ErrorCount { get; set; } = 0;
        public string? LastErrorMessage { get; set; }

        public bool IsExamFinished => CurrentQuestionIndex >= 20 || ErrorCount > 2;

        public bool IsPassed => ErrorCount <= 2 && CurrentQuestionIndex >= 20;

        public void Reset()
        {
            CurrentTicket = null;
            CurrentQuestionIndex = 0;
            ErrorCount = 0;
            LastErrorMessage = null;
        }
    }
}