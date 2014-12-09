using ThreesRipoff.Model.Tile;

namespace ThreesRipoff.Model
{
    class Slot
    {
        public readonly int RowIndex;
        public readonly int ColIndex;
        public ITile Tile { get; private set; }

        public Slot(int rowIndex, int colIndex)
        {
            RowIndex = rowIndex;
            ColIndex = colIndex;
        }

        public bool HasTile()
        {
            return Tile != null;
        }

        public void SetTile(ITile tile)
        {
            Tile = tile;
        }

        public void ClearTile()
        {
            Tile = null;
        }

        protected bool Equals(Slot other)
        {
            return RowIndex == other.RowIndex
                && ColIndex == other.ColIndex;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = RowIndex;
                hashCode = (hashCode*397) ^ ColIndex;
                return hashCode;
            }
        }

        public TransferResult TransferTo(Slot toSlot)
        {
            if (!HasTile())
            {
                return TransferResult.NoOp;
            }
            if (!toSlot.HasTile())
            {
                toSlot.SetTile(Tile);
				ClearTile();
                return TransferResult.Move;
            }
			var combinedTile = toSlot.Tile.Combine(Tile);
            if (combinedTile == null)
            {
                return TransferResult.NoOp;
            }

			toSlot.SetTile(combinedTile);
			ClearTile();
			return TransferResult.Combination;
        }

        public string CoordString()
        {
            return RowIndex + "," + ColIndex;
        }
    }
}
