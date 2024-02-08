using System;
using System.Collections.Generic;

public static class Util
{
    /// <summary>
    /// Converts a string representation of an enum value to its corresponding enum value.
    /// </summary>
    /// <typeparam name="T">The enum type.</typeparam>
    /// <param name="enumValue">A string representing an enum value.</param>
    /// <returns>The enum value that matches the input string, or the default value for the enum if not found.</returns>
    public static T ReturnEnumValueFromStringValue<T>(string enumValue)
    {
        // enumValue is empty/null
        if (enumValue == null) { return default(T); }

        T[] test = (T[])Enum.GetValues(typeof(T));

        for (int x = 0; x < test.Length; x++)
        {
            if (test[x].ToString().Equals(enumValue))
            {
                return test[x];
            }
        }

        return default(T);
    }

    /// <summary>
    /// Returns an array of enum values from the given array of string values.
    /// </summary>
    /// <typeparam name="T">The enum type.</typeparam>
    /// <param name="enumValues">Array of string values to convert to enum values.</param>
    /// <returns>An array of enum values.</returns>
    public static T[] ReturnEnumValuesFromStringValues<T>(string[] enumValues)
    {
        // enumValues is empty/null
        if (enumValues == null) { return default(T[]); }

        T[] result = new T[enumValues.Length];

        for (int i = 0; i < enumValues.Length; i++)
        {
            result[i] = ReturnEnumValueFromStringValue<T>(enumValues[i]);
        }

        return result;
    }

    /// <summary>
    /// Returns a random element from the given array.
    /// </summary>
    /// <typeparam name="T">The type of elements in the array.</typeparam>
    /// <param name="array">The array from which to select a random element.</param>
    /// <returns>A randomly selected element from the array.</returns>
    public static T GetRandomValue<T>(T[] array)
    {
        Random random = new Random();
        int randomIndex = random.Next(array.Length);
        return array[randomIndex];
    }

    /// <summary>
    /// Returns a random element from the given list.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    /// <param name="array">The list from which to select a random element.</param>
    /// <returns>A randomly selected element from the list.</returns>
    public static T GetRandomValue<T>(List<T> list)
    {
        Random random = new Random();
        int randomIndex = random.Next(list.Count);
        return list[randomIndex];
    }
}
