using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using BarbarianTMwarsTM.Units;

namespace BarbarianTMwarsTM.Maps.BattleInputHandlers
{
    class StandardInputHandler : IInputHandler
    {
        Map BattleMap;
        Point oldPosition= new Point(0,0);
        public StandardInputHandler(Map battleMap)
        {
            BattleMap = battleMap;
            BattleMap.drawHighlightedTile = true;
            BattleMap.drawMovementArrows = false;
        }
        public void LeftMouseClick(Point mousePosition)
        {
            //What happens on a mouseclick. Should handle stuff like dialogs first over
            //map clicking. Currently only unit clicking for testing stuff.
            Point gridPos = BattleMap.GetGridPosition(mousePosition);

            if ((gridPos.X > -1 && gridPos.Y > -1))
            {
                if (BattleMap.unitPositions[gridPos.X, gridPos.Y] != null)
                {
                    Unit tempUnit = BattleMap.unitPositions[gridPos.X, gridPos.Y];
                    if (!tempUnit.hasMoved)
                    {
                        BattleMap.selectedUnit = tempUnit;
                        tempUnit.Select();
                        BattleMap.showMovementSquares = true;
                        //Change to a different input handler
                        unitMovementInputHandler newHandler =  new unitMovementInputHandler(BattleMap);
                        newHandler.oldPosition = this.oldPosition;
                        BattleMap.inputHandler = newHandler;
                        
                    }                  
                }             
            }
          
        }

        public void MouseMove(Point newPosition)
        {
            BattleMap.UpdateTileHighlight(newPosition);

            oldPosition = newPosition;
        }
        
        public void RightMouseClick(Point mousePosition)
        {
            
        }
    }
}
