using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace PersonalExpenseTracker
{
    public class ExpenseManager
    {
        private List<Expense> expenses;
        private string dataFilePath;

        public ExpenseManager(string dataFilePath = "expenses.json")
        {
            this.dataFilePath = dataFilePath;
            expenses = new List<Expense>();
            LoadExpenses();
        }

        public void AddExpense(Expense expense)
        {
            if (expenses.Count == 0)
            {
                expense.Id = 1;
            }
            else
            {
                expense.Id = expenses.Max(e => e.Id) + 1;
            }
            expenses.Add(expense);
            SaveExpenses();
        }

        public List<Expense> GetAllExpenses()
        {
            return expenses.OrderByDescending(e => e.Date).ToList();
        }

        public Expense GetExpenseById(int id)
        {
            return expenses.FirstOrDefault(e => e.Id == id);
        }

        public bool UpdateExpense(int id, string description, decimal amount, string category, DateTime date)
        {
            var expense = GetExpenseById(id);
            if (expense != null)
            {
                expense.Description = description;
                expense.Amount = amount;
                expense.Category = category;
                expense.Date = date;
                SaveExpenses();
                return true;
            }
            return false;
        }

        public bool DeleteExpense(int id)
        {
            var expense = GetExpenseById(id);
            if (expense != null)
            {
                expenses.Remove(expense);
                SaveExpenses();
                return true;
            }
            return false;
        }

        public List<Expense> GetExpensesByCategory(string category)
        {
            return expenses.Where(e => e.Category.Equals(category, StringComparison.OrdinalIgnoreCase))
                          .OrderByDescending(e => e.Date)
                          .ToList();
        }

        public List<Expense> GetExpensesByDateRange(DateTime startDate, DateTime endDate)
        {
            return expenses.Where(e => e.Date >= startDate && e.Date <= endDate)
                          .OrderByDescending(e => e.Date)
                          .ToList();
        }

        public decimal GetTotalExpenses()
        {
            return expenses.Sum(e => e.Amount);
        }

        public decimal GetTotalExpensesByCategory(string category)
        {
            return expenses.Where(e => e.Category.Equals(category, StringComparison.OrdinalIgnoreCase))
                          .Sum(e => e.Amount);
        }

        public Dictionary<string, decimal> GetExpensesByCategorySummary()
        {
            return expenses.GroupBy(e => e.Category)
                          .ToDictionary(g => g.Key, g => g.Sum(e => e.Amount));
        }

        public List<string> GetAllCategories()
        {
            return expenses.Select(e => e.Category)
                          .Distinct()
                          .OrderBy(c => c)
                          .ToList();
        }

        private void SaveExpenses()
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                var json = JsonSerializer.Serialize(expenses, options);
                File.WriteAllText(dataFilePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving expenses: {ex.Message}");
            }
        }

        private void LoadExpenses()
        {
            try
            {
                if (File.Exists(dataFilePath))
                {
                    var json = File.ReadAllText(dataFilePath);
                    expenses = JsonSerializer.Deserialize<List<Expense>>(json) ?? new List<Expense>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading expenses: {ex.Message}");
                expenses = new List<Expense>();
            }
        }
    }
}

