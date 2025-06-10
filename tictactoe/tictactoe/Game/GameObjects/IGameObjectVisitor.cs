using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tictactoe.Game.GameObjects
{
    public interface IGameObjectVisitor
    {
        void Visit(Circle circle);

        void Visit(Cross cross);
    }
}
