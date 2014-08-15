using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BarbarianTMwarsTM.Maps;

namespace BarbarianTMwarsTM.BattleMenu
{
    public abstract class BattleMenuItem
    {
        public MenuContent menuContent;
        Texture2D normalSprite;
        Map battleMap;
        public BattleMenuItem(Map battleMap, Texture2D sprite)
        {
            normalSprite = sprite;
            this.battleMap = battleMap;
        }


        public Texture2D NormalSprite
        {
            get
            {
             return normalSprite;   
            }
        }
        Texture2D SelectedSprite
        {
            get
            {
                return normalSprite;
            }
        }
 
        bool Highlighted
        {
            get;
            set;
        }
        public MenuContent MenuContent
        {
            get
            {
                return menuContent;
            }
            set
            {
                menuContent = value;
            }
        }
        public abstract void Activate();

    }
}
