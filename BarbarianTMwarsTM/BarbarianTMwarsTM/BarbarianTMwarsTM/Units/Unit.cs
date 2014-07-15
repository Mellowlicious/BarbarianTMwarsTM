using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using BarbarianTMwarsTM.Maps;
namespace BarbarianTMwarsTM.Units
{
    public class Unit
    {
        Map gameMap;

        int hpInterval=100;
        int maxHP = 100;
        int moraleInterval=100;
        int maxMorale = 100;
        int minMorale = 0;


        //Later put shit in unitporoperties, don't use it right now
        public UnitProperties unitProps;
        //Integer saying which player is controlling the unit. Enum later maybe?
        public int ControllingPlayer;

        //Whether the unit has already moved this turn (thus inactive)
        public bool hasMoved=true;
        public bool isMoving = false;
        //Whether the unit is busy capturing a village
        public bool isCapturing=false;
        //Whether the unit is currently being selected by mousepress/buttonpress
        public bool selected = false;
        //Integer (x,y) position on the battlefield in squares (not pixels!)
        public Point Position;
        

        //Later put this in unitproperties
        int movementAllowance;

        //For easy access of what kind of unit it is
        public UnitTypeEnum unitType;
        public MovementType movementType;

        //Later we need to also implement sprites for moving, being selected, etcetera. This is mostly placeholder. 
        Texture2D idleSprite;

        public Unit(Map gameMap, UnitTypeEnum type, Texture2D sprite, Point pos, int player, bool hasMoved)
        {
            //Create a testing dummy unit

            this.gameMap = gameMap;
            ControllingPlayer = player;
            unitType = type;
            
            switch (unitType)
            {
                case UnitTypeEnum.Militia:
                    {
                        movementType = MovementType.Militia;
                        movementAllowance = 3;
                        break;
                    }
                default:
                    {                                              
                        movementType = MovementType.Militia;
                        movementAllowance = 3;
                        break;
                    }
            }

            Position = pos;
            this.hasMoved = hasMoved;
            idleSprite = sprite;

            gameMap.listOfUnits[player].Add(this);
            gameMap.unitPositions[pos.X, pos.Y] = this;
        }


        public void Draw(GameTime gameTime)
        {
            //Assuming a static sprite-size of 64x64: Change this later when implementing zoom levels!

            int xOffset = -gameMap.viewPort.X + Position.X*64;
            int yOffset = -gameMap.viewPort.Y + Position.Y*64;
            if (!hasMoved)
                gameMap.Game.GetSpriteBatch.Draw(idleSprite,new Rectangle(xOffset,yOffset,64,64),Color.White);
            else
                gameMap.Game.GetSpriteBatch.Draw(idleSprite, new Rectangle(xOffset, yOffset, 64, 64), Color.Gray);

        }

        public void MouseClick()
        {
            //What happens on a mouseclick
            if (hasMoved)
                return;

            else
            {
                this.selected = true;
                bool[,] movementPos = FindMovementPositions();
                gameMap.moving = true;
                gameMap.movementSquares = movementPos;
            }

        }

        public void Destroy()
        {
            //This actually shouldn't return void maybe; you get stuff like
            //officer power and stats from destruction. 
            Dispose();
        }

        public void Dispose()
        {
            //Removes from the gameMap 
            gameMap.listOfUnits[ControllingPlayer].Remove(this);
            gameMap.unitPositions[Position.X, Position.Y] = null;
        }

        public void ChangePosition(Point newGridPosition)
        {
            gameMap.unitPositions[Position.X, Position.Y] = null;
            Position = newGridPosition;
            gameMap.unitPositions[Position.X, Position.Y] = this;
        }

        public bool[,] FindMovementPositions()
        {
            bool[,] retVal = new bool[gameMap.GridDimension.X,gameMap.GridDimension.Y];
            int[,] workingMatrix = new int[gameMap.GridDimension.X,gameMap.GridDimension.Y];
            //We're going to store in the workingMatrix how much of our movement allowance has been used up. 
            //We'll go through all squares in a depth-first search way.
            //For this we store a list of all possible squares to move to.

            Stack<Point> DFSlist = new Stack<Point>();
            DFSlist.Push(Position);
            
            while (DFSlist.Count > 0)
            {
            
                //Check all four directions (this should be done more efficiently but whatever)
                Point currentPos = DFSlist.Pop();
                

                List<Point> newPoss = new List<Point>();
                newPoss.Add(new Point(currentPos.X, currentPos.Y - 1));
                newPoss.Add( new Point(currentPos.X + 1, currentPos.Y));
                newPoss.Add(new Point(currentPos.X - 1, currentPos.Y));
                newPoss.Add(new Point(currentPos.X, currentPos.Y + 1));
                for (int i = 0; i < newPoss.Count; i++)
                {

                    

                    //Check here if the new position is accessible by the current unit, terrain-wise. If not, quit.
                    //TODO
                    //Check here if an enemy unit is blocking the way. If so, quit.
                    if (gameMap.unitPositions[newPoss[i].X, newPoss[i].Y] != null && gameMap.unitPositions[newPoss[i].X, newPoss[i].Y].ControllingPlayer != this.ControllingPlayer)
                    {

                        

                        break;
                    }
                    //Check if moving from this position is either possible or better. currently this uses 1 
                    //movement allowance for every square, later change this to use the appriopriate movement
                    //allowance from the terrain.
                    int newMovAll = workingMatrix[currentPos.X, currentPos.Y] + 1 ;

                    
                    if (newMovAll<= movementAllowance)
                    {
                        if (workingMatrix[newPoss[i].X, newPoss[i].Y] > 0)
                        {
                            if (newMovAll < workingMatrix[newPoss[i].X, newPoss[i].Y])
                            {
                                workingMatrix[newPoss[i].X, newPoss[i].Y] = newMovAll;
                                DFSlist.Push(newPoss[i]);
                            }
                        }
                        else
                        {
                            //Add the movement allowance and add the new position to the DFS stack
                            workingMatrix[newPoss[i].X, newPoss[i].Y] = newMovAll;
                            DFSlist.Push(newPoss[i]);
                        }
                    }
                   // System.Diagnostics.Debugger.Break();
                }             

            }

            for (int i = 0; i < workingMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < workingMatrix.GetLength(1); j++)
                {
                    if (workingMatrix[i, j] > 0)
                        retVal[i,j] = true;
                }
            }
            retVal[Position.X, Position.Y] = true;
            return retVal;
        }

        
    }



}
