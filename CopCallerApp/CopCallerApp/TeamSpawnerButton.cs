using GTA;
using System;

namespace CopCallerApp {
    public class TeamSpawnerButton : MenuButton {
        protected Action<string> action;
        protected string caption;
        public TeamSpawnerButton(string caption, string description, Action<string> activationAction) : base(caption, description, () => {}) {
            this.action = activationAction;
            this.caption = caption;
        }

        public override void Activate() {
            base.Activate();
            action.Invoke(this.caption);
        }
    }
}
