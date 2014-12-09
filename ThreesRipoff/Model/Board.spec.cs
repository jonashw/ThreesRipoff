using System.Linq;
using NUnit.Framework;
using ThreesRipoff.Model.Tile;

namespace ThreesRipoff.Model
{
    [TestFixture]
    public class BoardSpec
    {
        [Test]
        public void TestCanMove()
        {
            var board = new Board();
            foreach (var slot in board.GetAllSlots())
            {
                slot.SetTile(PrimitiveTile.One);
            }
			Assert.AreEqual(false, board.CanMove(),"CanMove()");
			board.Rows.First().First().ClearTile();
			Assert.AreEqual(true, board.CanMove(),"CanMove()");
        }

        [Test]
        public void TestCanMoveTwoByTwo()
        {
            var board = new Board(2);
			board.Rows[0][0].SetTile(PrimitiveTile.One);
			board.Rows[1][0].SetTile(new ThreesTile(1));
			board.Rows[0][1].SetTile(new ThreesTile(0));
			board.Rows[1][1].SetTile(PrimitiveTile.One);
			Assert.AreEqual(false, board.CanMove(),"CanMove()");
        }
    }
}
