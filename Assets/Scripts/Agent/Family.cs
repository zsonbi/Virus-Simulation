using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// A whole family
/// </summary>
public class Family : MonoBehaviour
{
    private World world; //Reference to the world
    private bool sentForFood = false; //Had someone gone to the shop

    /// <summary>
    /// Reference to the family's favorite market
    /// </summary>
    public Market FavoriteMarket { get; private set; }

    /// <summary>
    /// Reference to the house of the family
    /// </summary>
    public House House { get; private set; }

    /// <summary>
    /// How to get to the shop from the house
    /// </summary>
    public List<Vector2> pathToShop;

    /// <summary>
    /// How to get home from the shop
    /// </summary>
    public List<Vector2> pathFromShop;

    /// <summary>
    /// The house's position
    /// </summary>
    public Vector2 HouseLoc { get => House.transform.position; }

    /// <summary>
    /// The market's position
    /// </summary>
    public Vector2 MarketLoc { get => FavoriteMarket.transform.position; }

    /// <summary>
    /// Enterance to the market
    /// </summary>
    public Vector2 MarketEnteranceLoc { get => FavoriteMarket.NearestRoad; }

    /// <summary>
    /// Enterance to the house
    /// </summary>
    public Vector2 HouseEnteraceLoc { get => House.NearestRoad; }

    /// <summary>
    /// The size of the family
    /// </summary>
    public byte FamilySize { get => (byte)peopleInFamily.Count; }

    /// <summary>
    /// The remaining amount of supplies at home
    /// </summary>
    public float SupplyAmount { get; private set; } = Settings.FamilySuppliesStock;

    /// <summary>
    /// Every person in the family
    /// </summary>
    public List<Person> peopleInFamily { get; private set; }

    //--------------------------------------------------------------------------
    /// <summary>
    /// Set the default parameters for the family
    /// </summary>
    /// <param name="house">The house where they will live</param>
    /// <param name="sizeOfFamily">The size of the family</param>
    /// <param name="world">The world where the family lives</param>
    public void SetDefaultParams(House house, byte sizeOfFamily, World world)
    {
        this.world = world;
        this.House = house;
        peopleInFamily = new List<Person>();
        this.FavoriteMarket = (Market)this.world.LookForBuilding((int)HouseLoc.x, (int)HouseLoc.y, 100, BuildingType.Market);

        world.CreatePath(out pathToShop, HouseEnteraceLoc, MarketEnteranceLoc);
        pathToShop.Insert(0, new Vector2(MarketLoc.x + UnityEngine.Random.Range(-0.5f, 0.5f), MarketLoc.y + UnityEngine.Random.Range(-0.5f, 0.5f)));

        world.CreatePath(out pathFromShop, MarketEnteranceLoc, HouseEnteraceLoc);
        pathFromShop.Insert(0, new Vector2(HouseLoc.x + UnityEngine.Random.Range(-0.5f, 0.5f), HouseLoc.y + UnityEngine.Random.Range(-0.5f, 0.5f)));

        for (int i = 0; i < sizeOfFamily; i++)
        {
            GameObject person = Instantiate(world.DefaultPerson, this.transform);
            byte age = (byte)Random.Range(i >= 1 ? 3 : 18, 80);
            peopleInFamily.Add(person.GetComponent<Person>());
            peopleInFamily.Last().SetDefaultParameters(world, this, age);
            person.transform.position = HouseLoc;
        }
    }

    //-------------------------------------------------------------------
    /// <summary>
    /// Get everyone in the family infected
    /// </summary>
    /// <param name="virus">The virus they got</param>
    public void GetInitialInfection(VirusType virus)
    {
        foreach (var person in peopleInFamily)
        {
            person.StartingInfected(virus);
        }
    }

    //--------------------------------------------------------------------
    /// <summary>
    /// Gives a sign that one of the family members returned with the supplies
    /// </summary>
    public void GotSuppliesFromShop()
    {
        SupplyAmount = Settings.FamilySuppliesStock;
        sentForFood = false;
    }

    //-----------------------------------------------------------------
    /// <summary>
    /// Kill's the person (just removes him from the family)
    /// </summary>
    /// <param name="personToKill">Who to kill</param>
    public void KillPerson(Person personToKill)
    {
        this.peopleInFamily.Remove(personToKill);
    }

    //------------------------------------------------------------------
    //Runs every frame
    public void UpdateFamilySupplies(float elapsedTime)
    {
        //Decrease the amount of supplies
        if (SupplyAmount > 0)
        {
            SupplyAmount -= elapsedTime * Settings.RealTimeToSimulationTime * peopleInFamily.Count;
        }

        //If it is time send someone for supplies
        if (Settings.familyFoodStockCritAmount >= SupplyAmount && !sentForFood)
        {
            for (int i = 0; i < peopleInFamily.Count; i++)
            {
                if (peopleInFamily[i].SendToTheShop())
                {
                    sentForFood = true;
                    Debug.Log("Sent for food!");
                    break;
                }
            }
        }
    }
}