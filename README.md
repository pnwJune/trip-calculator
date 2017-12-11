# trip-calculator
A sample C# .NET project for iTrellis. Calculates shares of total trip expenses for a group of people

## To run in debugger:
1. Open the solution in Visual Studio
2. If first time opening the solution, Perform a NuGet Package Restore 

	Right click on Solution in solution explorer and select __Restore NuGet Packages__

3. Click Start to begin debugging (or hit F5).

## To run unit tests:
1. Open the solution in Visual Studio
2. 2. If first time opening the solution, Perform a NuGet Package Restore 

	Right click on Solution in solution explorer and select __Restore NuGet Packages__

3. Open Test Explorer in Visual Studio

	Go to the top toolbar in VS and select Test > Windows > Test Explorer

4. Build the solution

	Go to the top toolbar in VS and select Build > Build Solution, or hit F6 on the keyboard

5. Select Run All in the test explorer pane in the visual studio.

## Project Details - 12/10/2017

For this project, I used the **"Trip Calculator"** domain and the **"Native GUI"** interface. I used Unity and MVVM to create a loosely-coupled UI to make ease of unit-testing the viewmodel for the main window dialog. 

I think I have room for a lot of improvement in the application, specifically in the UI design (it's a bit clunky to add lots of expenses) and in the functionality (I would love to add saving/loading, for example).

I completed the project to this state in about 6 hours, which I believe showcases my proficiency with MVVM and application development using Visual Studio and C#, as well as my familiarity with test-driven development, since all the core functions of the data models and viewmodels have corresponding unit tests which verify their behavior.

Here are the release notes for the current version 1.0.0 for 12/10/2017:

* CALC-1: users should be able to create a trip with travelers
* CALC-2: users should be able to add expenses
* CALC-4: users should be able to end a trip
* CALC-8: trip should be able to calculate the current division of expenses
* CALC-10: application UI should output as 0.00 format
* CALC-11: mainviewmodel should have unit tests