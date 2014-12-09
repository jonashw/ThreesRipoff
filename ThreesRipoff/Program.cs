using System;
using System.Linq;
using ThreesRipoff.Model;
using ThreesRipoff.Model.Tile;

namespace ThreesRipoff
{
    class Program
    {
        private static Board Board;
		private static ITile NextTile;
        public static void Main(string[] args)
        {
			Console.Clear();
            Board = new Board();
            var seedTileCount = new Random().Next(4,6);
            var seedTiles = TileFactory.Random(seedTileCount).ToList();
            var seedSlots = Board.RandomSlots(seedTileCount).ToList();
            foreach (var tileSlotPair in seedTiles.Zip(seedSlots, Tuple.Create))
            {
				tileSlotPair.Item2.SetTile(tileSlotPair.Item1);
            }

            var playing = true;

            while (playing)
            {
                NextTile = TileFactory.Random();
				printBoard(Board);
				var key = Console.ReadKey();
				Console.Clear();
                if (!Board.CanMove())
                {
                    playing = false;
                    gameOverMessage();
                    return;
                }
				switch (key.Key)
				{
					case ConsoleKey.LeftArrow:
						move(Direction.Left);
						break;
					case ConsoleKey.RightArrow:
						move(Direction.Right);
						break;
					case ConsoleKey.UpArrow:
						move(Direction.Up);
						break;
					case ConsoleKey.DownArrow:
						move(Direction.Down);
						break;
					case ConsoleKey.Escape:
				        playing = false;
				        exitMessage();
						break;
				}
            }
        }

        private static void gameOverMessage()
        {
			Console.WriteLine("GAME OVER");
			Console.WriteLine("");
			Console.WriteLine("Your score:");
            var scoreLegend = Board.GetAllSlots()
                .Select(s => Tuple.Create(s.Tile.FaceValue, s.Tile.Score)).ToList().AsReadOnly();
            var totalScore = scoreLegend.Sum(p => p.Item2);
            foreach (var pair in scoreLegend)
            {
                Console.WriteLine("\t" + pair.Item1.ToString().PadLeft(4) + " -> " + pair.Item2.ToString().PadLeft(4));
            }
			Console.WriteLine("\t" + "".PadRight(12,'-'));
			Console.WriteLine("\tTotal: " + totalScore.ToString().PadLeft(5));
			Console.WriteLine();
			Console.WriteLine("Press escape to quit. Press any other key to play again.");
            var key = Console.ReadKey();
			switch (key.Key)
			{
				case ConsoleKey.Escape:
					break;
				default:
					Main(new string[]{});
			        break;
			}
        }

        private static void exitMessage()
        {
			Console.WriteLine("You quit early. BOOOO");
        }

        private static void move(Direction direction)
        {
            var result = Board.Move(direction,NextTile);
            Console.WriteLine("Attempting to move " + direction);
            Console.WriteLine("Move result: " + result);
        }

        private static void printBoard(Board board)
        {
            var rowsText = board.Rows.Select(row =>
                String.Join("",row.Select(slotText)));

            foreach (var rowText in rowsText)
            {
				Console.WriteLine(rowText);
            }
			Console.WriteLine();
			Console.WriteLine("Next Tile: " + tileText(NextTile));
        }

		const int TileWidthInChars = 4;
        private static readonly string EmptyTileText = String.Join("", Enumerable.Range(0, TileWidthInChars).Select(i => ' '));
        private static string slotText(Slot slot)
        {
            return slot.HasTile()
                ? tileText(slot.Tile)
                : EmptyTileText;
        }

        private static string tileText(ITile tile)
        {
            return tile.FaceValue.ToString().PadRight(TileWidthInChars);
        }
    }
}