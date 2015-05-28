using System.Collections.Generic;
using CopCallerApp;
using GTA;
using System;
using System.Xml.Linq;
using System.Linq;

namespace CopCallerApp {
    public class Team {
        protected const string CONFIG_FILE = "CopCallerApp.xml";

        string vehicleModel { get; set; }
        protected List<String> officerModels = new List<String>();
        protected List<string> weapons = new List<string>();
        protected List<int> ammoCounts = new List<int>();

        public string name { get; set; }

        protected static List<Team> teams = new List<Team>();
        protected static List<string> teamNames = new List<string>();

        public void spawnTeam() {
            PoliceVehicle vehicle = new PoliceVehicle(this.vehicleModel);
            Vehicle v = vehicle.getVehicle();
            Random rnd = new Random();
            for (int seat = -1; seat <= v.PassengerSeats; seat++) {
                PoliceOfficer p = new PoliceOfficer(this.officerModels[rnd.Next(this.officerModels.Count)]);
                p.addWeapons(this.weapons);
                p.addWeaponAmmoCounts(this.ammoCounts);
                p.spawnIntoVehicle(vehicle, seat);
            }
            vehicle.spawn();
        }

        public static Team getByName(string name) {
            List<string> tnames = Team.teamNames;
            int i = 0;
            while (i < tnames.Count && !(tnames[i] == name)) i++;
            return (i < tnames.Count ? Team.teams[i] : null);
        }

        public static List<string> getTeamNames() {
            return Team.teamNames;
        }

        public static void loadAll() {
            try {
                Team.teams = new List<Team>();
                XElement data = XElement.Load(CONFIG_FILE);
                IEnumerable<XElement> teams = data.Descendants("team");

                foreach (XElement team in teams) {
                    Team t = new Team();
                    // :|
                    t.name = new List<XElement>(team.Descendants("name")).First().Value;
                    Team.teamNames.Add(t.name);
                    t.vehicleModel = new List<XElement>(team.Descendants("vehicle")).First().Value;
                    IEnumerable<XElement> officers = team.Descendants("officer");
                    foreach (XElement cop in officers) {
                        t.officerModels.Add(new List<XElement>(cop.Descendants("model")).First().Value);
                        IEnumerable<XElement> weapons = cop.Descendants("weapon");
                        foreach (XElement weapon in weapons) {
                            t.weapons.Add(new List<XElement>(weapon.Descendants("type")).First().Value);
                            t.ammoCounts.Add(int.Parse(new List<XElement>(team.Descendants("ammoCount")).First().Value));
                        }
                    }
                    Team.teams.Add(t);
                }
                UI.Notify("Team data loading complete. All is well.");
            } catch (Exception e) {
                UI.Notify(e.Message);
            }
        }
    }
}
