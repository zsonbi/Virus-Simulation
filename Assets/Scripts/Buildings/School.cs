using System.Collections.Generic;

public class School : Building
{
    private List<Person> students;

    public void Start()
    {
        students = new List<Person>();
        Capacity = 50;
        CurrentSize = 0;
        BuildingType = BuildingType.School;
    }

    public void NewStudent(Person student)
    {
        CurrentSize++;
        students.Add(student);
    }

    public void KickStudent(Person student)
    {
        CurrentSize--;
        students.Remove(student);
    }
}