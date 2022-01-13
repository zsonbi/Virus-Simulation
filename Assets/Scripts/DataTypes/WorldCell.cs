using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class WorldCell
{
    private List<Person> persons;
    private List<Building> BuildingsInCell;
    public int NumberOfPeopleInTheCell { get => persons.Count; }
    public int XIndex { get; private set; }
    public int YIndex { get; private set; }

    public WorldCell(int xIndex, int yIndex)
    {
        persons = new List<Person>();
        BuildingsInCell = new List<Building>();
        this.XIndex = xIndex;
        this.YIndex = yIndex;
    }

    public Building GetAvalibleBuilding(BuildingType buildingType)
    {
        return BuildingsInCell.Find(x => x.BuildingType == buildingType && x.HasFreeSpace());
    }

    public void AddBuilding(Building building)
    {
        BuildingsInCell.Add(building);
    }

    public void AddPerson(Person personToAdd)
    {
        persons.Add(personToAdd);
    }

    public void RemovePerson(Person personToRemove)
    {
        persons.Remove(personToRemove);
    }

    public List<Person> GetEveryPersonInTheCell()
    {
        return persons.Select(x => x).ToList();
    }
}