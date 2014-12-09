namespace ThreesRipoff.Model.Tile
{
    public abstract class AbstractTile
    {
        protected abstract bool CanCombineWith(ITile other, out ITile combined);

        public ITile Combine(ITile other)
        {
            ITile combined;
            CanCombineWith(other, out combined);
            return combined;
        }

        public bool CanCombineWith(ITile other)
        {
            ITile combined;
            return CanCombineWith(other, out combined);
        }
    }
}
