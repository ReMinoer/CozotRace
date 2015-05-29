using UnityEngine;

static public class VectorExtension
{
    static public Vector2 ToXz(this Vector3 vector)
    {
        return new Vector2(vector.x, vector.z);
    }

    static public Vector3 ToX0Y(this Vector2 vector)
    {
        return new Vector3(vector.x, 0, vector.y);
    }
}