using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BarbarianTMwarsTM.Maps;
using BarbarianTMwarsTM.Maps.BattleInputHandlers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BarbarianTMwarsTM.BattleMenu;

namespace BarbarianTMwarsTM.Maps.BattleInputHandlers
{
    class StandardMenuInputHandler : IInputHandler
    {
        Map BattleMap;
        Point oldPosition;
        List<IBattleMenuItem> MenuItems;
        Rectangle drawingRectangle;

        public StandardMenuInputHandler(Map battleMap, Point clickPosition, List<IBattleMenuItem> menuItems)
        {
            BattleMap = battleMap;
            oldPosition = clickPosition;
            drawingRectangle = new Rectangle(clickPosition.X, clickPosition.Y, 128, 30 + 40 * menuItems.Count);
            MenuItems = menuItems;
            BattleMap.menuStartPos = new Point(drawingRectangle.X, drawingRectangle.Y);
            BattleMap.drawMenu = true;

            BattleMap.menuDrawOrder = buildDrawOrder();
            BattleMap.drawHighlightedTile = false;
            
        }

        List<Texture2D> buildDrawOrder()
        {
            List<Texture2D> retval = new List<Texture2D>();            
            for (int i = 0; i < MenuItems.Count; i++)
            {
                retval.Add(MenuItems[i].NormalSprite);
            }
            return retval;

        }

        public void LeftMouseClick(Point mousePosition)
        {
            if (drawingRectangle.Contains(mousePosition))
            {
                Point menuOffset = new Point(mousePosition.X - drawingRectangle.X, mousePosition.Y - drawingRectangle.Y);
                int yOffset = 15;
                for (int i = 0; i < MenuItems.Count; i++)
                {
                    if (new Rectangle(drawingRectangle.X, drawingRectangle.Y + yOffset, 128, 40).Contains(mousePosition))
                    {
                        MenuItems[i].Activate();
                    }
                    yOffset += 40;
                }
            }
            else
            {
                CloseMenu();
            }
        }

        public void RightMouseClick(Point mousePosition)
        {
            CloseMenu();
        }

        public void MouseMove(Point newPosition)
        {
            BattleMap.UpdateTileHighlight(newPosition);
        }
        public void CloseMenu()
        {
            BattleMap.drawMenu = false;
            StandardInputHandler newHandler = new StandardInputHandler(BattleMap);
            newHandler.oldPosition = oldPosition;
            BattleMap.inputHandler = newHandler;
        }
    }
}
