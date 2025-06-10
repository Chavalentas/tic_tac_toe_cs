using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tictactoe.Game.GameObjects
{
    public interface IGameObjectVisitor<T>
    {
        T Visit(Circle circle);

        T Visit(Cross cross);
    }
}
