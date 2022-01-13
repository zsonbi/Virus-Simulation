using UnityEngine;

/// <summary>
/// 2d coordinate storage
/// </summary>
public class Coord
{
    /// <summary>
    /// X coord (on the 2d plane)
    /// </summary>
    public float x;

    /// <summary>
    /// Y coord (on the 2d plane)
    /// </summary>
    public float y;

    public int IntX { get => (int)x; }
    public int IntY { get => (int)y; }

    //-------------------------------------------------------
    /// <summary>
    /// Creates a new instance
    /// </summary>
    /// <param name="x">x coord (on the 2d plane)</param>
    /// <param name="y">y coord (on the 2d plane)</param>
    public Coord(float x, float y)
    {
        this.x = x;
        this.y = y;
    }

    //--------------------------------------------------------
    /// <summary>
    /// Creates a new instance with the x and y null
    /// </summary>
    public Coord()
    {
    }

    //----------------------------------------------------------------------
    /// <summary>
    /// Sets the coord x and y value
    /// </summary>
    /// <param name="x">The x value</param>
    /// <param name="y">The y value</param>
    public void SetCoord(float x, float y)
    {
        this.x = x;
        this.y = y;
    }

    //---------------------------------------------------------------
    /// <summary>
    /// Calculates the distance of two points
    /// </summary>
    /// <param name="x1">first point's x coord</param>
    /// <param name="y1">first point's y coord</param>
    /// <param name="x2">second point's x coord</param>
    /// <param name="y2">second point's y coord</param>
    /// <returns>the distance</returns>
    public static float CalcDistance(float x1, float y1, float x2, float y2)
    {
        return Mathf.Sqrt(Mathf.Pow((x1 - x2), 2) + Mathf.Pow((y1 - y2), 2));
    }

    //--------------------------------------------------------------
    /// <summary>
    /// Calculates the distance of two points
    /// </summary>
    /// <param name="coord1">first point</param>
    /// <param name="coord2">second point</param>
    /// <returns>the distance</returns>
    public static float CalcDistance(Coord coord1, Coord coord2)
    {
        return Mathf.Sqrt(Mathf.Pow((coord1.x - coord2.x), 2) + Mathf.Pow((coord1.y - coord2.y), 2));
    }

    //-----------------------------------------------------------------------
    /// <summary>
    /// Calculates an angle of the vector
    /// </summary>
    /// <param name="x1">x coord of the base of the vector</param>
    /// <param name="y1">y coord of the base of the vector</param>
    /// <param name="x2">x coord of the end of the vector</param>
    /// <param name="y2">y coord of the end of the vector</param>
    /// <returns>the angle</returns>
    public static float CalcAngle(float x1, float y1, float x2, float y2)
    {
        return Mathf.Atan2(y2 - y1, x2 - x1);
    }

    //-----------------------------------------------------------------------
    /// <summary>
    /// Calculates an angle of the vector
    /// </summary>
    /// <param name="baseOfTheVector">The point where the vector's base is</param>
    /// <param name="endOfTheVector">The point where the vector's end is</param>
    /// <returns>the angle</returns>
    public static float CalcAngle(Coord baseOfTheVector, Coord endOfTheVector)
    {
        return Mathf.Atan2(endOfTheVector.y - baseOfTheVector.y, endOfTheVector.x - baseOfTheVector.x);
    }

    public override bool Equals(object obj)
    {
        return x == (obj as Coord).x && y == (obj as Coord).y;
    }

    public override string ToString()
    {
        return "x:" + x + " y:" + y;
    }

    public string ToStringWholeCoords()
    {
        return "x:" + IntX + " y:" + IntY;
    }
}