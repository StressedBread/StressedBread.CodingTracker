using CodingTracker.StressedBread.Controllers;
using CodingTracker.StressedBread.UI;

MainMenu ui = new();
CodingController codingController = new();

codingController.CreateTableOnStartQuery();
ui.Menu();