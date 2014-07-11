using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using BarbarianTMwarsTM.MapClasses;

namespace BarbarianTMwarsTM.MapClasses
{
    class Map : DrawableGameComponent
    {
        new public BW Game;
        public TileSet tileSet;
        public Rectangle viewPort;
        //if the mouse is outside this box, the viewport changes
        public Rectangle movementBox;
        

        
        public Map(BW game) : base(game)
        {
            Game = game;
            tileSet = new TileSet(this);
            
        }

        public override void Initialize()
        {
            Console.WriteLine("map.init");

            movementBox = new Rectangle(200, 150, Game.Resolution.X - 400, Game.Resolution.Y - 300);
            viewPort = new Rectangle(-200, -200, Game.Resolution.X, Game.Resolution.Y);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            Console.WriteLine("map.loadcontent");
            base.LoadContent();
            tileSet.LoadContent();
        }
        public override void Update(GameTime gameTime)
        {
            Point mousePosition = new Point(Mouse.GetState().X, Mouse.GetState().Y);
            if (mousePosition.X < movementBox.Left)
            {
                //scroll left
                
                viewPort.X -= (int)( gameTime.ElapsedGameTime.TotalMilliseconds /3);
            }
            else if (mousePosition.X > movementBox.Right)
            {
                //scroll right
                viewPort.X += (int)(  gameTime.ElapsedGameTime.TotalMilliseconds/3);
            }
            if (mousePosition.Y < movementBox.Top)
            {
                //scroll up
                viewPort.Y -= (int)(gameTime.ElapsedGameTime.TotalMilliseconds /3);
            }
            else if (mousePosition.Y > movementBox.Bottom)
            {
                //scroll down
                viewPort.Y += (int)(gameTime.ElapsedGameTime.TotalMilliseconds/3);
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
