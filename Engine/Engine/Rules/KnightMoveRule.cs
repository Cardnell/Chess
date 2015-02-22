using System;
using System.Collections.Generic;

namespace Cardnell.Chess.Engine.Rules
{
    public class KnightMoveRule : IMoveRule
    {
        public bool IsMoveLegal(Move move, IBoard board, IList<Move> moves)
        {
            int rankMove = Math.Abs(move.InitialPosition.Rank - move.FinalPosition.Rank);
            int fileMove = Math.Abs(move.InitialPosition.File - move.FinalPosition.File);

            if (rankMove == 0 || rankMove > 2 || fileMove == 0 || fileMove > 2)
            {
                return false;
            }

            return (rankMove + fileMove) == 3;
        }
    }
}