using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using BarbarianTMwarsTM.Maps;
namespace BarbarianTMwarsTM.Units
{
    class Unit
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
    }



}
