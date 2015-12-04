using System;
using System.Collections.Generic;
using System.Linq;

namespace Cardnell.Chess.Engine.Rules
{
    public class RefactoredClassicalRules : IRulesEngine
    {
        IRulesEngine _simplePieceRules = new SimplePieceRules();
        IRulesEngine _castlingEngine;

        public RefactoredClassicalRules()
        {
            _castlingEngine = new CastlingRule(_simplePieceRules);
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
            ////  return true;
            board.MovePiece(move);
            try
            {
                Position kingPosition = board.GetKingPosition(move.Mover);
 
                IEnumerable<Tuple<Piece, Position>> oppositePieces = board.GetPieces(move.Mover + 1);

                return !oppositePieces.Any(
                    piece =>
                        IsMoveLegal(new Move(piece.Item2, kingPosition, piece.Item1.Colour, null, null), board, moves, false));
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                board.ReverseMove(move);
            }
            return true;
            throw new NotImplementedException();
        }
    }
}