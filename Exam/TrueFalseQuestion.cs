using System;

namespace ExamSystem
{
    public class TrueFalseQuestion : Question
    {
        public override string QuestionType => "True/False";
        public TrueFalseQuestion() : base()
        {
            Answers = new[]
            {
                new Answer(1, "True"),
                new Answer(2, "False")
            };
            RightAnswer = Answers[0]; 
        }

        public TrueFalseQuestion(string header, string body, int mark, bool isTrue)
            : base(header, body, mark)
        {
            Answers = new[]
            {
                new Answer(1, "True"),
                new Answer(2, "False")
            };
            RightAnswer = isTrue ? Answers[0] : Answers[1];
        }

        public override void Show()
        {
            Console.WriteLine("True / False:");
            base.Show();
        }

        public override Answer GetUserAnswer()
        {
            return ReadAnswerSelection();
        }
    }
}
