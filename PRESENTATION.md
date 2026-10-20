# Group presentation preparation

Use the latest main branch. This guide and the implementation were prepared with
AI assistance; disclose that assistance and follow the lecturer's permitted-use rules.
Each member should explain their own understanding rather than memorise an authorship claim.
Allow at least three minutes per member and time for the complete demonstration.

## What the application does

The centre records an animal's identity, species, age and recovery score. Staff can
add records, view the file, correct a selected animal, delete an animal after
confirmation, and produce a summary of the centre's current records. Recovery score
determines both status and housing, so those two outputs are never typed by the user.

## Member 1: Alexander — Add and validation

1. Show Animal.cs: private fields hold five inputs and two calculated outputs.
   Its constructor calls AnimalClassifier.Classify once, with two out parameters.
2. Show AnimalValidator.TryCreate: trim text, uppercase ID, check WR- plus four digits,
   enforce required text length and reject pipes/newlines, then TryParse whole numbers
   in the 0–100 range. Return false with a field-specific error before constructing an Animal.
3. Show MainForm.btnAdd_Click: capture input, validate, call TryAppend inside try/catch,
   show success/classification, clear inputs, and raise AnimalsChanged after saving.
4. Show FileManager.TryAppend: reread IDs, foreach/Split at pipes, reject duplicates,
   then FileMode.Append and StreamWriter.WriteLine one seven-field record.
5. Demonstrate WR-0101, Naledi, Serval, 3, 62. Try the same ID again and then score 62.5.
   Explain the duplicate/whole-number messages. Clear cancels unfinished input.

Know these bands: 0–19 Critical / Intensive Care Unit; 20–39 Serious /
High-Dependency Ward; 40–59 Stable / Recovery Ward; 60–79 Recovering /
Outdoor Enclosure; 80–100 Release-Ready / Pre-Release Camp.

## Member 2: Phomolo — View and loading

1. Explain MainForm.LoadAnimals and FileManager.TryLoadAll. Startup creates a missing
   empty file, reads lines, requires seven fields, validates inputs and counts skipped lines.
2. Show ConfigureAnimalsGrid: read-only, no new/delete rows, full row selection,
   one selected row, no sorting. Records remain in file order.
3. Explain AnimalsChanged is subscribed once and raised only after successful Add,
   Update or Delete. Search and invalid input do not change the file.
4. Demonstrate Add appearing immediately, edit changing the row, and delete removing it.
   Use a disposable copy to demonstrate malformed or missing data; back up real records first.

## Member 3: Max — Search, update and delete

1. Show MainForm.Member3.cs. Search and grid CellClick call FindAndLoadAnimal and
   populate the same inputs. A missing ID shows feedback without clearing unsaved fields.
2. Explain Edit Mode: ID read-only, Add and Search disabled, Save enabled. Clear
   cancels editing. Saving reuses validation/classification with the original ID.
3. Show UpdateDeleteService: find the matching line, replace/remove only that row,
   preserve other lines, write a temporary file and replace the original only after writing.
4. Demonstrate score 30 changing to 85, then show Release-Ready and Pre-Release Camp.
5. Demonstrate delete No (keep record) and Yes (remove). The dialog shows ID and name
   and defaults to No. No selection gives a helpful message.

## Member 4: Dean — Summary, repository and submission

1. Show SummaryReportService.Create and SummaryReport: ordinary foreach totals age
   and score, counts the five existing statuses, and divides only if count is positive.
2. Explain ToText: two decimal places, invariant culture, local generation timestamp,
   zero counts and N/A averages for an empty file.
3. Show btnSummaryReport_Click: reload current file, display the report in the read-only
   text box, then overwrite summary.txt beside the executable. Demonstrate regeneration.
4. Show eight starter records, README build instructions, branch/PR history and ignored files.
   Check all members' real names, numbers, contributions and submission access.

## Questions everyone should be able to answer

- Why one classifier? Add and Update must use identical bands and keep status/housing consistent.
- Why pipe-separated text? It is the brief's storage format; one animal occupies one line.
- Why reject a pipe or newline in a name? Either would split a valid record into extra fields/lines.
- Why TryParse? Invalid numbers produce validation feedback instead of a conversion crash.
- Why a read-only ID during edit? An update must target the original animal, not create another identity.
- Why read from disk before saving/reporting? In-memory data may be stale; uniqueness and reports need current records.
- Why AppContext.BaseDirectory? Data is beside the executable on any computer, without a C:\Users path.
- Why using? Streams close even if reading or writing throws.
- What happens if the file is locked? A friendly error appears; typed input remains available for retry.
- Why a temporary rewrite? Avoid replacing the original until the new complete contents have been written.
- What is a pull request? A proposed branch change reviewed and merged into the shared main branch.
- What remains a limitation? One running application instance; no coordinated concurrent editing.

## Final rehearsal

Use the clean starter data or a backup copy. Demonstrate all five features in order,
read the saved seven-field record, inspect the generated summary, and build from a
fresh clone. Do not delete application classes, the starter file, or teammate code to
make the repository look smaller. Keep bin/obj/.vs and generated summary out of the ZIP.
