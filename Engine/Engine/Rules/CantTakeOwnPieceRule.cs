using System.Collections.Generic;

namespace Cardnell.Chess.Engine.Rules
{
    public class CantTakeOwnPieceRule : IMoveRule
    {
        public bool IsMoveLegal(Move move, IBoard board, IList<Move> moves)
        {
            Piece piece = board.GetPieceAt(move.FinalPosition);
            if (piece == null)
            {
                return true;
            }
            return piece.Colour != move.Mover;
        }
    }
}