using UnityEngine;
using System;
using System.Collections.Generic;

/// <summary>
/// A single worthless person
/// </summary>
public class Person : MonoBehaviour
{
    public SpriteRenderer AgeCircle;
    public SpriteRenderer StatusCircle;

    /// <summary>
    /// The age of the person
    /// This will determine his occupation mostly
    /// </summary>
    public byte Age { get; private set; }

    /// <summary>
    /// The gender of the person
    /// </summary>
    public Gender Gender { get; private set; }

    /// <summary>
    /// The person's position on the x axis
    /// </summary>
    public float XPos { get => this.gameObject.transform.position.x; }

    /// <summary>
    /// The person's position on the y axis
    /// </summary>
    public float YPos { get => this.gameObject.transform.position.y; }

    /// <summary>
    /// Is the person infected
    /// </summary>
    public bool IsInfected { get; private set; } = false;

    private World world; //Reference to the world where the 'person' lives
    private Family family; //The person's family
    private Building occupationBuilding; //Where the person is working/learning at
    private Building currentBuilding; //The building which the person is currently inside
    private float time = float.MaxValue; //Timer
    private ActionState currActionState = ActionState.RelaxingAtHome; //Currently what action is the person doing for example going to work
    private Occupation occupation = Occupation.None; //The person occupation
    private float currActionTimeLeft = 0f; //The remaining time from the waiting during actions
    private WorkTime workTime; //When the person's work start and end

    //Movement varriables

    public Vector2 nextMoveTarget; //Where the person should move next
    private List<Vector2> pathToOccupation; //The path to the person's occupation building
    private List<Vector2> pathFromOccupation; //The path to the person's home from the occupation building
    private List<Vector2> path; //Pointer to the currently used path
    private float moveTargetSwapInterval = 20f; //The speed which the moveTargets should be swapped at (sec)
    private bool generatedPath = false; //Is there already a generated path
    private short pathIndex = -1; //The index of the cell in the path list
    private Vector2 worldCellIndex; //The index of the world cell where the person is
    private VirusType currentlyInfectedBy; //Reference to the virus which infected him
    private float recoveryTime = 0f; //The remaining time till full recovery
    private float immunityTime = 0f; //The remaining time from the immunity against the specific virus

    //**************************************************************************************
    /// <summary>
    /// Sets the person's world and family variable
    /// </summary>
    /// <param name="world">Reference to the world class</param>
    /// <param name="family">Reference to the person's family</param>
    public void SetDefaultParameters(World world, Family family, byte age)
    {
        if (this.world == null && this.family == null)
        {
            this.world = world;
            this.family = family;
            this.Age = age;
        }
        else
            throw new Exception("Can't change world or family one of them is already set");
    }

    //-------------------------------------------------------------------
    //Called just before the first frame
    private void Start()
    {
        //Error detection
        if (world == null)
        {
            throw new Exception("Please set the world for this poor person");
        }
        this.worldCellIndex = new Vector2(XPos / world.CellSize, YPos / world.CellSize);
        this.world.AddPersonToWorldCell(worldCellIndex, this);

        this.Gender = (Gender)UnityEngine.Random.Range(0, 2);
        //Decide occupation based on age
        if (Age <= 18)
        {
            //If it didn't find a school try going to work
            if (!FindSchool())
            {
                FindWorkPlace();
            }
            this.AgeCircle.color = Color.blue;
        }
        else
        {
            FindWorkPlace();

            this.AgeCircle.color = Color.magenta;
        }
        currActionTimeLeft = workTime.GetTimeTillWorkStart(world.dayTime);
        currentBuilding = family.House;
        currentBuilding.Enter(this);
    }

    //Called every frame
    private void Update()
    {
        //If the person is infected start recovering
        if (IsInfected)
        {
            recoveryTime -= Time.deltaTime * Settings.RealTimeToSimulationTime;
            if (recoveryTime <= 0f)
            {
                this.IsInfected = false;
                this.immunityTime = currentlyInfectedBy.ImmunityTime;
                this.currentlyInfectedBy = null;
                this.StatusCircle.color = Color.green;
                Debug.Log("Recovered");
            }
        }
        //Else decrease his/her immunity
        else
        {
            if (immunityTime > 0)
            {
                immunityTime -= Time.deltaTime * Settings.RealTimeToSimulationTime;
            }
        }

        //Increase the timer by the elapsed time
        time += Time.deltaTime * Settings.RealTimeToSimulationTime;
        //If it's an action which doesn't require movement wait till the currActionTimeLeft goes to 0
        if ((byte)currActionState % 2 == 1)
        {
            if (IsInfected)
            {
                if (time > Settings.InfectionRateInside)
                {
                    InfectInside();
                    time = 0f;
                }
            }

            currActionTimeLeft -= Time.deltaTime * Settings.RealTimeToSimulationTime;
            if (currActionTimeLeft <= 0f)
            {
                DecideNextActionState();
            }
        }
        //Otherwise move
        else
        {
            if (time > moveTargetSwapInterval)
            {
                GetNextMoveTarget();
                MoveToMoveTarget();
                time = 0f;
            }
        }
    }

    //---------------------------------------------------------
    //Tries to find a school true-found, false-didn't found
    private bool FindSchool()
    {
        occupationBuilding = world.LookForBuilding((int)XPos, (int)YPos, 100, BuildingType.School);
        if (occupationBuilding == null)
        {
            Debug.Log("Can't find school");
            return false;
        }
        else
        {
            Debug.Log("Found school");
            (occupationBuilding as School).NewStudent(this);
            occupation = Occupation.Learning;
            workTime = new WorkTime(28800f);
            return true;
        }
    }

    //--------------------------------------------------------------------
    //Tries to find a work place
    private void FindWorkPlace()
    {
        occupationBuilding = world.LookForBuilding((int)XPos, (int)YPos, 100, BuildingType.WorkPlace);
        if (occupationBuilding == null)
        {
            Debug.Log("Can't find job");
            occupation = Occupation.Unemployed;
        }
        else
        {
            Debug.Log("Found job");
            (occupationBuilding as WorkPlace).GiveJob(this);
            occupation = Occupation.Working;
            workTime = new WorkTime(Settings.PossibleWorkStartTimes[UnityEngine.Random.Range(0, Settings.PossibleWorkStartTimes.Length)]);
        }
    }

    //--------------------------------------------------------------------
    /// <summary>
    /// Infects everyone who is on the same cell
    /// </summary>
    private void InfectOutside()
    {
        this.world.InfectInRange(this.worldCellIndex, this.transform.position, currentlyInfectedBy);
    }

    //------------------------------------------------------------------
    /// <summary>
    /// Infects everyone who is in infection range in the current building
    /// </summary>
    private void InfectInside()
    {
        currentBuilding.InfectInRange(this.transform.position, currentlyInfectedBy.RangeInsideBuilding, currentlyInfectedBy);
    }

    //---------------------------------------------------------------------
    /// <summary>
    /// Checks if the person can be infected
    /// </summary>
    /// <returns>true - it is possible, false - it is not possible</returns>
    public bool Infectable()
    {
        return !this.IsInfected && immunityTime <= 0;
    }

    //---------------------------------------------------------------------
    //TODO add more conditions and stuff
    /// <summary>
    /// This should be called when someone tries to infect him
    /// </summary>
    /// <param name="virus">The type of virus he caught</param>
    public void InfectedInRange(VirusType virus)
    {
        this.currentlyInfectedBy = virus;
        this.IsInfected = true;
        StatusCircle.color = Color.red;
        this.recoveryTime = virus.RecoveryTime;
        Debug.Log("Got infected");
    }

    //--------------------------------------------------------------------
    /// <summary>
    /// He is one of the many who rolled badly :(
    /// </summary>
    /// <param name="virus">The type of virus he caught</param>
    public void StartingInfected(VirusType virus)
    {
        InfectedInRange(virus);
    }

    //---------------------------------------------------------------------
    //Gets the next move target if there isn't a path select one
    private void GetNextMoveTarget()
    {
        //If the path is used up select a new one
        if (pathIndex < 0)
        {
            //If the move action ended select a new action
            if (generatedPath)
            {
                DecideNextActionState();
                generatedPath = false;
                return;
            }

            GetPath(currActionState);
            pathIndex = (short)(path.Count - 1);
        }

        //If the person lives next to the target
        if (path.Count == 0)
        {
            nextMoveTarget = this.transform.position;
            return;
        }

        nextMoveTarget = path[pathIndex];
        pathIndex--;
    }

    //----------------------------------------------------------------------------
    //Decides the persons next action
    private void DecideNextActionState()
    {
        switch (currActionState)
        {
            case ActionState.RelaxingAtHome:
                currActionState = ActionState.GoingToWork;
                this.transform.position = family.HouseEnteraceLoc;
                currentBuilding.Leave(this);
                break;

            case ActionState.Waiting:
                break;

            case ActionState.GoingToWork:
                currActionTimeLeft = workTime.GetActionTime();
                this.transform.position = new Vector2(occupationBuilding.transform.position.x + UnityEngine.Random.Range(-0.5f, 0.5f), occupationBuilding.transform.position.y + UnityEngine.Random.Range(-0.5f, 0.5f));
                currActionState = ActionState.Working;
                currentBuilding = occupationBuilding;
                currentBuilding.Enter(this);
                break;

            case ActionState.Working:
                this.transform.position = occupationBuilding.NearestRoad;
                currActionState = ActionState.GoingHome;
                currentBuilding.Leave(this);
                break;

            case ActionState.GoingShopping:
                this.transform.position = this.transform.position = new Vector2(family.MarketLoc.x + UnityEngine.Random.Range(-0.5f, 0.5f), family.MarketLoc.y + UnityEngine.Random.Range(-0.5f, 0.5f));
                currentBuilding = family.FavoriteMarket;
                currActionTimeLeft = 3600f;
                currActionState = ActionState.Shopping;
                currentBuilding.Enter(this);
                break;

            case ActionState.Shopping:
                this.transform.position = family.MarketEnteranceLoc;
                currActionState = ActionState.GoingHomeFromShopping;
                currentBuilding.Leave(this);
                break;

            case ActionState.GoingHomeFromShopping:
                this.transform.position = new Vector2(family.HouseLoc.x + UnityEngine.Random.Range(-0.5f, 0.5f), family.HouseLoc.y + UnityEngine.Random.Range(-0.5f, 0.5f));
                currActionTimeLeft = workTime.GetTimeTillWorkStart(world.dayTime);
                currActionState = ActionState.RelaxingAtHome;
                family.GotSuppliesFromShop();
                currentBuilding = family.House;
                currentBuilding.Enter(this);
                break;

            case ActionState.GoingHome:
                this.transform.position = new Vector2(family.HouseLoc.x + UnityEngine.Random.Range(-0.5f, 0.5f), family.HouseLoc.y + UnityEngine.Random.Range(-0.5f, 0.5f));
                currActionTimeLeft = workTime.GetTimeTillWorkStart(world.dayTime);
                currActionState = ActionState.RelaxingAtHome;
                currentBuilding = family.House;
                currentBuilding.Enter(this);
                break;

            default:
                break;
        }
        MoveInWorldCellGrid();
    }

    //-----------------------------------------------------------------
    //Gets the appropiate path if it isn't created yet create it
    public void GetPath(ActionState nextActionState)
    {
        generatedPath = true;
        bool foundPath = true;
        switch (nextActionState)
        {
            case ActionState.GoingHome:
                if (pathFromOccupation == null)
                {
                    foundPath = world.CreatePath(out pathFromOccupation, occupationBuilding.NearestRoad, family.HouseEnteraceLoc);
                }
                path = pathFromOccupation;
                break;

            case ActionState.GoingToWork:
                if (pathToOccupation == null)
                {
                    foundPath = world.CreatePath(out pathToOccupation, family.HouseEnteraceLoc, occupationBuilding.NearestRoad);
                }
                path = pathToOccupation;
                break;

            case ActionState.GoingShopping:
                path = family.pathToShop;
                break;

            case ActionState.GoingHomeFromShopping:
                path = family.pathFromShop;
                break;

            default:
                break;
        }

        if (!foundPath)
        {
            Debug.Log("Can't find path");
            currActionState = ActionState.Waiting;
            nextMoveTarget = this.transform.position;
            this.gameObject.SetActive(false);
        }
    }

    //----------------------------------------------------------------------
    /// <summary>
    /// Tries to send this person to the shop
    /// </summary>
    /// <returns>True if the person accepts false if he/she refuses</returns>
    public bool SendToTheShop()
    {
        if (this.currActionState != ActionState.RelaxingAtHome && this.currActionTimeLeft <= 10800f)
        {
            return false;
        }
        Debug.Log("Sent for food");
        this.currActionState = ActionState.GoingShopping;
        this.transform.position = family.HouseEnteraceLoc;
        return true;
    }

    //-------------------------------------------------------------
    //Moves the person to the nextMoveTarget
    private void MoveToMoveTarget()
    {
        if ((byte)currActionState % 2 == 0)
        {
            // this.transform.position = Vector2.Lerp(this.transform.position, nextMoveTarget, time);
            this.transform.position = nextMoveTarget;
            MoveInWorldCellGrid();
            if (IsInfected)
                InfectOutside();
        }
    }

    //-----------------------------------------------------------
    //TODO optimize this method calling rate
    /// <summary>
    /// Moves the person in the world cell grid
    /// </summary>
    private void MoveInWorldCellGrid()
    {
        Vector2 newWorldCellIndex = new Vector2(XPos / world.CellSize, YPos / world.CellSize);
        world.MovePerson(worldCellIndex, newWorldCellIndex, this);
        worldCellIndex = newWorldCellIndex;
    }
}