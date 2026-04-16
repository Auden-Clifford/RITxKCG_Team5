namespace Barrel
{
    public class ThrowableBarrel : BarrelBase
    {
        protected override void Start()
        {
            base.Start();
        }

        // not allowing to move on throwable barrels, as it will be thrown
        protected override void Move() { }
    }
}