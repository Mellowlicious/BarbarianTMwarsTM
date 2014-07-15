using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using BarbarianTMwarsTM.Maps;

namespace BarbarianTMwarsTM.Maps
{
    class Map : DrawableGameComponent
    {
        new public BW Game;
        public TileSet tileSet;
        public Rectangle viewPort;
        //if the mouse is outside this box, the viewport changes
        public Rectangle movementBox;
        //mapBox is the total area that is scrollable on the map
        Rectangle mapBox;
        

        
        public Map(BW game) : base(game)
        {
            Game = game;
            tileSet = new TileSet(this);
            
        }

        public override void Initialize()
        {
            Console.WriteLine("map.init");

            movementBox = new Rectangle(10, 10, Game.Resolution.X - 20, Game.Resolution.Y - 20);
           // viewPort = new Rectangle(-200, -200, Game.Resolution.X, Game.Resolution.Y);
            //
          //  mapBox = new Rectangle(viewPort
            base.Initialize();

            
            
        }

        protected override void LoadContent()
        {
            Console.WriteLine("map.loadcontent");
            base.LoadContent();
            tileSet.LoadContent();

            AfterLoadContent();
        }

        private void AfterLoadContent()
        {
            BuildMapBox();
            viewPort = new Rectangle(mapBox.Center.X-(Game.Resolution.X/2), mapBox.Center.Y-(Game.Resolution.Y/2), Game.Resolution.X, Game.Resolution.Y);

            
            
            //This is to test the scrolling and bounding of the map: Delete asap
            mapBox.Inflate(50, 50);
        }

        private void BuildMapBox()
        {
            //Builds a new mapBox, the complete viewable rectangle

            //Note: This should also be called after a zoom in/zoom out!
            //This is currently static, but should also depend on the zoom level. Currently expects a hard-coded size of 64x64 for every tile.
            int tilesWidth = tileSet.Tiles.GetLength(0)*64;
            int tilesLength = tileSet.Tiles.GetLength(1)*64;
            mapBox = new Rectangle(0, 0, tilesWidth, tilesLength);
         
            if (mapBox.Width < Game.Resolution.X)
            {
                //If the width of our mapbox doesn't span the complete resolution, enlarge it until we reach our wanted width. Inflate expands on both sides,
                //so we halve the difference.
                mapBox.Inflate((Game.Resolution.X-mapBox.Width)/2,0);
            }
            if (mapBox.Height < Game.Resolution.Y)
            {
                mapBox.Inflate(0, (Game.Resolution.Y - mapBox.Height) / 2);
            }

        }

        public override void Update(GameTime gameTime)
        {
            Point mousePosition = new Point(Mouse.GetState().X, Mouse.GetState().Y);
            if (mousePosition.X < movementBox.Left)
            {
                //scroll left
                
                viewPort.X -= (int)( gameTime.ElapsedGameTime.TotalMilliseconds /3);
                if (viewPort.Left < mapBox.Left)
                    viewPort.X = mapBox.X;
            }
            else if (mousePosition.X > movementBox.Right)
            {
                //scroll right
                viewPort.X += (int)(  gameTime.ElapsedGameTime.TotalMilliseconds/3);
                if (viewPort.Right > mapBox.Right)
                    viewPort.X = mapBox.X + mapBox.Width-viewPort.Width;
            }
            if (mousePosition.Y < movementBox.Top)
            {
                //scroll up
                viewPort.Y -= (int)(gameTime.ElapsedGameTime.TotalMilliseconds /3);
                if (viewPort.Top < mapBox.Top)
                    viewPort.Y = mapBox.Y;
            }
            else if (mousePosition.Y > movementBox.Bottom)
            {
                //scroll down
                viewPort.Y += (int)(gameTime.ElapsedGameTime.TotalMilliseconds/3);
                if (viewPort.Bottom > mapBox.Bottom)
                    viewPort.Y = mapBox.Y + mapBox.Height-viewPort.Height;
            }
            base.Update(gameTime);
        }
        public override void Draw(GameTime gameTime)
        {

            base.Draw(gameTime);
            tileSet.Draw(gameTime);
            
        }
    }
}
