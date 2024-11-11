using Figgle;
using Newtonsoft.Json;
using Spectre.Console;
namespace QuizPortfolioApp
{
    internal class Program
    {
        static void Main()
        {
            // Display the title using Figgle
            string title = "Quiz App";
            var figgleTitle = FiggleFonts.Standard.Render(title);
            AnsiConsole.MarkupLine($"[bold green]{figgleTitle}[/]");

            // Load questions from JSON file
            string json = File.ReadAllText("questions.json");
            var quizData = JsonConvert.DeserializeObject<QuizData>(json);

            int totalQuestions = 0;
            int correctAnswers = 0;
            int wrongAnswers = 0;

            // Loop through categories and questions
            foreach (var category in quizData.Categories)
            {
                AnsiConsole.MarkupLine($"[bold yellow]{category.Name}[/]\n");

                foreach (var question in category.Questions)
                {
                    totalQuestions++;
                    bool isCorrect = AskQuestion(question);

                    if (isCorrect)
                    {
                        correctAnswers++;
                        AnsiConsole.MarkupLine("[green]Correct![/]");
                    }
                    else
                    {
                        wrongAnswers++;
                        AnsiConsole.MarkupLine("[red]Wrong![/]");
                    }

                    AnsiConsole.MarkupLine(""); // Empty line for spacing
                }
            }

            // Display statistics
            DisplayStatistics(totalQuestions, correctAnswers, wrongAnswers);
        }

        static bool AskQuestion(Question question)
        {
            var options = new List<string>(question.Options);
            options.Add("Quit");

            // Display the question and options
            AnsiConsole.MarkupLine($"[bold]Q:[/] {question.question}");

            // Display the options in a styled way
            var selected = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[bold]Choose your answer[/]:")
                    .PageSize(10)
                    .AddChoices(options));

            // Check if the answer is correct
            return selected == question.Answer;
        }

        static void DisplayStatistics(int total, int correct, int wrong)
        {
            AnsiConsole.MarkupLine("[bold cyan]Quiz Statistics:[/]");
            AnsiConsole.MarkupLine($"[bold]Total Questions:[/] {total}");
            AnsiConsole.MarkupLine($"[bold]Correct Answers:[/] {correct}");
            AnsiConsole.MarkupLine($"[bold]Wrong Answers:[/] {wrong}");

            double score = (double)correct / total * 100;
            AnsiConsole.MarkupLine($"[bold green]Your Score:[/] {score:0.00}%");
        }
    }

    // Classes to represent the JSON structure
    public class QuizData
    {
        public List<Category> Categories { get; set; }
    }

    public class Category
    {
        public string Name { get; set; }
        public List<Question> Questions { get; set; }
    }

    public class Question
    {
        public string question { get; set; }
        public List<string> Options { get; set; }
        public string Answer { get; set; }
    }
}