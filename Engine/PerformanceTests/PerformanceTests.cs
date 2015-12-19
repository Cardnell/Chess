using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Cardnell.Chess.Engine;
using NUnit.Framework;
using NUnit.Framework.Compatibility;

namespace PerformanceTests
{
    public class PerformanceTests
    {
        private static int _numberOfMoves;
        public static void Main(string[] args)
        {
            var watch = new Stopwatch();
            Game game = Game.ClassicalGame();
            watch.Start();
            FirstIteration(game, 6);
            watch.Stop();
            Console.WriteLine(_numberOfMoves);
            Console.WriteLine(watch.Elapsed);
        }

        private static void FirstIteration(Game game, int totalLevels)
        {
            IList<Move> moves = game.GetPossibleMoves();
            foreach (Move newMove in moves)
            {
                IterateMoves(game, newMove, 1, totalLevels);
            }
        }

        private static void IterateMoves(Game game, Move move, int level, int totalLevels)
        {
            if (level >= totalLevels)
            {
                return;
            }
            Game newGame = game.Copy();
            level++;
            newGame.MakeMove(move);
            IList<Move> moves = newGame.GetPossibleMoves();
            foreach (Move newMove in moves)
            {
                _numberOfMoves++;
                IterateMoves(newGame, newMove, level, totalLevels);
            }
        }
    }
}
