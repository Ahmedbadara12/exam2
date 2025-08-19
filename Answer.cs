using System;

namespace ExamSystem
{
    public class Answer : ICloneable, IComparable<Answer>
    {
        public int AnswerId { get; set; }
        public string AnswerText { get; set; }

        public Answer()
        {
            AnswerText = string.Empty;
        }

        public Answer(int id, string text)
        {
            if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id), "AnswerId must be positive.");
            AnswerId = id;
            AnswerText = text ?? string.Empty;
        }

        public object Clone() => new Answer(AnswerId, AnswerText);

        public int CompareTo(Answer? other)
        {
            if (other is null) return 1;
            return AnswerId.CompareTo(other.AnswerId);
        }

        public override string ToString() => $"{AnswerId}. {AnswerText}";
    }
}
