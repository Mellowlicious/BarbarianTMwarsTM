using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BarbarianTMwarsTM.MapClasses;

namespace BarbarianTMwarsTM.MapClasses
{
    class Tile
    {
        public TileType tileType;
        public SpriteType spriteType;
        public Tile(TileType tile, SpriteType sprite)
        {
            tileType = tile;
            spriteType = sprite;
        }
        public Tile(TileType tile)
        {
            tileType = tile;

        }
    }

    enum TileType { Sea, River, Plains, Road, Mountain, Forest };
    enum SpriteType { Sea };

}
