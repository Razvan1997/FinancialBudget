using Dollet.Core.Entities;

namespace Dollet.Core.DTOs
{
    public class IncomesDetailsGroupDto(DateTime date, List<Income> incomes) : List<Income>(incomes)
    {
        public DateTime Date { get; private set; } = date;
    }
}