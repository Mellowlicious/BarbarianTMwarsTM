using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using BarbarianTMwarsTM.Maps;
using BarbarianTMwarsTM;

namespace BarbarianTMwarsTM
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class BW : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Map GameMap;

        //put this in another class later
        Texture2D arrow;

        public BW()
        {
            graphics = new GraphicsDeviceManager(this);
            this.graphics.PreferredBackBufferHeight = 1080;
            this.graphics.PreferredBackBufferWidth = 1920;
            this.graphics.IsFullScreen = false;
            
            ContentManager.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            GameMap = new Map(this);
            this.Components.Add(GameMap);
            base.Initialize();
           
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            arrow = this.ContentManager.Load<Texture2D>("Placeholders/UI/arrow");
        
            base.LoadContent();
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            // TODO: Add your drawing code here


          //  GameMap.Draw(gameTime);

            base.Draw(gameTime);
            int x = Mouse.GetState().X;
            int y = Mouse.GetState().Y;

            spriteBatch.Draw(arrow, new Rectangle(x, y, arrow.Width, arrow.Height), Color.White);
            spriteBatch.End();
        }


        public Point Resolution
        {
            get
            {
                return new Point(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            }
        }

        public SpriteBatch GetSpriteBatch
        {
            get
            {
                return spriteBatch;
            }
        }
        public ContentManager ContentManager
        {
            get
            {
                return this.Content;
            }
        }
    }
}
