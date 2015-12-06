using System;
using static System.String;

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

        public Move(string move, PieceColour mover)
        {
            Mover = mover;
            if (Compare(move, "0-0", StringComparison.OrdinalIgnoreCase) == 0)
            {
                string rank = Mover == PieceColour.White ? "1" : "8";
                InitialPosition = new Position("e" + rank);
                FinalPosition = new Position("g" + rank);
            }
            if (Compare(move, "0-0-0", StringComparison.OrdinalIgnoreCase) == 0)
            {
                string rank = Mover == PieceColour.White ? "1" : "8";
                InitialPosition = new Position("e" + rank);
                FinalPosition = new Position("c" + rank);
            }
        }

        public Position InitialPosition { get; private set; }
        public Position FinalPosition { get; private set; }
        public Piece PieceMoved { get; set; }
        public PieceColour Mover { get; private set; }
        public bool HasMoved { get; set; }
        public Piece PieceTaken { get; set; }
        public bool EnPassant { get; set; }
    }
}