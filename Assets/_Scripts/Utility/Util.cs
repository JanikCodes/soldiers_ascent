using System;
using System.Collections.Generic;
using UnityEngine;

public static class Util
{
    private static readonly System.Random random = new System.Random();

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

    public static Vector3 GetRandomPositionInRadius(Vector3 basePosition, float radius, float minRadius = 0f)
    {
        float effectiveRadius = UnityEngine.Mathf.Max(radius, minRadius);

        // Generate random angles in radians
        float randomAngle = UnityEngine.Random.Range(0f, UnityEngine.Mathf.PI * 2f);

        // Calculate random distance within the effective radius
        float randomDistance = UnityEngine.Random.Range(minRadius, effectiveRadius);

        // Calculate random X and Z offsets based on the random angle and distance
        float offsetX = UnityEngine.Mathf.Cos(randomAngle) * randomDistance;
        float offsetZ = UnityEngine.Mathf.Sin(randomAngle) * randomDistance;

        // Calculate the random position based on the offsets and the basePosition's Y component
        return new Vector3(basePosition.x + offsetX, basePosition.y, basePosition.z + offsetZ);
    }

    /// <summary>
    /// Returns a random integer from given min and max value.
    /// </summary>
    /// <returns>A random integer.</returns>
    public static int GetRandomValue(int min, int max)
    {
        return UnityEngine.Random.Range(min, max);
    }

    /// <summary>
    /// Returns a random element from the given array.
    /// </summary>
    /// <typeparam name="T">The type of elements in the array.</typeparam>
    /// <param name="array">The array from which to select a random element.</param>
    /// <returns>A randomly selected element from the array.</returns>
    public static T GetRandomValue<T>(T[] array)
    {
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
        int randomIndex = random.Next(list.Count);
        return list[randomIndex];
    }
}
