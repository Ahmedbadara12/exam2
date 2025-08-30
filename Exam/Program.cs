using ExamSystem;

static int ReadInt(string prompt)
{
	while (true)
	{
		Console.Write(prompt);
		string? raw = Console.ReadLine();
		if (int.TryParse(raw, out int value)) return value;
		Console.WriteLine("Invalid number. Please enter a valid integer.");
	}
}

static int ReadPositiveInt(string prompt)
{
	while (true)
	{
		int value = ReadInt(prompt);
		if (value > 0) return value;
		Console.WriteLine("Value must be greater than 0.");
	}
}

static int ReadIntInRange(string prompt, int min, int max)
{
	while (true)
	{
		int value = ReadInt(prompt);
		if (value >= min && value <= max) return value;
		Console.WriteLine($"Please enter a number between {min} and {max}.");
	}
}

static string ReadNonEmptyString(string prompt)
{
	while (true)
	{
		Console.Write(prompt);
		string? text = Console.ReadLine();
		if (!string.IsNullOrWhiteSpace(text)) return text.Trim();
		Console.WriteLine("Text cannot be empty. Please try again.");
	}
}

static bool ReadYesNo(string prompt)
{
	while (true)
	{
		Console.Write(prompt);
		string? s = Console.ReadLine();
		if (string.Equals(s, "y", StringComparison.OrdinalIgnoreCase) ||
			string.Equals(s, "yes", StringComparison.OrdinalIgnoreCase)) return true;
		if (string.Equals(s, "n", StringComparison.OrdinalIgnoreCase) ||
			string.Equals(s, "no", StringComparison.OrdinalIgnoreCase)) return false;
		Console.WriteLine("Please answer with 'y' or 'n'.");
	}
}

static Answer[] ReadMcqAnswersFromUser(int count)
{
	var answers = new Answer[count];
	for (int i = 0; i < count; i++)
	{
		string text = ReadNonEmptyString($"Enter answer #{i + 1} text: ");
		answers[i] = new Answer(i + 1, text);
	}
	return answers;
}

Console.WriteLine("Welcome to the Exam System \n");
int subjectId = ReadPositiveInt("Enter Subject Id: ");
string subjectName = ReadNonEmptyString("Enter Subject Name: ");
var subject = new Subject(subjectId, subjectName);

Console.WriteLine("Choose Exam Type: 1) Final  2) Practical");
int examType = ReadIntInRange("Your choice: ", 1, 2);

int minutes = ReadPositiveInt("Enter exam duration in minutes: ");
TimeSpan duration = TimeSpan.FromMinutes(minutes);

int numQuestions = ReadPositiveInt("Enter number of questions: ");

var questions = new Question[numQuestions];
for (int i = 0; i < numQuestions; i++)
{
	Console.WriteLine($"\nCreating Question #{i + 1}");
	int qType = ReadIntInRange("Select type: 1) True/False  2) MCQ: ", 1, 2);
	string header = ReadNonEmptyString("Enter Question Header: ");
	string body = ReadNonEmptyString("Enter Question Body: ");
	int mark = ReadPositiveInt("Enter Question Mark: ");

	if (qType == 1)
	{
		bool isTrue = ReadYesNo("Is the correct answer True? (y/n): ");
		questions[i] = new TrueFalseQuestion(header, body, mark, isTrue);
	}
	else
	{
		int choices = ReadIntInRange("How many choices (2-6): ", 2, 6);
		var answers = ReadMcqAnswersFromUser(choices);
		int rightId = ReadIntInRange("Enter the id of the correct answer: ", 1, choices);
		questions[i] = new McqQuestion(header, body, mark, answers, rightId);
	}
}

Exam exam = examType == 2
	? new PracticalExam(duration, questions)
	: new FinalExam(duration, questions);

subject.CreateExam(exam);

Console.WriteLine("\n----- Exam Starts -----\n");
exam.Start();
Console.WriteLine("\n----- Exam Finished -----\n");

Console.WriteLine("\n----- Exam Review -----\n");
exam.ShowExam();

Console.WriteLine("\nThanks for using the Exam System!");
Console.WriteLine("Press Enter to exit...");
Console.ReadLine();
