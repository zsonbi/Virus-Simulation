using System.Collections.Generic;

/// <summary>
/// The work place
/// </summary>
public class WorkPlace : Building
{
    private List<Person> workers; //Thoose who work here

    /// <summary>
    /// Employ someone
    /// </summary>
    /// <param name="employee">The one who got employed</param>
    public void GiveJob(Person employee)
    {
        workers.Add(employee);
        CurrentSize++;
    }

    //-----------------------------------------------------------
    //Runs before the first frame
    private void Awake()
    {
        workers = new List<Person>();
        this.Necessary = UnityEngine.Random.Range(0f, 1f) <= Settings.BuildingNecessity;
        Capacity = UnityEngine.Random.Range(30, 50);
        CurrentSize = 0;
        BuildingType = BuildingType.WorkPlace;
    }
}