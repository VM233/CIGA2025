namespace RoomPuzzle
{
    public enum ElementInteractionMode
    {
        None = 0,
        TryEnter = 1,
        Keyed = 2,
        Overlap = 4,
        All = TryEnter | Keyed | Overlap
    }
}