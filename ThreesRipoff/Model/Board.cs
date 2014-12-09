using System;
using System.Collections.Generic;
using System.Linq;
using ThreesRipoff.Model.Tile;

namespace ThreesRipoff.Model
{
    class Board
    {
        public readonly IReadOnlyList<IReadOnlyList<Slot>> Rows;
        public readonly IReadOnlyList<IReadOnlyList<Slot>> Cols;
        public readonly int BoardSize;
        public Board(int boardSize = 4)
        {
            BoardSize = boardSize;
            var rowIndexes = Enumerable.Range(0, boardSize).ToList().AsReadOnly();
            var colIndexes = rowIndexes;

            Rows = rowIndexes.Select(rowIndex =>
                colIndexes.Select(colIndex =>
                    new Slot(rowIndex, colIndex)).ToList()).ToList();

            Cols = colIndexes.Select(colIndex =>
                Rows.Select(row => row[colIndex]).ToList()).ToList();
        }

        public IEnumerable<Slot> RandomSlots(int slotCount)
        {
            var slots = new HashSet<Slot>();
            while (slots.Count() < slotCount)
            {
                slots.Add(RandomSlot());
            }
            return slots;
        }

		private readonly Random _random = new Random();
        public Slot RandomSlot()
        {
            var rowIndex = _random.Next(0, Rows.Count);
            var colIndex = _random.Next(0, Cols.Count);
            return Rows[rowIndex][colIndex];
        }

        public bool CanMove()
        {
            if (GetAllSlots().Any(s => !s.HasTile())) //any slots are empty
            {
                return true;
            }
		   //or all slots are full ... at least one pair of neighbors can combine
            var combinablePairs = getAllNeighbors().Where(pair =>
                pair.Item1.Tile.CanCombineWith(pair.Item2.Tile));
            return combinablePairs.Any();
        }

        private IEnumerable<Tuple<Slot, Slot>> getAllNeighbors()
        {
            var neighbors = GetAllSlots().SelectMany(slot =>
				new List<Tuple<int,int>>
				{
                    Tuple.Create(slot.RowIndex, slot.ColIndex + 1),
                    Tuple.Create(slot.RowIndex + 1, slot.ColIndex)
				}.Where(p => p.Item1 < BoardSize && p.Item2 < BoardSize)
				.Select(p => Tuple.Create(
                    slot,
                    Rows[p.Item1][p.Item2])));
            return neighbors;
        }

        public IEnumerable<Slot> GetAllSlots()
        {
            return Rows.SelectMany(row => row);
        } 

        public MoveResult Move(Direction direction, ITile nextTile)
        {
            var channels = directionalChannels(direction);
            var positive = DirectionUtil.Positive(direction);
            var success = channels
                .Select(channel => moveChannel(channel, positive))
				.ToList()
                .Any(moved => moved);
            if (!success)
            {
				return MoveResult.Failure;
            }
            var insertionSlot = getNextInsertionSlot(direction);
            if (insertionSlot == null)
            {
                return MoveResult.GameOver;
            }
			insertionSlot.SetTile(nextTile);   
            return MoveResult.Success;
        }

		private readonly Dictionary<Direction,Slot> _lastDirectionSlotDict = new Dictionary<Direction, Slot>();
        private Slot getNextInsertionSlot(Direction direction)
        {
			//consecutive moves in one direction should yield the same insertionSlot each time, if the slot can hold a new tile
            if (_lastDirectionSlotDict.ContainsKey(direction))
            {
				//reset all other directions
                foreach (var key in _lastDirectionSlotDict.Keys.Where(k => k != direction).ToList())//materialize to avoid mutation errors
                {
                    _lastDirectionSlotDict.Remove(key);
                }
                var slot = _lastDirectionSlotDict[direction];
                if (slot.HasTile())
                {
                    _lastDirectionSlotDict.Remove(direction);
                }
                else
                {
                    return slot;
                }
            }
            var positive = DirectionUtil.Positive(direction);
            var candidateSlots = directionalChannels(direction)
                .Select(channel => (positive ? channel : channel.Reverse()).First())
                .Where(slot => !slot.HasTile()).ToList().AsReadOnly();
            if (!candidateSlots.Any())
            {
                return null;
            }
            var freshSlot = randomItem(candidateSlots);
            _lastDirectionSlotDict[direction] = freshSlot;
            return freshSlot;
        }

        public enum MoveResult
        {
            Success,Failure,GameOver
        }

        private static readonly Random Rand = new Random();
        private static T randomItem<T>(IEnumerable<T> items)
        {
            var xs = items.ToArray();
            var index = Rand.Next(0, xs.Length - 1);
            return xs[index];
        }

        private static bool moveChannel(IEnumerable<Slot> channel, bool positive)
        {
            var chan = (positive ? channel : channel.Reverse()).ToArray();
            var success = false;
            for (var i = chan.Length - 1; i > 0; i--)
            {
                var transferResult = chan[i-1].TransferTo(chan[i]);
                if (transferResult == TransferResult.Move || transferResult == TransferResult.Combination)
                {
                    success = true;
                }
            }
            return success;
        }
		//
        private IEnumerable<IReadOnlyList<Slot>> directionalChannels(Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                case Direction.Down:
                    return Cols;
				case Direction.Left:
				case Direction.Right:
                    return Rows;
            }
			//execution should never get this far:
            return new List<IReadOnlyList<Slot>>();
        }
    }
}