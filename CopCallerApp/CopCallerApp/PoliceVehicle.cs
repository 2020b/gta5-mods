using GTA;

namespace CopCallerApp {
    public class PoliceVehicle {
        protected Vehicle vehicle;
        protected static GTA.Math.Vector3 spawnPos;

        public PoliceVehicle(string model) {
            PoliceVehicle.spawnPos = Game.Player.Character.Position + (Game.Player.Character.ForwardVector * 60.0f);
            vehicle = World.CreateVehicle(model, PoliceVehicle.spawnPos);
            // so it will unspawn like a normal cop car
            vehicle.IsPersistent = false;
        }

        public Vehicle getVehicle() {
            return vehicle;
        }

        public void spawn() {
            vehicle.PlaceOnNextStreet();
            vehicle.SirenActive = true;
            // Drive to the player. BEWARE it might run over you lol
            vehicle.GetPedOnSeat(VehicleSeat.Driver).Task.DriveTo(vehicle, Game.Player.Character.Position, 10, 50);
            PoliceVehicle.spawnPos += Game.Player.Character.ForwardVector * 20.0f;
        }
    }
}
