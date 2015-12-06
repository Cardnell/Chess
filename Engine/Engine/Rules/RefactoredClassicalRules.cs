using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Cardnell.Chess.Engine.Rules
{
    public class RefactoredClassicalRules : IRulesEngine
    {
        IRulesEngine _simplePieceRules = new SimplePieceRules();
        IMoveRule _castlingEngine;
        IMoveRule _checkRulesEngine;

        public RefactoredClassicalRules()
        {
            _castlingEngine = new CastlingRule(_simplePieceRules);
            _checkRulesEngine = new CantMoveIntoCheck(_simplePieceRules);
        }

        public bool IsMoveLegal(Move move, IBoard board, IList<Move> moves)
        {
            return IsMoveLegal(move, board, moves, true);
        }

        public bool IsMoveLegal(Move move, IBoard board, IList<Move> moves, bool checkContraint)
        {
            if (!board.IsPositionOnBoard(move.InitialPosition) || !board.IsPositionOnBoard(move.FinalPosition))
            {
                return false;
            }
            if (!_simplePieceRules.IsMoveLegal(move, board, moves) && !_castlingEngine.IsMoveLegal(move, board, moves))
            {
                return false;
            }
            if (!checkContraint) return true;
            return CantMoveIntoCheck(move, board, moves);
        }

        private bool CantMoveIntoCheck(Move move, IBoard board, IList<Move> moves)
        {
            return _checkRulesEngine.IsMoveLegal(move, board, moves);
        }


    }
}