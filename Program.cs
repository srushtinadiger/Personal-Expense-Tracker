using System;
using System.Collections.Generic;
using System.Globalization;

namespace PersonalExpenseTracker
{
    class Program
    {
        private static ExpenseManager expenseManager = new ExpenseManager();

        static void Main(string[] args)
        {
            Console.WriteLine("=== Personal Expense Tracker ===\n");
            
            bool running = true;
            while (running)
            {
                ShowMenu();
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddExpense();
                        break;
                    case "2":
                        ViewAllExpenses();
                        break;
                    case "3":
                        EditExpense();
                        break;
                    case "4":
                        DeleteExpense();
                        break;
                    case "5":
                        ViewSummary();
                        break;
                    case "6":
                        ViewExpensesByCategory();
                        break;
                    case "7":
                        ViewExpensesByDateRange();
                        break;
                    case "8":
                        running = false;
                        Console.WriteLine("Thank you for using Personal Expense Tracker!");
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }

                if (running)
                {
                    Console.WriteLine("\nPress any key to continue...");
                    Console.ReadKey();
                    Console.Clear();
                }
            }
        }

        static void ShowMenu()
        {
            Console.WriteLine("Main Menu:");
            Console.WriteLine("1. Add Expense");
            Console.WriteLine("2. View All Expenses");
            Console.WriteLine("3. Edit Expense");
            Console.WriteLine("4. Delete Expense");
            Console.WriteLine("5. View Summary");
            Console.WriteLine("6. View Expenses by Category");
            Console.WriteLine("7. View Expenses by Date Range");
            Console.WriteLine("8. Exit");
            Console.Write("\nEnter your choice: ");
        }

        static void AddExpense()
        {
            Console.WriteLine("\n--- Add New Expense ---");
            
            Console.Write("Description: ");
            string description = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(description))
            {
                Console.WriteLine("Description cannot be empty.");
                return;
            }

            Console.Write("Amount: $");
            if (!decimal.TryParse(Console.ReadLine(), out decimal amount) || amount <= 0)
            {
                Console.WriteLine("Invalid amount. Please enter a positive number.");
                return;
            }

            Console.Write("Category: ");
            string category = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(category))
            {
                category = "Uncategorized";
            }

            Console.Write("Date (yyyy-mm-dd) or press Enter for today: ");
            string dateInput = Console.ReadLine();
            DateTime date;

            if (string.IsNullOrWhiteSpace(dateInput))
            {
                date = DateTime.Now;
            }
            else if (!DateTime.TryParseExact(dateInput, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
            {
                Console.WriteLine("Invalid date format. Using today's date.");
                date = DateTime.Now;
            }

            Expense expense = new Expense(0, description, amount, category, date);
            expenseManager.AddExpense(expense);
            Console.WriteLine("\nExpense added successfully!");
        }

        static void ViewAllExpenses()
        {
            Console.WriteLine("\n--- All Expenses ---");
            var expenses = expenseManager.GetAllExpenses();

            if (expenses.Count == 0)
            {
                Console.WriteLine("No expenses found.");
                return;
            }

            Console.WriteLine($"\nTotal Expenses: {expenses.Count}\n");
            foreach (var expense in expenses)
            {
                Console.WriteLine(expense);
            }

            Console.WriteLine($"\nTotal Amount: ${expenseManager.GetTotalExpenses():F2}");
        }

        static void EditExpense()
        {
            Console.WriteLine("\n--- Edit Expense ---");
            ViewAllExpenses();

            Console.Write("\nEnter Expense ID to edit: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Invalid ID.");
                return;
            }

            var expense = expenseManager.GetExpenseById(id);
            if (expense == null)
            {
                Console.WriteLine("Expense not found.");
                return;
            }

            Console.WriteLine($"\nCurrent Expense: {expense}\n");

            Console.Write("New Description (or press Enter to keep current): ");
            string description = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(description))
            {
                description = expense.Description;
            }

            Console.Write("New Amount (or press Enter to keep current): ");
            string amountInput = Console.ReadLine();
            decimal amount;
            if (string.IsNullOrWhiteSpace(amountInput))
            {
                amount = expense.Amount;
            }
            else if (!decimal.TryParse(amountInput, out amount) || amount <= 0)
            {
                Console.WriteLine("Invalid amount. Keeping current amount.");
                amount = expense.Amount;
            }

            Console.Write("New Category (or press Enter to keep current): ");
            string category = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(category))
            {
                category = expense.Category;
            }

            Console.Write("New Date (yyyy-mm-dd) or press Enter to keep current: ");
            string dateInput = Console.ReadLine();
            DateTime date;
            if (string.IsNullOrWhiteSpace(dateInput))
            {
                date = expense.Date;
            }
            else if (!DateTime.TryParseExact(dateInput, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
            {
                Console.WriteLine("Invalid date format. Keeping current date.");
                date = expense.Date;
            }

            if (expenseManager.UpdateExpense(id, description, amount, category, date))
            {
                Console.WriteLine("\nExpense updated successfully!");
            }
            else
            {
                Console.WriteLine("\nFailed to update expense.");
            }
        }

        static void DeleteExpense()
        {
            Console.WriteLine("\n--- Delete Expense ---");
            ViewAllExpenses();

            Console.Write("\nEnter Expense ID to delete: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Invalid ID.");
                return;
            }

            var expense = expenseManager.GetExpenseById(id);
            if (expense == null)
            {
                Console.WriteLine("Expense not found.");
                return;
            }

            Console.WriteLine($"\nExpense to delete: {expense}");
            Console.Write("Are you sure you want to delete this expense? (y/n): ");
            string confirm = Console.ReadLine();

            if (confirm?.ToLower() == "y")
            {
                if (expenseManager.DeleteExpense(id))
                {
                    Console.WriteLine("\nExpense deleted successfully!");
                }
                else
                {
                    Console.WriteLine("\nFailed to delete expense.");
                }
            }
            else
            {
                Console.WriteLine("Deletion cancelled.");
            }
        }

        static void ViewSummary()
        {
            Console.WriteLine("\n--- Expense Summary ---");
            
            var totalExpenses = expenseManager.GetTotalExpenses();
            var categorySummary = expenseManager.GetExpensesByCategorySummary();

            Console.WriteLine($"\nTotal Expenses: ${totalExpenses:F2}");
            Console.WriteLine($"Number of Expenses: {expenseManager.GetAllExpenses().Count}");

            if (categorySummary.Count > 0)
            {
                Console.WriteLine("\nExpenses by Category:");
                foreach (var category in categorySummary)
                {
                    decimal percentage = totalExpenses > 0 ? (category.Value / totalExpenses) * 100 : 0;
                    Console.WriteLine($"  {category.Key}: ${category.Value:F2} ({percentage:F1}%)");
                }
            }
        }

        static void ViewExpensesByCategory()
        {
            Console.WriteLine("\n--- Expenses by Category ---");
            
            var categories = expenseManager.GetAllCategories();
            if (categories.Count == 0)
            {
                Console.WriteLine("No categories found.");
                return;
            }

            Console.WriteLine("\nAvailable Categories:");
            for (int i = 0; i < categories.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {categories[i]}");
            }

            Console.Write("\nEnter category name or number: ");
            string input = Console.ReadLine();

            string category;
            if (int.TryParse(input, out int index) && index > 0 && index <= categories.Count)
            {
                category = categories[index - 1];
            }
            else
            {
                category = input;
            }

            var expenses = expenseManager.GetExpensesByCategory(category);
            
            if (expenses.Count == 0)
            {
                Console.WriteLine($"\nNo expenses found for category: {category}");
                return;
            }

            Console.WriteLine($"\nExpenses in '{category}':");
            foreach (var expense in expenses)
            {
                Console.WriteLine(expense);
            }

            Console.WriteLine($"\nTotal for '{category}': ${expenseManager.GetTotalExpensesByCategory(category):F2}");
        }

        static void ViewExpensesByDateRange()
        {
            Console.WriteLine("\n--- Expenses by Date Range ---");

            Console.Write("Start Date (yyyy-mm-dd): ");
            string startInput = Console.ReadLine();
            if (!DateTime.TryParseExact(startInput, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime startDate))
            {
                Console.WriteLine("Invalid start date format.");
                return;
            }

            Console.Write("End Date (yyyy-mm-dd): ");
            string endInput = Console.ReadLine();
            if (!DateTime.TryParseExact(endInput, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime endDate))
            {
                Console.WriteLine("Invalid end date format.");
                return;
            }

            if (startDate > endDate)
            {
                Console.WriteLine("Start date cannot be after end date.");
                return;
            }

            var expenses = expenseManager.GetExpensesByDateRange(startDate, endDate);

            if (expenses.Count == 0)
            {
                Console.WriteLine($"\nNo expenses found between {startDate:yyyy-MM-dd} and {endDate:yyyy-MM-dd}");
                return;
            }

            Console.WriteLine($"\nExpenses from {startDate:yyyy-MM-dd} to {endDate:yyyy-MM-dd}:");
            foreach (var expense in expenses)
            {
                Console.WriteLine(expense);
            }

            decimal total = expenses.Sum(e => e.Amount);
            Console.WriteLine($"\nTotal: ${total:F2}");
        }
    }
}

