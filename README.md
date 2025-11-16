# Personal-Expense-Tracker
A console-based Personal Expense Tracker application built with C# that helps you manage and track your daily expenses.

## Features

- ✅ **Add Expenses**: Record new expenses with description, amount, category, and date
- ✅ **View All Expenses**: Display all expenses sorted by date (newest first)
- ✅ **Edit Expenses**: Update existing expense details
- ✅ **Delete Expenses**: Remove expenses with confirmation
- ✅ **View Summary**: See total expenses and breakdown by category
- ✅ **Filter by Category**: View expenses for a specific category
- ✅ **Filter by Date Range**: View expenses within a specific date range
- ✅ **Data Persistence**: All expenses are saved to a JSON file automatically

## Requirements

- .NET 8.0 SDK or later

## How to Run

1. Open a terminal in the project directory
2. Run the following commands:

```bash
dotnet restore
dotnet run
```

## Usage

The application provides a menu-driven interface:

1. **Add Expense**: Enter description, amount, category, and date
2. **View All Expenses**: See all your expenses with totals
3. **Edit Expense**: Modify an existing expense by ID
4. **Delete Expense**: Remove an expense by ID (with confirmation)
5. **View Summary**: See total expenses and category breakdown
6. **View Expenses by Category**: Filter expenses by category
7. **View Expenses by Date Range**: Filter expenses by date range
8. **Exit**: Close the application

## Data Storage

Expenses are automatically saved to `expenses.json` in the same directory as the application. The file is created automatically when you add your first expense.

## Example Usage

```
=== Personal Expense Tracker ===

Main Menu:
1. Add Expense
2. View All Expenses
3. Edit Expense
4. Delete Expense
5. View Summary
6. View Expenses by Category
7. View Expenses by Date Range
8. Exit

Enter your choice: 1

--- Add New Expense ---
Description: Groceries
Amount: $45.50
Category: Food
Date (yyyy-mm-dd) or press Enter for today: 2024-01-15

Expense added successfully!
```

## Project Structure

- `Program.cs`: Main entry point and user interface
- `Expense.cs`: Expense model class
- `ExpenseManager.cs`: Business logic and data persistence
- `PersonalExpenseTracker.csproj`: Project configuration file

## Notes

- Dates should be entered in `yyyy-mm-dd` format
- Amounts must be positive numbers
- Categories are case-insensitive
- All data is persisted automatically after each operation

