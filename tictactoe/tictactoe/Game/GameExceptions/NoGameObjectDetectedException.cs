using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tictactoe.Game.GameExceptions
{
    public class NoGameObjectDetectedException : Exception
    {
        public NoGameObjectDetectedException(string message): base(message)
        {
        }
    }
}
