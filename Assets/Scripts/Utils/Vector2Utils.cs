using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class Vector2Utils
{
    public static List<Vector2> RemoveDuplicates(List<Vector2> surface)
    {
        return surface.Distinct(new Vector2Comparer()).ToList();
    }

    // Компаратор для Vector2
    private class Vector2Comparer : IEqualityComparer<Vector2>
    {
        private const float Tolerance = 0.0001f; // точность сравнения

        public bool Equals(Vector2 a, Vector2 b)
        {
            return Vector2.SqrMagnitude(a - b) < Tolerance * Tolerance;
        }

        public int GetHashCode(Vector2 v)
        {
            // Округляем до 4 знаков после запятой, чтобы избежать floating point ошибок
            int x = Mathf.RoundToInt(v.x * 10000f);
            int y = Mathf.RoundToInt(v.y * 10000f);
            return x * 397 ^ y;
        }
    }

    public static Vector2 GetLowestPoint(List<Vector2> points)
    {
        if (points == null || points.Count == 0)
            throw new System.ArgumentException("Points list is null or empty");

        Vector2 lowest = points[0];

        for (int i = 1; i < points.Count; i++)
        {
            if (points[i].y < lowest.y)
                lowest = points[i];
        }

        return lowest;
    }

    public static Vector2 GetHigestPoint(List<Vector2> points)
    {
        if (points == null || points.Count == 0)
            throw new System.ArgumentException("Points list is null or empty");

        Vector2 higest = points[0];

        for (int i = 1; i < points.Count; i++)
        {
            if (points[i].y > higest.y)
                higest = points[i];
        }

        return higest;
    }
}
