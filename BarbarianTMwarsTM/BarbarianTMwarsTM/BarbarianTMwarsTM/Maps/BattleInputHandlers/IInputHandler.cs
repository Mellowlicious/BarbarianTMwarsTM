using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace BarbarianTMwarsTM.Maps.BattleInputHandlers
{
    public interface IInputHandler
    {

        void LeftMouseClick(Point mousePosition);
        void RightMouseClick(Point mousePosition);
        void MouseMove(Point newPosition);     

    }
}
