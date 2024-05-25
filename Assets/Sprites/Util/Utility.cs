using System.Collections.Generic;
using System.Text.RegularExpressions;

public static class Utility
{
    private static System.Random rng = new System.Random();

    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            (list[n], list[k]) = (list[k], list[n]);
        }
    }
}


public static class StringExtensions
{
    public static string ToSnakeCase(this string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        // Convert to lowercase
        string lowerCase = input.ToLower();

        // Replace spaces with underscores
        string snakeCase = Regex.Replace(lowerCase, @"\s+", "_");

        // Optionally, remove any non-alphanumeric characters except for underscores
        snakeCase = Regex.Replace(snakeCase, @"[^a-z0-9_]", "");

        return snakeCase;
    }
}

