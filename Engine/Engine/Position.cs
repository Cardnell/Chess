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
            Rank = algebraicNotation[1] - 49;
            File = algebraicNotation[0] - 97;
            if (File < 0)
            {
                File += 32;
            }
        }

        //        // override object.Equals
        //        public override bool Equals(object obj)
        //        {
        //            //       
        //            // See the full list of guidelines at
        //            //   http://go.microsoft.com/fwlink/?LinkID=85237  
        //            // and also the guidance for operator== at
        //            //   http://go.microsoft.com/fwlink/?LinkId=85238
        //            //

        //            if (obj == null || GetType() != obj.GetType())
        //            {
        //                return false;
        //            }

        //            if (GetHashCode() != obj.GetHashCode())
        //            {
        //                return false;
        //            }
        //            if (Rank != ((Position)obj).Rank)
        //            {
        //                return false;
        //            }
        //            return File == ((Position)obj).File;
        //        }

        //// override object.GetHashCode
        //        public override int GetHashCode()
        //        {
        //            return Rank.GetHashCode() ^ File.GetHashCode();
        //;
        //        }
    }

}