using Spectre.Console;
using Spectre.Console.Cli;
using System.Reflection;
using System.Text.Json;

namespace WordGuess.Commands;

internal class WordGuessCommand : Command
{
    public override int Execute(CommandContext context)
    {
        AnsiConsole.Write(new FigletText("Word Guess")
            .LeftAligned()
            .Color(Color.Blue));

        var allWordAsArray = GetAllWords();
        var wordToGuess = allWordAsArray[new Random()
            .Next(0, allWordAsArray.Length)];
        var numberOfGuesses = 0;

        while (numberOfGuesses < 6)
        {
            var guessedWord = GetWordFromUser();

            if (allWordAsArray.Contains(guessedWord))
            {
                var wordMatches = 0;

                for (var i = 0; i < guessedWord.Length; i++)
                {
                    if (wordToGuess[i] == guessedWord[i])
                    {
                        AnsiConsole.Markup($"[green]{guessedWord[i]}[/]");
                        wordMatches++;
                    }
                    else if (wordToGuess.Contains(guessedWord[i]))
                    {
                        AnsiConsole.Markup($"[yellow]{guessedWord[i]}[/]");
                    }
                    else
                    {
                        AnsiConsole.Markup($"[red]{guessedWord[i]}[/]");
                    }
                }

                AnsiConsole.WriteLine();

                if (wordMatches == wordToGuess.Length)
                {
                    AnsiConsole.MarkupLine("[green]You win![/]");
                    break;
                }

                if (numberOfGuesses == 5 && wordMatches != wordToGuess.Length)
                {
                    AnsiConsole.MarkupLine("[red]You lose![/]");
                    break;
                }

                numberOfGuesses++;
            }
            else
            {
                AnsiConsole.MarkupLine("[red]The word isn't a valid word.[/]");
            }
        }


        return 0;

        static string[] GetAllWords()
        {
            var assembly = Assembly.GetExecutingAssembly();

            var fileName = $"WordGuess.Assets.words.json";

            using Stream stream = assembly.GetManifestResourceStream(fileName);
            using StreamReader reader = new(stream);

            return JsonSerializer.Deserialize<string[]>(reader.ReadToEnd());
        }

        static string GetWordFromUser()
        {
            return AnsiConsole.Prompt(
                new TextPrompt<string>("Enter a FIVE letter word:")
                    .Validate(word => word.Length == 5
    ? ValidationResult.Success()
    : ValidationResult.Error("[yellow]Your entered word doesn't have FIVE letters.[/]")))
                .ToUpper();
        }
    }
}
