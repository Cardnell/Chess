using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cardnell.Chess.Engine
{
    public class Move
    {
        public Move(Position initialPosition, Position finalPosition, PieceColour mover, Piece pieceMoved, bool tookPeice)
        {
            InitialPosition = initialPosition;
            FinalPosition = finalPosition;
            PieceMoved = pieceMoved;
            TookPeice = tookPeice;
            Mover = mover;
        }

        public Position InitialPosition { get; private set; }
        public Position FinalPosition { get; private set; }
        public Piece PieceMoved { get; private set; }
        public bool TookPeice { get; private set; }
        public PieceColour Mover { get; private set; }

    }
}
