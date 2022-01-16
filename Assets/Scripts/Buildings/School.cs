using System.Collections.Generic;

/// <summary>
/// A school
/// </summary>
public class School : Building
{
    private List<Person> students; //The students at the school

    //-----------------------------------------------------------
    //Runs before the first frame
    private void Start()
    {
        students = new List<Person>();
        Capacity = 50;
        CurrentSize = 0;
        BuildingType = BuildingType.School;
    }

    //-------------------------------------------------------------------
    /// <summary>
    /// Adds a new student to the school
    /// </summary>
    /// <param name="student">The new student</param>
    public void NewStudent(Person student)
    {
        CurrentSize++;
        students.Add(student);
    }

    //------------------------------------------------------------------
    /// <summary>
    /// Remove a student from the school
    /// </summary>
    /// <param name="student">The student who got kicked out</param>
    public void KickStudent(Person student)
    {
        CurrentSize--;
        students.Remove(student);
    }
}