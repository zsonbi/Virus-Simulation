using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WorldGenerator : MonoBehaviour
{
    public int XSize;
    public int YSize;
    public int XOffSet = 0;
    public int YOffSet = 0;
    public int cellXCount { get; private set; }
    public int cellYCount { get; private set; }

    public int CellSize = 5;

    public GameObject HousePrefab;
    public GameObject MarketPrefab;
    public GameObject WorkPlacePrefab;
    public GameObject SchoolPrefab;
    public GameObject RoadPrefab;

    public CellType[,] cells;
    public WorldCell[,] worldCells;

    public List<House> Houses { get; private set; }

    public void Awake()
    {
        cellXCount = XSize / CellSize;
        cellYCount = YSize / CellSize;
        cells = new CellType[YSize, XSize];
        worldCells = new WorldCell[cellYCount, cellXCount];

        for (int i = 0; i < cellYCount; i++)
        {
            for (int j = 0; j < cellXCount; j++)
            {
                worldCells[i, j] = new WorldCell(i, j);
            }
        }

        GenerateRoads();
        GenerateBuildings();
    }

    public int ConvertWorldXCoordToCellIndex(int xCoord)
    {
        return (xCoord + XSize / 2) / CellSize;
    }

    public int ConvertWorldYCoordToCellIndex(int yCoord)
    {
        return (yCoord + YSize / 2) / CellSize;
    }

    private void GenerateRoads()
    {
        List<GameObject> roads = new List<GameObject>();
        GameObject Roads = new GameObject("roads");
        Roads.transform.parent = this.transform;

        for (int i = 0; i < YSize; i += 5)
        {
            for (int j = 0; j < XSize; j++)
            {
                roads.Add(Instantiate(RoadPrefab, Roads.transform));
                roads.Last().transform.position = new Vector2(j, i);
                cells[i, j] = CellType.Road;
            }
        }

        for (int i = 0; i < XSize; i += 13)
        {
            for (int j = 0; j < YSize; j++)
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

    private void GenerateBuildings()
    {
        List<GameObject> houses = new List<GameObject>();
        List<GameObject> markets = new List<GameObject>();
        List<GameObject> workPlaces = new List<GameObject>();
        List<GameObject> schools = new List<GameObject>();
        Houses = new List<House>();

        GameObject buildings = new GameObject("Buildings");
        buildings.transform.parent = this.transform;
        for (int i = 0; i < YSize - 1; i++)
        {
            for (int j = 0; j < XSize - 1; j++)
            {
                if (cells[i, j] == CellType.None)
                {
                    float noise = Mathf.PerlinNoise(((float)(j / (float)XSize * 5f) + XOffSet) * 10, ((float)(i / (float)YSize * 5f) + YOffSet) * 10 - 3.8f);
                    CellType cellType;
                    Building tempBuilding;

                    if (noise > 0.27f)
                    {
                        houses.Add(Instantiate(HousePrefab, buildings.transform));
                        houses.Last().transform.position = new Vector2(j + 0.5f, i + 0.5f);
                        cellType = CellType.House;
                        tempBuilding = houses.Last().GetComponent<Building>();
                        worldCells[i / CellSize, j / CellSize].AddBuilding(tempBuilding);
                        Houses.Add((House)tempBuilding);
                    }
                    else if (noise > 0.24f)
                    {
                        markets.Add(Instantiate(MarketPrefab, buildings.transform));
                        markets.Last().transform.position = new Vector2(j + 0.5f, i + 0.5f);
                        cellType = CellType.Market;
                        tempBuilding = markets.Last().GetComponent<Building>();
                        worldCells[i / CellSize, j / CellSize].AddBuilding(tempBuilding);
                    }
                    else if (noise > 0.19f)
                    {
                        schools.Add(Instantiate(SchoolPrefab, buildings.transform));
                        schools.Last().transform.position = new Vector2(j + 0.5f, i + 0.5f);
                        cellType = CellType.School;
                        tempBuilding = schools.Last().GetComponent<Building>();
                        worldCells[i / CellSize, j / CellSize].AddBuilding(tempBuilding);
                    }
                    else
                    {
                        workPlaces.Add(Instantiate(WorkPlacePrefab, buildings.transform));
                        workPlaces.Last().transform.position = new Vector2(j + 0.5f, i + 0.5f);
                        cellType = CellType.WorkPlace;
                        tempBuilding = workPlaces.Last().GetComponent<Building>();
                        worldCells[i / CellSize, j / CellSize].AddBuilding(tempBuilding);
                    }

                    if (0 <= i - 1 && cells[i - 1, j] == CellType.Road)
                    {
                        tempBuilding.SetNearestRoadLocation(new Vector2(j, i - 1));
                    }
                    else if (YSize > i + 2 && cells[i + 2, j] == CellType.Road)
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
        Debug.Log(Houses.Count + " number of houses");
    }
}