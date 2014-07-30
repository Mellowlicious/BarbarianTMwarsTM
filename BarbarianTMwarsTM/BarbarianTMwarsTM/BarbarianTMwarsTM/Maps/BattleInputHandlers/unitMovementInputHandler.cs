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
        //The position during the previous mouse update
        public Point oldPosition;

        //List of trailing arrow behind a unit, signifying the way the unit moves
        public List<Point> arrowPositions;
        

        public unitMovementInputHandler(Map battleMap)
        {
            BattleMap = battleMap;
            selectedUnit = BattleMap.selectedUnit;
            arrowPositions = new List<Point>();
            BattleMap.movementArrows = arrowPositions;
            Activate();
        }

        public void LeftMouseClick(Point mousePosition)
        {
            Point gridPos = BattleMap.GetGridPosition(mousePosition);
            if (gridPos.X >= 0&&gridPos.Y>=0)
            {
                if (BattleMap.movementSquares[gridPos.X, gridPos.Y])
                {
                    if (BattleMap.unitPositions[gridPos.X, gridPos.Y] != null)
                    {
                        //There's a unit at target location; do nothing or play error sound or something

                    }
                    else
                    {                        
                        selectedUnit.ChangePosition(gridPos);
                        //selectedUnit.hasMoved = true;
                        SwapToStandardInput();
                    }
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
            //BattleMap.isTileHighlighted = false;
            Point newGridPos = BattleMap.GetGridPosition(newPosition);
            bool madeListLonger = false;

            //Check if the new position is in the bounds of the map
            if (newGridPos.X >= 0 && newGridPos.Y >= 0)
            {
                //Check if the new position is one of the valid squares for movement
                if (BattleMap.movementSquares[newGridPos.X, newGridPos.Y])
                {
                    //If it is, we're going to have to change the arrowPosition list somehow.
                    Point oldGridPos = BattleMap.GetGridPosition(oldPosition);
                    //Check if our mouse movement has actually changed squares. Maybe do this earlier?
                    if (newGridPos.X != oldGridPos.X || newGridPos.Y != oldGridPos.Y)
                    {
                        //Check whether our new square is the square our unit is standing on to reset movement
                        if (newGridPos.X == selectedUnit.Position.X && newGridPos.Y == selectedUnit.Position.Y)
                        {
                            arrowPositions = new List<Point>();
                        }
                        else
                        {
                            //We need to check here later if the two squares don't connect directly
                            if (Math.Abs( newGridPos.X - oldGridPos.X) == 1 && Math.Abs(newGridPos.Y - oldGridPos.Y)==1)
                            {
                                //Fall through for now. Check corner case here.


                            }
                            //Here we check whether our new square was already in the list of arrowpositions. If it is,
                            //find this position and cull everything in the list that comes after it.
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
                            //If none of these cases have happened, check whether the Manhattan distance between the two consecutive
                            //points is larger than 1, meaning that they do not connect
                            if (!posExisted)
                            {
                                //We probably entered from outside of the array. Just calculate cheapest path.
                                if (Math.Abs(newGridPos.X - oldGridPos.X) + Math.Abs(newGridPos.Y - oldGridPos.Y) > 1)
                                {
                                    arrowPositions = FindCheapestPath(newGridPos);
                                }
                                else
                                {
                                    arrowPositions.Add(newGridPos);
                                    madeListLonger = true;
                                }
                            }
                        }
                    }
                }
                else
                {

                }
                if (madeListLonger)
                {
                    //Actually we need to check how much our movement allowance we've used, but since we haven't actually 
                    //added this property, we just count how many tiles we have in our list.
                    if (arrowPositions.Count > selectedUnit.movementAllowance)
                    {
                        //Whoops, our path is too long! Calculate a new one
                        arrowPositions = FindCheapestPath(newGridPos);
                    }

                }
            }
            BattleMap.movementArrows = arrowPositions;

            BattleMap.UpdateTileHighlight(newPosition);
            oldPosition = newPosition;
        }

        public List<Point> FindCheapestPath(Point gridPos)
        {
            //Finds a cheapest path from the selected square to the origin. Uses the allowance matrix calculated
            //by selecting a unit.
            List<Point> newPath = new List<Point>();
            //We're going to be working backwards, so every time we add the new point at the beginning of the list
            Point currentPoint = gridPos;
            newPath.Add(currentPoint);
            while (currentPoint.X != selectedUnit.Position.X || currentPoint.Y != selectedUnit.Position.Y)
            {
                //Every time we look in the allowanceArray which is the optimal square to go to next.
                //This method is deterministic and uses the following preference: left>down>right>up

                //First we check if the new square is our destination. If it is, we immediately take it.
                //then we check whether the value in the allowance array is larger than 0 (meaning that it is valid
                //to move to), and then whether this is a better (greedy) solution.
                //We need to check for destination separately as our destination always has a value of 0 in our allowance array.
                int currentBest = selectedUnit.allowanceArray[currentPoint.X, currentPoint.Y];
                Point currentBestPoint = currentPoint;
                if ((currentPoint.X-1 == selectedUnit.Position.X && currentPoint.Y == selectedUnit.Position.Y)||
                    (currentPoint.X-1>=0 && 
                        selectedUnit.allowanceArray[currentPoint.X-1, currentPoint.Y]>0 &&
                        selectedUnit.allowanceArray[currentPoint.X - 1, currentPoint.Y] < currentBest))
                {
                    
                    currentBestPoint = new Point(currentPoint.X - 1, currentPoint.Y);
                    currentBest = selectedUnit.allowanceArray[currentBestPoint.X, currentBestPoint.Y];
                }
                if ((currentPoint.X  == selectedUnit.Position.X && currentPoint.Y+1 == selectedUnit.Position.Y) ||
                    (currentPoint.Y < selectedUnit.allowanceArray.GetLength(1) && 
                        selectedUnit.allowanceArray[currentPoint.X, currentPoint.Y + 1] > 0 &&
                        selectedUnit.allowanceArray[currentPoint.X , currentPoint.Y+1] < currentBest))
                {
                    
                    currentBestPoint = new Point(currentPoint.X , currentPoint.Y+1);
                    currentBest = selectedUnit.allowanceArray[currentBestPoint.X, currentBestPoint.Y];
                }
                if ((currentPoint.X + 1 == selectedUnit.Position.X && currentPoint.Y == selectedUnit.Position.Y) ||
                        (currentPoint.X < selectedUnit.allowanceArray.GetLength(0) && 
                        selectedUnit.allowanceArray[currentPoint.X + 1, currentPoint.Y] > 0 &&
                        selectedUnit.allowanceArray[currentPoint.X + 1, currentPoint.Y] < currentBest))
                {
                    
                    currentBestPoint = new Point(currentPoint.X + 1, currentPoint.Y);
                    currentBest = selectedUnit.allowanceArray[currentBestPoint.X, currentBestPoint.Y];
                }
                if ((currentPoint.X  == selectedUnit.Position.X && currentPoint.Y-1 == selectedUnit.Position.Y) ||
                        (currentPoint.Y - 1 >= 0 && 
                        selectedUnit.allowanceArray[currentPoint.X, currentPoint.Y - 1] > 0 &&
                        selectedUnit.allowanceArray[currentPoint.X, currentPoint.Y - 1] < currentBest))
                {
                    
                    currentBestPoint = new Point(currentPoint.X, currentPoint.Y - 1);
                    currentBest = selectedUnit.allowanceArray[currentBestPoint.X, currentBestPoint.Y];
                }
                currentPoint = currentBestPoint;
                newPath.Insert(0, currentPoint);
            }
            newPath.RemoveAt(0);
            Console.WriteLine(newPath.Count);
            return newPath;
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
            StandardInputHandler newHandler = new StandardInputHandler(BattleMap);
            newHandler.oldPosition = this.oldPosition;
            BattleMap.inputHandler = newHandler;                

        }


        public void Activate()
        {
            BattleMap.drawMovementArrows = true;
            BattleMap.drawHighlightedTile = false;
            
        }
    }
}
