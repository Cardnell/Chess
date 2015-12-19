using System;

namespace Cardnell.Chess.Engine
{
    public struct Position
    {
        public int Rank { get; private set; }
        public int File { get; private set; }

        public Position(int rank, int file) : this()
        {
            Rank = rank;
            File = file;
        }

        public Position(string v) : this()
        {
            char[] algebraicNotation = v.ToCharArray();
            Rank = GetRank(algebraicNotation[1]);
            File = GetFile(algebraicNotation[0]);

        }

        public static int GetFile(char file)
        {
            int output = file - 97;
            if (output < 0)
            {
                output += 32;
            }
            return output;
        }

        public static int GetRank(char rank)
        {
            return rank - 49;
        }

        public override string ToString()
        {
            return string.Format("[{0},{1}]", Rank + 1, File + 1);
        }
    }

}