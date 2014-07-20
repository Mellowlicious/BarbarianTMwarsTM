using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BarbarianTMwarsTM.Maps;
using BarbarianTMwarsTM.Maps.BattleInputHandlers;

namespace BarbarianTMwarsTM.BattleMenu.StandardMenuItems
{
    class StandardMenuItemClose : IBattleMenuItem
    {
        Texture2D normalSprite;
      //  StandardMenuInputHandler inputHandler;
        Map BattleMap;
        public StandardMenuItemClose(Map battleMap, Texture2D sprite)
        {
            normalSprite = sprite;
            BattleMap = battleMap;
            //inputHandler = handler;
        }
    
        public Texture2D  NormalSprite
        {
	        get { return normalSprite;}
        }

        public Texture2D  SelectedSprite
        {
            get { return normalSprite; }
        }

        public void  Activate()
        {
            ((StandardMenuInputHandler)BattleMap.inputHandler).CloseMenu();
        }


        public bool Highlighted
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}
