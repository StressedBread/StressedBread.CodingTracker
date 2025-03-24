using CodingTracker.StressedBread.Controllers;
using CodingTracker.StressedBread.Helpers;
using CodingTracker.StressedBread.UI;

/// <summary>
/// Executes the whole program.
/// </summary>

MainMenu ui = new();
CodingController codingController = new();
MainHelpers mainHelpers = new MainHelpers();
StringFormatting stringFormatting = new();

mainHelpers.CreateDatabaseFolder();
codingController.CreateTableOnStartQuery();
ui.Menu();