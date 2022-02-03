using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

/// <summary>
/// Handles the family's update on a separate core
/// </summary>
public class FamilyTask
{
    //Reference to the families list in the world
    private List<Family> families;

    //---------------------------------------------
    /// <summary>
    /// Creates a new object
    /// </summary>
    /// <param name="families">Reference to the family list in the world</param>
    public FamilyTask(List<Family> families)
    {
        this.families = families;
    }

    //----------------------------------------------------
    /// <summary>
    /// Starts the family task
    /// </summary>
    public void Start()
    {
        Task.Run(() => FamilyTasks());
    }

    //--------------------------------------------------
    /// <summary>
    /// The family task repeats endlessly
    /// </summary>
    private async Task FamilyTasks()
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        while (Settings.TaskRun)
        {
            for (int i = 0; i < families.Count; i++)
            {
                families[i].UpdateFamilySupplies((float)stopwatch.ElapsedMilliseconds / 1000f);
            }
            UnityEngine.Debug.Log("FamilyTask is running");
            stopwatch.Restart();
            await Task.Delay(1000);
        }
    }
}