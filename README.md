Spreadsheet-Application
=======================

A Simple Spreadsheet Application using .NET supporting formula evaluation , text/number cells etc.

Spreadsheet Application
-A Simple sheet App supporting formulas.
_______________________________________________________________________
Used Language: C#
Used GUI: Windows Form
Used IDE: Visual Studio 2012
 

There is a file name ClassDiagram.cd in the Solution which can be opened in visual studio where you can dive in more details.
Running & Building the Project only needs Visual Studio >=2012 nothing more. 



If Had more time ?
To be Honest I Spent a bit more than 4 hours working on the project (most of the time I had problems with the GUI which wasted my Time, I am not into GUI’s but I have applied for a backend Software Engineer anyway :D ).
I had more free time I would extensively test it on an automatic test case generator.
Also the Code was too big for me to make unit testing on every single method in the Project for the time I had last 2 days (studying Mid-Semester Exam’s).
I have already represented the Graph Dependencies in a very good way as in my App Updating Dependent Formula’s in the worst case will be in the Order of Number of Formula’s which exist (the best we can do actually).
Although It works fine, I would have represented it in a separate datastructure instead of using the formula’s themselves as nodes.

I would make the Sheet App support allow multiple sheets in same process.

I think I came out with a good design which I made for mainly for extensionality not for modification if you want to add another Cell type other than text or floating numbers or formulas you can easy inherit from the Cell class, as all Cell types Classes are self evaluating and generalized.

I would have solved coupling between the sheet and the Cells but my time was short and I was in a hurry for a working solution.

I had very short time also so I would have used better coding habits for more readable and cleaner code.
_______________________________________________
Design:(Take a look on ClassDiagram.cs)
Sheet: 
-Mainly a 2D Cells Matrix
- Expand() when we reach near limits (double allocated size)
- insertCell(Cell, Address) 
adds a new cell after determing it’s type using a parser
- refreshSheet()
This is very Important function which called after changing a Cell which depends on the Dependency Graph on the formula where we model the formula as a directed Graph with directed nodes, where we traverse on the dependencies according to the changed value and marking the formula(nodes) as visited as we traverse.
Also we will handle a case which is a Dependency Cycle which is an invalid sets of formula’s and should output an error  which happen when the starting nodes while traversing returns to itself which means that it depend on itself (while it is a cell so it’s impossible).
-	Other method’s are self explanatory






Cell: has type show() for GUI output and evaluate() which overrides  depending on type (instantly returning value or evaluating in case of formula)
Cell has NumberCell , TextCell , FormulaCell and Error Cell Children.
The first two are obvious the third is the for the formula which their EvaluateFunction() will evaluate formula’s in a recursive manner.
ErrorCell is generated for Cell Errors which has Dependency Loop Error (see above) , Invalid Type (using text in a number field) and a general Error.
Expressions:
Are the Expressions of the formula either an Address or literal constant or an operator (infix operators and brackets only here).

Running The App:
Simply Enter your Number/Equation in the Form (Obvious).
The GUI is a bit buggy and sometimes it doesn’t update the New Updated Cells from formula’s.
Testing:

1st  Parsing & Evaluation Function: 
Assuming we have Cell B1,C1,D1,E1 with values 5,-3,12,0 resp.
Sample Input:
Simple Case
1-“=B1*C1+D1”  -3

Sorry for the inconvenience but I ran out of time before completing & documenting the Testing Phase, I also thought that I had an extra hour but I figured out the Summer Time zone issue in the U.S, I assure the correctness of the design and the logic and the dependency graph and so on.
Sorry Again for the inconvenience but my time was very strict preparing for my Mid Semester Exams, If I had an hour or two I would have assured covering all the test cases; I hope you accept my apology and appreciate my severe effort. 



