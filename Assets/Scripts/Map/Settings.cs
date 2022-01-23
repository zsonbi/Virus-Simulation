public static class Settings
{
    /// <summary>
    /// The number of seconds should be elapsed in the simulation while 1 seconds elapse in the real world
    /// </summary>
    public static float RealTimeToSimulationTime = 1200;

    /// <summary>
    /// The possible work start times
    /// </summary>
    public static float[] PossibleWorkStartTimes = new float[3] { 0f, 28800f, 57600f };

    /// <summary>
    /// The default maximum supplies a family gets
    /// </summary>
    public static float FamilySuppliesStock = 172800f;

    /// <summary>
    /// When should they decide to go to the market
    /// </summary>
    public static float familyFoodStockCritAmount = FamilySuppliesStock * 0.3f;

    /// <summary>
    /// The minimum size of the family
    /// </summary>
    public static byte MinSizeOfFamily = 1;

    /// <summary>
    /// The maximum size of the family
    /// </summary>
    public static byte MaxSizeOfFamily = 6;

    /// <summary>
    /// Sets the rate of how frequently should the method be called
    /// </summary>
    public static float InfectionRateInside = RealTimeToSimulationTime * 0.5f;

    public static float VirusVarience = 0.7f;

    public static float InfectionRateMultiplier = 1f;
}