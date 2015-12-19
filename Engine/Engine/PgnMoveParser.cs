using System;
using System.Collections.Generic;
using System.Linq;
using Cardnell.Chess.Engine.Rules;

namespace Cardnell.Chess.Engine
{
    public class PgnMoveParser
    {
        private readonly IRulesEngine _rulesEngine;

        public PgnMoveParser(IRulesEngine rulesEngine)
        {
            _rulesEngine = rulesEngine;
        }

        private static Position GetPGNMoveLocation(string move)
        {
            return new Position(move[move.Length - 2].ToString() + move[move.Length - 1]);
        }

        private static PieceType GetPieceType(string move, bool isCapture)
        {
            if (move.Length == (isCapture ? 3 : 2))
            {
                return PieceType.Pawn;
            }
            string pieceTypeString = move[0].ToString();
            if (pieceTypeString == "N")
            {
                return PieceType.Knight;
            }
            if (pieceTypeString == "B")
            {
                return PieceType.Bishop;
            }
            if (pieceTypeString == "R")
            {
                return PieceType.Rook;
            }
            if (pieceTypeString == "Q")
            {
                return PieceType.Queen;
            }
            if (pieceTypeString == "K")
            {
                return PieceType.King;
            }
            throw new ArgumentException("Piece type not recognised");
        }


        private static bool PgnMoveIsCapture(string move)
        {
            if (move.Length == 2)
            {
                return false;
            }
            return move[move.Length - 3] == 'x';
        }


        internal Move ParseMove(string move, PieceColour pieceColour, IBoard board, IList<Move> moves)
        {
            Position finalPosition = GetPGNMoveLocation(move);
            bool isCapture = PgnMoveIsCapture(move);
            PieceType pieceType = GetPieceType(move, isCapture);
            Position initalPostion;

            if (move.Length == (isCapture ? 5 : 4))
            {
                char pieceDeteminant = move[1];
                double output;
                if (double.TryParse(pieceDeteminant.ToString(), out output))
                {
                    initalPostion = GetInitialPosition(finalPosition,
                        pieceType,
                        pieceColour,
                        board,
                        moves,
                        x => x.Rank == Position.GetRank(pieceDeteminant));
                }
                else
                {
                    initalPostion = GetInitialPosition(finalPosition,
                        pieceType,
                        pieceColour,
                        board,
                        moves,
                        x => x.File == Position.GetFile(pieceDeteminant));
                }
            }
            else
            {
                initalPostion = GetInitialPosition(finalPosition, pieceType, pieceColour, board, moves, x => true);
            }
            var parsedMove = new Move(initalPostion, finalPosition, pieceColour, null, null);
            return parsedMove;
        }


        private Position GetInitialPosition(Position finalPosition,
            PieceType pieceType,
            PieceColour pieceColour,
            IBoard board,
            IList<Move> moves,
            Func<Position, bool> condition)
        {
            IEnumerable<Tuple<Piece, Position>> piecePositions = board.GetPieces(pieceColour, pieceType);
            return
                piecePositions.First(
                    x =>
                        _rulesEngine.IsMoveLegal(new Move(x.Item2, finalPosition, pieceColour, null, null), board, moves)
                        && condition(x.Item2)).Item2;
        }
    }
}