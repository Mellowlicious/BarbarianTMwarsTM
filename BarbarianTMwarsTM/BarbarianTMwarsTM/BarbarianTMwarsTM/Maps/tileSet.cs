using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using BarbarianTMwarsTM.Maps;

namespace BarbarianTMwarsTM.Maps
{


    public class TileSet
    {
        BW Game;
        Map MapParent;


        bool loaded = false;
        //infinitile is the repeating tile around the edge, also used for empty tiles      /* maar wat doen we dan als de map een kustlijn is? ik denk dat we out of bounds gewoon naar black moeten faden */
        Tile infiniTile;
        public Tile[,] Tiles;

        //replace with library
        Texture2D Sea;
        Texture2D Plains;
        Texture2D Mountain;
        Texture2D Forest;

        public TileSet(Map mapParent)
        {
            MapParent = mapParent;
            Game = mapParent.Game;
            infiniTile = new Tile(TileType.Sea, SpriteType.Sea);                            /* THIJS これから voorbeeldkaart, vervangen door load of de constructor arguments die dezelfde data bevatten (?) */
            //Tiles = new Tile[15, 11];                                                       
            //for (int i = 0; i < Tiles.GetLength(0); i++)
            //{
            //    for (int j = 0; j < Tiles.GetLength(1); j++)
            //    {
            //        Tiles[i, j] = new Tile(TileType.Sea, SpriteType.Sea);
            //    }
            //}                                                                               /* THIJS これまで　voorbeeldkaart, vervangen door load of de constructor arguments die dezelfde data bevatten (?) */

        }

       
        public void LoadContent()
        {
            Sea = Game.ContentManager.Load<Texture2D>("Placeholders/Map/terrain_sea");
            Plains = Game.ContentManager.Load<Texture2D>("Placeholders/Map/terrain_plains");
            Mountain = Game.ContentManager.Load<Texture2D>("Placeholders/Map/terrain_mountain");
            Forest = Game.ContentManager.Load<Texture2D>("Placeholders/Map/terrain_forest");
            if (
                (Sea != null)&&
                (Mountain != null)&&
                (Plains != null)&&
                (Forest != null)
                )
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
                for (int j = 0; j < Tiles.GetLength(1); j++)
                {
                    for (int i = 0; i < Tiles.GetLength(0); i++)
                    {
                        try
                        {
                            switch (Tiles[i, j].spriteType)
                            {
                                case SpriteType.Sea:
                                    {
                                        sb.Draw(Sea, new Rectangle(offSet.X + i * spriteSize.X, offSet.Y + j * spriteSize.Y, spriteSize.X, spriteSize.Y), Color.White);
                                        break;
                                    }
                                case SpriteType.Mountain:
                                    {
                                        sb.Draw(Mountain, new Rectangle(offSet.X + i * spriteSize.X, offSet.Y + j * spriteSize.Y, spriteSize.X, spriteSize.Y), Color.White);
                                        break;
                                    }
                                case SpriteType.Forest:
                                    {
                                        sb.Draw(Forest, new Rectangle(offSet.X + i * spriteSize.X, offSet.Y + j * spriteSize.Y, spriteSize.X, spriteSize.Y), Color.White);
                                        break;
                                    }
                                case SpriteType.Plains:
                                    {
                                        sb.Draw(Plains, new Rectangle(offSet.X + i * spriteSize.X, offSet.Y + j * spriteSize.Y, spriteSize.X, spriteSize.Y), Color.White);
                                        break;
                                    }
                                default:
                                    {
                                        sb.Draw(Sea, new Rectangle(offSet.X + i * spriteSize.X, offSet.Y + j * spriteSize.Y, spriteSize.X, spriteSize.Y), Color.White);
                                        break;
                                    }
                            }
                                
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
