using System;
using System.Collections.Generic;
using System.Linq;

namespace Cardnell.Chess.Engine.Rules
{
    public class CantMoveIntoCheck : IMoveRule
    {
        private readonly IRulesEngine _simplePieceSimplePieceRules;

        public CantMoveIntoCheck(IRulesEngine simplePieceRules)
        {
            _simplePieceSimplePieceRules = simplePieceRules;
        }

        public bool IsMoveLegal(Move move, IBoard board, IList<Move> moves)
        {
            Board newPosition = board.Copy();
            var newMoves = new List<Move>();
            newMoves.AddRange(moves);
            newMoves.Add(move);
            newPosition.MovePiece(move);
            PieceColour oppasiteColour = move.Mover == PieceColour.White ? PieceColour.Black : PieceColour.White;
            Position kingPosition = newPosition.GetKingPosition(move.Mover);
            IEnumerable<Tuple<Piece, Position>> pieces = newPosition.GetPieces(oppasiteColour);

            if (
                pieces.Select(piecePosition => new Move(piecePosition.Item2, kingPosition, oppasiteColour, null, null))
                    .Any(possibleMove => _simplePieceSimplePieceRules.IsMoveLegal(possibleMove, newPosition, newMoves)))
            {
                return false;
            }
            return true;
        }
    }
}