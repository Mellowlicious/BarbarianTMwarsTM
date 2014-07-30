using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BarbarianTMwarsTM.Maps.BattleInputHandlers;

namespace BarbarianTMwarsTM.BattleMenu
{
    public class MenuContent
    {
        public List<BattleMenuItem> ItemList;
        public IInputHandler prevInput;
        public StandardMenuInputHandler inputHandler;
        public MenuContent(List<BattleMenuItem> itemList, IInputHandler prev)
        {
            ItemList = itemList;
            prevInput = prev;
            for (int i = 0; i < ItemList.Count; i++)
            {
                ItemList[i].MenuContent = this;
            }
        }

    }
}
