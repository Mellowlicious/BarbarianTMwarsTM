using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using BarbarianTMwarsTM.Units;
using BarbarianTMwarsTM.BattleMenu;
using BarbarianTMwarsTM.BattleMenu.StandardMenuItems;

namespace BarbarianTMwarsTM.Maps.BattleInputHandlers
{
    class StandardInputHandler : IInputHandler
    {
        Map BattleMap;
        public Point oldPosition= new Point(0,0);
        public StandardInputHandler(Map battleMap)
        {
            BattleMap = battleMap;
            BattleMap.drawHighlightedTile = true;
            BattleMap.drawMovementArrows = false;
            battleMap.drawMenu = false;
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
                    if (tempUnit.ControllingPlayer == BattleMap.activePlayer)
                    {
                        if (!tempUnit.hasMoved)
                        {
                            BattleMap.selectedUnit = tempUnit;
                            tempUnit.Select();
                            BattleMap.showMovementSquares = true;
                            //Change to a different input handler
                            unitMovementInputHandler newHandler = new unitMovementInputHandler(BattleMap);
                            newHandler.oldPosition = this.oldPosition;
                            BattleMap.inputHandler = newHandler;

                        }
                    }
                    else
                    {
                        //Can check enemy player movement; needs a different inputHandler?
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
            List<IBattleMenuItem> menuItems = new List<IBattleMenuItem>();
            menuItems.Add(new StandardMenuItemClose(BattleMap, BattleMap.menuClose));
            StandardMenuInputHandler newHandler = new StandardMenuInputHandler(BattleMap, mousePosition, menuItems);
            BattleMap.inputHandler = newHandler;
        }
    }
}
