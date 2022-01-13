using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Family : MonoBehaviour
{
    private List<Person> peopleInFamily;
    private World world;
    private House house;
    public Stack<Vector2> pathToShop;

    public Vector2 HouseLoc { get => house.transform.position; }

    public Vector2 HouseEnteraceLoc { get => house.NearestRoad; }

    public byte FamilySize { get => (byte)peopleInFamily.Count; }

    public float SupplyAmount { get; private set; }

    public void SetDefaultParams(House house, byte sizeOfFamily, World world)
    {
        peopleInFamily = new List<Person>();

        for (int i = 0; i < sizeOfFamily; i++)
        {
            GameObject person = Instantiate(world.DefaultPerson, this.transform);
            peopleInFamily.Add(person.GetComponent<Person>());
            peopleInFamily.Last().SetWorldAndFamily(world, this);
            this.house = house;
            person.transform.position = HouseEnteraceLoc;
        }
    }

    public void GotSuppliesFromShop()
    {
        SupplyAmount = 100f;
    }

    public void Update()
    {
        if (SupplyAmount > 0)
        {
            SupplyAmount -= Time.deltaTime;
        }
    }
}