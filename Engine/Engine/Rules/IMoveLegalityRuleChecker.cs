using System.Collections.Generic;

namespace Cardnell.Chess.Engine.Rules
{
    public interface IMoveLegalityRuleChecker
    {
        bool IsMoveLegal(Move move, IBoard board, IList<Move> moves);
    }
}