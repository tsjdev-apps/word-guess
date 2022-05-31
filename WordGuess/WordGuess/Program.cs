using Spectre.Console.Cli;
using WordGuess.Commands;

var app = new CommandApp();

app.Configure(config =>
{
    config.AddCommand<WordGuessCommand>("word-guess");
});

return await app.RunAsync(args);