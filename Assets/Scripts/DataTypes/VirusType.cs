using Newtonsoft.Json;

/// <summary>
/// One type of the virus
/// </summary>
public class VirusType
{
    /// <summary>
    /// The name of the virus
    /// </summary>
    public string NameOfTheVirus { get; private set; }

    /// <summary>
    /// The spread range inside a building
    /// </summary>
    public float RangeInsideBuilding { get; private set; }

    /// <summary>
    /// The death rate from the virus
    /// </summary>
    public float DeathRate { get; private set; }

    /// <summary>
    /// The time it takes for a complete recovery
    /// </summary>
    public float RecoveryTime { get; private set; }

    /// <summary>
    /// The amount of immunity he/she recieves
    /// </summary>
    public float ImmunityTime { get; private set; }

    /// <summary>
    /// The time it takes to the virus to be discovered
    /// </summary>
    public float TimeToDiscover { get; private set; }

    /// <summary>
    /// How many families should the virus infect on startup
    /// </summary>
    public int NumberOfFamiliesToInfectOnStart { get; private set; }

    /// <summary>
    /// How many "attemps" it takes to get infected
    /// </summary>
    public float InfectionRate { get; private set; } = 800f;

    /// <summary>
    /// Creates a new virus type
    /// </summary>
    /// <param name="name">The name of the virus</param>
    /// <param name="rangeInsideBuilding">The spread range inside a building</param>
    /// <param name="deathRate">The death rate from the virus</param>
    /// <param name="recoveryTime">The time it takes for a complete recovery</param>
    /// <param name="ImmunityTime">The amount of immunity he/she recieves</param>
    [JsonConstructor]
    public VirusType(string NameOfTheVirus, float RangeInsideBuilding, float DeathRate, float RecoveryTime, float ImmunityTime, float TimeToDiscover, int NumberOfFamiliesToInfectOnStart)
    {
        this.NameOfTheVirus = NameOfTheVirus;
        this.RangeInsideBuilding = RangeInsideBuilding;
        this.DeathRate = DeathRate;
        this.RecoveryTime = RecoveryTime;
        this.ImmunityTime = ImmunityTime;
        this.TimeToDiscover = TimeToDiscover;
        this.NumberOfFamiliesToInfectOnStart = NumberOfFamiliesToInfectOnStart;
    }

    /// <summary>
    /// Creates a new virus type from one row of the csv
    /// </summary>
    /// <param name="rowOFCSV">A row from the csv (except the first row)</param>
    public VirusType(string rowOFCSV)
    {
        string[] splitted = rowOFCSV.Split(';');
        this.NameOfTheVirus = splitted[0].Trim(' ');
        this.RangeInsideBuilding = (float)System.Convert.ToDouble(splitted[1].Trim(' '));
        this.DeathRate = (float)(System.Convert.ToDouble(splitted[2].Trim(new char[] { ' ', '%' })) / 100f);
        this.RecoveryTime = System.Convert.ToInt32(splitted[3].Trim(' ')) * 86400;
        this.ImmunityTime = System.Convert.ToInt32(splitted[4].Trim(' ')) * 86400;
    }
}