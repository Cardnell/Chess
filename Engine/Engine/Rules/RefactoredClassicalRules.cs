using System;
using System.Collections.Generic;
using System.Linq;

namespace Cardnell.Chess.Engine.Rules
{
    public class RefactoredClassicalRules : IRulesEngine
    {
        private readonly Dictionary<PieceType, List<IMoveRule>> _moveLegality;

        public RefactoredClassicalRules()
        {
            _moveLegality = new Dictionary<PieceType, List<IMoveRule>>
            {
                {PieceType.Bishop, new List<IMoveRule> {new BishopMoveRule()}},
                {PieceType.King, new List<IMoveRule> {new KingMoveRule()}},
                {PieceType.Knight, new List<IMoveRule> {new KnightMoveRule()}},
                {PieceType.Queen, new List<IMoveRule> {new QueenMoveRule()}},
                {PieceType.Rook, new List<IMoveRule> {new RookMoveRule()}},
                {PieceType.Pawn, new List<IMoveRule> {new PawnMoveRule()}}
            };

            foreach (var ruleList in _moveLegality.Values)
            {
                ruleList.Add(new CantTakeOwnPieceRule());
            }
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
            Piece pieceToMove = board.GetPieceAt(move.InitialPosition);
            if (pieceToMove == null)
            {
                return false;
            }
            if (!(_moveLegality.ContainsKey(pieceToMove.PieceType) &&
                  _moveLegality[pieceToMove.PieceType].All(rule => rule.IsMoveLegal(move, board, moves))))
            {
                return false;
            }
            if (!checkContraint) return true;
            return CantMoveIntoCheck(move, board, moves);
        }

        private bool CantMoveIntoCheck(Move move, IBoard board, IList<Move> moves)
        {
            //  return true;
            board.MovePiece(move);
            try
            {
                Piece king = board.GetKing(move.Mover);
                if (king == null)
                {
                    return true;
                }
                IEnumerable<Piece> oppositePieces = board.GetPieces(move.Mover + 1);

                return !oppositePieces.Any(
                    piece =>
                        IsMoveLegal(new Move(piece.Position, king.Position, piece.Colour, null, null), board, moves, false));
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                board.ReverseMove(move);
            }
        }
    }
}