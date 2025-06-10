using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tictactoe
{
    public class BoardCoordinate
    {
        private int _row;

        private int _col;

        public BoardCoordinate(int row, int col)
        {
            Row = row;
            Col = col;
        }

        public int Row
        {
            get
            {
                return _row;
            }

            private set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(Row), "Cannot be negative!");
                }

                _row = value;
            }
        }

        public int Col
        {
            get
            {
                return _col;
            }

            private set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(Col), "Cannot be negative!");
                }

                _col = value;
            }
        }
    }
}
