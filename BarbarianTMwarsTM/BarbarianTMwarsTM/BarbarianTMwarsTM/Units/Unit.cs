using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
namespace BarbarianTMwarsTM.Units
{
    abstract class Unit
    {
        int hpInterval;
        int maxHP = 100;
        int moraleInterval;
        int maxMorale = 100;
        int minMorale = 0;

        public UnitProperties unitProps;
        public int ControllingPlayer;

        public bool hasMoved=true;
        public bool isCapturing=false;
        public bool selected = false;
        public Point Position;

        public UnitTypeEnum unitType;
    }



}
