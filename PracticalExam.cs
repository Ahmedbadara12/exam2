using System;

namespace ExamSystem
{
    public class PracticalExam : Exam
    {
        public PracticalExam() : base() { }

        public PracticalExam(TimeSpan duration, Question[] questions) : base(duration, questions) { }

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
            Console.WriteLine("\n--- Practical Exam - Correct Answers ---");
            int idx = 1;
            int total = MaxGrade();
            int achieved = AchievedGradeInternal();
            foreach (var q in Questions)
            {
                Console.WriteLine($"\nQ{idx++}: [{q.QuestionType}] {q.Header} | Mark: {q.Mark}");
                Console.WriteLine(q.Body);
                Console.WriteLine($"Right Answer: {q.RightAnswer}");
                if (q.UserAnswer is not null)
                    Console.WriteLine($"Your Choice:  {q.UserAnswer}");
            }
            Console.WriteLine($"\nYour Score: {achieved} / {total}");
        }
    }
}
