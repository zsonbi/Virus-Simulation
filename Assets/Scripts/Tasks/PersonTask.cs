using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

/// <summary>
/// Handles a group of people's thread safe operations
/// </summary>
public class PersonTask
{
    //Where should the people indexing start and end
    private int beginIndex;

    private int endIndex;

    //Reference to the people list in the world
    private List<Person> people;

    /// <summary>
    /// Is the task still running
    /// </summary>
    public bool Running { get; private set; } = false;

    //-----------------------------------------------------------
    /// <summary>
    /// Create a new PersonTask
    /// </summary>
    /// <param name="beginIndex">Where should the indexing start</param>
    /// <param name="endIndex">Where should the indexing end</param>
    /// <param name="persons">Reference to the list in the world</param>
    public PersonTask(int beginIndex, int endIndex, List<Person> persons)
    {
        this.beginIndex = beginIndex;
        this.endIndex = endIndex;
        this.people = persons;
    }

    //----------------------------------------------------------
    /// <summary>
    /// Start it on a separate thread
    /// </summary>
    public void Start()
    {
        Task.Run(() => HandlePersons());
    }

    //----------------------------------------------------------
    /// <summary>
    /// Decrease the end index
    /// </summary>
    public void DecreaseEndIndex()
    {
        this.endIndex--;
        Running = endIndex != beginIndex;
    }

    //-----------------------------------------------------------------
    //Go through the people and handle them
    private async void HandlePersons()
    {
        Running = true;
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        while (Settings.TaskRun)
        {
            stopwatch.Restart();
            if (endIndex == beginIndex)
            {
                break;
            }

            for (int i = beginIndex; i < endIndex; i++)
            {
                people[i].UpdatePersonParametersOnThread((float)((stopwatch.ElapsedMilliseconds + 250) / 1000f));
            }

            UnityEngine.Debug.Log($"PeopleThread is running with beginIndex: {beginIndex} and endIndex: {endIndex}");
            await Task.Delay((int)(250));
        }
        Running = false;
    }
}