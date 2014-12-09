namespace ThreesRipoff.Model
{
    public enum Direction
    {
		Up, Down, Left, Right
    }

	public static class DirectionUtil
	{
		public static bool Positive(Direction direction)
		{
			switch (direction)
			{
				case Direction.Down:
				case Direction.Right:
					return true;
				default:
					return false;
			}
		}
	}
}
