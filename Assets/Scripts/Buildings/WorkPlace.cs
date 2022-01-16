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
    private void Start()
    {
        workers = new List<Person>();
        Capacity = 50;
        CurrentSize = 0;
        BuildingType = BuildingType.WorkPlace;
    }
}