using System.Collections.Generic;

namespace Cardnell.Chess.Engine.Rules
{
    public class PositionsInMoveLegal: IMoveLegalityRuleChecker
    {
        public bool IsMoveLegal(Move move, IBoard board, IList<Move> moves)
        {
            return board.IsPositionOnBoard(move.InitialPosition) && board.IsPositionOnBoard(move.FinalPosition);
        }
    }
}