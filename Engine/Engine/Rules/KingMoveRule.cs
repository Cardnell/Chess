using System;
using System.Collections.Generic;

namespace Cardnell.Chess.Engine.Rules
{
    public class KingMoveRule : IMoveRule
    {
        public bool IsMoveLegal(Move move, IBoard board, IList<Move> moves)
        {
            return Math.Abs(move.InitialPosition.Rank - move.FinalPosition.Rank) <= 1
                   && Math.Abs(move.InitialPosition.File - move.FinalPosition.File) <= 1;
        }
    }
}