using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

public class Person : MonoBehaviour
{
    public byte Age { get; private set; }
    public Gender Gender { get; private set; }
    public float Speed { get; private set; }
    public float XPos { get => this.gameObject.transform.position.x; }
    public float YPos { get => this.gameObject.transform.position.y; }

    //Reference to the world where the 'person' lives
    private World world;

    private Family family;

    private Coord position;

    private Vector2 nextMoveTarget;

    private List<Vector2> pathToOccupation;
    private List<Vector2> pathFromOccupation;

    private Stack<Vector2> path;
    private Building occupationBuilding;

    private float moveDir;
    private float time = float.MaxValue;
    private float moveTargetSwapInterval = 1f;
    private ActionState currActionState = ActionState.RelaxingAtHome;
    private Occupation occupation = Occupation.None;
    private float currActionTimeLeft = 0f;
    private bool generatedPath = false;

    public void SetWorldAndFamily(World world, Family family)
    {
        if (this.world == null && this.family == null)
        {
            this.world = world;
            this.family = family;
        }
        else
            throw new Exception("Can't change world or family one of them is already set");
    }

    public void Start()
    {
        if (world == null)
        {
            throw new Exception("Please set the world for this poor person");
        }

        Age = (byte)UnityEngine.Random.Range(6, 90);
        if (Age <= 18)
        {
            occupationBuilding = world.LookForBuilding((int)XPos, (int)YPos, 100, BuildingType.School);
            if (occupationBuilding == null)
            {
                Debug.Log("Can't find school");
            }
            else
            {
                Debug.Log("Found school");
                (occupationBuilding as School).NewStudent(this);
            }
            occupation = Occupation.Learning;
        }
        else
        {
            occupationBuilding = world.LookForBuilding((int)XPos, (int)YPos, 100, BuildingType.WorkPlace);
            if (occupationBuilding == null)
            {
                Debug.Log("Can't find job");
            }
            else
            {
                Debug.Log("Found job");
                (occupationBuilding as WorkPlace).GiveJob(this);
            }
            occupation = Occupation.Working;
        }
    }

    public void Update()
    {
        time += Time.deltaTime;
        if ((byte)currActionState % 2 == 1)
        {
            currActionTimeLeft -= Time.deltaTime;
            if (currActionTimeLeft <= 0f)
            {
                currActionState = DecideNextActionState();
            }
        }
        else
        {
            if (time > moveTargetSwapInterval)
            {
                GetNextMoveTarget();
                time = 0f;
            }
            if (generatedPath)
                Move();
        }
    }

    public void InfectInRange()
    {
    }

    public void GetInfected()
    {
    }

    private void GetNextMoveTarget()
    {
        if (path == null || path.Count == 0)
        {
            if (generatedPath)
            {
                currActionState = DecideNextActionState();
                generatedPath = false;
                return;
            }

            GetPath(currActionState);
            generatedPath = true;
        }

        //TODO
        if (path.Count == 0)
        {
            Debug.Log("Can't find path");
            currActionState = ActionState.RelaxingAtHome;
            nextMoveTarget = this.transform.position;
            this.gameObject.SetActive(false);
            return;
        }

        nextMoveTarget = path.Pop();
    }

    private ActionState DecideNextActionState()
    {
        switch (currActionState)
        {
            case ActionState.RelaxingAtHome:
                return ActionState.GoingToWork;
                break;

            case ActionState.Waiting:
                break;

            case ActionState.GoingToWork:
                return ActionState.Working;
                break;

            case ActionState.Working:
                return ActionState.GoingHome;
                break;

            case ActionState.GoingToSchool:
                return ActionState.Studying;
                break;

            case ActionState.Commuting:
                break;

            case ActionState.Shopping:
                break;

            case ActionState.Studying:
                return ActionState.GoingHome;
                break;

            case ActionState.GoingHome:
                return ActionState.RelaxingAtHome;
                break;

            default:
                break;
        }
        return ActionState.Waiting;
    }

    public void GetPath(ActionState nextActionState)
    {
        switch (nextActionState)
        {
            case ActionState.GoingHome:
                if (pathFromOccupation == null)
                {
                    world.CreatePath(out pathFromOccupation, occupationBuilding.NearestRoad, family.HouseEnteraceLoc);
                }
                path = new Stack<Vector2>(pathFromOccupation);

                //world.CreatePath(out pathFromOccupation, occupationBuilding.NearestRoad, family.HouseEnteraceLoc);
                // path = pathFromOccupation;

                break;

            case ActionState.GoingToWork:
                if (pathToOccupation == null)
                {
                    world.CreatePath(out pathToOccupation, family.HouseEnteraceLoc, occupationBuilding.NearestRoad);
                }
                path = new Stack<Vector2>(pathToOccupation);

                //     path = pathToOccupation;
                //   world.CreatePath(out pathToOccupation, family.HouseEnteraceLoc, occupationBuilding.NearestRoad);

                break;
            /*
        case ActionState.GoingToSchool:
            break;

        case ActionState.GoingShopping:
            break;
            */
            default:
                break;
        }
    }

    public void Move()
    {
        if ((byte)currActionState % 2 == 0)
            this.transform.position = Vector2.Lerp(this.transform.position, nextMoveTarget, time);
    }
}