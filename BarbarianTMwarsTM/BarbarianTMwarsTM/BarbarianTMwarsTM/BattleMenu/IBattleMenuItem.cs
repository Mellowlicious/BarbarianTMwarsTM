using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BarbarianTMwarsTM.BattleMenu
{
    public interface IBattleMenuItem
    {
        Texture2D NormalSprite
        {
            get;            
        }
        Texture2D SelectedSprite
        {
            get;            
        }
 
        bool Highlighted
        {
            get;
            set;
        }
        void Activate();

    }
}
