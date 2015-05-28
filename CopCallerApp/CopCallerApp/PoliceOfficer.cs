using GTA;
using System;
using GTA.Native;
using System.Collections.Generic;

namespace CopCallerApp {
    public class PoliceOfficer {
        protected Model m;
        protected List<string> weapons = new List<string>();
        protected List<int> ammoCounts = new List<int>();

        public PoliceOfficer(string model) {
            this.m = new Model(model);
        }

        public void addWeapons(List<string> weapons) {
            this.weapons = weapons;
        }

        public void addWeaponAmmoCounts(List<int> ammoCounts) {
            this.ammoCounts = ammoCounts;
        }

        public void spawnIntoVehicle(PoliceVehicle v, int seat) {
            Vehicle copCar = v.getVehicle();
            // Add the officer into the car & do the setup work
            Function.Call(Hash.CREATE_PED_INSIDE_VEHICLE, copCar.Handle, 26, this.m.Hash, seat, true, true);
            Ped officer = Function.Call<Ped>(Hash.GET_PED_IN_VEHICLE_SEAT, copCar.Handle, seat);
            for (int i = 0; i < this.weapons.Count; i++) {
                try {
                    // converts the string in the config file to a weapon hash
                    uint wp_hash = (uint) Function.Call<int>(Hash.GET_HASH_KEY, this.weapons[i]);
                    officer.Weapons.Give((WeaponHash)wp_hash, this.ammoCounts[i], true, true);
                } catch (Exception e) {
                        UI.Notify("GEBASZ VAN: " + e.Message);
                }
            }
            // We're police officers, after all.
            Function.Call(Hash.SET_PED_AS_COP, officer, true);
            officer.IsPersistent = false;
        }
    }
}