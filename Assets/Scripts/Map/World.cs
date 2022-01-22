using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.Linq;

/// <summary>
/// The world
/// </summary>
public class World : MonoBehaviour
{
    public GameObject DefaultPerson;
    public WorldGenerator WorldGenerator;
    public StatusHandler StatusHandler;

    /// <summary>
    /// The time of the day 1 = 1sec
    /// </summary>
    public float dayTime = 0f;

    /// <summary>
    /// The number of days passed
    /// </summary>
    public ushort DayCounter { get; private set; } = 0;

    /// <summary>
    /// The X size of the map
    /// </summary>
    public int XSize { get => WorldGenerator.XSize; }

    /// <summary>
    /// The Y size of the map
    /// </summary>
    public int YSize { get => WorldGenerator.YSize; }

    /// <summary>
    /// The number of cells in the worldcells array on the x axis
    /// </summary>
    public int cellXCount { get => WorldGenerator.cellXCount; }

    /// <summary>
    /// The number of cells in the worldcells array on the y axis
    /// </summary>
    public int cellYCount { get => WorldGenerator.cellYCount; }

    /// <summary>
    /// The cells of the world this can be used to find nearby objects
    /// </summary>
    public WorldCell[,] worldCells { get => WorldGenerator.worldCells; }

    /// <summary>
    /// A size of a single world cell
    /// </summary>
    public byte CellSize { get => WorldGenerator.CellSize; }

    /// <summary>
    /// The type of each of cell
    /// </summary>
    public CellType[,] Cells { get => WorldGenerator.cells; }

    private PathMaker pathMaker; //The pathmaker algorithm
    private List<VirusType> viruses;
    private List<Family> families;
    private List<Person> people;

    //--------------------------------------------------------------------------
    // Start is called before the first frame update
    private void Start()
    {
        //Cap the fps to 60
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;

        pathMaker = new PathMaker((byte)WorldGenerator.XSize, this);
        FillWorldWithFamilies(WorldGenerator.Houses.Count);
        ReadInViruses();
    }

    //----------------------------------------------------------------------------
    //Called each frame
    private void Update()
    {
        dayTime += Time.deltaTime * Settings.RealTimeToSimulationTime;

        if (dayTime >= 86400f)
        {
            DayCounter++;
            Debug.Log($"Day {DayCounter} complete");
            StatusHandler.UpdateDayTime(DayCounter);
            dayTime = 0f;
        }
    }

    //-------------------------------------------------------------------
    // Reads in the viruses from the json
    private void ReadInViruses(string nameOfTheFile = "viruses.json")
    {
        viruses = JsonConvert.DeserializeObject<List<VirusType>>(System.IO.File.ReadAllText(nameOfTheFile));

        foreach (var virus in viruses)
        {
            InfectRandomFamilies(virus);
        }
    }

    //-----------------------------------------------------------------
    //Infect families according to the virus's number of families to infect property
    private void InfectRandomFamilies(VirusType virus)
    {
        for (int i = 0; i < virus.NumberOfFamiliesToInfectOnStart; i++)
        {
            families[Random.Range(0, families.Count)].GetInitialInfection(virus);
        }
    }

    //------------------------------------------------------------------------------
    //Fills the world with families
    private void FillWorldWithFamilies(int familyCount)
    {
        people = new List<Person>();
        GameObject familiesParent = new GameObject("families");
        familiesParent.transform.parent = this.transform;
        int houseIndex = Random.Range(0, WorldGenerator.Houses.Count);
        families = new List<Family>();
        for (int i = 0; i < familyCount; i++)
        {
            if (WorldGenerator.Houses.Count <= i)
            {
                break;
            }
            GameObject family = new GameObject("family" + i, typeof(Family));
            family.transform.parent = familiesParent.transform;
            family.GetComponent<Family>().SetDefaultParams(WorldGenerator.Houses[i], (byte)Random.Range(Settings.MinSizeOfFamily, Settings.MaxSizeOfFamily + 1), this);
            families.Add(family.GetComponent<Family>());
            WorldGenerator.Houses[i].Occupy();
            people.AddRange(families.Last().peopleInFamily);
            //  WorldGenerator.Houses.RemoveAt(houseIndex);
        }

        StatusHandler.SetInitialPeopleCount(people.Count);
    }

    //-------------------------------------------------------------------
    /// <summary>
    /// Looks for the nearest building with the specified type
    /// </summary>
    /// <param name="xCoord">Start of the search on the x axis</param>
    /// <param name="yCoord">Start of the search on the y axis</param>
    /// <param name="maxSearchRange">The maximum search range</param>
    /// <param name="buildingType">The type of the building</param>
    /// <returns>The building it found returns with null if it didn't found any</returns>
    public Building LookForBuilding(int xCoord, int yCoord, int maxSearchRange, BuildingType buildingType)
    {
        Direction dir = Direction.Left; //Direction the iteration is going
        byte length = 0; //The length of the iteration
        int row = yCoord / CellSize; //The row in the matrix
        int col = xCoord / CellSize; //The col in the matrix

        do
        {
            //Increment the length by one every 2 cycle
            if ((byte)dir % 2 == 0)
            {
                length++;
            }
            //Iterate through the side of the spiral
            for (int i = 0; i < length; i++)
            {
                if ((byte)dir % 2 == 0)
                {
                    col += (byte)dir - 1;
                }
                else
                {
                    row -= (byte)dir - 2;
                }
                if (row >= cellYCount || row < 0 || col >= cellXCount || col < 0)
                {
                    continue;
                }

                Building possibleBuilding = worldCells[row, col].GetAvalibleBuilding(buildingType);
                if (possibleBuilding != null)
                {
                    return possibleBuilding;
                }
            }
            //Change the direction
            if (dir == Direction.Up)
            {
                dir = Direction.Left;
            }
            else
            {
                dir += 1;
            }
        } while (((float)length / 2f) <= maxSearchRange);
        return null;
    }

    /// <summary>
    /// Infect everyone on the same cell
    /// </summary>
    /// <param name="worldCellIndex">The index of the world cell</param>
    /// <param name="personPos">The source of the infection</param>
    /// <param name="virus">The virus</param>
    public void InfectInRange(Vector2 worldCellIndex, Vector2 personPos, VirusType virus)
    {
        /*  for (int i = (int)worldCellIndex.y - 1; i <= worldCellIndex.y + 1; i++)
          {
              if (i < 0 || i >= cellYCount)
              {
                  continue;
              }
              for (int j = (int)worldCellIndex.x - 1; j < worldCellIndex.x + 1; j++)
              {
                  if (j < 0 || i >= cellXCount)
                  {
                      continue;
                  }
                  this.worldCells[i, j].TryToInfectEveryOneInRange(personPos, range);
              }
          }*/

        this.worldCells[(int)worldCellIndex.y, (int)worldCellIndex.x].TryToInfectEveryOneInRange(personPos, virus);
    }

    //-----------------------------------------------------------------------
    /// <summary>
    /// Adds a person to the world cell
    /// </summary>
    /// <param name="xIndex">The x index of the world cell</param>
    /// <param name="yIndex">The y index of the world cell</param>
    /// <param name="personToAdd">The person to add</param>
    public void AddPersonToWorldCell(Vector2 indices, Person personToAdd)
    {
        this.worldCells[(int)indices.y, (int)indices.x].AddPerson(personToAdd);
    }

    //---------------------------------------------------------------------------
    /// <summary>
    /// Moves a person from the old position to the new position int the world cells
    /// </summary>
    /// <param name="personToMove">Reference to the person to move</param>
    /// <param name="oldPos">The old worldCell position</param>
    /// <param name="newPos">The new worldCell position</param>
    public void MovePerson(Vector2 oldPos, Vector2 newPos, Person personToMove)
    {
        this.worldCells[(int)oldPos.y, (int)oldPos.x].RemovePerson(personToMove);

        this.worldCells[(int)newPos.y, (int)newPos.x].AddPerson(personToMove);
    }

    //-------------------------------------------------------------------------------
    /// <summary>
    /// Creates a path from the start vector to the goal vector
    /// </summary>
    /// <param name="path">The path it will create</param>
    /// <param name="startCoord">The start of the path (must be on the road)</param>
    /// <param name="goalCoord">The goal of the path (must be on the road)</param>
    /// <returns>true if it created a path false if it failed to create</returns>
    public bool CreatePath(out List<Vector2> path, Vector2 startCoord, Vector2 goalCoord)
    {
        Debug.Log("Generated path");
        return pathMaker.CreatePath(out path, startCoord, goalCoord);
    }

    /// <summary>
    /// Calculates the distance between two vectors
    /// </summary>
    /// <param name="coord1">The first vector</param>
    /// <param name="coord2">The second vector</param>
    /// <returns>The distance</returns>
    public static float CalcDistance(Vector2 coord1, Vector2 coord2)
    {
        return Mathf.Sqrt(Mathf.Pow((coord1.x - coord2.x), 2) + Mathf.Pow((coord1.y - coord2.y), 2));
    }

    public void Infected()
    {
        StatusHandler.IncreaseInfectedCount();
    }

    public void Recovered()
    {
        StatusHandler.DecreaseInfectedCount();
    }

    public void Died(Person person)
    {
        if (person.IsInfected)
        {
            StatusHandler.DecreaseInfectedCount();
        }

        this.people.Remove(person);
        StatusHandler.UpdatePeopleCount(people.Count);
    }
}