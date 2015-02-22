namespace Cardnell.Chess.Engine
{
    public class Move
    {
        public Move(Position initialPosition,
            Position finalPosition,
            PieceColour mover,
            Piece pieceMoved,
            Piece pieceTaken)
        {
            InitialPosition = initialPosition;
            FinalPosition = finalPosition;
            PieceMoved = pieceMoved;
            PieceTaken = pieceTaken;
            Mover = mover;
        }

        public Position InitialPosition { get; private set; }
        public Position FinalPosition { get; private set; }
        public Piece PieceMoved { get; set; }
        public PieceColour Mover { get; private set; }
        public bool HasMoved { get; set; }
        public Piece PieceTaken { get; set; }
    }
}