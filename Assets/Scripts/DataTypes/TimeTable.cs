internal struct TimeTable
{
    public float StartTime { get; private set; }
    public float EndTime { get; private set; }

    public TimeTable(float startTime, float endTime)
    {
        this.StartTime = startTime;
        this.EndTime = endTime;
    }
}