using Dollet.Core.Entities;

namespace Dollet.Core.DTOs
{
    public class ExpensesDetailsGroupDto(DateTime date, List<Expense> expenses) : List<Expense>(expenses)
    {
        public DateTime Date { get; private set; } = date;
    }
}