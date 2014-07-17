using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BarbarianTMwarsTM.Units;
using Microsoft.Xna.Framework;

namespace BarbarianTMwarsTM.Maps.BattleInputHandlers
{
    class unitMovementInputHandler : IInputHandler
    {
        Map BattleMap;
        Unit selectedUnit;
        public Point oldPosition;
        public List<Point> arrowPositions;
        int allowanceUsed;

        public unitMovementInputHandler(Map battleMap)
        {
            BattleMap = battleMap;
            selectedUnit = BattleMap.selectedUnit;
            BattleMap.drawMovementArrows = true;
            arrowPositions = new List<Point>();
            BattleMap.movementArrows = arrowPositions;
        }

        public void LeftMouseClick(Point mousePosition)
        {
            Point gridPos = BattleMap.GetGridPosition(mousePosition);
            if (gridPos.X >= 0&&gridPos.Y>=0)
            {
                if (BattleMap.movementSquares[gridPos.X, gridPos.Y])
                {
                    selectedUnit.ChangePosition(gridPos);
                    //selectedUnit.hasMoved = true;
                    SwapToStandardInput();
                }
                else
                {
                    SwapToStandardInput();
                }
            }
        }

        public void MouseMove(Point newPosition)
        {
            //BattleMap.UpdateTileHighlight(newPosition);
            BattleMap.isTileHighlighted = false;
            Point newGridPos = BattleMap.GetGridPosition(newPosition);
            if (newGridPos.X >= 0 && newGridPos.Y >= 0)
            {
                if (BattleMap.movementSquares[newGridPos.X, newGridPos.Y])
                {
                  
                    Point oldGridPos = BattleMap.GetGridPosition(oldPosition);
                    if (newGridPos.X != oldGridPos.X || newGridPos.Y != oldGridPos.Y)
                    {
                        if (newGridPos.X == selectedUnit.Position.X && newGridPos.Y == selectedUnit.Position.Y)
                        {
                            arrowPositions = new List<Point>();
                        }
                        else
                        {
                            if (newGridPos.X != oldGridPos.X && newGridPos.Y != oldGridPos.Y)
                            {
                                //Corner case. Fall through for now.
                            }
                            bool posExisted = false;
                            for (int i = 0; i < arrowPositions.Count; i++)
                            {
                                if (arrowPositions[i].X == newGridPos.X && arrowPositions[i].Y == newGridPos.Y)
                                {
                                    arrowPositions.RemoveRange(i + 1, arrowPositions.Count - i - 1);
                                    posExisted = true;
                                    break;
                                }
                            }
                            if (!posExisted)
                            {
                                arrowPositions.Add(newGridPos);
                            }
                        }
                    }
                }
                else
                {

                }
            }
            BattleMap.movementArrows = arrowPositions;
            oldPosition = newPosition;
        }


        public void RightMouseClick(Point mousePosition)
        {
            SwapToStandardInput();
        }

        private void SwapToStandardInput()
        {
            selectedUnit.isMoving = false;
            selectedUnit.Unselect();
            BattleMap.showMovementSquares = false;
            BattleMap.inputHandler = new StandardInputHandler(BattleMap);

        }
    }
}
