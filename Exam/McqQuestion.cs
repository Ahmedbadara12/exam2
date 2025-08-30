using System;
using System.Linq;

namespace ExamSystem
{
    public class McqQuestion : Question
    {
        public override string QuestionType => "MCQ";
        public McqQuestion() : base()
        {
        }

        public McqQuestion(string header, string body, int mark, Answer[] answers, int rightAnswerId)
            : base(header, body, mark)
        {
            if (answers == null || answers.Length < 2)
                throw new ArgumentException("MCQ must have at least two answers.", nameof(answers));

            if (!answers.Any(a => a.AnswerId == rightAnswerId))
                throw new ArgumentException("rightAnswerId must exist in answers.", nameof(rightAnswerId));

         
            if (answers.GroupBy(a => a.AnswerId).Any(g => g.Count() > 1) || answers.Any(a => a.AnswerId <= 0))
                throw new ArgumentException("AnswerId values must be positive and unique.", nameof(answers));

            Answers = answers;
            RightAnswer = answers.First(a => a.AnswerId == rightAnswerId);
        }

        public override void Show()
        {
            Console.WriteLine("MCQ:");
            base.Show();
        }

        public override Answer GetUserAnswer()
        {
            return ReadAnswerSelection();
        }
    }
}
