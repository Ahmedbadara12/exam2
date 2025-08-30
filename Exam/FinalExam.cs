using System;
using System.Linq;

namespace ExamSystem
{
    public class FinalExam : Exam
    {
        public FinalExam() : base() { }

        public FinalExam(TimeSpan duration, Question[] questions) : base(duration, questions) { }

        public override void Start()
        {
            PrintHeader();
            BeginTimer();
            try
            {
                foreach (var q in Questions)
                {
                    if (IsTimeUp()) break;
                    Console.WriteLine();
                    Console.WriteLine($"[Time Left: {RemainingTime().Minutes:00}:{RemainingTime().Seconds:00}]");
                    q.Show();
                    q.UserAnswer = q.GetUserAnswer();
                }
            }
            finally
            {
                EndTimer();
            }
        }

        public override void ShowExam()
        {
            Console.WriteLine("\n--- Final Exam Review ---");
            int total = MaxGrade();
            int achieved = AchievedGradeInternal();

            int idx = 1;
            foreach (var q in Questions)
            {
                Console.WriteLine($"\nQ{idx++}: [{q.QuestionType}] {q.Header} | Mark: {q.Mark}");
                Console.WriteLine(q.Body);

                foreach (var a in q.Answers)
                {
                    string tag = a.AnswerId == q.RightAnswer.AnswerId ? " (Correct)" : "";
                    string chosen = (q.UserAnswer != null && a.AnswerId == q.UserAnswer.AnswerId) ? " [Your Choice]" : "";
                    Console.WriteLine($"{a}{tag}{chosen}");
                }
            }

            Console.WriteLine($"\nGrade: {achieved} / {total}");
        }
    }
}
