using System.Collections.Generic;

namespace Cardnell.Chess.Engine.Rules
{
    public class PieceAtInitialLocationValid : IMoveLegalityRuleChecker
    {
        public bool IsMoveLegal(Move move, IBoard board, IList<Move> moves)
        {
            Piece piece = board.GetPieceAt(move.InitialPosition);
            return piece?.Colour == move.Mover;
        }



    }
}