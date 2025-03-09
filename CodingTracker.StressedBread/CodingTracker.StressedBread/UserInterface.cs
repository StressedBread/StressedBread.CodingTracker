using Spectre.Console;
using static CodingTracker.StressedBread.Enums;

namespace CodingTracker.StressedBread;

internal class UserInterface
{
    RecordHelper recordHelper = new();
    Helpers helpers = new();

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
                    recordHelper.ViewRecordsHelper();
                    break;

                case MenuChoice.AddRecord:
                    recordHelper.AddRecordHelper();
                    break;

                case MenuChoice.EditRecord:
                    recordHelper.EditRecordHelper();
                    break;

                case MenuChoice.DeleteRecord:
                    recordHelper.DeleteRecordHelper();
                    break;

                case MenuChoice.CloseApplication:
                    helpers.CloseApplication();
                    break;
            }
        }
    }

    
}
