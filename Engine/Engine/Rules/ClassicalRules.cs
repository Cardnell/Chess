using System;
using System.Collections.Generic;
using System.Linq;
using Cardnell.Chess.Engine.Rules;

namespace Cardnell.Chess.Engine
{
    public class ClassicalRules : IRulesEngine
    {

        //public bool IsMoveLegal(Move move, IBoard board)
        //{
        //    Piece pieceOnFinalSquare = board.GetPieceAt(move.FinalPosition);
        //    if (pieceOnFinalSquare != null)
        //    {
        //        if (pieceOnFinalSquare.Colour == move.Mover)
        //        {
        //            return false;
        //        }
        //    }

        //    if (! SpecificPieceLegalicy(move, board, pieceOnFinalSquare))
        //    {
        //        return false;
        //    }
        //    board.MovePiece(move.InitialPosition, move.FinalPosition);
        //    bool output =  IsInCheck(board, move.PieceMoved.Colour);
        //    board.MovePiece(move.FinalPosition, move.InitialPosition);
        //    return output;
        //}

        //public bool IsInCheck(IBoard board, PieceColour colour)
        //{
        //    PieceColour moverColour = colour + 1;
        //    Piece king = board.GetKing(colour);
        //    foreach (Piece piece in board.GetPieces(moverColour))
        //    {
        //        //blah
        //    }

        //    return board.GetPieces(colour).Any(t => IsMoveLegal(new Move(t.Position, king.Position, moverColour, t, false), board));
        //}
        //private bool SpecificPieceLegalicy(Move move, IBoard board, Piece pieceOnFinalSquare)
        //{
        //    switch (move.PieceMoved.PieceType)
        //    {
        //        case PieceType.King:
        //            return IsKingMoveLegal(move);
        //        case PieceType.Knight:
        //            return IsKnightMoveLegal(move);
        //        case PieceType.Rook:
        //            return IsRookMoveLegal(move, board);
        //        case PieceType.Bishop:
        //            return IsBishopMoveLegal(move, board);
        //        case PieceType.Queen:
        //            return IsQueenMoveLegal(move, board);
        //        case PieceType.Pawn:
        //            return IsPawnMoveLegal(move, board, pieceOnFinalSquare);
        //        default:
        //            return false;
        //    }
        //}

        private bool IsPawnMoveLegal(Move move, IBoard board, Piece pieceOnFinalSquare)
        {

            if (pieceOnFinalSquare == null)
            {
                if (move.InitialPosition.File != move.FinalPosition.File)
                {
                    return false;
                } 
                if (move.InitialPosition.Rank + 1 == move.FinalPosition.Rank)
                {
                    return true;
                }
                return (move.InitialPosition.Rank + 2 == move.FinalPosition.Rank)
                       && board.GetPieceAt(move.InitialPosition).HasMoved == false;
            }
            return (Math.Abs(move.InitialPosition.File - move.FinalPosition.File) == 1 &&
                    (move.InitialPosition.Rank + 1 == move.FinalPosition.Rank));
        }

        private bool IsQueenMoveLegal(Move move, IBoard board)
        {
            return IsBishopMoveLegal(move, board) || IsRookMoveLegal(move, board);
        }

        private bool IsBishopMoveLegal(Move move, IBoard board)
        {
            if (Math.Abs(move.InitialPosition.Rank - move.FinalPosition.Rank) !=
                Math.Abs(move.InitialPosition.File - move.FinalPosition.File))
            {
                return false;
            }
            if (move.InitialPosition.Rank < move.FinalPosition.Rank)
            {
                if (move.InitialPosition.File > move.FinalPosition.File)
                {
                    for (int i = 1; i < (move.InitialPosition.File - move.FinalPosition.File); i++)
                    {
                        if (board.IsPieceAt(new Position(move.InitialPosition.Rank + 1, move.InitialPosition.File - i)))
                        {
                            return false;
                        }
                    }
                    return true;
                }
                for (int i = 1; i < (move.FinalPosition.File - move.InitialPosition.File); i++)
                {
                    if (board.IsPieceAt(new Position(move.InitialPosition.Rank + 1, move.InitialPosition.File + i)))
                    {
                        return false;
                    }
                }
                return true;
            }
            if (move.InitialPosition.File > move.FinalPosition.File)
            {
                for (int i = 1; i < (move.InitialPosition.File - move.FinalPosition.File); i++)
                {
                    if (board.IsPieceAt(new Position(move.InitialPosition.Rank - 1, move.InitialPosition.File - i)))
                    {
                        return false;
                    }
                }
                return true;
            }
            for (int i = 1; i < (move.FinalPosition.File - move.InitialPosition.File); i++)
            {
                if (board.IsPieceAt(new Position(move.InitialPosition.Rank - 1, move.InitialPosition.File + i)))
                {
                    return false;
                }
            }
            return true;
        }

        private bool IsRookMoveLegal(Move move, IBoard board)
        {
            if (move.InitialPosition.Rank == move.FinalPosition.Rank)
            {
                if (move.InitialPosition.File == move.FinalPosition.File)
                {
                    return false;
                }
                if (move.InitialPosition.File > move.FinalPosition.File)
                {
                    for (int i = 1; i < (move.InitialPosition.File - move.FinalPosition.File); i++)
                    {
                        if (board.IsPieceAt(new Position(move.InitialPosition.Rank, move.InitialPosition.File - i)))
                        {
                            return false;
                        }
                    }
                    return true;
                }
                for (int i = 1; i <( move.FinalPosition.File - move.InitialPosition.File  ); i++)
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
                for (int i = 1; i < (move.InitialPosition.Rank - move.FinalPosition.Rank); i++)
                {
                    if (board.IsPieceAt(new Position(move.InitialPosition.Rank - i, move.InitialPosition.File)))
                    {
                        return false;
                    }
                }
                return true;
            }
            for (int i = 1; i < (move.FinalPosition.Rank - move.InitialPosition.Rank); i++)
            {
                if (board.IsPieceAt(new Position(move.InitialPosition.Rank + i, move.InitialPosition.File)))
                {
                    return false;
                }
            }
            return true;
        }

        private bool IsKnightMoveLegal(Move move)
        {
            int rankMove = Math.Abs(move.InitialPosition.Rank - move.FinalPosition.Rank);
            int fileMove = Math.Abs(move.InitialPosition.File - move.FinalPosition.File);

            if (rankMove == 0 || rankMove > 2 || fileMove == 0 || fileMove > 2)
            {
                return false;
            }

            return (rankMove + fileMove) == 3;
        }

        private bool IsKingMoveLegal(Move move)
        {
            return Math.Abs(move.InitialPosition.Rank - move.FinalPosition.Rank) <= 1
                   && Math.Abs(move.InitialPosition.File - move.FinalPosition.File) <= 1;
        }

        public bool IsMoveLegal(Move move, IBoard board)
        {
            throw new NotImplementedException();
        }

        public bool IsMoveLegal(Move move, IBoard board, IList<Move> moves)
        {
            throw new NotImplementedException();
        }
    }
}