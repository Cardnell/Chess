using System.Collections.Generic;

namespace Cardnell.Chess.Engine.Rules
{
    public interface IRulesEngine
    {
        bool IsMoveLegal(Move move, IBoard board, IList<Move> moves);
        IList<Move> GetLegalMoves(Position startingPositiong, IBoard board, IList<Move> moves);
    }
}