using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// A whole family
/// </summary>
public class Family : MonoBehaviour
{
    private List<Person> peopleInFamily; //Every person in the family
    private World world; //Reference to the world
    private House house; //Reference to the house of the family
    private Market favoriteMarket; //Reference to the family's favorite market
    private float time = 0f; //Timer
    private bool sentForFood = false; //Had someone gone to the shop

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
    public Vector2 HouseLoc { get => house.transform.position; }

    /// <summary>
    /// The market's position
    /// </summary>
    public Vector2 MarketLoc { get => favoriteMarket.transform.position; }

    /// <summary>
    /// Enterance to the market
    /// </summary>
    public Vector2 MarketEnteranceLoc { get => favoriteMarket.NearestRoad; }

    /// <summary>
    /// Enterance to the house
    /// </summary>
    public Vector2 HouseEnteraceLoc { get => house.NearestRoad; }

    /// <summary>
    /// The size of the family
    /// </summary>
    public byte FamilySize { get => (byte)peopleInFamily.Count; }

    /// <summary>
    /// The remaining amount of supplies at home
    /// </summary>
    public float SupplyAmount { get; private set; } = Settings.FamilySuppliesStock;

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
        this.house = house;
        peopleInFamily = new List<Person>();
        this.favoriteMarket = (Market)this.world.LookForBuilding((int)HouseLoc.x, (int)HouseLoc.y, 100, BuildingType.Market);

        world.CreatePath(out pathToShop, HouseEnteraceLoc, MarketEnteranceLoc);
        world.CreatePath(out pathFromShop, MarketEnteranceLoc, HouseEnteraceLoc);

        for (int i = 0; i < sizeOfFamily; i++)
        {
            GameObject person = Instantiate(world.DefaultPerson, this.transform);
            byte age = (byte)Random.Range(i > 1 ? 6 : 18, 99);
            peopleInFamily.Add(person.GetComponent<Person>());
            peopleInFamily.Last().SetDefaultParameters(world, this, age);
            person.transform.position = HouseLoc;
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

    //------------------------------------------------------------------
    //Runs every frame
    private void Update()
    {
        //Decrease the amount of supplies
        if (SupplyAmount > 0)
        {
            SupplyAmount -= Time.deltaTime * Settings.RealTimeToSimulationTime * peopleInFamily.Count;
        }

        //If it is time send someone for supplies
        if (Settings.familyFoodStockCritAmount >= SupplyAmount && !sentForFood)
        {
            time += Time.deltaTime;

            if (time >= 1f)
            {
                for (int i = 0; i < peopleInFamily.Count; i++)
                {
                    if (peopleInFamily[i].SendToTheShop())
                    {
                        sentForFood = true;
                        break;
                    }
                }
                time = 0f;
            }
        }
    }
}