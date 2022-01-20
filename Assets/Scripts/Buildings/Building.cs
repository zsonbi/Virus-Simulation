using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Some kind of building
/// </summary>
public abstract class Building : MonoBehaviour
{
    /// <summary>
    /// The maximum size of the building
    /// </summary>
    public int Capacity { get; protected set; }

    /// <summary>
    /// The current size of the building
    /// </summary>
    public int CurrentSize { get; protected set; }

    /// <summary>
    /// The type of the building
    /// </summary>
    public BuildingType BuildingType { get; protected set; }

    /// <summary>
    /// The enterance to the building
    /// </summary>
    public Vector2 NearestRoad { get; private set; }

    protected List<Person> PersonsInside = new List<Person>();

    //----------------------------------------------------------------------
    /// <summary>
    /// Does it has free capacity
    /// </summary>
    /// <returns>true-has, false-doesn't has</returns>
    public bool HasFreeSpace()
    {
        return CurrentSize < Capacity;
    }

    //-----------------------------------------------------------------------
    /// <summary>
    /// Set the enterance
    /// </summary>
    /// <param name="roadLocation">The location of the enterance</param>
    public void SetNearestRoadLocation(Vector2 roadLocation)
    {
        this.NearestRoad = roadLocation;
    }

    //--------------------------------------------------------------
    /// <summary>
    /// Infects everyone in range
    /// </summary>
    /// <param name="source">The source of the infection</param>
    /// <param name="range">The range of the virus</param>
    /// <param name="virus">Reference to the virus</param>
    public void InfectInRange(Vector2 source, float range, VirusType virus)
    {
        foreach (var person in PersonsInside)
        {
            if (person.Infectable() && Vector2.Distance(source, person.transform.position) < range)
            {
                person.InfectedInRange(virus);
            }
        }
    }

    //----------------------------------------------------
    /// <summary>
    /// Enter the building
    /// </summary>
    /// <param name="personWhoEntered">The person who entered</param>
    public void Enter(Person personWhoEntered)
    {
        this.PersonsInside.Add(personWhoEntered);
    }

    //-------------------------------------------------
    /// <summary>
    /// Leave the building
    /// </summary>
    /// <param name="personWhoLeft">The person who left</param>
    public void Leave(Person personWhoLeft)
    {
        this.PersonsInside.Remove(personWhoLeft);
    }
}