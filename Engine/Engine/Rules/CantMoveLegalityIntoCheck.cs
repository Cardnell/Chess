using System;
using System.Collections.Generic;
using System.Linq;

namespace Cardnell.Chess.Engine.Rules
{
    public class CantMoveLegalityIntoCheck : IMoveLegalityRuleChecker
    {
        private readonly IRulesEngine _simplePieceSimplePieceRules;

        public CantMoveLegalityIntoCheck(IRulesEngine simplePieceRules)
        {
            _simplePieceSimplePieceRules = simplePieceRules;
        }

        public bool IsMoveLegal(Move move, IBoard board, IList<Move> moves)
        {

            //Board newPosition = board.Copy();
            //var newMoves = new List<Move>();
            //newMoves.AddRange(moves);
            moves.Add(move);
            board.MovePiece(move);
            try
            {
                PieceColour oppasiteColour = move.Mover == PieceColour.White ? PieceColour.Black : PieceColour.White;
                Position kingPosition = board.GetKingPosition(move.Mover);
                IEnumerable<Tuple<Piece, Position>> pieces = board.GetPieces(oppasiteColour);

                if (
                    pieces.Select(
                        piecePosition => new Move(piecePosition.Item2, kingPosition, oppasiteColour, null, null))
                        .Any(possibleMove => _simplePieceSimplePieceRules.IsMoveLegal(possibleMove, board, moves)))
                {
                    return false;
                }
                return true;
            }
            finally
            {
                board.ReverseMove(move);
                moves.RemoveAt(moves.Count - 1);

            }
        }
    }
}