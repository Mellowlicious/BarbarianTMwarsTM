using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using BarbarianTMwarsTM.MapClasses;

namespace BarbarianTMwarsTM.MapClasses
{


    class TileSet
    {
        BW Game;
        Map MapParent;


        bool loaded = false;
        //infinitile is the repeating tile around the edge, also used for empty tiles
        Tile infiniTile;
        Tile[,] Tiles;

        //replace with library
        Texture2D Sea;

        public TileSet(Map mapParent)
        {
            MapParent = mapParent;
            Game = mapParent.Game;
            infiniTile = new Tile(TileType.Sea, SpriteType.Sea);
            Tiles = new Tile[15, 11];
            for (int i = 0; i < Tiles.GetLength(0); i++)
            {
                for (int j = 0; j < Tiles.GetLength(1); j++)
                {
                    Tiles[i, j] = new Tile(TileType.Sea, SpriteType.Sea);
                }
            }

        }

       
        public void LoadContent()
        {
            Sea = Game.ContentManager.Load<Texture2D>("SeaNoAnim");
            Console.WriteLine("Ok made it here");
            if (Sea != null)
                loaded = true;
        }

        public void Draw(GameTime gameTime)
        {
            SpriteBatch sb = Game.GetSpriteBatch;
            Rectangle viewPort = MapParent.viewPort;
            Point spriteSize = new Point(64,64);
            Point offSet = new Point(-viewPort.X, -viewPort.Y);
            if (loaded)
            {
                for (int i = 0; i < Tiles.GetLength(0); i++)
                {
                    for (int j = 0; j < Tiles.GetLength(1); j++)
                    {
                        try
                        {
                            sb.Draw(Sea, new Rectangle(offSet.X + i * spriteSize.X, offSet.Y + j * spriteSize.Y, spriteSize.X, spriteSize.Y), Color.White);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.ToString());
                        }

                    }

                }
            }


        }


    }
}
