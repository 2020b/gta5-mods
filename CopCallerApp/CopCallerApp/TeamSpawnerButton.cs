using GTA;
using System;

namespace CopCallerApp {
    public class TeamSpawnerButton : MenuButton {
        protected Action<string> action;
        protected string caption, teamName;
        public TeamSpawnerButton(string teamName, string caption, Action<string> activationAction) : base(caption, "", () => {}) {
            this.action = activationAction;
            this.caption = caption;
            this.teamName = teamName;

        }

        public override void Activate() {
            base.Activate();
            action.Invoke(this.teamName);
        }
    }
}
