using System;
using System.Collections.Generic;
using System.Linq;

namespace Cardnell.Chess.Engine.Rules
{
    public class PawnMoveLegalityRuleChecker : IPieceMoveDeterminer
    {
        public bool IsMoveLegal(Move move, IBoard board, IList<Move> moves)
        {
            int direction = GetDirection(move.Mover);
            Piece pieceOnFinalSquare = board.GetPieceAt(move.FinalPosition);
            if (pieceOnFinalSquare != null)
            {
                return (Math.Abs(move.InitialPosition.File - move.FinalPosition.File) == 1 &&
                        (move.InitialPosition.Rank + (1*direction) == move.FinalPosition.Rank));
            }
            if (EnpassantTest(move, board, moves))
            {
                return true;
            }
            if (move.InitialPosition.File != move.FinalPosition.File)
            {
                return false;
            }
            if (move.InitialPosition.Rank + (1*direction) == move.FinalPosition.Rank)
            {
                return true;
            }
            Piece initialPiece = board.GetPieceAt(move.InitialPosition);
            return (move.InitialPosition.Rank + (2 * direction) == move.FinalPosition.Rank)
                   && moves.All(x => x.PieceMoved != initialPiece);
        }

        public IList<Move> GetLegalMoves(Position startingPosition, PieceColour colour, IBoard board, IList<Move> moves)
        {
            var output = new List<Move>();
            Piece initialPiece = board.GetPieceAt(startingPosition);
            int direction = GetDirection(colour);
            var finalPosition = new Position(startingPosition.Rank + (1 *direction), startingPosition.File);
            AddMoveIfSquareEmpty(startingPosition, colour, board, finalPosition, output, initialPiece);
            if (moves.All(x => x.PieceMoved != initialPiece))
            {
                finalPosition = new Position(startingPosition.Rank + (2 * direction), startingPosition.File);
                AddMoveIfSquareEmpty(startingPosition, colour, board, finalPosition, output, initialPiece);
            }
            finalPosition = new Position(startingPosition.Rank + (1 * direction), startingPosition.File+1);
            AddMoveIfTakingPiece(startingPosition, colour, board, finalPosition, output, initialPiece, moves);
            finalPosition = new Position(startingPosition.Rank + (1 * direction), startingPosition.File-1);
            AddMoveIfTakingPiece(startingPosition, colour, board, finalPosition, output, initialPiece, moves);
            return output;
        }

        private void AddMoveIfTakingPiece(Position startingPosition, PieceColour colour, IBoard board, Position finalPosition, List<Move> output, Piece initialPiece, IList<Move> moves)
        {

            if (EnpassantTest(new Move(startingPosition, finalPosition, colour, null, null), board, moves))
            {
                //todo add enpassant piece
                var enpassantMove = new Move(startingPosition, finalPosition, colour, initialPiece, null);
                output.Add(enpassantMove);
            }
            if (!board.IsPositionOnBoard(finalPosition))
            {
                return;
            }
            Piece piece = board.GetPieceAt(finalPosition);
            if (piece != null && piece.Colour!=colour)
            {
                output.Add(new Move(startingPosition, finalPosition, colour, initialPiece, piece));
            }
        }

        private static void AddMoveIfSquareEmpty(Position startingPosition,
            PieceColour colour,
            IBoard board,
            Position finalPosition,
            List<Move> output,
            Piece initialPiece)
        {
            if (!board.IsPositionOnBoard(finalPosition))
            {
                return;
            }
            Piece piece = board.GetPieceAt(finalPosition);
            if (piece == null)
            {
                output.Add(new Move(startingPosition, finalPosition, colour, initialPiece, null));
            }
        }

        private bool EnpassantTest(Move move, IBoard board, IList<Move> moves)
        {
            if (moves.Count == 0)
            {
                return false;
            }
            if (Math.Abs(move.InitialPosition.File - move.FinalPosition.File) != 1)
            {
                return false;
            }
            
            int direction = GetDirection(move.Mover);
            if (direction*(move.FinalPosition.Rank - move.InitialPosition.Rank) != 1)
            {
                return false;
            }

            Move lastMove = moves[moves.Count - 1];
            Piece lastPiece = lastMove.PieceMoved;;
            if (lastPiece.PieceType != PieceType.Pawn)
            {
                return false;
            }
            if (lastPiece.Colour == move.Mover)
            {
                return false;
            }
            if (lastMove.FinalPosition.File != move.FinalPosition.File)
            {
                return false;
            }
            if (move.FinalPosition.Rank - lastMove.FinalPosition.Rank == 1 * direction)
            {
                return true;
            }

            return false;
        }

        private static int GetDirection(PieceColour colour)
        {
            if (colour == PieceColour.White)
            {
                return 1;
            }
            return -1;
        
        }
    }
}