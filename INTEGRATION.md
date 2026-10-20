# Integrated application

All four feature branches have been merged into the shared application. The final
integration fixes connect their workflows without adding a second form or Animal class.

## Shared refresh

MainForm subscribes once to `AnimalsChanged` through `ReloadAnimals`. A successful
Add, Update or Delete raises it after the file operation finishes. The loader clears
and repopulates the read-only grid in file order. `AnimalAdded` remains as an Add-only
compatibility event; the grid does not subscribe to both events.

## Edit and delete

A grid CellClick and the Search button both call `FindAndLoadAnimal`. It rereads the
record and fills the same five input boxes. The original ID and name are retained
for update and delete confirmation. Edit Mode locks ID and disables Add and Search;
Save Changes validates with the original ID. Clear cancels editing. A failed search
only shows an error; it does not clear unsaved fields. Delete without a loaded animal
asks for selection. Confirmation shows ID and name, with No as the default.

## File and report

Startup creates a missing empty animals.txt. FileManager.TryLoadAll validates seven
fields, skips malformed lines and returns their count. Both View and Summary use
that loader. Status and housing are recalculated by the one shared classifier.
The report uses all five statuses, a timestamp, invariant two-decimal averages and
N/A for empty data. Its read-only text box stays visible after saving summary.txt.

UpdateDeleteService retains the member's original read/list/rewrite approach. It
preserves unrelated lines and uses a temporary file before replacing animals.txt.
All operations use the same file beside the executable, without absolute machine paths.

## Review

Run the regression project listed in README.md and demonstrate the real confirmation
dialog manually. Test data is disposable and separate from runtime records. Refer to
README.md for the assistance disclosure and outstanding student/Git requirements.
