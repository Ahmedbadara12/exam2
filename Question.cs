using System;
using System.Linq;

namespace ExamSystem
{
    public abstract class Question : ICloneable, IComparable<Question>
    {
        public string Header { get; set; }
        public string Body { get; set; }
        public int Mark { get; set; }

        public Answer[] Answers { get; set; }
        public Answer RightAnswer { get; set; }

        public Answer? UserAnswer { get; set; }

        public virtual string QuestionType => "Question";

        public int RightAnswerId => RightAnswer?.AnswerId ?? -1;

        protected Question()
        {
            Header = string.Empty;
            Body = string.Empty;
            Answers = Array.Empty<Answer>();
            RightAnswer = new Answer(1, "True"); 
            Mark = 1;
        }

        protected Question(string header, string body, int mark)
            : this()
        {
            Header = header ?? string.Empty;
            Body = body ?? string.Empty;
            if (mark <= 0) throw new ArgumentOutOfRangeException(nameof(mark), "Mark must be positive.");
            Mark = mark;
        }

        public virtual void Show()
        {
            Console.WriteLine($"{Header}");
            Console.WriteLine(Body);
            Console.WriteLine($"Mark: {Mark}");
            foreach (var a in Answers)
                Console.WriteLine(a.ToString());
        }

        public abstract Answer GetUserAnswer();

        protected Answer ReadAnswerSelection()
        {
            if (Answers is null || Answers.Length == 0)
                throw new InvalidOperationException("Answers list is empty.");

            Console.Write("Your choice (enter answer id): ");
            while (true)
            {
                var raw = Console.ReadLine();
                if (int.TryParse(raw, out int id))
                {
                    var found = Answers.FirstOrDefault(a => a.AnswerId == id);
                    if (found != null) return found;
                }
                Console.Write("Invalid input. Enter a valid answer id: ");
            }
        }

        public object Clone()
        {
            var clonedAnswers = Answers.Select(a => (Answer)a.Clone()).ToArray();
            var copy = (Question)MemberwiseClone();
            copy.Answers = clonedAnswers;

            // RightAnswer: 
            copy.RightAnswer = clonedAnswers.First(a => a.AnswerId == RightAnswer.AnswerId);
            copy.UserAnswer = null;
            return copy;
        }

        public int CompareTo(Question? other)
        {
            if (other == null) return 1;
            return Mark.CompareTo(other.Mark);
        }

        public override string ToString()
            => $"{Header}\n{Body}\nMark: {Mark}";
    }
}
