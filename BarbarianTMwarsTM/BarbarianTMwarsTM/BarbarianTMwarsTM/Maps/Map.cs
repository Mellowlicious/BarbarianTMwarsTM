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
using BarbarianTMwarsTM.Units;

namespace BarbarianTMwarsTM.Maps
{
    public class Map : DrawableGameComponent
    {
        new public BW Game;
        public TileSet tileSet;
        public Rectangle viewPort;
        //if the mouse is outside this box, the viewport changes
        public Rectangle movementBox;
        //mapBox is the total area that is scrollable on the map
        Rectangle mapBox;

        //Amount of factions. Every player is assigned an integer (player 1 is integer 0, player 2 is integer 1,
        //etc). Later we assign a faction, color, CO, maybe even player name to every integer. Or use a struct or something.
        public int amountOfPlayers;

        public Texture2D unitPlaceholder;

        Texture2D selectionCursor;


        public List<List<Unit>> listOfUnits;

        public Unit[,] unitPositions;
        public bool[,] movementSquares;
        public bool moving = false;

        Texture2D movementSelection;

        
        public Map(BW game) : base(game)
        {
            Game = game;
            tileSet = new TileSet(this);

            //Amount of players should be passed to the map creation, naturally, together with other info like CO
            amountOfPlayers=4;
            listOfUnits = new List<List<Unit>>();
            for (int i = 0; i < amountOfPlayers; i++)
            {
                listOfUnits.Add(new List<Unit>());
            }
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

            

            unitPlaceholder = Game.Content.Load<Texture2D>("Placeholders/Units/guy");
            selectionCursor = Game.Content.Load<Texture2D>("Placeholders/UI/SelectionCursor");
            movementSelection = Game.Content.Load<Texture2D>("Placeholders/UI/MovementSelection");

            MapIOHandler.LoadGame(this, "testmap");       //TEMP ~~ LOAD IN MAP
            Console.WriteLine("Map Loaded succesfully");

            AfterLoadContent();
        }

        private void AfterLoadContent()
        {
            

            BuildMapBox();
            viewPort = new Rectangle(mapBox.Center.X-(Game.Resolution.X/2), mapBox.Center.Y-(Game.Resolution.Y/2), Game.Resolution.X, Game.Resolution.Y);
            movementSquares = new bool[tileSet.Tiles.GetLength(0), tileSet.Tiles.GetLength(1)];
            
            //new Unit(this, UnitTypeEnum.Militia, unitPlaceholder, new Point(5, 4), 0, false);
            //new Unit(this, UnitTypeEnum.Militia, unitPlaceholder, new Point(5, 5), 0, true);
            
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

            if (moving)
            {
                for (int i = 0; i < movementSquares.GetLength(0); i++)
                {
                    for (int j = 0; j < movementSquares.GetLength(1); j++)
                    {
                        if (movementSquares[i,j])
                        {
                            Game.GetSpriteBatch.Draw(movementSelection, new Rectangle(i * 64 - viewPort.X, j * 64 - viewPort.Y, 64, 64), Color.White*0.3f);
                        }
                    }
                }
            }


            for (int i = 0; i < listOfUnits.Count; i++)
            {
                for (int j = 0; j < listOfUnits[i].Count; j++)
                {
                    listOfUnits[i][j].Draw(gameTime);
                }
            }

            //Draw the cursor selection if it's on the gamemap boundary
            //Note: should be skipped if the mouse is currently over a dialog: Basically, 
            //calculate all this in Update(), save it and then draw it here. To be done later.
            Point mousePosition = new Point(Mouse.GetState().X, Mouse.GetState().Y);
            Point gridPosition = GetGridPosition(mousePosition);
            if (gridPosition.X>-1 && gridPosition.Y>-1)
            {
                Game.GetSpriteBatch.Draw(selectionCursor, new Rectangle((gridPosition.X) * 64 - viewPort.X, (gridPosition.Y) * 64 - viewPort.Y, 64, 64), Color.White);
            }

        }

        public Point GetGridPosition(Point mousePosition)
        {
            //Assumes a vanilla mouseclick, no viewport offsets
            //returns -1,-1 if out of bounds!
            Point tempPos = new Point((mousePosition.X + viewPort.X)/64, (mousePosition.Y + viewPort.Y)/64);
            if (tempPos.X<0||tempPos.X>= tileSet.Tiles.GetLength(0) || tempPos.Y<0 || tempPos.Y>= tileSet.Tiles.GetLength(1))
                return new Point(-1,-1);
            return tempPos;
        }

        public void MouseClickHandler(Point mousePosition)
        {
            //What happens on a mouseclick. Should handle stuff like dialogs first over
            //map clicking. Currently only unit clicking for testing stuff.
            Point gridPos = GetGridPosition(mousePosition);

            if ( (gridPos.X>-1 && gridPos.Y>-1)&& unitPositions[gridPos.X, gridPos.Y] != null)
            {
                unitPositions[gridPos.X, gridPos.Y].MouseClick();


            }


        }

        public Point GridDimension
        {
            get
            {
                return new Point(tileSet.Tiles.GetLength(0), tileSet.Tiles.GetLength(1));
            }
        }
    }
}
