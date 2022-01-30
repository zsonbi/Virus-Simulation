using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Handles the main menu interactions
/// </summary>
public class MainMenu : MonoBehaviour
{
    [Header("The input field which stores the world's size")]
    public InputField WorldSizeText;

    [Header("The input field which stores the family supply size")]
    public InputField FamilySuppliesText;

    [Header("The input field which stores the minimum size of the family")]
    public InputField MinSizeOfFamilyText;

    [Header("The input field which stores the maximum size of the family size")]
    public InputField MaxSizeOfFamilyText;

    [Header("The input field which stores the variance of the virus's parameters")]
    public InputField VirusVarianceText;

    [Header("The input field which stores the number of families to infect on start of the simulation")]
    public InputField NumberOfFamiliesToInfectOnStartText;

    //---------------------------------------------------------
    // Start is called before the first frame update
    private void Start()
    {
        //Cap the fps to 60
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;

        //Set the base values
        this.WorldSizeText.text = Settings.WorldSize.ToString();
        FamilySuppliesText.text = (Settings.FamilySuppliesStock / 86400f).ToString();
        MinSizeOfFamilyText.text = Settings.MinSizeOfFamily.ToString();
        MaxSizeOfFamilyText.text = Settings.MaxSizeOfFamily.ToString();
        VirusVarianceText.text = Settings.VirusVarience.ToString();
        NumberOfFamiliesToInfectOnStartText.text = Settings.NumberOfFamiliesToInfectOnStart.ToString();
    }

    //-----------------------------------------------------------
    /// <summary>
    /// Loads the simulation scene
    /// </summary>
    public void StartSimulation()
    {
        SceneManager.LoadScene("Simulation", LoadSceneMode.Single);
    }

    //--------------------------------------------------------------
    /// <summary>
    /// Exits the application
    /// </summary>
    public void Exit()
    {
        Application.Quit();
    }

    //----------------------------------------------------
    /// <summary>
    /// Updates the world's size in the settings
    /// Keeps the (n%5 == 1) condition
    /// </summary>
    public void WorldSizeChanged()
    {
        int inputAmount = Convert.ToInt32(WorldSizeText.text);
        Settings.WorldSize = (inputAmount - inputAmount % 5) + 1;
    }

    //-------------------------------------------------------
    /// <summary>
    /// Updates the supply amount in the settings (also multiplies it by 86400)
    /// </summary>
    public void SupplyAmountChanged()
    {
        Settings.FamilySuppliesStock = (float)(Convert.ToDouble(WorldSizeText.text.Replace('.', ',')) * 86400);
    }

    //----------------------------------------------------------------
    /// <summary>
    /// Updates the family's minimum size in the settings
    /// </summary>
    public void MinFamilySizeChanged()
    {
        Settings.MinSizeOfFamily = (byte)Convert.ToInt32(MinSizeOfFamilyText.text);
    }

    //--------------------------------------------------------------
    /// <summary>
    /// Updates the family's maximum size in the settings
    /// </summary>
    public void MaxFamilySizeChanged()
    {
        Settings.MaxSizeOfFamily = (byte)Convert.ToInt32(MaxSizeOfFamilyText.text);
    }

    //-------------------------------------------------------------
    /// <summary>
    /// Updates the virus's variance in the settings
    /// </summary>
    public void VirusVarianceChanged()
    {
        Settings.VirusVarience = (float)Convert.ToDouble(VirusVarianceText.text.Replace('.', ','));
    }

    //------------------------------------------------------------------------------
    /// <summary>
    /// Updates the Number of families to infect on start varriable in the settings
    /// </summary>
    public void FamiliesToInfectChanged()
    {
        Settings.NumberOfFamiliesToInfectOnStart = Convert.ToInt32(NumberOfFamiliesToInfectOnStartText.text);
    }
}