using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

namespace CopCallerApp {
    public class Messages {
        private static Messages instance = null;
        public static string LANG_CODE;
        private Dictionary<string, string> messages = new Dictionary<string, string>();

        private Messages() {
            string locFile = "mod_config/"  +(File.Exists("mod_config/" + LANG_CODE + ".xml") ? LANG_CODE + ".xml" : "en.xml");
            XElement data = XElement.Load(locFile);
            IEnumerable<XElement> msgs = data.Descendants("message");
            foreach (XElement message in msgs) {
                string key = message.Attribute("key").Value;
                string value = message.Value;
                this.messages.Add(key, value);
            }
        } 

        public static Messages getInstance() {
            if (Messages.instance == null) {
                Messages.instance = new Messages();
            }

            return Messages.instance;
        }

        public string getMessage(string key, string[] arguments = null) {
            string messageText = (this.messages.ContainsKey(key) ? this.messages[key] : "");
            if (arguments != null) {
                for (int i = 0; i < arguments.Length; i++) {
                    messageText = messageText.Replace("$" + (i + 1), arguments[i]);
                }
            }
            return messageText;
        }

        public static string get(string key, string[] arguments = null) {
            return Messages.getInstance().getMessage(key, arguments);
        }

        public static void purge() {
            Messages.instance = null;
        }
    }
}
