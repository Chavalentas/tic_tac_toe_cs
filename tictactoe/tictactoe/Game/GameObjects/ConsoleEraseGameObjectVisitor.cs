using ConsoleUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tictactoe.Game.GameObjects
{
    public class ConsoleEraseGameObjectVisitor : DrawGameObjectVisitor
    {
        private int _currentX;

        private int _currentY;

        public ConsoleEraseGameObjectVisitor(int initialX, int initialY,
            ConsoleColor initialBackgroundColor, ConsoleColor initialForegroundColor)
        {
            CurrentX = initialX;
            CurrentY = initialY;
            CurrentBackgroundColor = initialBackgroundColor;
            CurrentForegroundColor = initialForegroundColor;
        }

        public int CurrentX
        {
            get
            {
                return _currentX;
            }

            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(CurrentX), "Cannot be out of range!");
                }

                _currentX = value;
            }
        }

        public int CurrentY
        {
            get
            {
                return _currentY;
            }

            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(CurrentY), "Cannot be out of range!");
                }

                _currentY = value;
            }
        }

        public ConsoleColor CurrentForegroundColor
        {
            get;
            set;
        }

        public ConsoleColor CurrentBackgroundColor
        {
            get;
            set;
        }

        public override void Visit(Circle circle)
        {
            if (circle == null)
            {
                throw new ArgumentNullException(nameof(circle), "Cannot be null!");
            }

            ConsolePrinter.Print(CurrentX, CurrentY, " ", CurrentBackgroundColor, CurrentForegroundColor);
        }

        public override void Visit(Cross cross)
        {
            if (cross == null)
            {
                throw new ArgumentNullException(nameof(cross), "Cannot be null!");
            }

            ConsolePrinter.Print(CurrentX, CurrentY, " ", CurrentBackgroundColor, CurrentForegroundColor);
        }
    }
}
