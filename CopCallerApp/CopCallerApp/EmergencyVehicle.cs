using GTA;

namespace CopCallerApp {
    public class EmergencyVehicle {
        protected Vehicle vehicle;
        protected static GTA.Math.Vector3 spawnPos;
        public static float DISTANCE_MULTIPLIER = 60.0f;

        public EmergencyVehicle(string model) {
            EmergencyVehicle.spawnPos = Game.Player.Character.Position + (Game.Player.Character.ForwardVector * DISTANCE_MULTIPLIER);
            vehicle = World.CreateVehicle(model, EmergencyVehicle.spawnPos);
            // so it will unspawn like a normal emergency vehicle
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
        }
    }
}
