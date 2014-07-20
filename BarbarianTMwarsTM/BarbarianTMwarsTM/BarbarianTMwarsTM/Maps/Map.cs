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
using BarbarianTMwarsTM.Maps.BattleInputHandlers;

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
        public int activePlayer = 0;

        public Texture2D unitPlaceholder;

        Texture2D selectionCursor;
        public Texture2D menuTop;
        public Texture2D menuFight;
        public Texture2D menuWait;
        public Texture2D menuClose;


        public List<Texture2D> menuDrawOrder;
        public bool drawMenu;
        public Point menuStartPos;


        public List<List<Unit>> listOfUnits;

        public Unit[,] unitPositions;
        public bool[,] movementSquares;
        public bool showMovementSquares = false;

        public IInputHandler inputHandler;
        public Point highlightedTile;
        public bool isTileHighlighted=false;
        public bool drawHighlightedTile = false;

        public Unit selectedUnit;
        public Unit highlightedUnit;
        public bool isUnitHighlighted = false;

        public List<Point> movementArrows;
        public bool drawMovementArrows = false;

        Texture2D movementSelection;

        
        public Map(BW game) : base(game)
        {
            Game = game;

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
            tileSet = new TileSet(this);
            inputHandler = new StandardInputHandler(this);

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


            menuTop = Game.Content.Load<Texture2D>("Placeholders/UI/MenuItems/workingMenuTop");
            menuFight = Game.Content.Load<Texture2D>("Placeholders/UI/MenuItems/workingFight");
            menuWait = Game.Content.Load<Texture2D>("Placeholders/UI/MenuItems/workingWait");
            menuClose = Game.Content.Load<Texture2D>("Placeholders/UI/MenuItems/workingClose");

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

            if (showMovementSquares)
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


            if (drawMovementArrows)
            {
                for (int i = 0; i < movementArrows.Count; i++)
                {
                    Game.GetSpriteBatch.Draw(selectionCursor, new Rectangle((movementArrows[i].X) * 64 - viewPort.X, (movementArrows[i].Y) * 64 - viewPort.Y, 64, 64), Color.White);
                }
            }
   
            if (isTileHighlighted&&drawHighlightedTile)
                Game.GetSpriteBatch.Draw(selectionCursor, new Rectangle((highlightedTile.X) * 64 - viewPort.X, (highlightedTile.Y) * 64 - viewPort.Y, 64, 64), Color.White);

            if (drawMenu)
            {
                int offset = 0;
                Game.GetSpriteBatch.Draw(menuTop, new Rectangle(menuStartPos.X, menuStartPos.Y, 128, 15), Color.White);
                offset += 15;
                for (int i = 0; i < menuDrawOrder.Count; i++)
                {
                    Game.GetSpriteBatch.Draw(menuDrawOrder[i], new Rectangle(menuStartPos.X, menuStartPos.Y + offset, 128, 40), Color.White);
                    offset += 40;
                }
                Game.GetSpriteBatch.Draw(menuTop, new Rectangle(menuStartPos.X, menuStartPos.Y + offset, 128, 15), null, Color.White, 0f, new Vector2(0, 0), SpriteEffects.FlipVertically, 0);

            }
        }

        public Point GetGridPosition(Point mousePosition)
        {
            //Assumes a vanilla mouseclick, no viewport offsets
            //returns -1,-1 if out of bounds!
            Point tempPos = new Point((int)Math.Floor(((float)mousePosition.X + (float)viewPort.X)/64f) , (int)Math.Floor(((float)mousePosition.Y + (float)viewPort.Y)/64f));
            if (tempPos.X<0||tempPos.X>= tileSet.Tiles.GetLength(0) || tempPos.Y<0 || tempPos.Y>= tileSet.Tiles.GetLength(1))
                return new Point(-1,-1);
            return tempPos;
        }

        public void UpdateTileHighlight(Point mousePosition)
        {
            Point gridPosition = GetGridPosition(mousePosition);
            highlightedTile = gridPosition;
            if (gridPosition.X >= 0 && gridPosition.Y >= 0)
            {
                isTileHighlighted = true;
            }
            else
                isTileHighlighted = false;

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
