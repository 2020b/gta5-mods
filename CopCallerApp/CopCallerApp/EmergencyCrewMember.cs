using GTA;
using System;
using GTA.Native;
using System.Collections.Generic;

namespace CopCallerApp {
    public class EmergencyCrewMember {
        protected Model m;
        protected List<string> weapons = new List<string>();
        protected List<int> ammoCounts = new List<int>();
        protected bool isPoliceOfficer;

        public EmergencyCrewMember(string model, bool isPoliceOfficer = true) {
            int mHash = Function.Call<int>(Hash.GET_HASH_KEY, model);
            Function.Call(Hash.REQUEST_MODEL, mHash);
            this.m = new Model(model);
            this.isPoliceOfficer = isPoliceOfficer;
        }

        public void addWeapons(List<string> weapons) {
            this.weapons = weapons;
        }

        public void addWeaponAmmoCounts(List<int> ammoCounts) {
            this.ammoCounts = ammoCounts;
        }

        public void spawnIntoVehicle(EmergencyVehicle v, int seat) {
            Vehicle copCar = v.getVehicle();
            // Add the officer into the car & do the setup work
            Function.Call(Hash.CREATE_PED_INSIDE_VEHICLE, copCar.Handle, 26, this.m.Hash, seat, true, true);
            Ped crewMember = Function.Call<Ped>(Hash.GET_PED_IN_VEHICLE_SEAT, copCar.Handle, seat);
            for (int i = 0; i < this.weapons.Count; i++) {
                try {
                    // converts the string in the config file to a weapon hash
                    uint wp_hash = (uint) Function.Call<int>(Hash.GET_HASH_KEY, this.weapons[i]);
                    crewMember.Weapons.Give((WeaponHash)wp_hash, this.ammoCounts[i], true, true);
                } catch (Exception e) {
                        UI.Notify(Messages.get("app-something-went-wrong", new string[] { e.Message }));
                }
            }

            if (this.isPoliceOfficer) {
                // We're police officers, after all.
                Function.Call(Hash.SET_PED_AS_COP, crewMember, true);
            }
            crewMember.IsPersistent = false;
        }
    }
}