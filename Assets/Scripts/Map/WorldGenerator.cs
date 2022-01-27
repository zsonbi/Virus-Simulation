using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// Generates a new world
/// </summary>
public class WorldGenerator : MonoBehaviour
{
    public int Size { get; private set; }

    [Header("The house's prefab")]
    public GameObject HousePrefab;

    [Header("The market's prefab")]
    public GameObject MarketPrefab;

    [Header("The workplace's prefab")]
    public GameObject WorkPlacePrefab;

    [Header("The school's prefab")]
    public GameObject SchoolPrefab;

    [Header("The road's prefab")]
    public GameObject RoadPrefab;

    /// <summary>
    /// The size of a single world cell
    /// </summary>
    public byte CellSize = 5;

    /// <summary>
    /// The number cells on the x axis
    /// </summary>
    public int cellXCount { get; private set; }

    /// <summary>
    /// The number of cells on the y axis
    /// </summary>
    public int cellYCount { get; private set; }

    /// <summary>
    /// The type of every cell each cell is 1 by 1 size
    /// </summary>
    public CellType[,] cells;

    /// <summary>
    /// A bigger cell for better indexing
    /// </summary>
    public WorldCell[,] worldCells;

    /// <summary>
    /// All of the building categorized
    /// </summary>
    public Dictionary<BuildingType, List<Building>> Buildings { get; private set; }

    //---------------------------------------------------------
    //Runs when the script is loaded
    private void Awake()
    {
        Size = Settings.WorldSize;
        cellXCount = UnityEngine.Mathf.CeilToInt(Size / (float)CellSize);
        cellYCount = UnityEngine.Mathf.CeilToInt(Size / (float)CellSize);
        cells = new CellType[Size, Size];
        //Creates the world cells
        worldCells = new WorldCell[cellYCount, cellXCount];
        for (int i = 0; i < cellYCount; i++)
        {
            for (int j = 0; j < cellXCount; j++)
            {
                worldCells[i, j] = new WorldCell(j, i);
            }
        }

        GenerateRoads();
        GenerateBuildings();
    }

    //--------------------------------------------------------
    //Generate the roads
    private void GenerateRoads()
    {
        List<GameObject> roads = new List<GameObject>();
        GameObject Roads = new GameObject("roads");
        Roads.transform.parent = this.transform;

        for (int i = 0; i < Size; i += 5)
        {
            for (int j = 0; j < Size; j++)
            {
                roads.Add(Instantiate(RoadPrefab, Roads.transform));
                roads.Last().transform.position = new Vector2(j, i);
                cells[i, j] = CellType.Road;
            }
        }

        for (int i = 0; i < Size; i += 13)
        {
            for (int j = 0; j < Size; j++)
            {
                if (cells[j, i] == CellType.None)
                {
                    roads.Add(Instantiate(RoadPrefab, Roads.transform));
                    roads.Last().transform.position = new Vector2(i, j);
                    cells[j, i] = CellType.Road;
                }
            }
        }

        Roads.transform.position = new Vector3(0, 0, 10);
        //TODO combine sprites
    }

    //-----------------------------------------------------------
    //Generate the buildings
    private void GenerateBuildings()
    {
        List<GameObject> houses = new List<GameObject>();
        List<GameObject> markets = new List<GameObject>();
        List<GameObject> workPlaces = new List<GameObject>();
        List<GameObject> schools = new List<GameObject>();

        Buildings = new Dictionary<BuildingType, List<Building>>();
        Buildings.Add(BuildingType.House, new List<Building>());
        Buildings.Add(BuildingType.WorkPlace, new List<Building>());
        Buildings.Add(BuildingType.School, new List<Building>());
        Buildings.Add(BuildingType.Market, new List<Building>());

        GameObject buildings = new GameObject("Buildings");
        buildings.transform.parent = this.transform;
        for (int i = 0; i < Size - 1; i++)
        {
            for (int j = 0; j < Size - 1; j++)
            {
                if (cells[i, j] == CellType.None)
                {
                    //   float noise = Mathf.PerlinNoise(((float)(j / (float)XSize * 5f) + XOffSet) * 10, ((float)(i / (float)YSize * 5f) + YOffSet) * 10 - 3.8f);
                    float noise = Random.Range(0f, 1f);
                    CellType cellType;
                    Building tempBuilding;
                    //House
                    if (noise > 0.2f)
                    {
                        houses.Add(Instantiate(HousePrefab, buildings.transform));
                        houses.Last().transform.position = new Vector2(j + 0.5f, i + 0.5f);
                        cellType = CellType.House;
                        tempBuilding = houses.Last().GetComponent<Building>();
                        worldCells[i / CellSize, j / CellSize].AddBuilding(tempBuilding);
                    }
                    //Market
                    else if (noise > 0.15f)
                    {
                        markets.Add(Instantiate(MarketPrefab, buildings.transform));
                        markets.Last().transform.position = new Vector2(j + 0.5f, i + 0.5f);
                        cellType = CellType.Market;
                        tempBuilding = markets.Last().GetComponent<Building>();
                        worldCells[i / CellSize, j / CellSize].AddBuilding(tempBuilding);
                    }
                    //School
                    else if (noise > 0.1f)
                    {
                        schools.Add(Instantiate(SchoolPrefab, buildings.transform));
                        schools.Last().transform.position = new Vector2(j + 0.5f, i + 0.5f);
                        cellType = CellType.School;
                        tempBuilding = schools.Last().GetComponent<Building>();
                        worldCells[i / CellSize, j / CellSize].AddBuilding(tempBuilding);
                    }
                    //WorkPlace
                    else
                    {
                        workPlaces.Add(Instantiate(WorkPlacePrefab, buildings.transform));
                        workPlaces.Last().transform.position = new Vector2(j + 0.5f, i + 0.5f);
                        cellType = CellType.WorkPlace;
                        tempBuilding = workPlaces.Last().GetComponent<Building>();
                        worldCells[i / CellSize, j / CellSize].AddBuilding(tempBuilding);
                    }
                    Buildings[tempBuilding.BuildingType].Add(tempBuilding);
                    //Get where is the building's enterance
                    if (0 <= i - 1 && cells[i - 1, j] == CellType.Road)
                    {
                        tempBuilding.SetNearestRoadLocation(new Vector2(j, i - 1));
                    }
                    else if (Size > i + 2 && cells[i + 2, j] == CellType.Road)
                    {
                        tempBuilding.SetNearestRoadLocation(new Vector2(j, i + 2));
                    }

                    cells[i, j] = cellType;
                    cells[i + 1, j] = cellType;
                    cells[i, j + 1] = cellType;
                    cells[i + 1, j + 1] = cellType;
                }
            }
        }
    }
}