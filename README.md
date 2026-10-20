# Baobab Ridge Wildlife Rehabilitation Records System

## Group members

| Name | Student number |
| --- | --- |
| Alexander Meyer | 604082 |
| Golokile Phomolo Seleke | 605189 |
| Willem Abraham Jacobus Kruger | 600221 |
| Dean Martin Hendrikz | 603341 |

## How to build and run

You need a Windows computer, Visual Studio with **.NET desktop development** installed, and the .NET 8 SDK.

1. Download and unzip the project, or clone the GitHub repository.
2. Open `BaobabRidge.sln` in Visual Studio.
3. Select **Build > Build Solution**. Allow Visual Studio to restore any required files if asked.
4. Press **F5** or click the green **Start** button to run the program.

The project includes eight starter animal records. The build copies `animals.txt` into the same folder as the program.

## Features

- **Add an animal:** Enter the animal's ID, name, species, age and recovery score. The program checks the details and prevents duplicate IDs. It works out the status and housing unit from the recovery score.
- **View animals:** See the saved animals in a table when the program opens. Invalid records are skipped and counted.
- **Update an animal:** Search by ID or select an animal in the table. Change its details and save. Its ID stays the same, and its status and housing unit are updated when needed.
- **Delete an animal:** Select an animal and confirm before deleting it.
- **Summary report:** View the total number of animals, average age, average recovery score and number of animals in each status. Save these results in `summary.txt`.

## File formats

Both files are plain text files saved beside the running program.

**animals.txt** stores one animal on each line. There is no heading row. The details are separated by `|` in this order: Animal ID, Name, Species, Age, Recovery Score, Status and Housing Unit.

Example:

```text
WR-0002|Naledi|Serval|3|62|Recovering|Outdoor Enclosure
```

**summary.txt** stores the report heading, date and time, total animals, average age, average recovery score and the count for each of the five statuses. Averages have two decimal places. If there are no animals, the counts are zero and the averages show `N/A`. Each new report replaces the previous report.

## Known limitations

- The program runs on Windows.
- Use only one copy of the program at a time to avoid changes to the same file at the same time.
- Use the form to change animal details. The table cannot be edited directly.
- On a small screen or with large display scaling, parts of the form may not fit on the screen.
- Back up your saved animals before installing a new build. A newer starter file may replace the existing `animals.txt` during a build.
