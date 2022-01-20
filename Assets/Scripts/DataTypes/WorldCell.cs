using System.Collections.Generic;
using System.Linq;

/// <summary>
/// A bigger cell
/// </summary>
public class WorldCell
{
    private List<Person> persons; //The persons in the cell
    private List<Building> BuildingsInCell; //The buildings in the cell

    /// <summary>
    /// How many people are in the cell
    /// </summary>
    public int NumberOfPeopleInTheCell { get => persons.Count; }

    /// <summary>
    /// The cell's x index
    /// </summary>
    public int XIndex { get; private set; }

    /// <summary>
    /// The cell's y index
    /// </summary>
    public int YIndex { get; private set; }

    //-------------------------------------------------------------------
    /// <summary>
    /// Creates a new world cell
    /// </summary>
    /// <param name="xIndex">The cell's x index</param>
    /// <param name="yIndex">The cell's y index</param>
    public WorldCell(int xIndex, int yIndex)
    {
        persons = new List<Person>();
        BuildingsInCell = new List<Building>();
        this.XIndex = xIndex;
        this.YIndex = yIndex;
    }

    //-----------------------------------------------------------------------
    /// <summary>
    /// Returns an avalible building
    /// </summary>
    /// <param name="buildingType">The type of the building </param>
    /// <returns>Reference to the building</returns>
    public Building GetAvalibleBuilding(BuildingType buildingType)
    {
        return BuildingsInCell.Find(x => x.BuildingType == buildingType && x.HasFreeSpace());
    }

    //--------------------------------------------------------------------
    /// <summary>
    /// Adds a building to the cell
    /// </summary>
    /// <param name="building">The building which should be added</param>
    public void AddBuilding(Building building)
    {
        BuildingsInCell.Add(building);
    }

    //-------------------------------------------------------------------
    /// <summary>
    /// Adds a person to the cell
    /// </summary>
    /// <param name="personToAdd">The person who should be added</param>
    public void AddPerson(Person personToAdd)
    {
        persons.Add(personToAdd);
    }

    //--------------------------------------------------------------------
    /// <summary>
    /// Remove the person from the cell
    /// </summary>
    /// <param name="personToRemove">The person who should be removed</param>
    public void RemovePerson(Person personToRemove)
    {
        persons.Remove(personToRemove);
    }

    //-------------------------------------------------------------------
    /// <summary>
    /// Returns a list of every person in the cell
    /// </summary>
    /// <returns>The list (deep copy)</returns>
    public List<Person> GetEveryPersonInTheCell()
    {
        return persons.Select(x => x).ToList();
    }

    /// <summary>
    /// Tries to infect everyone who is on the same cell
    /// </summary>
    /// <param name="source">The source of the virus</param>
    /// <param name="virus">The type of the virus</param>
    public void TryToInfectEveryOneInRange(UnityEngine.Vector2 source, VirusType virus)
    {
        foreach (var person in this.persons)
        {
            if (person.Infectable() && source.Equals(person.transform.position))
            {
                person.InfectedInRange(virus);
            }
        }
    }
}