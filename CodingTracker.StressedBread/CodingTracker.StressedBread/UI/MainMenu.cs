﻿using CodingTracker.StressedBread.Helpers;
using Spectre.Console;
using static CodingTracker.StressedBread.Enums;

namespace CodingTracker.StressedBread.UI;

/// <summary>
/// Handles the Main Menu UI.
/// </summary>

internal class MainMenu
{
    RecordHelper recordHelper = new();
    MainHelpers mainHelpers = new();
    StopWatchSession stopWatchSession = new();

    internal void Menu()
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

                case MenuChoice.CodingSession:
                    stopWatchSession.StartSession();
                    break;

                case MenuChoice.Report:
                    recordHelper.ReportHelper();
                    break;

                case MenuChoice.SetGoal:
                    recordHelper.GoalHelper();
                    break;

                case MenuChoice.CloseApplication:
                    mainHelpers.CloseApplication();
                    break;
            }
        }
    }
}