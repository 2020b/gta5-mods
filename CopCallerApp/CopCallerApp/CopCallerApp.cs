using System;
using System.Drawing;
using System.Windows.Forms;
using GTA;
using System.Collections.Generic;

namespace CopCallerApp {
    public class CopCallerApp : Script {
        public enum MenuState { OPEN, CLOSED }
        public MenuState menuState = MenuState.CLOSED;

        public CopCallerApp() {
            Tick += onTick;
            KeyUp += onKeyUp;
            KeyDown += onKeyDown;
            Team.loadAll();
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
                            new TeamSpawnerButton("Spawn " + name + " team", "", this.onTeamSelected)
                        );
                    });
                    GTA.Menu menu = new GTA.Menu("Cop Caller App", menuItems.ToArray());
                    menu.HeaderColor = Color.DarkBlue;
                    menu.SelectedItemColor = Color.Aqua;
                    menu.FooterColor = Color.White;
                    View.AddMenu(menu);
                    this.menuState = MenuState.OPEN;
                } else {
                    View.CloseAllMenus();
                    this.menuState = MenuState.CLOSED;
                }
            } else if (e.KeyCode == Keys.L) {
                UI.Notify("Reloading configuration file.");
                Team.loadAll();
            }
        }

        public void onTeamSelected(string buttonText) {
            string teamName = buttonText.Substring(buttonText.IndexOf(" ") + 1, buttonText.LastIndexOf(" ") - buttonText.IndexOf(" ") - 1);
            UI.Notify("Spawning a " + teamName + " team");
            Team.getByName(teamName).spawnTeam();
        }

        private void onKeyDown(object sender, KeyEventArgs e) { }
    }
}