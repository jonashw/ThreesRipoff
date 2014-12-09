using System.Globalization;
using NUnit.Framework;

namespace ThreesRipoff.Model.Tile
{
    public class ThreesTile : AbstractTile, ITile
    {
        protected readonly int N;
        public ThreesTile(int n)
        {
            N = n;
        }

        public int Score
        {
            get { return MathUtil.IntPow(3, N + 1); }
        }

        public int FaceValue
        {
            get { return 3 * MathUtil.IntPow(2, N); }
        }

        public bool IsThrees
        {
            get { return true; }
        }

		protected override bool CanCombineWith(ITile other, out ITile combined)
		{
			combined = null;
            if (!other.IsThrees)
            {
                return false;
            }
			var otherThrees = (ThreesTile) other;
		    if(otherThrees.N != N)
		    {
		        return false;
		    }
			combined = new ThreesTile(N + 1);
			return true;
		}

        public override string ToString()
        {
            return FaceValue.ToString(CultureInfo.InvariantCulture);
        }
    }
}