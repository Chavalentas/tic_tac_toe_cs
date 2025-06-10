using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tictactoe.Game.GameExceptions
{
    public class ReservedFieldException : Exception
    {
        public ReservedFieldException(string message): base(message)
        {
        }
    }
}
