using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BarbarianTMwarsTM.Maps;
using BarbarianTMwarsTM.BattleMenu;
using BarbarianTMwarsTM.Maps.BattleInputHandlers;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace BarbarianTMwarsTM.BattleMenu.StandardMenuItems
{
    class StandardMenuItemNext : BattleMenuItem
    {
        Map BattleMap;
        public StandardMenuItemNext(Map battleMap, Texture2D sprite)
            : base(battleMap, sprite)
        {
            BattleMap = battleMap;

        }

        public override void Activate()
        {
            List<BattleMenuItem> menuItems = new List<BattleMenuItem>();            
            menuItems.Add(new StandardMenuItemClose(BattleMap, BattleMap.menuClose));
            MenuContent newMenu = new MenuContent(menuItems, MenuContent.inputHandler);
            StandardMenuInputHandler newHandler = new StandardMenuInputHandler(BattleMap, new Point (MenuContent.inputHandler.drawingRectangle.X,MenuContent.inputHandler.drawingRectangle.Y), newMenu);
            BattleMap.inputHandler = newHandler;
        }
    }
}
