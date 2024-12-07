namespace Dollet.Core.ExtensionMethods
{
    public static class StringExtensions
    {
        public static string ToFirstUpper(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            return char.ToUpper(input[0]) + input[1..].ToLower();
        }
    }
}
