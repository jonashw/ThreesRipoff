namespace ThreesRipoff.Model.Tile
{
    public interface ITile
    {
        int Score { get; }
        int FaceValue { get; }
        bool IsThrees { get; }
        ITile Combine(ITile other);
        bool CanCombineWith(ITile tile);
    }
}