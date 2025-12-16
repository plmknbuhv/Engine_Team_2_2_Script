namespace Code.Units.UnitAnimals.Pigs
{
    public class Pig : FriendlyUnit
    {
        public bool IsEaten { get; set; }

        public override void SetUpUnit()
        {
            base.SetUpUnit();
            IsEaten = false;
        }
    }
}