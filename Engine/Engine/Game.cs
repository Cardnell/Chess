using System;
using System.Collections.Generic;
using Cardnell.Chess.Engine.Rules;

namespace Cardnell.Chess.Engine
{
    public class Game
    {
        public readonly List<Move> Moves;
        private readonly IRulesEngine _rulesEngine;

        public Game(IBoard board, IRulesEngine rulesEngine)
        {
            Board = board;
            _rulesEngine = rulesEngine;
            Moves = new List<Move>();
        }


        public Game(IBoard board, IRulesEngine rulesEngine, IEnumerable<Tuple<Piece, Position>> pieces)
        {
            Board = board;
            _rulesEngine = rulesEngine;
            Moves = new List<Move>();
            foreach (var piece in pieces)
            {
                board.AddPiece(piece.Item1, piece.Item2);
            }
        }

        public IBoard Board { get; }
        

        public bool IsMoveLegal(Position initialPosition, Position finalPosition, PieceColour mover)
        {
            var piece = Board.GetPieceAt(initialPosition);
            if (piece == null)
            {
                return false;
            }
            return piece.Colour == mover &&
                   _rulesEngine.IsMoveLegal(new Move(initialPosition, finalPosition, mover, piece, null), Board, Moves);
        }

    
        public void MakeMove(Position initialPosition, Position finalPosition, PieceColour mover)
        {
            if (!IsMoveLegal(initialPosition, finalPosition, mover))
            {
                throw new ArgumentException("Move not legal");
            }
            var move = new Move(initialPosition, finalPosition, mover, null, null);
            Board.MovePiece(move);
            Moves.Add(move);

        }
    }
}