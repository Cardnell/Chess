using System;
using System.Collections.Generic;
using System.Linq;

namespace Cardnell.Chess.Engine.Rules
{
    public class CantMoveIntoCheck : IMoveRule
    {
        IRulesEngine _simplePieceSimplePieceRules;
        public CantMoveIntoCheck(IRulesEngine simplePieceRules)
        {
            _simplePieceSimplePieceRules = simplePieceRules;
        }

        public bool IsMoveLegal(Move move, IBoard board, IList<Move> moves)
        {
            if (move.PieceMoved.PieceType == PieceType.King)
            {
                IEnumerable<Tuple<Piece, Position>> oppositePieces = board.GetPieces(move.Mover + 1);
                return !oppositePieces.Any(
                    piece =>
                        _simplePieceSimplePieceRules.IsMoveLegal(new Move(piece.Item2, move.FinalPosition, piece.Item1.Colour, null, null), board, moves));
            }

            Position kingPosition = board.GetKingPosition(move.Mover);
            if (kingPosition.File == move.InitialPosition.File)
            {
                int direction = move.InitialPosition.Rank > kingPosition.Rank ? 1 : -1;
                var initalPosition = new Position(kingPosition.Rank + direction, kingPosition.File);

                return CheckLine(move, board, x => new Position(x.Rank + direction, x.File),
                    x => x != PieceType.Queen && x != PieceType.Rook, initalPosition);
            }

            if (kingPosition.Rank == move.InitialPosition.Rank)
            {
                int direction = move.InitialPosition.File > kingPosition.File ? 1 : -1;
                var initalPosition = new Position(kingPosition.Rank, kingPosition.File + direction);

                return CheckLine(move, board, x => new Position(x.Rank, x.File + direction),
                    x => x != PieceType.Queen && x != PieceType.Rook, initalPosition);
            }

            if (Math.Abs(move.InitialPosition.File - kingPosition.File) ==
                Math.Abs(move.InitialPosition.Rank - kingPosition.Rank))
            {
                int fileDirection = move.InitialPosition.File > kingPosition.File ? 1 : -1;
                int rankDirection = move.InitialPosition.Rank > kingPosition.Rank ? 1 : -1;

                var initalPosition = new Position(kingPosition.Rank + rankDirection, kingPosition.File + fileDirection);

                return CheckLine(move, board, x => new Position(x.Rank + rankDirection, x.File + fileDirection),
                    x => x != PieceType.Queen && x != PieceType.Bishop, initalPosition);
            }
            return true;
        }

        private static bool CheckLine(Move move, IBoard board, Func<Position, Position> moveIncrement, Func<PieceType, bool> checkConstraint, Position startPosition)
        {
            Position positionToCheck = startPosition;
            while (board.IsPositionOnBoard(positionToCheck))
            {
                if (!positionToCheck.Equals(move.InitialPosition))
                {
                    if (board.IsPieceAt(positionToCheck))
                    {
                        Piece pieceToCheck = board.GetPieceAt(positionToCheck);
                        if (pieceToCheck.Colour == move.Mover)
                        {
                            return true;
                        }
                        return checkConstraint(pieceToCheck.PieceType);
                    }
                }
                positionToCheck = moveIncrement(positionToCheck);
            }
            return true;
        }
    }
}