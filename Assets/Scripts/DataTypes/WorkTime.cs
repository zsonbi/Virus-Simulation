/// <summary>
/// Stores the work time of a person
/// </summary>
public struct WorkTime
{
    /// <summary>
    /// When his shift starts
    /// </summary>
    public float StartTime { get; private set; }

    /// <summary>
    /// When his shift ends
    /// </summary>
    public float EndTime { get; private set; }

    //The duration of his work
    private float actionTime;

    //---------------------------------------------------------------------
    /// <summary>
    /// Creates a new work time object with a 8 hour long shift
    /// </summary>
    /// <param name="startTime"></param>
    public WorkTime(float startTime)
    {
        this.StartTime = startTime;
        this.EndTime = (startTime + 3600 * 8) % 86400;
        this.actionTime = (EndTime - StartTime + 86400) % 86400;
    }

    //------------------------------------------------------------------------
    /// <summary>
    /// Gets the length of the shift
    /// </summary>
    /// <returns>the length of the shift</returns>
    public float GetActionTime()
    {
        return actionTime;
    }

    //---------------------------------------------------------------------
    /// <summary>
    /// Returns the time remaining until the start of the shift
    /// </summary>
    /// <param name="currTime">The current day time</param>
    /// <returns>Time till the shift starts</returns>
    public float GetTimeTillWorkStart(float currTime)
    {
        return (StartTime - currTime + 86400) % 86400;
    }
}