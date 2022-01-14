using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    public GameObject DefaultPerson;
    public WorldGenerator WorldGenerator;

    public int XSize { get => WorldGenerator.XSize; }
    public int YSize { get => WorldGenerator.YSize; }

    public int cellXCount { get => WorldGenerator.cellXCount; }
    public int cellYCount { get => WorldGenerator.cellYCount; }

    private List<Person> persons;
    private PathMaker pathMaker;

    public WorldCell[,] worldCells { get => WorldGenerator.worldCells; }
    public int CellSize { get => WorldGenerator.CellSize; }

    public CellType[,] Cells { get => WorldGenerator.cells; }

    // Start is called before the first frame update
    private void Start()
    {
        //Cap the fps to 60
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;

        FillWorldWithFamilies(100);
        pathMaker = new PathMaker((byte)WorldGenerator.XSize, this);
    }

    private void FillWorldWithFamilies(int familyCount)
    {
        persons = new List<Person>();

        GameObject families = new GameObject("families");
        families.transform.parent = this.transform;
        int houseIndex = Random.Range(0, WorldGenerator.Houses.Count);

        for (int i = 0; i < familyCount; i++)
        {
            if (WorldGenerator.Houses.Count < i)
            {
                break;
            }
            GameObject family = new GameObject("family" + i, typeof(Family));
            family.transform.parent = families.transform;
            family.GetComponent<Family>().SetDefaultParams(WorldGenerator.Houses[i], 4, this);
            //  WorldGenerator.Houses.RemoveAt(houseIndex);
        }
    }

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

    public bool CreatePath(out List<Vector2> path, Vector2 startCoord, Vector2 goalCoord)
    {
        Debug.Log("Generated path");
        return pathMaker.CreatePath(out path, startCoord, goalCoord);
    }

    public static float CalcDistance(Vector2 coord1, Vector2 coord2)
    {
        return Mathf.Sqrt(Mathf.Pow((coord1.x - coord2.x), 2) + Mathf.Pow((coord1.y - coord2.y), 2));
    }
}