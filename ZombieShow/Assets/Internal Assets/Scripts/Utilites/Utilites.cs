using System.Text.RegularExpressions;
using UnityEngine;

public static class Utilites
{
    public static float GetTime(float distance, float speed)
    {
        return distance / speed;
    }

    public static float GetTime(Vector2 from, Vector2 to, float speed)
    {
        return Vector2.Distance(from, to) / speed;
    }

    public static string SplitCamelCase(string str)
    {
        return Regex.Replace(Regex.Replace(str, @"(\P{Ll})(\P{Ll}\p{Ll})", "$1 $2"),
            @"(\p{Ll})(\P{Ll})", "$1 $2");
    }
}

public static class RectTransformExtensions
{
    public static bool IsVisibleWithinRect(this RectTransform thisRect, RectTransform rect, float padding = 0f)
    {
        Vector3[] thisRectBounds = new Vector3[4];
        thisRect.GetWorldCorners(thisRectBounds);

        float maxY = Mathf.Max(thisRectBounds[0].y, thisRectBounds[1].y, thisRectBounds[2].y, thisRectBounds[3].y);
        float minY = Mathf.Min(thisRectBounds[0].y, thisRectBounds[1].y, thisRectBounds[2].y, thisRectBounds[3].y);
        //No need to check horizontal visibility: there is only a vertical scroll rect
        //float maxX = Mathf.Max (v [0].x, v [1].x, v [2].x, v [3].x);
        //float minX = Mathf.Min (v [0].x, v [1].x, v [2].x, v [3].x);

        Vector3[] rectBounds = new Vector3[4];
        rect.GetComponent<RectTransform>().GetWorldCorners(rectBounds);

        float maxFieldY = Mathf.Max(rectBounds[0].y, rectBounds[1].y, rectBounds[2].y, rectBounds[3].y);
        float minFieldY = Mathf.Min(rectBounds[0].y, rectBounds[1].y, rectBounds[2].y, rectBounds[3].y);

        return !(minY > maxFieldY - padding || maxY < minFieldY + padding);
    }
}
