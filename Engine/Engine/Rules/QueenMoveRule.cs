using System.Collections.Generic;
using System.Linq;

namespace Cardnell.Chess.Engine.Rules
{
    public class QueenMoveRule : IMoveRule
    {
        private readonly List<IMoveRule> _moveRules;

        public QueenMoveRule()
        {
            _moveRules = new List<IMoveRule> {new RookMoveRule(), new BishopMoveRule()};
        }

        public bool IsMoveLegal(Move move, IBoard board, IList<Move> moves)
        {
            return _moveRules.Any(moveRule => moveRule.IsMoveLegal(move, board, moves));
        }
    }
}