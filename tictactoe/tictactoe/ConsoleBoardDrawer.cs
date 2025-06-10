using ConsoleUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tictactoe.Game;
using tictactoe.Game.GameExceptions;
using tictactoe.Game.GameObjects;

namespace tictactoe
{
    public class ConsoleBoardDrawer
    {
        private Board _board;

        private int _leftUpperCornerX;

        private int _leftUpperCornerY;

        private char _frameChar;

        private char _fieldChar;

        private ConsoleColor _frameForeground;

        private ConsoleColor _frameBackground;

        private ConsoleColor _fieldForeground;

        private ConsoleColor _fieldBackground;

        private ConsoleColor _legendForeground;

        private ConsoleColor _legendBackground;

        private int _fieldSize;

        public ConsoleBoardDrawer(Board board, int leftUpperCornerX, int leftUpperCornerY,
            char frameChar, ConsoleColor frameForeground, ConsoleColor frameBackground, int fieldSize,
            char fieldChar, ConsoleColor fieldForeground, ConsoleColor fieldBackground,
            ConsoleColor legendForeground, ConsoleColor legendBackground)
        {
            _board = board ?? throw new ArgumentNullException(nameof(board), "Cannot be null!");
            _leftUpperCornerX = leftUpperCornerX >= 0 ? leftUpperCornerX : throw new ArgumentOutOfRangeException(nameof(leftUpperCornerX), "Cannot be negative!");
            _leftUpperCornerY = leftUpperCornerY >= 0 ? leftUpperCornerY : throw new ArgumentOutOfRangeException(nameof(leftUpperCornerY), "Cannot be negative!");
            _frameChar = frameChar;
            _frameForeground = frameForeground;
            _frameBackground = frameBackground;
            _fieldChar = fieldChar;
            _fieldForeground = fieldForeground;
            _fieldBackground = fieldBackground;
            _legendForeground = legendForeground;
            _legendBackground = legendBackground;
            _fieldSize = fieldSize >= 1 && fieldSize % 2 == 1 ? fieldSize : throw new ArgumentOutOfRangeException(nameof(fieldSize), "Field size must be an odd number!"); 
        }

        public void DrawFrame()
        {
            DrawHorizontalFrameBars();
            DrawVerticalFrameBars();
        }

        public void DrawFields()
        {
            int boardHeight = _board.Height;
            int boardWidth = _board.Width;

            for (int row = 0; row < boardHeight; row++)
            {
                for (int col = 0; col < boardWidth; col++)
                {
                    DrawField(row, col); 
                }
            }
        }

        public void DrawLegend()
        {
            DrawHorizontalLegend();
            DrawVerticalLegend();
        }

        private void DrawHorizontalLegend()
        {
            string[] numberTextsHorizontal = Enumerable.Range(0, _board.Width).Select(n => Convert.ToString(n)).ToArray();
            int startX = _leftUpperCornerX + (_fieldSize / 2) + 1;
            int startY = _leftUpperCornerY + _board.Height * (_fieldSize + 1) + 1;

            for (int col = 0; col < numberTextsHorizontal.Length; col++)
            {
                ConsolePrinter.Print(startX + col * (_fieldSize + 1), startY, numberTextsHorizontal[col], _legendBackground, _legendForeground);
            }
        }

        private void DrawVerticalLegend()
        {
            string[] numberTextsVertical = Enumerable.Range(0, _board.Height).Select(n => Convert.ToString(n)).ToArray();
            int startX = _leftUpperCornerX + _board.Width * (_fieldSize + 1) + 1;
            int startY = (_fieldSize / 2) + 1;

            for (int row = 0; row < numberTextsVertical.Length; row++)
            {
                ConsolePrinter.Print(startX, startY + row * (_fieldSize + 1), numberTextsVertical[row], _legendBackground, _legendForeground);
            }
        }

        public void DrawGameObject(int row, int col, ConsoleDrawGameObjectVisitor visitor)
        {
            if (!(row >= 0 && row < _board.Height))
            {
                throw new ArgumentOutOfRangeException(nameof(row), "Cannot be out of board range!");
            }

            if (!(col >= 0 && col < _board.Width))
            {
                throw new ArgumentOutOfRangeException(nameof(col), "Cannot be out of board range!");
            }

            if (visitor == null)
            {
                throw new ArgumentNullException(nameof(visitor), "Cannot be null!");
            }

            if (!_board.DoesGameObjectExist(row, col))
            {
                throw new NoGameObjectDetectedException("No game object was detected at the given position!");
            }

            var go = _board.Get(row, col);
            int fieldLeftUpperCornerX = GetFieldLeftUpperX(col);
            int fieldLeftUpperCornerY = GetFieldLeftUpperY(row);
            visitor.CurrentX = fieldLeftUpperCornerX + (_fieldSize / 2);
            visitor.CurrentY = fieldLeftUpperCornerY + (_fieldSize / 2);
            go.AcceptVisitor(visitor);
        }

        public void EraseGameObject(int row, int col, ConsoleEraseGameObjectVisitor visitor)
        {
            if (!(row >= 0 && row < _board.Height))
            {
                throw new ArgumentOutOfRangeException(nameof(row), "Cannot be out of board range!");
            }

            if (!(col >= 0 && col < _board.Width))
            {
                throw new ArgumentOutOfRangeException(nameof(col), "Cannot be out of board range!");
            }

            if (visitor == null)
            {
                throw new ArgumentNullException(nameof(visitor), "Cannot be null!");
            }

            var go = _board.Get(row, col);
            int fieldLeftUpperCornerX = GetFieldLeftUpperX(col);
            int fieldLeftUpperCornerY = GetFieldLeftUpperY(row);
            visitor.CurrentX = fieldLeftUpperCornerX + (_fieldSize / 2);
            visitor.CurrentY = fieldLeftUpperCornerY + (_fieldSize / 2);
            go.AcceptVisitor(visitor);
            DrawField(row, col);
        }

        public void DrawField(int row, int col)
        {
            if (!(row >= 0 && row < _board.Height))
            {
                throw new ArgumentOutOfRangeException(nameof(row), "Cannot be out of board range!");
            }

            if (!(col >= 0 && col < _board.Width))
            {
                throw new ArgumentOutOfRangeException(nameof(col), "Cannot be out of board range!");
            }

            int x = GetFieldLeftUpperX(col);
            int y = GetFieldLeftUpperY(row);

            for (int r = 0; r < _fieldSize; r++)
            {
                for (int c = 0; c < _fieldSize; c++)
                {
                    ConsolePrinter.Print(x + c, y + r, Convert.ToString(_fieldChar), _fieldBackground, _fieldForeground);
                }    
            }
        }

        private void DrawVerticalFrameBars()
        {
            int barLength = (_fieldSize * _board.Height) + _board.Height + 1;

            for (int col = 0; col < _board.Width + 1; col++)
            {
                int x = _leftUpperCornerX + col * (_fieldSize + 1);

                for (int row = 0; row < barLength; row++)
                {
                    int y = _leftUpperCornerY + row;
                    ConsolePrinter.Print(x, y, Convert.ToString(_frameChar), _frameBackground, _frameForeground);
                }
            }
        }

        private void DrawHorizontalFrameBars()
        {
            string barToDraw = string.Join("", _frameChar.RepeatFor((_fieldSize * _board.Width) + _board.Width + 1));

            for (int i = 0; i < _board.Height + 1; i++)
            {
                ConsolePrinter.Print(_leftUpperCornerX, _leftUpperCornerY + i * (_fieldSize + 1), barToDraw,  _frameBackground, _frameForeground);
            }
        }

        private int GetFieldLeftUpperX(int col)
        {
            if (!(col >= 0 && col < _board.Width))
            {
                throw new ArgumentOutOfRangeException(nameof(col), "Cannot be out of board range!");
            }

            return _leftUpperCornerX + 1 + col * (_fieldSize + 1);
        }

        private int GetFieldLeftUpperY(int row)
        {
            if (!(row >= 0 && row < _board.Height))
            {
                throw new ArgumentOutOfRangeException(nameof(row), "Cannot be out of board range!");
            }

            return _leftUpperCornerY + 1 + row * (_fieldSize + 1);
        }
    }
}
