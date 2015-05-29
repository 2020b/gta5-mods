using System.Collections.Generic;
using CopCallerApp;
using GTA;
using System;
using System.Xml.Linq;
using System.Linq;

namespace CopCallerApp {
    public class Team {
        public string vehicleModel { get; set; }
        protected List<String> memberModels = new List<String>();
        protected List<string> weapons = new List<string>();
        protected List<int> ammoCounts = new List<int>();
        protected bool isPoliceTeam;

        public string name { get; set; }

        protected static List<Team> teams = new List<Team>();
        protected static List<string> teamNames = new List<string>();

        public static Team getByName(string name) {
            List<string> tnames = Team.teamNames;
            int i = 0;
            while (i < tnames.Count && !(tnames[i] == name)) i++;
            return (i < tnames.Count ? Team.teams[i] : null);
        }

        public static List<string> getTeamNames() {
            return Team.teamNames;
        }

        public static void clearAll() {
            Team.teams = new List<Team>();
            Team.teamNames = new List<string>();
        }

        public static void add(Team t) {
            Team.teams.Add(t);
            Team.teamNames.Add(t.name);
        }

        public void addWeapon(string weaponName, int ammoCount) {
            this.weapons.Add(weaponName);
            this.ammoCounts.Add(ammoCount);
        }

        public void addCrewModel(string model) {
            this.memberModels.Add(model);
        }

        public void setIsPoliceTeam(bool yes) {
            this.isPoliceTeam = yes;
        }

        public void spawnTeam() {
            EmergencyVehicle vehicle = new EmergencyVehicle(this.vehicleModel);
            Vehicle v = vehicle.getVehicle();
            Random rnd = new Random();
            for (int seat = -1; seat <= v.PassengerSeats; seat++) {
                EmergencyCrewMember p = new EmergencyCrewMember(this.memberModels[rnd.Next(this.memberModels.Count)]);
                p.addWeapons(this.weapons);
                p.addWeaponAmmoCounts(this.ammoCounts);
                p.spawnIntoVehicle(vehicle, seat);
            }
            vehicle.spawn();
        }
    }
}
