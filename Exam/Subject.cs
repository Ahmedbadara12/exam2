using System;

namespace ExamSystem
{
    public class Subject : ICloneable, IComparable<Subject>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Exam? Exam { get; private set; }

        public Subject()
        {
            Name = string.Empty;
        }

        public Subject(int id, string name) : this()
        {
            if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id));
            Id = id;
            Name = name ?? string.Empty;
        }

        public void CreateExam(Exam exam)
        {
            Exam = exam ?? throw new ArgumentNullException(nameof(exam));
            Exam.Subject = this;
        }

        public object Clone()
        {
          
            return new Subject(Id, Name);
        }

        public int CompareTo(Subject? other)
        {
            if (other == null) return 1;
            return Id.CompareTo(other.Id);
        }

        public override string ToString() => $"Subject: {Id} - {Name}";
    }
}
