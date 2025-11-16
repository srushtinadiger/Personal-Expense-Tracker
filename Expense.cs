using System;

namespace PersonalExpenseTracker
{
    public class Expense
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public string Category { get; set; }
        public DateTime Date { get; set; }

        public Expense()
        {
            Date = DateTime.Now;
        }

        public Expense(int id, string description, decimal amount, string category, DateTime date)
        {
            Id = id;
            Description = description;
            Amount = amount;
            Category = category;
            Date = date;
        }

        public override string ToString()
        {
            return $"{Id}. {Description} - ${Amount:F2} ({Category}) - {Date:yyyy-MM-dd}";
        }
    }
}

