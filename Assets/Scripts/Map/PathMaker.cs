using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathMaker
{
    /// <summary>
    /// Each cell in the grid is a node
    /// </summary>
    private class Node
    {
        public float DistanceFromGoal { get; private set; } //Distance from the target
        public bool Visited; //Was it already visited before
        public bool passable; //Is it passable
        public Vector2 position; //It's position
        public short[] indices = new short[2]; //The index it is in the grid

        //----------------------------------------------------------------------
        /// <summary>
        /// Creates a new instance of it
        /// </summary>
        /// <param name="xPos">The x position in the world</param>
        /// <param name="yPos">The y position in the world</param>
        /// <param name="passable">Is it passable</param>
        /// <param name="indices">It's position in the grid</param>
        public Node(short[] indices)
        {
            this.indices = indices;
            this.position = new Vector2();
        }

        public Node(short[] indices, int xPos, int yPos, bool passable)
        {
            this.indices = indices;
            this.passable = passable;

            this.position = new Vector2(xPos, yPos);
            this.Visited = false;
            this.DistanceFromGoal = float.MaxValue;
        }

        //-------------------------------------------------------
        /// <summary>
        /// Sets the distance
        /// </summary>
        /// <param name="dist">The distance to the target</param>
        public void SetDist(float dist)
        {
            this.DistanceFromGoal = dist;
        }

        //--------------------------------------------------------
        /// <summary>
        /// Sets the visited property
        /// </summary>
        public void Visit()
        {
            this.Visited = true;
        }

        public void Reset(int xPos, int yPos, bool passable)
        {
            this.passable = passable;

            this.position.Set((float)xPos, (float)yPos);
            this.Visited = false;
            this.DistanceFromGoal = float.MaxValue;
        }

        public void Reset()
        {
            this.passable = false;
        }

        public void Reset(bool passable)
        {
            this.passable = passable;
            this.Visited = false;
        }
    }

    private const short maxDepth = 1024;
    private Node[,] map; //The grid
    private World world; //Reference to the world
    private List<Vector2> path; //Path which it will return
    private Node start; //Start of the path
    private Node goal; //The target
    private short size; //The size of the allocated map
    private Stack<Node> visitedNodes = new Stack<Node>();

    /// <summary>
    /// Creates a new instance of the PathMaker
    /// </summary>
    /// <param name="start">The starting position</param>
    /// <param name="goal">The target's position</param>
    /// <param name="size">The size of the grid (x size = Size *2, y size = Size*2)</param>
    /// <param name="world">The world it is in</param>
    public PathMaker(short size, World world)
    {
        this.world = world;
        this.size = size;
        map = new Node[world.XSize, world.YSize];
        //Fill in the grid
        for (short i = 0; i < map.GetLength(0); i++)
        {
            for (short j = 0; j < map.GetLength(1); j++)
            {
                map[i, j] = new Node(new short[] { i, j }, i, j, world.Cells[i, j] == CellType.Road);
            }
        }
    }

    //-------------------------------------------------------------
    /// <summary>
    /// Resets the goal and start back to what it should be and reverses the visited nodes state to not visited
    /// </summary>
    private void ResetBack()
    {
        while (visitedNodes.Count != 0)
        {
            visitedNodes.Pop().Visited = false;
        }

        start.Reset(true);
        goal.Reset(true);
    }

    //---------------------------------------------------------------------
    /// <summary>
    /// Creates a new path
    /// </summary>
    /// <param name="moveTargetList">The stack which will store the coord toward the target</param>
    /// <returns>False if there can't be a path made True if it was a success</returns>
    public bool CreatePath(out List<Vector2> moveTargetList, Vector2 startCoord, Vector2 goalCoord)
    {
        //  Set the goal and the start
        this.goal = map[(int)goalCoord.y, (int)goalCoord.x];
        this.goal.passable = true; //If it is water make it passable (maybe someday I will make this better)
        this.start = map[(int)startCoord.y, (int)startCoord.x];

        path = new List<Vector2>();
        //Search for the goal
        if (!SearchForGoal(start, 0))
        {
            moveTargetList = path;
            ResetBack();
            return false;
        }
        moveTargetList = path;
        ResetBack();
        return true;
    }

    //-------------------------------------------------------------
    //Checks if the node is valid for being part of the path
    private bool CheckIfValid(Node node)
    {
        return node.passable && !node.Visited;
    }

    //------------------------------------------------------------
    //Search for the node recursively
    //Returns true if it managed to find the goal, returns false if it didn't manage to find the goal
    private bool SearchForGoal(Node current, short depth)
    {
        current.Visit(); //Visit the node
        visitedNodes.Push(current);

        //Check if it is the goal
        if (current.Equals(goal))
        {
            return true;
        }
        //So it doesn't make a path which kills the animal in the process
        if (depth >= maxDepth)
        {
            return false;
        }

        //Get the possible ways it can go
        List<Node> possibleWays = new List<Node>();
        if (current.indices[0] - 1 >= 0 && CheckIfValid(map[current.indices[0] - 1, current.indices[1]]))
        {
            possibleWays.Add(map[current.indices[0] - 1, current.indices[1]]);
        }

        if (current.indices[1] - 1 >= 0 && CheckIfValid(map[current.indices[0], current.indices[1] - 1]))
        {
            possibleWays.Add(map[current.indices[0], current.indices[1] - 1]);
        }

        if (current.indices[0] + 1 < map.GetLength(0) && CheckIfValid(map[current.indices[0] + 1, current.indices[1]]))
        {
            possibleWays.Add(map[current.indices[0] + 1, current.indices[1]]);
        }
        if (current.indices[1] + 1 < map.GetLength(1) && CheckIfValid(map[current.indices[0], current.indices[1] + 1]))
        {
            possibleWays.Add(map[current.indices[0], current.indices[1] + 1]);
        }
        //Calculate their distance from the goal
        for (int i = 0; i < possibleWays.Count; i++)
        {
            possibleWays[i].SetDist(World.CalcDistance(goal.position, possibleWays[i].position));
        }

        //Order them by the distance and try them each
        foreach (var item in possibleWays.OrderBy(x => x.DistanceFromGoal))
        {
            if (SearchForGoal(item, (short)(depth + 1)))
            {
                path.Add(new Vector2(item.position.y, item.position.x));
                return true;
            }
        }
        //If none of the ways were good return false
        return false;
    }
}