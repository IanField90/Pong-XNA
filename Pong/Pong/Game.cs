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
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace Pong
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game : Microsoft.Xna.Framework.Game
    {
        #region Variables

        // Graphics
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private Rectangle TitleSafe;
        Rectangle viewportRect;
        SpriteFont Font1;

        // Objects
        int PlayerScore = 0;
        int CompScore = 0;
        int ScoreBar = 30;

        GameObject Ball, LeftBat, RightBat;
        Vector2 CompScorePos;
        Vector2 PlayerScorePos;

        int VertVelocDir = 1; // 1 up -1 down
        int HoriVelocDir = 1; // 1 right -1 left

        const int MAX_SCORE = 5;

        #endregion
        public Game()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
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

            Font1 = Content.Load<SpriteFont>("Fonts\\ScoreFont");
            Ball = new GameObject(Content.Load<Texture2D>("Sprites\\Ball1"));
            LeftBat = new GameObject(Content.Load<Texture2D>("Sprites\\Bat1"));
            RightBat = new GameObject(Content.Load<Texture2D>("Sprites\\Bat2"));

            //drawable area of the game screen.
            viewportRect = new Rectangle(0, 0,
                graphics.GraphicsDevice.Viewport.Width,
                graphics.GraphicsDevice.Viewport.Height);
            // 5 px from bottom is max border ball must not go past this

            // SET initial positions of objects
            Ball.position = new Vector2(viewportRect.Width / 2 - 5, viewportRect.Height / 2 - 5 + ScoreBar);
            LeftBat.position = new Vector2(viewportRect.Left + 5, viewportRect.Top + 5 + ScoreBar);
            RightBat.position = new Vector2(viewportRect.Right - 18, viewportRect.Bottom - 94);
            // SET initial positions of scores
            CompScorePos = new Vector2(viewportRect.Width / 2 - 200, viewportRect.Top + 14);
            PlayerScorePos = new Vector2(viewportRect.Width / 2 + 200, viewportRect.Top + 14);
            // TODO: Set Ball initial direction. Rand?
            base.LoadContent();

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
            #region Exit Action
            // Allows the game to exit on escape press
#if XBOX
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
#else
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();
#endif
            #endregion

            // TODO: Add your update logic here
            // MOVE BALL
            // Move RightBat regarding keyboard input
            // AI for LeftBat only after ball passed halfway

            base.Update(gameTime);
        }
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();

            drawScores();
            drawObjects();

            spriteBatch.End();
            base.Draw(gameTime);
        }

        private void drawScores()
        {
            // Computer Score
            Vector2 FontOrigin = Font1.MeasureString(CompScore.ToString()) / 2;
            spriteBatch.DrawString(Font1, CompScore.ToString(),
                CompScorePos, Color.Yellow, 0, FontOrigin, 1.0f, SpriteEffects.None, 0.5f);
            // Player Score
            FontOrigin = Font1.MeasureString(PlayerScore.ToString()) / 2;
            spriteBatch.DrawString(Font1, PlayerScore.ToString(),
                PlayerScorePos, Color.Yellow, 0, FontOrigin, 1.0f, SpriteEffects.None, 0.5f);
        }

        private void drawObjects()
        {
            spriteBatch.Draw(Ball.sprite, Ball.position, Color.White);
            spriteBatch.Draw(LeftBat.sprite, LeftBat.position, Color.White);
            spriteBatch.Draw(RightBat.sprite, RightBat.position, Color.White);
        }
    }
}