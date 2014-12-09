using NUnit.Framework;

namespace ThreesRipoff.Model.Tile
{
        [TestFixture]
        public class TileSpec
        {
            [Test]
            public void TestCanCombineWith()
            {
                var a = new ThreesTile(0);
				test(a, new ThreesTile(0), true);
				test(a, new ThreesTile(1), false);
				test(a, new ThreesTile(2), false);
				test(a, new ThreesTile(3), false);
				test(a, PrimitiveTile.One, false);
				test(a, PrimitiveTile.Two, false);
            }

            private void test(ITile thisTile, ITile otherTile, bool canCombine)
            {
				Assert.AreEqual(canCombine, thisTile.CanCombineWith(otherTile), thisTile.FaceValue + " + " + otherTile + " == " + canCombine);
				Assert.AreEqual(canCombine, otherTile.CanCombineWith(thisTile), otherTile.FaceValue + " + " + thisTile + " == " + canCombine);
            }
        }
}
