using System;
using System.Drawing;
using System.Windows.Forms;
using GTA;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;

namespace CopCallerApp {
    public class CopCallerApp : Script {
        public enum MenuState { OPEN, CLOSED }
        public MenuState menuState = MenuState.CLOSED;

        public const string CONFIG_FILE = "mod_config/CopCallerApp.xml";

        public CopCallerApp() {
            Tick += onTick;
            KeyUp += onKeyUp;
            KeyDown += onKeyDown;
            this.loadConfig();
        }

        private void onTick(object sender, EventArgs e) {
        }

        private void onKeyUp(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.T) {
                if (this.menuState == MenuState.CLOSED) {
                    List<GTA.MenuItem> menuItems = new List<GTA.MenuItem>();
                    List<string> tnames = Team.getTeamNames();
                    tnames.ForEach((string name) => {
                        menuItems.Add(
                            new TeamSpawnerButton(name,
                                Messages.get("app-button-spawn-label", new string[] {name}), this.onTeamSelected
                            )
                        );
                    });
                    GTA.Menu menu = new GTA.Menu(Messages.get("app-menu-title"), menuItems.ToArray());
                    menu.HeaderColor = Color.DarkBlue;
                    menu.SelectedItemColor = Color.Aqua;
                    menu.FooterColor = Color.White;
                    menu.FooterHeight = 5;
                    View.AddMenu(menu);
                    this.menuState = MenuState.OPEN;
                } else {
                    View.CloseAllMenus();
                    this.menuState = MenuState.CLOSED;
                }
            } else if (e.KeyCode == Keys.L) {
                UI.Notify(Messages.get("app-notify-config-reload"));
                this.loadConfig();
            }
        }

        public void onTeamSelected(string teamName) {
            UI.Notify(Messages.get("app-notify-team-spawned", new string[] { teamName }));
            Team.getByName(teamName).spawnTeam();
        }

        private void onKeyDown(object sender, KeyEventArgs e) { }

        private void loadConfig() {
            try {
                Messages.purge();
                Team.clearAll();
                XElement data = XElement.Load(CONFIG_FILE);
                IEnumerable<XElement> teams = data.Descendants("team");

                foreach (XElement team in teams) {
                    Team t = new Team();
                    // :|
                    t.name = new List<XElement>(team.Descendants("name")).First().Value;
                    t.vehicleModel = new List<XElement>(team.Descendants("vehicle")).First().Value;
                    t.setIsPoliceTeam(new List<XElement>(team.Descendants("isPoliceTeam")).First().Value.ToLower() == "yes");
                    IEnumerable<XElement> crewMembers = team.Descendants("officer");
                    foreach (XElement member in crewMembers) {
                        t.addCrewModel(new List<XElement>(member.Descendants("model")).First().Value);
                        IEnumerable<XElement> weapons = member.Descendants("weapon");
                        foreach (XElement weapon in weapons) {
                            t.addWeapon(
                                new List<XElement>(weapon.Descendants("type")).First().Value,
                                int.Parse(new List<XElement>(team.Descendants("ammoCount")).First().Value)
                            );
                        }
                    }
                    Team.add(t);
                }

                // Load miscellaneous parameters
                EmergencyVehicle.DISTANCE_MULTIPLIER = float.Parse(new List<XElement>(data.Descendants("distanceMultiplier")).First().Value);
                Messages.LANG_CODE = new List<XElement>(data.Descendants("language")).First().Value;
                UI.Notify(Messages.get("app-notify-loading-finished"));
            } catch (Exception e) {
                UI.Notify(Messages.get("app-something-went-wrong", new string[] { e.Message }));
            }
        }
    }
}