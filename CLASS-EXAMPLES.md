# How the classroom examples map to Member 1

This rewrite is based on the supplied C# source examples. The assignment still
controls the validation rules, file format, classification bands, and append behaviour.

| Class example | Pattern used here |
| --- | --- |
| Day3 FileHandler,Datahandler: Student.cs | Private fields, constructor assignments using `this`, properties exposing fields. See Animal.cs and ValidationError. |
| Day3 FileHandler,Datahandler: FileHandler.cs | Separate file handler, FileStream, StreamReader, StreamWriter, ReadLine and Split. See Services/FileManager.cs. |
| ReadWrite: Program.cs | A List<string> collects lines; a while loop reads and a foreach loop processes them. |
| AfternoonFileHandeling / Updated | Separate presentation and file responsibilities; build saved text by joining object properties. |
| StudentForm: Form1.cs | Keep a handler object on the form and use named button click handlers. |
| Employee Management: Form1.cs | Capture text box values into local variables; use try/catch around saving. |
| DataSetsAndDataTables / DVGandListView | Reviewed for the form/handler pattern. Their database and grid features belong to other members' scope. |

## Read the Add flow

1. MainForm captures the five text box values.
2. AnimalValidator.TryCreate checks them and creates an Animal only if they are valid.
3. The Animal constructor calls the single AnimalClassifier.Classify method.
4. FileManager.Read reads the current lines using FileStream and StreamReader.
5. TryAppend uses foreach and Split to reject an existing ID.
6. FileMode.Append and StreamWriter write one new record.
7. The form displays success, clears inputs, and raises AnimalAdded for the future grid.

## Small differences required by this project

- Use `|` between the seven fields because that is the project file format.
- Use `FileMode.Append` when adding. The lecture's `FileMode.Create` rewrites a whole
  list and would erase existing records if used to save just one animal.
- Keep `int.TryParse` and range checks: typing invalid numbers must show an error.
- Keep Animal properties read-only. Editing creates a new validated Animal so its
  recovery score, status, and housing cannot disagree. This preserves the shared API.
- Use `using` blocks to close streams even when a file operation fails.
- Keep data beside the executable so launching from a different folder still finds it.
- A small last-byte check handles files whose final line has no newline. The existing
  BOM-only, Unicode, duplicate, and file-error checks remain covered by tests.
- Feedback stays in the existing form labels and ErrorProvider. The examples use
  MessageBox in places; changing the message UI is not needed to follow their code structure.
- Keep the existing method names and AnimalAdded event so teammates can integrate
  without learning a different interface. The separate test runner is verification
  support; it is not the application code to study first.

No database is added. Member 1 still implements only Add Animal and its shared helpers.
