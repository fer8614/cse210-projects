using System.Globalization;

int checks = 0;
void Equal<T>(T expected, T actual, string label)
{
    if (!EqualityComparer<T>.Default.Equals(expected, actual))
        throw new Exception($"{label}: expected {expected}, got {actual}");
    checks++;
}
void Invalid(Action action, string label)
{
    try { action(); }
    catch (ArgumentOutOfRangeException) { checks++; return; }
    throw new Exception($"{label}: expected ArgumentOutOfRangeException");
}

Goal simple = new SimpleGoal("Read", "A chapter", 10);
Equal(10, simple.RecordEvent(), "simple first reward");
Equal(0, simple.RecordEvent(), "simple second reward");
Equal(true, simple.IsComplete(), "simple complete");
Equal("[X] Read (A chapter)", simple.GetDetailsString(), "simple details");
Equal("SimpleGoal:Read,A%20chapter,10,True", simple.GetStringRepresentation(), "simple save");
Equal(0, new SimpleGoal("Loaded", "Done", 10, true).RecordEvent(), "loaded simple");

Goal eternal = new EternalGoal("Practice", "Daily", 5);
for (int i = 0; i < 3; i++)
{
    Equal(5, eternal.RecordEvent(), "eternal repeat");
    Equal(false, eternal.IsComplete(), "eternal incomplete");
}
Equal("[ ] Practice (Daily)", eternal.GetDetailsString(), "eternal details");
Equal("EternalGoal:Practice,Daily,5", eternal.GetStringRepresentation(), "eternal save");

ChecklistGoal checklist = new ChecklistGoal("Walk", "Outside", 10, 3, 50);
Equal(10, checklist.RecordEvent(), "checklist first");
Equal(false, checklist.IsComplete(), "checklist incomplete");
Equal("[ ] Walk (Outside) -- Currently completed: 1/3", checklist.GetDetailsString(), "progress");
Equal(10, checklist.RecordEvent(), "checklist second");
Equal(60, checklist.RecordEvent(), "checklist bonus");
Equal(true, checklist.IsComplete(), "checklist complete");
Equal(0, checklist.RecordEvent(), "checklist no farming");
Equal("[X] Walk (Outside) -- Currently completed: 3/3", checklist.GetDetailsString(), "count clamped");
// Checklist fields: escaped name, escaped description, points, target, bonus, amountCompleted.
Equal("ChecklistGoal:Walk,Outside,10,3,50,3", checklist.GetStringRepresentation(), "checklist save order");
Equal(60, new ChecklistGoal("Loaded", "Partial", 10, 3, 50, 2).RecordEvent(), "loaded partial bonus");
Equal(0, new ChecklistGoal("Loaded", "Complete", 10, 3, 50, 3).RecordEvent(), "loaded complete");
ChecklistGoal one = new ChecklistGoal("One", "Once", 2, 1, 7);
Equal(9, one.RecordEvent(), "target one bonus");
Equal(0, one.RecordEvent(), "target one repeated");
SimpleGoal zeroSimple = new SimpleGoal("Zero", "", 0);
Equal(0, zeroSimple.RecordEvent(), "zero simple reward");
Equal(true, zeroSimple.IsComplete(), "zero simple completes");
Equal(0, new EternalGoal("Zero", "", 0).RecordEvent(), "zero eternal");
ChecklistGoal zeroChecklist = new ChecklistGoal("Zero", "", 0, 1, 0);
Equal(0, zeroChecklist.RecordEvent(), "zero checklist");
Equal(true, zeroChecklist.IsComplete(), "zero checklist completes");

Invalid(() => new SimpleGoal("", "", -1), "negative simple points");
Invalid(() => new EternalGoal("", "", -1), "negative eternal points");
Invalid(() => new ChecklistGoal("", "", -1, 1, 0), "negative checklist points");
Invalid(() => new ChecklistGoal("", "", 0, 0, 0), "zero target");
Invalid(() => new ChecklistGoal("", "", 0, -1, 0), "negative target");
Invalid(() => new ChecklistGoal("", "", 0, 1, -1), "negative bonus");
Invalid(() => new ChecklistGoal("", "", 0, 2, 0, -1), "negative count");
Invalid(() => new ChecklistGoal("", "", 0, 2, 0, 3), "count over target");

string text = "Comma,colon:percent%\nline";
string escaped = "Comma%2Ccolon%3Apercent%25%0Aline";
CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("ar-SA");
Goal[] escapedGoals = {
    new SimpleGoal(text, text, 123),
    new EternalGoal(text, text, 123),
    new ChecklistGoal(text, text, 123, 2, 456, 1)
};
string[] saves = {
    $"SimpleGoal:{escaped},{escaped},123,False",
    $"EternalGoal:{escaped},{escaped},123",
    $"ChecklistGoal:{escaped},{escaped},123,2,456,1"
};
for (int i = 0; i < escapedGoals.Length; i++)
{
    Equal(saves[i], escapedGoals[i].GetStringRepresentation(), "escaped invariant save");
    Equal(true, escapedGoals[i].GetDetailsString().Contains(text), "details retain raw text");
    Equal(text, Uri.UnescapeDataString(escapedGoals[i].GetStringRepresentation().Split(':')[1].Split(',')[0]), "text round trip");
}
Invalid(() => new ChecklistGoal("Overflow", "Bonus", int.MaxValue, 1, 1), "reward overflow");
GoalManager manager = new GoalManager();
manager.AddGoal(new SimpleGoal(text, text, 999));
manager.AddGoal(new EternalGoal("Daily", "Repeat", 1));
manager.AddGoal(new ChecklistGoal("Steps", "Three", 10, 3, 50));
Equal(999, manager.RecordEvent(0), "manager award");
Equal(1L, manager.GetLevel(), "level at 999");
Equal(0, manager.RecordEvent(0), "manager no farming");
manager.RecordEvent(1);
Equal(1000L, manager.GetScore(), "score at boundary");
Equal(2L, manager.GetLevel(), "level at 1000");
manager.RecordEvent(2);
string fixture = "tests/EternalQuestChecks/obj/manager-check.txt";
manager.SaveGoals(fixture);
GoalManager loaded = new GoalManager();
loaded.AddGoal(new EternalGoal("Replace", "Old", 0));
loaded.LoadGoals(fixture);
Equal(1010L, loaded.GetScore(), "loaded score");
Equal(2L, loaded.GetLevel(), "loaded level");
Equal(3, loaded.GetGoals().Count, "load replaces");
for (int i = 0; i < 3; i++)
    Equal(manager.GetGoals()[i].GetStringRepresentation(), loaded.GetGoals()[i].GetStringRepresentation(), "round trip goal");
Equal(0, loaded.RecordEvent(0), "loaded simple no farming");
Equal(10, loaded.RecordEvent(2), "loaded progress");
Equal(60, loaded.RecordEvent(2), "loaded bonus");
Equal(0, loaded.RecordEvent(2), "loaded checklist no farming");
string[] badFiles = {
    "-1", "not-score", "0\nUnknown:a,b,1", "0\nSimpleGoal:a,b,1",
    "0\nSimpleGoal:a,b,1,maybe", "0\nEternalGoal:a,b,-1",
    "0\nChecklistGoal:a,b,1,0,0,0", "0\nChecklistGoal:a,b,1,2,0,3",
    "0\nEternalGoal:a,b,1\nSimpleGoal:a,b,1,maybe"
};
foreach (string bad in badFiles)
{
    File.WriteAllText(fixture, bad);
    long before = loaded.GetScore();
    string goalBefore = loaded.GetGoals()[2].GetStringRepresentation();
    try { loaded.LoadGoals(fixture); throw new Exception("Malformed load accepted"); }
    catch (FormatException) { checks++; }
    Equal(before, loaded.GetScore(), "failed load score unchanged");
    Equal(goalBefore, loaded.GetGoals()[2].GetStringRepresentation(), "failed load goals unchanged");
}
File.WriteAllText(fixture, $"{long.MaxValue}\nSimpleGoal:a,b,1,False");
loaded.LoadGoals(fixture);
try { loaded.RecordEvent(0); throw new Exception("Score overflow accepted"); }
catch (OverflowException) { checks++; }
Equal(long.MaxValue, loaded.GetScore(), "overflow score preserved");
Equal(false, loaded.GetGoals()[0].IsComplete(), "overflow progress preserved");
File.WriteAllText(fixture, "0\nEternalGoal:a,b,2147483647");
loaded.LoadGoals(fixture);
loaded.RecordEvent(0);
loaded.RecordEvent(0);
Equal(4294967294L, loaded.GetScore(), "score exceeds int safely");

TextReader originalIn = Console.In;
TextWriter originalOut = Console.Out;
string RunMenu(GoalManager menu, string input)
{
    using StringReader reader = new StringReader(input);
    using StringWriter writer = new StringWriter();
    try
    {
        Console.SetIn(reader);
        Console.SetOut(writer);
        menu.Start();
        return writer.ToString();
    }
    finally { Console.SetIn(originalIn); Console.SetOut(originalOut); }
}
GoalManager menuManager = new GoalManager();
string output = RunMenu(menuManager, "bad\n9\n1\n1\n\nRead\n\nChapter\nbad\n-1\n1000\n2\n5\n0\n1\n5\n1\n6\n");
Equal(1000L, menuManager.GetScore(), "scripted menu score");
Equal(true, output.Contains("Level: 2"), "menu level output");
Equal(true, output.Contains("Explorer"), "menu title output");
Equal(true, output.Contains("[ ] Read"), "menu details output");
foreach (string input in new[] { "", "1\n", "1\n1\n", "1\n1\nName\n", "1\n1\nName\nDesc\n", "1\n3\nName\nDesc\n1\n", "1\n3\nName\nDesc\n1\n2\n", "3\n", "4\n", "5\n" })
{
    GoalManager eof = new GoalManager();
    eof.AddGoal(new EternalGoal("A", "B", 1));
    RunMenu(eof, input);
    Equal(1, eof.GetGoals().Count, "EOF exits without partial creation");
}
GoalManager allTypesMenu = new GoalManager();
output = RunMenu(allTypesMenu,
    $"1\n2\nDaily\nPractice\n2\n1\n3\nSteps\nWalk\n3\n0\n2\n-1\n4\n5\n2\n3\n{fixture}\n5\n2\n4\n{fixture}\n2\n6\n");
Equal(2, allTypesMenu.GetGoals().Count, "menu creates eternal and checklist");
Equal(3L, allTypesMenu.GetScore(), "menu load replaces later score");
Equal(true, output.Contains("1/2"), "menu checklist progress");
Equal(true, output.Contains("Goals saved."), "interactive save");
Equal(true, output.Contains("Goals loaded."), "interactive load");
output = RunMenu(allTypesMenu, "4\ntests/EternalQuestChecks/obj/no-such-file.txt\n6\n");
Equal(true, output.Contains("File error:"), "interactive missing file handled");
File.WriteAllText(fixture, "invalid");
output = RunMenu(allTypesMenu, $"4\n{fixture}\n6\n");
Equal(true, output.Contains("Invalid file:"), "interactive malformed file handled");
Equal(3L, allTypesMenu.GetScore(), "interactive failed load preserves score");
Console.WriteLine($"PASS: {checks} deterministic assertions");
