using System.Collections.Generic;

/// <summary>
/// Handles multiple person tasks
/// </summary>
public class PersonTasks
{
    private List<PersonTask> personTasks; //Stores all of the tasks

    /// <summary>
    /// Splits up the people and delegate the tasks
    /// </summary>
    /// <param name="people">Reference to the people in the world</param>
    public PersonTasks(List<Person> people)
    {
        personTasks = new List<PersonTask>();
        int gap = people.Count / Settings.PersonTaskCount;
        int currentBeginIndex = 0;
        for (int i = 0; i < Settings.PersonTaskCount - 1; i++)
        {
            personTasks.Add(new PersonTask(currentBeginIndex, currentBeginIndex + gap, people));
            currentBeginIndex += gap;
        }
        personTasks.Add(new PersonTask(currentBeginIndex, people.Count, people));
    }

    //---------------------------------------------------
    /// <summary>
    /// Start the tasks
    /// </summary>
    public void Start()
    {
        for (int i = 0; i < personTasks.Count; i++)
        {
            personTasks[i].Start();
        }
    }

    //-------------------------------------------------------
    /// <summary>
    /// Need to be called when the number of people decreased
    /// </summary>
    public void DecreaseLastRunningEndIndex()
    {
        personTasks[personTasks.Count - 1].DecreaseEndIndex();

        if (!personTasks[personTasks.Count - 1].Running)
        {
            personTasks.RemoveAt(personTasks.Count - 1);
        }
    }
}