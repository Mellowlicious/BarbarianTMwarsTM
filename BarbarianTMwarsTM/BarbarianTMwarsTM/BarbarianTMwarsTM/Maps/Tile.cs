using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BarbarianTMwarsTM.Maps;

namespace BarbarianTMwarsTM.Maps
{
    public class Tile
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

    public enum TileType { Sea, River, Plains, Road, Mountain, Forest };
    public enum SpriteType { Sea, River, Plains, Road, Mountain, Forest }; /* WAARSCHUWING, NIET SAFE ATM. ALS IE CRASHED IS HET OMDAT IE IETS ANDERS DAN ZEE PROBEERT TE TEKENEN */

}
