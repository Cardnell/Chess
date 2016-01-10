using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Cardnell.Chess.Engine.Rules
{
    public class RefactoredClassicalRules : IRulesEngine
    {
        readonly IRulesEngine _simplePieceRules = new SimplePieceRules();
        readonly IMoveLegalityRuleChecker _castlingEngine;
        private readonly List<IMoveLegalityRuleChecker> _postPieceSpecificChecks = new List<IMoveLegalityRuleChecker>();
        private readonly List<IMoveLegalityRuleChecker> _prePieceSpecificChecks = new List<IMoveLegalityRuleChecker>();

        public RefactoredClassicalRules()
        {
            _castlingEngine = new CastlingLegalityRuleChecker(_simplePieceRules);
            _postPieceSpecificChecks.Add(new CantTakeOwnPieceLegalityRuleChecker());
            _postPieceSpecificChecks.Add(new CantMoveLegalityIntoCheck(_simplePieceRules));
            _prePieceSpecificChecks.Add(new PositionsInMoveLegal());
            _prePieceSpecificChecks.Add(new PieceAtInitialLocationValid());
        }

        public bool IsMoveLegal(Move move, IBoard board, IList<Move> moves)
        {
            if(_prePieceSpecificChecks.Any(x=>!x.IsMoveLegal(move, board, moves)))
            {
                return false;
            }
            if (!_simplePieceRules.IsMoveLegal(move, board, moves) && !_castlingEngine.IsMoveLegal(move, board, moves))
            {
                return false;
            }
            return _postPieceSpecificChecks.All(x => x.IsMoveLegal(move, board, moves));
        }

        public IList<Move> GetLegalMoves(Position startingPositiong, IBoard board, IList<Move> moves)
        {
            IList<Move> simplePieceMoves = _simplePieceRules.GetLegalMoves(startingPositiong, board, moves);
            return
                simplePieceMoves.Where(x => _postPieceSpecificChecks.All(y => y.IsMoveLegal(x, board, moves))).ToList();
        }




    }
}