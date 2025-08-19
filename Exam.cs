using System;
using System.Linq;
using System.Threading;

namespace ExamSystem
{
    public abstract class Exam : ICloneable
    {
        public TimeSpan Duration { get; set; }
        public Question[] Questions { get; set; }
        public Subject? Subject { get; set; }

        private volatile bool _timeUp;
        private DateTime _startUtc;
        private CancellationTokenSource? _timerCts;

        protected Exam()
        {
            Duration = TimeSpan.Zero;
            Questions = Array.Empty<Question>();
        }

        protected Exam(TimeSpan duration, Question[] questions)
        {
            if (duration < TimeSpan.Zero) throw new ArgumentOutOfRangeException(nameof(duration));
            Duration = duration;
            Questions = questions ?? Array.Empty<Question>();
            if (Questions.Length == 0)
                throw new ArgumentException("Exam must contain at least one question.", nameof(questions));
        }

        public int NumberOfQuestions => Questions.Length;

        public abstract void Start();

        public abstract void ShowExam();

        public int MaxGrade() => Questions.Sum(q => q.Mark);

        protected int GetAchievedGrade()
            => Questions.Where(q => q.UserAnswer is not null)
                        .Sum(q => q.UserAnswer!.AnswerId == q.RightAnswer.AnswerId ? q.Mark : 0);

        public object Clone()
        {
            Question[] cloned = Questions.Select(q => (Question)q.Clone()).ToArray();
            Exam copy = (Exam)MemberwiseClone();
            copy.Questions = cloned;
            return copy;
        }

        public override string ToString()
        {
            return $"Exam: {GetType().Name}, Duration: {Duration}, Questions: {NumberOfQuestions}";
        }

        protected void PrintHeader()
        {
            Console.WriteLine(new string('=', 50));
            Console.WriteLine(ToString());
            Console.WriteLine($"Subject: {Subject?.Name ?? "-"}");
            Console.WriteLine($"Max Grade: {MaxGrade()}");
            Console.WriteLine(new string('=', 50));
        }

        protected int AchievedGradeInternal() => GetAchievedGrade();

        protected void BeginTimer()
        {
            _timeUp = false;
            _startUtc = DateTime.UtcNow;
            _timerCts = new CancellationTokenSource();
            var token = _timerCts.Token;

            _ = System.Threading.Tasks.Task.Run(async () =>
            {
                try
                {
                    while (!token.IsCancellationRequested)
                    {
                        var remaining = RemainingTime();
                        Console.Title = $"Remaining: {Math.Max(0, (int)remaining.TotalMinutes):00}:{remaining.Seconds:00}";
                        if (remaining <= TimeSpan.Zero)
                        {
                            _timeUp = true;
                            Console.WriteLine();
                            Console.WriteLine("*** Time is up! The exam will end now. ***");
                            Console.Beep();
                            break;
                        }
                        await System.Threading.Tasks.Task.Delay(1000, token);
                    }
                }
                catch (OperationCanceledException) { }
            }, token);
        }

        protected void EndTimer()
        {
            try { _timerCts?.Cancel(); } catch { }
            _timerCts?.Dispose();
            _timerCts = null;
            Console.Title = "Exam Finished";
        }

        protected bool IsTimeUp() => _timeUp;

        protected TimeSpan RemainingTime()
        {
            var elapsed = DateTime.UtcNow - _startUtc;
            var remaining = Duration - elapsed;
            return remaining < TimeSpan.Zero ? TimeSpan.Zero : remaining;
        }
    }
}
