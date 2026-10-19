# Member 1 + Member 3 integration

Member 3 (Max20031009) supplied the Search, Save Changes, Delete, confirmation, and file-rewrite workflow in PR #3. This integration adapts that work to the existing MainForm and shared Animal class. It replaces the old Form1 partial class and separate editing fields; it does not add a second startup form.

## Try the combined features

1. Open BaobabRidge.csproj in Visual Studio and run it.
2. Add WR-0001, Naledi, Serval, age 3, recovery score 15.
3. Enter WR-0001 in Animal ID and click Search.
4. The record loads into the same fields. ID becomes read-only, Add and Search are disabled, and Save Changes and Delete are enabled.
5. Try score 101: validation prevents saving. Change it to 85 and Save Changes: the existing record stays in place, its status becomes Release-Ready and housing becomes Pre-Release Camp.
6. Search again. Clear cancels editing without changing the file.
7. Search again and Delete. No leaves the record intact; Yes removes it. Successful update/delete clears the form and returns to Add mode.

All three operations use animals.txt beside the running executable. Data/animals.txt is the starter source copied by the project. Update/Delete no longer use a separate runtime Data/animals.txt file.

## Member 2: connect the grid

Use the existing MainForm partial class and one grid. Subscribe once to AnimalsChanged after initializing the grid:

```csharp
AnimalsChanged += (sender, e) => LoadAnimals();
```

Replace LoadAnimals with your actual grid reload method. Call it once on startup as well. The event fires after a successful Add, Update, or Delete, after the streams close. Invalid input, cancelled deletion, search, Clear, and failed writes do not raise it. Do not also subscribe to AnimalAdded for the same refresh; that older event remains for compatibility and fires only on Add.

UpdateDeleteService.GetAllAnimals returns valid Animal objects in file order. It skips malformed rows; Member 2 still needs to implement the required skipped-record reporting and read-only DataGridView. Use FileManager.AnimalsPath for the same data location.

## Shared rules and file handling

Updates call AnimalValidator.TryCreate, which constructs Animal and calls the single AnimalClassifier.Classify method. The edit operation retains the original ID. Status and housing are always calculated from the recovery score, including when loading a record; saving corrects any stale derived fields.

The service reads strings into a List using FileStream and StreamReader, then updates or removes the matching row. Other lines, including unrelated malformed lines and blanks, remain in their original order. Rewrites use UTF-8 and normalize line endings. Duplicate target IDs and malformed target records are rejected rather than guessing which row to change.

Rewrites finish a temporary file in the same folder before replacing animals.txt. Failed writes retain input for retry and do not raise the refresh event. The application is intended for one running instance, as before; simultaneous editing from multiple processes is not supported.

## Verification

Run `dotnet run --project Tests/BaobabRidge.Tests.csproj -c Release` on Windows. The dependency-free test runner exercises the original Add tests plus shared-file search, edit-mode controls, validation, recalculation, ordering, malformed/duplicate records, missing files, locked/read-only failures, clear/cancel, confirmed deletion, final-record deletion, and refresh events. Temporary test records are created beneath the test output directory and removed afterward. The test subclass supplies Yes/No answers; the production form displays a confirmation dialog with No selected by default.

Before the final group submission, integrate Member 2's grid and Member 4's report/starter data, then test the complete application together.
