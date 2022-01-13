using System.Collections.Generic;

public class WorkPlace : Building
{
    private List<Person> workers;

    public void GiveJob(Person employee)
    {
        workers.Add(employee);
        CurrentSize++;
    }

    public void Start()
    {
        workers = new List<Person>();
        Capacity = 50;
        CurrentSize = 0;
        BuildingType = BuildingType.WorkPlace;
    }
}