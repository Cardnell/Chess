using System;
using System.Collections.Generic;
using Cardnell.Chess.Engine.Rules;

namespace Cardnell.Chess.Engine
{
    public class Game
    {
        private readonly List<Move> _moves;
        private readonly IRulesEngine _rulesEngine;

        public Game(IBoard board, IRulesEngine rulesEngine)
        {
            Board = board;
            _rulesEngine = rulesEngine;
            _moves = new List<Move>();
        }


        public Game(IBoard board, IRulesEngine rulesEngine, IEnumerable<Tuple<Piece, Position>> pieces)
        {
            Board = board;
            _rulesEngine = rulesEngine;
            _moves = new List<Move>();
            foreach (var piece in pieces)
            {
                board.AddPiece(piece.Item1, piece.Item2);
            }
        }

        public IBoard Board { get; private set; }

        public bool IsMoveLegal(Position initialPosition, Position finalPosition, PieceColour mover)
        {
            Piece piece = Board.GetPieceAt(initialPosition);
            if (piece == null)
            {
                return false;
            }
            return piece.Colour == mover &&
                   _rulesEngine.IsMoveLegal(new Move(initialPosition, finalPosition, mover, piece, null), Board, _moves);
        }

        public bool IsMoveLegal(Move move)
        {
            return false;
        }

        public void MakeMove(Position initialPosition, Position finalPosition, PieceColour mover)
        {
            if (!IsMoveLegal(initialPosition, finalPosition, mover))
            {
                throw new ArgumentException("Move not legal");
            }
            Board.MovePiece(new Move(initialPosition, finalPosition, mover, null, null));
        }
    }
}