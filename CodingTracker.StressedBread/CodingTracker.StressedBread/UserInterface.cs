using CodingTracker.StressedBread.Controllers;
using Spectre.Console;
using static CodingTracker.StressedBread.Enums;

namespace CodingTracker.StressedBread;

internal class UserInterface
{
    CodingController codingController = new();
    Validation validation = new();

    internal void MainMenu()
    {
        while (true)
        {
            Console.Clear();

            var selection = AnsiConsole.Prompt(new SelectionPrompt<MenuChoice>()
                .Title("Main Menu")
                .AddChoices(Enum.GetValues<MenuChoice>()));

            switch (selection)
            {
                case MenuChoice.ViewRecords:

                    break;
                case MenuChoice.AddRecord:
                    AddRecordInput();
                    break;
                case MenuChoice.EditRecord:

                    break;
                case MenuChoice.DeleteRecord:

                    break;
                case MenuChoice.CloseApplication:
                    CloseApplication();
                    break;
            }
        }
    }

    private void AddRecordInput()
    {
        string startTime = AnsiConsole.Ask<string>("Enter the start time of the coding session in yyyy-mm-dd hh:mm:ss format:");
        string endTime = AnsiConsole.Ask<string>("Enter the end time of the coding session yyyy-mm-dd hh:mm:ss format:");

        DateTime startTimeFormatted = validation.DateTimeValidation(startTime);
        DateTime endTimeFormatted = validation.DateTimeValidation(endTime);

        if (startTimeFormatted == DateTime.MinValue || endTimeFormatted == DateTime.MinValue)
        {
            AnsiConsole.MarkupLine("[red]Invalid date time format! Press any key to return to menu.[/]");
            Console.ReadKey();
            return;
        }
        int duration = codingController.DurationCalculation(startTimeFormatted, endTimeFormatted);
        codingController.AddRecord(startTimeFormatted, endTimeFormatted, duration);
    }

    private void CloseApplication()
    {
        Environment.Exit(0);
    }
}
