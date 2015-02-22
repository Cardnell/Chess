using System;
using System.Collections.Generic;

namespace Cardnell.Chess.Engine.Rules
{
    public class PawnMoveRule : IMoveRule
    {
        public bool IsMoveLegal(Move move, IBoard board, IList<Move> moves)
        {
            Piece pieceOnFinalSquare = board.GetPieceAt(move.FinalPosition);
            if (pieceOnFinalSquare != null)
                return (Math.Abs(move.InitialPosition.File - move.FinalPosition.File) == 1 &&
                        (move.InitialPosition.Rank + 1 == move.FinalPosition.Rank));
            if (move.InitialPosition.File != move.FinalPosition.File)
            {
                return false;
            }
            if (move.InitialPosition.Rank + 1 == move.FinalPosition.Rank)
            {
                return true;
            }
            return (move.InitialPosition.Rank + 2 == move.FinalPosition.Rank)
                   && board.GetPieceAt(move.InitialPosition).HasMoved == false;
        }
    }
}