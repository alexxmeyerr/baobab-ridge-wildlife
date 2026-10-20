# Animal Rehabilitation Management System

## Project Description

Manages animal rehabilitation records.

Allows users to add, view, update, and delete animal information.

Generates summary reports for rehabilitation statistics.

## Features

- Add Animal
- View Animals
- Update Animal Records
- Delete Animal Records
- Generate Summary Reports
- Save and Load Data from Text Files

## Technologies Used

- C#
- Windows Forms
- .NET 8
- Text File Storage

## Team Members

- Member 1: Add Animal & Validation
- Member 2: View Animals
- Member 3: Update & Delete Animals
- Member 4: Summary Reports, README, GitHub Management

## Files Used

- `Animal.cs`
- `MainForm.cs`
- `animals.txt`
- `summary.txt`
- `Services/SummaryReportService.cs`

## How to Run

1. Open the project in Visual Studio.
2. Build the solution.
3. Run the application.
4. Use the buttons to manage animal records.
5. Select **Generate Summary Report** to display the rehabilitation statistics and save them to `summary.txt`.

## Summary Report Includes

- Total Number of Animals
- Average Age
- Average Recovery Score
- Number of Animals per Status

The report is formatted as follows:

```text
===== Animal Rehabilitation Summary =====

Total Animals: 10
Average Age: 4.3 Years
Average Recovery Score: 71.5%
Ready for Release: 4
Needs Monitoring: 3
Critical Care: 3
```

The `summary.txt` file is saved beside `animals.txt` and is overwritten each time a new report is generated.

## GitHub Workflow

- Separate branches used for each feature.
- Pull requests used before merging.
- Features merged into the main branch after testing.

## Authors

- [Student Name 1]
- [Student Name 2]
- [Student Name 3]
- [Student Name 4]
