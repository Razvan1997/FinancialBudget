namespace Dollet.Core.DTOs
{
    public class IncomesGroupDto
    {
        public string Category { get; init; }
        public int? CategoryId { get; init; }
        public decimal Amount { get; init; }
        public decimal Percent { get; init; }
        public string Icon { get; init; }
        public string Color { get; init; }
    }
}
