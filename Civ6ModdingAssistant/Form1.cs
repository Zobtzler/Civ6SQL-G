using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;
using System.Configuration;
using System.Data.SqlClient;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace Civ6ModdingAssistant
{
    public partial class Form1 : Form
    {
        string Credentials = @"server=localhost;userid=trainuser;password=TrainPassword1!;database=civ6moddingassistant";
        MySqlConnection conn = null;
        //endtemp
        //SqlConnection Connection;
        //string ConnectionString;

        public string[] CityList, CitizenList, CitizenType, Features, Resources, Terrains, ActiveStartBiases;

        List<string> FeaturesList = new List<string>(), ResourcesList = new List<string>(), TerrainsList = new List<string>(), ActiveStartBiasesList = new List<string>();

        public Form1()
        {
            InitializeComponent();

            //ConnectionString = ConfigurationManager.ConnectionStrings[@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\CivilizationDB.mdf;Integrated Security=True"].ConnectionString;

            //Civ Basics
            CivilizationEthnicity.Items.Add("African");
            CivilizationEthnicity.Items.Add("Asian");
            CivilizationEthnicity.Items.Add("European");
            CivilizationEthnicity.Items.Add("Mediterranean");
            CivilizationEthnicity.Items.Add("South American");

            //Civ Start Biases
            CivilizationStartBiasMinor.Enabled = false;
            CivilizationStartBiasMain.Items.Add("Feature");
            CivilizationStartBiasMain.Items.Add("Resource");
            CivilizationStartBiasMain.Items.Add("Rivers");
            CivilizationStartBiasMain.Items.Add("Terrain");

            FeaturesList.Add("Floodplains");
            FeaturesList.Add("Floddplains Grassland");
            FeaturesList.Add("Floddplains Plains");
            FeaturesList.Add("Forest");
            FeaturesList.Add("Geothermal Fissure");
            FeaturesList.Add("Jungle");
            FeaturesList.Add("Marsh");
            FeaturesList.Add("Oasis");
            FeaturesList.Add("Volcano");
            Features = FeaturesList.ToArray();

            ResourcesList.Add("Bananas");
            ResourcesList.Add("Cattle");
            ResourcesList.Add("Copper");
            ResourcesList.Add("Deer");
            ResourcesList.Add("Rice");
            ResourcesList.Add("Sheep");
            ResourcesList.Add("Stone");
            ResourcesList.Add("Wheat");
            ResourcesList.Add("Citrus");
            ResourcesList.Add("Cocoa");
            ResourcesList.Add("Coffee");
            ResourcesList.Add("Cotton");
            ResourcesList.Add("Diamonds");
            ResourcesList.Add("Dyes");
            ResourcesList.Add("Furs");
            ResourcesList.Add("Gypsum");
            ResourcesList.Add("Incense");
            ResourcesList.Add("Ivory");
            ResourcesList.Add("Jade");
            ResourcesList.Add("Marble");
            ResourcesList.Add("Mercury");
            ResourcesList.Add("Pearls");
            ResourcesList.Add("Salt");
            ResourcesList.Add("Silk");
            ResourcesList.Add("Silver");
            ResourcesList.Add("Spices");
            ResourcesList.Add("Sugar");
            ResourcesList.Add("Tea");
            ResourcesList.Add("Tobacco");
            ResourcesList.Add("Truffles");
            ResourcesList.Add("Whales");
            ResourcesList.Add("Wine");
            ResourcesList.Add("Aluminum");
            ResourcesList.Add("Coal");
            ResourcesList.Add("Horses");
            ResourcesList.Add("Iron");
            ResourcesList.Add("Niter");
            ResourcesList.Add("Oil");
            ResourcesList.Add("Uranium");
            ResourcesList.Add("Amber");
            ResourcesList.Add("Olives");
            Resources = ResourcesList.ToArray();

            TerrainsList.Add("Grass");
            TerrainsList.Add("Grass Hills");
            TerrainsList.Add("Grass Mountain");
            TerrainsList.Add("Plains");
            TerrainsList.Add("Plains Hills");
            TerrainsList.Add("Plains Mountain");
            TerrainsList.Add("Desert");
            TerrainsList.Add("Desert Hills");
            TerrainsList.Add("Desert Mountain");
            TerrainsList.Add("Tundra");
            TerrainsList.Add("Tundra Hills");
            TerrainsList.Add("Tundra Mountain");
            TerrainsList.Add("Snow");
            TerrainsList.Add("Snow Hills");
            TerrainsList.Add("Snow Mountain");
            TerrainsList.Add("Coast");
            TerrainsList.Add("Ocean");
            Terrains = TerrainsList.ToArray();

        }

        private void Button_Click(object sender, EventArgs e)
        {
            conn = new MySqlConnection(Credentials);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "SELECT * FROM RequirementSetRequirements";

            MySqlDataReader reader = cmd.ExecuteReader();
            DataTable TempTable = new DataTable();

            while (reader.Read())
            {
                string query = "INSERT INTO RequirementSetRequirements (RequirementSetId, RequirementId) VALUES " +
                    "('" + reader.GetString(0) + "', '" + reader.GetString(1) + "');";
                Console.WriteLine(query);
                //SqlConnection Connection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\CivilizationDB.mdf;Integrated Security=True");
                //Connection.Open();
                //SqlCommand Command = new SqlCommand(query, Connection);
                //Command.ExecuteNonQuery();
                //Connection.Close();
            }
            conn.Close();
            /*TempTable.Load(reader);
            foreach (DataRow row in TempTable.Rows) Console.WriteLine(row);
            Console.WriteLine(reader);*/
            //this just doesn't want to work

            //Temporary only
            
        }

        private void CreateCiv_CheckedChanged(object sender, EventArgs e)
        {
            if (CreateCiv.Checked) CivilizationTabController.Enabled = true;
            else CivilizationTabController.Enabled = false;
        }

        private void CreateLeader_CheckedChanged(object sender, EventArgs e)
        {
            if (CreateLeader.Checked) LeaderTabController.Enabled = true;
            else LeaderTabController.Enabled = false;
        }

        private void LeaderGenerate_Click(object sender, EventArgs e)
        {
            string LeaderFileText = "";

            string Types = "INSERT INTO Types (Type, Kind)\nVALUES\n" +
                "('LEADER_" + TextConvert(AuthorName.Text) + "_" + TextConvert(LeaderName.Text) + "', 'KIND_LEADER'),\n" +
                // add agendas to the database
                "('TRAIT_AGENDA_" + TextConvert(AuthorName.Text) + "_" + TextConvert(LeaderHistoricalAgendaName.Text) + "', '');\n\n";

            string Leaders = "INSERT INTO Leaders (LeaderType, Name, InheritFrom, SceneLayers)\nVALUES\n" +
                "('LEADER_" + TextConvert(AuthorName.Text) + "_" + TextConvert(LeaderName.Text) + "'," +
                "'LOC_LEADER_" + TextConvert(AuthorName.Text) + "_" + TextConvert(LeaderName.Text) + "_NAME','LEADER_DEFAULT',4)";

            string CivilizationLeaders; //Leaders.xml instead of Civilizations.xml

            string LeaderQuotes = "";

            string LeaderTraits = "";

            string Traits;

            string TraitsModifiers;

            string AiListTypes;

            string Agendas;

            string HistoricalAgendas;

            string AgendaTraits;

            string ExclusiveAgendas;

            string Modifiers;

            string ModifierArguments;

            string ModifierStrings;
        }

        private void CivilizationGenerate_Click(object sender, EventArgs e)
        {
            //Gamedefines file for Civs
            string CivilizationsFileText = "";

            string Types = "INSERT INTO Types (Type, Kind)\nVALUES\n('CIVILIZATION_" + TextConvert(AuthorName.Text) + "_" + TextConvert(CivilizationName.Text) + "', 'KIND_CIVILIZATION');\n\n";
            //Fix after traits

            string CityNames = "INSERT INTO CityNames (CivilizationType, CityName)\nVALUES\n";
            Boolean First = true;
            int CityCount = 0;
            foreach (var word in CityList)
            {
                CityCount++;
                if (!First) CityNames += ",\n";
                First = false;
                CityNames += "('CIVILIZATION_" + TextConvert(AuthorName.Text) + "_" + TextConvert(CivilizationName.Text) + "', " +
                    "'LOC_CITY_NAME_" + TextConvert(AuthorName.Text) + "_" + TextConvert(CivilizationName.Text) + "_" + CityCount + "')";
            }
            CityNames += ";\n\n";

            string CivlizationCitizenNames = "INSERT INTO CivilizationCitizenNames (CivilizationType, CitizenName, Female, Modern)\nVALUES\n";
            CitizenList = CivilizationCitizenNames.Text.Split(';');
            First = true;
            int CitizenCount = 0;
            foreach (var word in CitizenList)
            {
                CitizenCount++;
                if (!First) CivlizationCitizenNames += "),\n";
                First = true;
                CitizenType = word.Split(',');
                CivlizationCitizenNames += "('CIVILIZATION_" + TextConvert(AuthorName.Text) + "_" + TextConvert(CivilizationName.Text) + "', " +
                    "'LOC_CITIZEN_" + TextConvert(AuthorName.Text) + "_" + TextConvert(CivilizationName.Text) + "_" + CitizenCount + "'";
                foreach (var thing in CitizenType)
                {
                    if (!First)
                    {
                        CivlizationCitizenNames += ",";
                        CivlizationCitizenNames += " " + thing;
                    }
                    First = false;
                }
            }
            CivlizationCitizenNames += ");\n\n";

            string Civilizations = "INSERT INTO Civilizations (CivilizationType, Name, Description, Adjective, StartingCivilizationLevelType, RandomCityNameDepth, Ethnicity)\nVALUES\n" +
                "('CIVILIZATION_" + TextConvert(AuthorName.Text) + "_" + TextConvert(CivilizationName.Text) + "', " +
                "'LOC_CIVILIZATION_" + TextConvert(AuthorName.Text) + "_" + TextConvert(CivilizationName.Text) + "_NAME', " +
                "'LOC_CIVILIZATION_" + TextConvert(AuthorName.Text) + "_" + TextConvert(CivilizationName.Text) + "_DESCRIPTION', " +
                "'LOC_CIVILIZATION_" + TextConvert(AuthorName.Text) + "_" + TextConvert(CivilizationName.Text) + "_ADJECTIVE', ";

            if (CityCount < 10) Civilizations += "'CIVILIZATION_LEVEL_FULL_CIV', " + CityCount + ",";
            else Civilizations += "'CIVILIZATION_LEVEL_FULL_CIV', " + 10 + ",";

            if (CivilizationEthnicity.Text == "African") Civilizations += "'ETHNICITY_AFRICAN');\n\n";
            if (CivilizationEthnicity.Text == "Asian") Civilizations += "'ETHNICITY_ASIAN');\n\n";
            if (CivilizationEthnicity.Text == "Mediterranean") Civilizations += "'ETHNICITY_MEDIT');\n\n";
            if (CivilizationEthnicity.Text == "South American") Civilizations += "'ETHNICITY_SOUTHAM');\n\n";
            else Civilizations += "'ETHNICITY_EURO');\n\n";

            string CivilizationInfo = "INSERT INTO CivilizationInfo (CivilizationType, Header, Caption, SortIndex)\nVALUES\n" +
                "('CIVILIZATION_" + TextConvert(AuthorName.Text) + "_" + TextConvert(CivilizationName.Text) + "', " +
                "'LOC_CIVINFO_LOCATION', 'LOC_CIVINFO_" + TextConvert(AuthorName.Text) + "_" + TextConvert(CivilizationName.Text) + "_LOCATION', 10),\n" +
                "('CIVILIZATION_" + TextConvert(AuthorName.Text) + "_" + TextConvert(CivilizationName.Text) + "', 'LOC_CIVINFO_SIZE', " +
                "'LOC_CIVINFO_" + TextConvert(AuthorName.Text) + "_" + TextConvert(CivilizationName.Text) + "_SIZE', 20),\n" +
                "('CIVILIZATION_" + TextConvert(AuthorName.Text) + "_" + TextConvert(CivilizationName.Text) + "', 'LOC_CIVINFO_POPULATION', " +
                "'LOC_CIVINFO_" + TextConvert(AuthorName.Text) + "_" + TextConvert(CivilizationName.Text) + "_POPULATION', 30),\n" +
                "('CIVILIZATION_" + TextConvert(AuthorName.Text) + "_" + TextConvert(CivilizationName.Text) + "', 'LOC_CIVINFO_CAPITAL', " +
                "'LOC_CIVINFO_" + TextConvert(AuthorName.Text) + "_" + TextConvert(CivilizationName.Text) + "_CAPITAL', 40);\n\n";

            string StartBiasResources;

            string StartBiasTerrains;

            string StartBiasFeatures;

            string StartBiasRivers;

            string Traits;

            string TraitModifiers;

            string Modifiers;

            string ModifierArguments;

            string RequireSets;

            string RequireSetRequirements;

            CivilizationsFileText = Types + Civilizations + CityNames + CivlizationCitizenNames + CivilizationInfo;
            Console.WriteLine(CivilizationsFileText);
        }

        public static string TextConvert(string input)
        {
            input = Regex.Replace(input.ToUpper().Trim(), @"\s+", "_");
            return new string(input.Where(c => !char.IsWhiteSpace(c)).ToArray());
        }

        private void CivilizationStartBiasMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            CivilizationStartBiasMinor.Items.Clear();
            CivilizationStartBiasMinor.Enabled = true;
            CivilizationStartBiasesAdd.Enabled = false;
            CivilizationStartBiasesAdd.Text = "Add";
            if (CivilizationStartBiasMain.Text == "Feature")
            {
                foreach (string str in Features)
                {
                    CivilizationStartBiasMinor.Items.Add(str);
                }
            }
            else if (CivilizationStartBiasMain.Text == "Resource")
            {
                foreach (string str in Resources)
                {
                    CivilizationStartBiasMinor.Items.Add(str);
                }
            }
            else if (CivilizationStartBiasMain.Text == "Terrain")
            {
                foreach (string str in Resources)
                {
                    CivilizationStartBiasMinor.Items.Add(str);
                }
            }
            else if (CivilizationStartBiasMain.Text == "Rivers")
            {
                CivilizationStartBiasesAdd.Enabled = true;
                CivilizationStartBiasMinor.Enabled = false;
            }
        }

        private void CivilizationStartBiasMinor_SelectedIndexChanged(object sender, EventArgs e)
        {
            CivilizationStartBiasesAdd.Enabled = true;
            var MatchStartBiases = ActiveStartBiasesList.FirstOrDefault(stringToCheck => stringToCheck.StartsWith(CivilizationStartBiasMain.Text + " " + CivilizationStartBiasMinor.Text));
            if (MatchStartBiases != null)
            {
                CivilizationStartBiasesAdd.Text = "Remove";
            }
            else
            {
                CivilizationStartBiasesAdd.Text = "Add";
            }
        }

        private void CivilizationStartBiasesAdd_Click(object sender, EventArgs e)
        {
            CivilizationStartBiasesActive.Text = "";
            var MatchStartBiases = ActiveStartBiasesList.FirstOrDefault(stringToCheck => stringToCheck.StartsWith(CivilizationStartBiasMain.Text + " " + CivilizationStartBiasMinor.Text));
            if (MatchStartBiases != null)
            {
                ActiveStartBiasesList.Remove(MatchStartBiases);
                CivilizationStartBiasesAdd.Text = "Add";
            }
            else
            {
                ActiveStartBiasesList.Add(CivilizationStartBiasMain.Text + " " + CivilizationStartBiasMinor.Text + "," + CivilizationStartBiasesTier.Value.ToString());
                CivilizationStartBiasesAdd.Text = "Remove";
            }

            ActiveStartBiasesList.Sort();
            ActiveStartBiases = ActiveStartBiasesList.ToArray();
            Boolean First = true;
            foreach (var line in ActiveStartBiases)
            {
                if (!First) CivilizationStartBiasesActive.Text += Environment.NewLine;
                First = false;
                CivilizationStartBiasesActive.Text += line.TrimEnd();
            }
        }

        private void CivilizationCitiesNames_TextChanged(object sender, EventArgs e)
        {
            CityList = CivilizationCitiesNames.Text.Split(';');
            CivilizationCitiesFound.Text = "";
            Boolean First = true;
            int CityCount = 0;
            foreach (var word in CityList)
            {
                CityCount++;
                if (!First) CivilizationCitiesFound.Text += ", ";
                First = false;
                CivilizationCitiesFound.Text += "\"" + word + "\"";
            }
            CivilizationCitiesFoundLabel.Text = "Cities Found " + CityCount;
            if (CivilizationCitiesNames.Text == "")
            {
                CivilizationCitiesFound.Text = "";
                CivilizationCitiesFoundLabel.Text = "Cities Found";
            }
        }

        private void CivilizationCitizenNames_TextChanged(object sender, EventArgs e)
        {
            CitizenList = CivilizationCitizenNames.Text.Split(';');
            CivilizationCitizenFound.Text = "";
            Boolean First = true;
            int CitizenCount = 0;
            foreach (var word in CitizenList)
            {
                CitizenCount++;
                if (!First) CivilizationCitizenFound.Text += ", ";
                First = true;
                CitizenType = word.Split(',');
                CivilizationCitizenFound.Text += "\"";
                foreach (var thing in CitizenType)
                {
                    if (!First) CivilizationCitizenFound.Text += ",";
                    First = false;
                    CivilizationCitizenFound.Text += thing;
                }
                CivilizationCitizenFound.Text += "\"";
            }
            CivilizationCitizenFoundLabel.Text = "Citizen Found " + CitizenCount;
            if (CivilizationCitizenNames.Text == "")
            {
                CivilizationCitizenFound.Text = "";
                CivilizationCitizenFoundLabel.Text = "Citizen Found";
            }
        }
    }
}
