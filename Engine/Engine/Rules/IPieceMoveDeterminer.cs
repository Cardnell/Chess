using System.Collections.Generic;

namespace Cardnell.Chess.Engine.Rules
{
    public interface IPieceMoveDeterminer: IMoveLegalityRuleChecker
    {
        IList<Move> GetLegalMoves(Position startingPosition, PieceColour colour, IBoard board, IList<Move> moves);
    }
}