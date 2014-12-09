using System;
using System.Collections.Generic;
using System.Linq;
using ThreesRipoff.Model.Tile;

namespace ThreesRipoff.Model.Tile
{
    public static class TileFactory
    {
        public static IEnumerable<ITile> Random(int tileCount)
        {
            return Enumerable.Range(0, tileCount).Select(i => Random());
        }

        public static ITile Random()
        {
            return randomBool()
                ? randomThrees() as ITile
                : randomPrimitive();
        }

        private static ThreesTile randomThrees()
        {
			/*
				3  -> 80%
				6  -> 12%
				12 -> 5%
				24 -> 2%
				48 -> 1%
			*/
            var r = Rand.NextDouble();
            var n = r < 0.85
                ? 0
                : r < 0.97
                    ? 1
                    : r < 0.99
                        ? 2
                        : 3;
			return new ThreesTile(n);
        }

        private static PrimitiveTile randomPrimitive()
        {
            return randomBool()
                ? PrimitiveTile.One
				: PrimitiveTile.Two;
        }

        private static bool randomBool()
        {
            return Rand.NextDouble() < 0.5;
        }
		private readonly static Random Rand = new Random();
    }
}