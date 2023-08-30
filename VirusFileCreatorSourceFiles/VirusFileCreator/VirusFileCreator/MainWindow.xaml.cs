using System.Collections.Generic;
using System.Windows;
using System.IO;
using Newtonsoft.Json;
using System.Linq;
using System;

namespace VirusFileCreator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<VirusType> viruses;

        //--------------------------------------------------------------------
        /// <summary>
        /// Loads in the main window
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            ReadInFile();
        }

        //--------------------------------------------------------------------
        //Reads in the json and fills the list
        private void ReadInFile(string fileName = "../viruses.json")
        {
            if (File.Exists(fileName))
            {
                viruses = JsonConvert.DeserializeObject<List<VirusType>>(File.ReadAllText(fileName));
            }
            else
            {
                viruses = new List<VirusType>();
            }
            RecordedVirusesLB.Items.Clear();
            RecordedVirusesLB.ItemsSource = viruses.Select(x => x.NameOfTheVirus);
        }

        //---------------------------------------------------------------------
        //Adds a new virus to the list according to the parameters from the textboxes
        private void AddVirus()
        {
            //Null input detection
            if (NameInput.Text == "")
            {
                MessageBox.Show("Please give it a name");
                return;
            }

            if (InsideRangeInput.Text == "")
            {
                MessageBox.Show("Please specify the inside range (0.0-10.0)");
                return;
            }

            if (DeathRateInput.Text == "")
            {
                MessageBox.Show("Please specify the death rate in percentage");
                return;
            }

            if (RecoveryTimeInput.Text == "")
            {
                MessageBox.Show("Please specify the recovery time in days (can be half a day even)");
                return;
            }

            if (ImmunityTimeInput.Text == "")
            {
                MessageBox.Show("Please specify the immunity time in days (can be half a day even)");
                return;
            }

            if (TimeToDiscoverInput.Text == "")
            {
                MessageBox.Show("Please specify the time it takes to discover the virus in days (can be half a day even)");
                return;
            }

            //Value conversions
            string name = NameInput.Text.Trim();
            float rangeInside = (float)Convert.ToDouble(InsideRangeInput.Text.Trim(' ').Replace('.', ','));
            float deathRate = (float)Convert.ToDouble(DeathRateInput.Text.Trim(new char[] { '%', ' ' }).Replace('.', ',')) / 100;
            float recoveryTime = (float)Convert.ToDouble(RecoveryTimeInput.Text.Trim(' ').Replace('.', ',')) * 86400;
            float immunityTime = (float)Convert.ToDouble(ImmunityTimeInput.Text.Trim(' ').Replace('.', ',')) * 86400;
            float timeToDiscover = (float)Convert.ToDouble(TimeToDiscoverInput.Text.Trim(' ').Replace('.', ',')) * 86400;

            this.viruses.Add(new VirusType(name, rangeInside, deathRate, recoveryTime, immunityTime, timeToDiscover));
            SaveViruses();
        }

        //----------------------------------------------------------------
        //Removes the virus from that index
        private void RemoveVirus(int index)
        {
            if (viruses.Count > 0)
                this.viruses.RemoveAt(index);
            SaveViruses();
        }

        //------------------------------------------------------------
        //Saves the viruses
        private void SaveViruses()
        {
            File.WriteAllText("../viruses.json", JsonConvert.SerializeObject(viruses, Formatting.Indented));
            RecordedVirusesLB.ItemsSource = viruses.Select(x => x.NameOfTheVirus);
        }

        //--------------------------------------------------------------
        //Remove virus click handler
        private void RemoveSelectedVirus_Click(object sender, RoutedEventArgs e)
        {
            int selectedIndex = RecordedVirusesLB.SelectedIndex;
            if (selectedIndex == -1)
            {
                MessageBox.Show("Nothing is selected currently please select a virus to remove first");
            }
            else
            {
                this.RemoveVirus(selectedIndex);
            }
        }

        //---------------------------------------------------------------------
        //Add virus click handler
        private void AddVirus_Click(object sender, RoutedEventArgs e)
        {
            this.AddVirus();
        }

        //------------------------------------------------------------------
        //When the selection changed
        private void RecordedVirusesLB_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            int virusIndex = RecordedVirusesLB.SelectedIndex;
            if (virusIndex == -1 || virusIndex >= RecordedVirusesLB.Items.Count)
            {
                return;
            }

            NameInput.Text = viruses[virusIndex].NameOfTheVirus;
            InsideRangeInput.Text = viruses[virusIndex].RangeInsideBuilding.ToString();
            DeathRateInput.Text = (viruses[virusIndex].DeathRate * 100f).ToString();
            RecoveryTimeInput.Text = (viruses[virusIndex].RecoveryTime / 86400f).ToString();
            ImmunityTimeInput.Text = (viruses[virusIndex].ImmunityTime / 86400f).ToString();
            TimeToDiscoverInput.Text = (viruses[virusIndex].TimeToDiscover / 86400f).ToString();
        }
    }
}