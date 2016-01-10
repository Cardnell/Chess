using System;
using System.Collections.Generic;

namespace Cardnell.Chess.Engine.Rules
{
    public class RookMoveLegalityRuleChecker : PieceMoveLegalityChecker
    {
        public override bool IsMoveLegal(Move move, IBoard board, IList<Move> moves)
        {
            if (move.InitialPosition.Rank == move.FinalPosition.Rank)
            {
                if (move.InitialPosition.File == move.FinalPosition.File)
                {
                    return false;
                }
                if (move.InitialPosition.File > move.FinalPosition.File)
                {
                    for (var i = 1; i < move.InitialPosition.File - move.FinalPosition.File; i++)
                    {
                        if (board.IsPieceAt(new Position(move.InitialPosition.Rank, move.InitialPosition.File - i)))
                        {
                            return false;
                        }
                    }
                    return true;
                }
                for (var i = 1; i < move.FinalPosition.File - move.InitialPosition.File; i++)
                {
                    if (board.IsPieceAt(new Position(move.InitialPosition.Rank, move.InitialPosition.File + i)))
                    {
                        return false;
                    }
                }
                return true;
            }
            if (move.InitialPosition.File != move.FinalPosition.File)
            {
                return false;
            }
            if (move.InitialPosition.Rank > move.FinalPosition.Rank)
            {
                for (var i = 1; i < move.InitialPosition.Rank - move.FinalPosition.Rank; i++)
                {
                    if (board.IsPieceAt(new Position(move.InitialPosition.Rank - i, move.InitialPosition.File)))
                    {
                        return false;
                    }
                }
                return true;
            }
            for (var i = 1; i < move.FinalPosition.Rank - move.InitialPosition.Rank; i++)
            {
                if (board.IsPieceAt(new Position(move.InitialPosition.Rank + i, move.InitialPosition.File)))
                {
                    return false;
                }
            }
            return true;
        }

        public override IList<Move> GetLegalMoves(Position startingPosition,
            PieceColour colour,
            IBoard board,
            IList<Move> moves)
        {
            var output = new List<Move>();
            output.AddRange(IterateMovesInOneDirection(startingPosition,
                colour,
                board,
                x => new Position(x.Rank + 1, x.File)));
            output.AddRange(IterateMovesInOneDirection(startingPosition,
                colour,
                board,
                x => new Position(x.Rank - 1, x.File)));
            output.AddRange(IterateMovesInOneDirection(startingPosition,
                colour,
                board,
                x => new Position(x.Rank, x.File + 1)));
            output.AddRange(IterateMovesInOneDirection(startingPosition,
                colour,
                board,
                x => new Position(x.Rank, x.File - 1)));

            return output;
        }
    }

    public abstract class PieceMoveLegalityChecker : IPieceMoveDeterminer
    {
        public abstract bool IsMoveLegal(Move move, IBoard board, IList<Move> moves);


        public abstract IList<Move> GetLegalMoves(Position startingPosition,
            PieceColour colour,
            IBoard board,
            IList<Move> moves);

        protected static IEnumerable<Move> IterateMovesInOneDirection(Position startingPosition,
            PieceColour colour,
            IBoard board,
            Func<Position, Position> generateNewPosition)
        {
            IList<Move> output = new List<Move>();
            Position positionToTest = generateNewPosition(startingPosition);
            while (board.IsPositionOnBoard(positionToTest))
            {
                Piece piece = board.GetPieceAt(positionToTest);
                if (CheckIfPieceIsOnPosition(startingPosition, colour, board, piece, output, positionToTest))
                {
                    break;
                }
                output.Add(new Move(startingPosition, positionToTest, colour, board.GetPieceAt(startingPosition), null));
                positionToTest = generateNewPosition(positionToTest);
            }
            return output;
        }

        private static bool CheckIfPieceIsOnPosition(Position startingPosition,
            PieceColour colour,
            IBoard board,
            Piece piece,
            IList<Move> output,
            Position positionToTest)
        {
            if (piece != null)
            {
                if (piece.Colour != colour)
                {
                    output.Add(new Move(startingPosition,
                        positionToTest,
                        colour,
                        board.GetPieceAt(startingPosition),
                        piece));
                }
                return true;
            }
            return false;
        }

        protected static void AddMoveIfLegitimate(Position startingPosition,
            Position finalPosition,
            PieceColour colour,
            IBoard board,
            List<Move> output)
        {
            if (!board.IsPositionOnBoard(finalPosition)) return;
            Piece piece = board.GetPieceAt(finalPosition);
            if (piece == null || piece.Colour != colour)
            {
                output.Add(new Move(startingPosition,
                    finalPosition,
                    colour,
                    board.GetPieceAt(startingPosition),
                    piece));
            }
        }
    }
}