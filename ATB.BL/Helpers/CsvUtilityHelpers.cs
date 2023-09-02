internal static class CsvUtilityHelpers
{

    internal static void PrintFlightRowErrors(int currentIndex, List<string> errors)
    {
        Console.WriteLine($"Error At Line {currentIndex}");
        foreach (string error in errors)
        {
            Console.WriteLine($"Validation Error: {error}");
        }
        Console.WriteLine();
    }
}