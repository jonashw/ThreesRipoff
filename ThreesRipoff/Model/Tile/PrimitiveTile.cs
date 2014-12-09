using System.Globalization;

namespace ThreesRipoff.Model.Tile
{
    public class PrimitiveTile : AbstractTile, ITile
    {
        public readonly static PrimitiveTile One = new PrimitiveTile(1);
        public readonly static PrimitiveTile Two = new PrimitiveTile(2);
		//
        protected readonly int N;
        private PrimitiveTile(int n)
        {
            N = n;
        }

        public int Score
        {
            get { return 0; }
        }

        public int FaceValue
        {
            get { return N; }
        }

        public bool IsThrees
        {
            get { return false; }
        }

		protected override bool CanCombineWith(ITile other, out ITile combined)
		{
			combined = null;
            if (other.IsThrees)
            {
                return false;
            }
			var otherPrimitive = (PrimitiveTile) other;
		    if ((otherPrimitive.N + N) != 3)
		    {
		        return false;
		    }
			combined = new ThreesTile(0);
			return true;
		}

        public override string ToString()
        {
            return FaceValue.ToString(CultureInfo.InvariantCulture);
        }
    }
}
