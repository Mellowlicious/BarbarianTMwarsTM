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
    public class StandardMenuInputHandler : IInputHandler
    {
        Map BattleMap;
        Point oldPosition;
        MenuContent MenuContent;
      //  List<IBattleMenuItem> MenuItems;
        public Rectangle drawingRectangle;

        public StandardMenuInputHandler(Map battleMap, Point clickPosition, MenuContent content) //List<IBattleMenuItem> menuItems)
        {
            content.inputHandler = this;
            BattleMap = battleMap;
            oldPosition = clickPosition;
            MenuContent = content;
            drawingRectangle = new Rectangle(clickPosition.X, clickPosition.Y, 128, 30 + 40 *  MenuContent.ItemList.Count);
           // MenuItems = menuItems;
            Activate();
            
        }
        public void Activate()
        {
            BattleMap.menuStartPos = new Point(drawingRectangle.X, drawingRectangle.Y);
            BattleMap.drawMenu = true;
            BattleMap.menuDrawOrder = buildDrawOrder();
            BattleMap.drawHighlightedTile = false;
        }

        List<Texture2D> buildDrawOrder()
        {
            List<Texture2D> retval = new List<Texture2D>();            
            for (int i = 0; i < MenuContent.ItemList.Count; i++)
            {
                retval.Add(MenuContent.ItemList[i].NormalSprite);
            }
            return retval;

        }

        public void LeftMouseClick(Point mousePosition)
        {
            if (drawingRectangle.Contains(mousePosition))
            {
                Point menuOffset = new Point(mousePosition.X - drawingRectangle.X, mousePosition.Y - drawingRectangle.Y);
                int yOffset = 15;
                for (int i = 0; i <MenuContent.ItemList.Count; i++)
                {
                    if (new Rectangle(drawingRectangle.X, drawingRectangle.Y + yOffset, 128, 40).Contains(mousePosition))
                    {
                        MenuContent.ItemList[i].Activate();
                    }
                    yOffset += 40;
                }
            }
            else
            {
                PrevInput();
            }
        }

        public void RightMouseClick(Point mousePosition)
        {
            PrevInput();
        }

        public void MouseMove(Point newPosition)
        {
            BattleMap.UpdateTileHighlight(newPosition);
        }
        public void PrevInput()
        {
            if (MenuContent.prevInput != null)
            {
                BattleMap.inputHandler = MenuContent.prevInput;
                BattleMap.inputHandler.Activate();
            }
            else
            {
                //There was no previous menu to step back into, so we switch back to the 
                BattleMap.drawMenu = false;
                StandardInputHandler newHandler = new StandardInputHandler(BattleMap);
                newHandler.oldPosition = oldPosition;
                BattleMap.inputHandler = newHandler;
            }
        }
    }
}
