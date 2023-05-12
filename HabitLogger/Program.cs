

using HabitLogger;
using System.Data.SQLite;
using System.Linq.Expressions;


void Prompt()
{
    //initial menu: prompts for a path to choose and goes to different functions based on the number entered
    Console.WriteLine("********************");
    Console.WriteLine("Enter a number:");
    Console.WriteLine("(1) Add Habit");
    Console.WriteLine("(2) Update Habit");
    Console.WriteLine("(3) View Habits");
    Console.WriteLine("(4) Delete Habit");
    Console.WriteLine("(0) Exit Application");
    Console.WriteLine("********************");
    int choice = int.Parse(Console.ReadLine());
    switch (choice)
    {
        case 1:
            NewHabit();
            break;
        case 2:
            UpdateHabit();
            break;
        case 3:
            ViewHabits();
            break;
        case 4:
            DeleteHabit();
            break;
        case 0:
            Environment.Exit(0);
            break;
        default:
            Console.WriteLine("Not a valid number! Try again."); Prompt();
            break;
    }

}



/*NewHabit function creates a new HabitModel object, prompts for user to enter name of the habit,
 * the units for the habit, and their current progress on that habit. Returns to main menu after adding the habit to an SQLite database
 */
void NewHabit()
{
    HabitModel habit = new HabitModel();

    Console.WriteLine("Enter a name for your new habit:");
    string habitName = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(habitName))
    {
        Console.WriteLine("You have to enter something! Try again!");
        Console.WriteLine("");
        NewHabit();
    }
    else
        habit.HabitName= habitName;
    Console.WriteLine("Enter units to track your new habit (miles,lbs,oz,etc):");
    string units = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(units)){
        Console.WriteLine("You have to enter something! Try again!");
        Console.WriteLine("");
        NewHabit();
    }
    else
        habit.Units = units;
    Console.WriteLine($"Enter a quantity of {habit.Units} completed so far:");
    string quantity = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(quantity)){
        Console.WriteLine("You have to enter something! Try again!");
        Console.WriteLine("");
        NewHabit();
    }
    else
        habit.Quantity = int.Parse(quantity);

    SqliteDataAccess.SaveHabit(habit);

    Console.WriteLine("Habit has been saved! Returning to main menu...");
    Console.WriteLine("");

    Prompt();

}

void UpdateHabit()
{
    List<HabitModel> list = SqliteDataAccess.LoadHabits();

    foreach (HabitModel habit in list)
    {
        Console.WriteLine($"({habit.Id}) {habit.HabitName} : {habit.Quantity} {habit.Units}");
    }
    Console.WriteLine("");
    Console.WriteLine("Enter the ID for the habit you would like to update:");
    string input = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(input))
    {
        Console.WriteLine("Invalid ID entered! Try again!");
        Console.WriteLine("");
        UpdateHabit();
    }
    int choice = int.Parse(input) ;
    HabitModel oldHabit = list.Find(x=>x.Id == choice);
    if (oldHabit == null) {
        Console.WriteLine("Invalid ID entered! Try again!");
        Console.WriteLine("");
        UpdateHabit();
    }

    Console.WriteLine("What would you like to update");
    Console.WriteLine("(1) Habit progress");
    Console.WriteLine("(2) Habit name");
    Console.WriteLine("(3) Habit units");
    Console.WriteLine("(4) Whole habit");
    input = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(input))
    {
        Console.WriteLine("Invalid ID entered! Try again!");
        Console.WriteLine("");
        UpdateHabit();
    }
    choice = int.Parse(input);

    switch (choice)
    {
        case 1:
            Console.WriteLine("Enter updated progress:");
            oldHabit.Quantity = int.Parse(Console.ReadLine());
            break;
        case 2:
            Console.WriteLine("Enter updated name:");
            oldHabit.HabitName = Console.ReadLine();
            break;
        case 3:
            Console.WriteLine("Enter updated units:");
            oldHabit.Units = Console.ReadLine();
            break;
        case 4:
            Console.WriteLine("Enter updated name:");
            oldHabit.HabitName = Console.ReadLine();
            Console.WriteLine("Enter updated progress:");
            oldHabit.Quantity=int.Parse(Console.ReadLine());
            Console.WriteLine("Enter updated units:");
            oldHabit.Units = Console.ReadLine();
            
            break;
        default:
            Console.WriteLine("Not a valid number! Try again."); UpdateHabit();
            break;
    }
    SqliteDataAccess.UpdateHabit(oldHabit);
    Console.WriteLine($"{oldHabit.HabitName} has been updated! Returning to main menu...");
    Prompt();


}

/*ViewHabits function makes a list of habitmodel objects from the SQLite database, prints the name and current progress
for each habit, then returns to the main menu.
 */
void ViewHabits()
{
    List<HabitModel> list = SqliteDataAccess.LoadHabits();

    foreach(HabitModel habit in list)
    {
        Console.WriteLine($"Name: {habit.HabitName} ||| Current Progress: {habit.Quantity} {habit.Units}");
    }
    Console.WriteLine("");

    Prompt();
}

void DeleteHabit()
{
    List<HabitModel> list = SqliteDataAccess.LoadHabits();

    foreach (HabitModel habit in list)
    {
        Console.WriteLine($"({habit.Id}) {habit.HabitName} : {habit.Quantity} {habit.Units}");
    }
    Console.WriteLine("");
    Console.WriteLine("Enter the ID for the habit you want to delete:");
    int habitID = int.Parse(Console.ReadLine());

    HabitModel habitToDelete = list.Find(x => x.Id == habitID);
    Console.WriteLine("Are you sure you want to delete the habit? Yes/No:");
    string answer = Console.ReadLine().ToLower();
    switch (answer)
    {
        case "no":
            Prompt();
            break;
        case "yes":
            SqliteDataAccess.DeleteHabit(habitToDelete);
            break;
        default:
            Console.WriteLine("Invalid entry! Try again!");
            DeleteHabit();
            break;
    }

    Console.WriteLine($"The {habitToDelete.HabitName} habit has been deleted from the database. Returning to main menu...");
    Console.WriteLine("");
    Prompt();
}

Prompt();