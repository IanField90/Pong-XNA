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
        
        // Game state
        // true = playing ; false = menu
        bool gameState = false;
        int Winner = 0;

        // Graphics
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        
        Rectangle viewportRect;
        SpriteFont Font1;

        // Objects
        int PlayerScore = 0;
        int CompScore = 0;
        int ScoreBar = 30;

        Texture2D lineTexture;

        GameObject Ball, LeftBat, RightBat;
        Vector2 CompScorePos;
        Vector2 PlayerScorePos;
        Vector2 InitBallPos;
        // TODO: Change init ball veloc to be initialised fairly raindomly
        Vector2 InitBallVeloc = new Vector2(-5, -1);

        const int MAX_SCORE = 5;
        const int EDGE = 5;

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
            // Bats Dim 13 x 89
            // Ball Dim 10 x 10
            Font1 = Content.Load<SpriteFont>("Fonts\\ScoreFont");
            Ball = new GameObject(Content.Load<Texture2D>("Sprites\\Ball1"), 10, 10);
            LeftBat = new GameObject(Content.Load<Texture2D>("Sprites\\Bat1"), 13, 89);
            RightBat = new GameObject(Content.Load<Texture2D>("Sprites\\Bat2"), 13, 89);
            lineTexture = Content.Load<Texture2D>("Sprites\\line");

            //drawable area of the game screen.
            viewportRect = new Rectangle(0, 0,
                graphics.GraphicsDevice.Viewport.Width,
                graphics.GraphicsDevice.Viewport.Height);
            InitBallPos = new Vector2(viewportRect.Width / 2 - Ball.getWidth() / 2, viewportRect.Height / 2 - Ball.getHeight() / 2 + ScoreBar / 2);

            // SET initial positions of objects
            Ball.position = InitBallPos;
            LeftBat.position = new Vector2(viewportRect.Left + EDGE, viewportRect.Top + ScoreBar);
            RightBat.position = new Vector2(viewportRect.Right - RightBat.getWidth() - EDGE, viewportRect.Bottom - RightBat.getHeight() - 300); // -300 is a test for right bat collision
            
            // SET initial positions of scores
            CompScorePos = new Vector2(viewportRect.Width/2 + 200, viewportRect.Top + 14);
            PlayerScorePos = new Vector2(viewportRect.Width/2 - 200, viewportRect.Top + 14);
            Ball.velocity = InitBallVeloc; // initialise ball movement for testing
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

            if (gameState)
            {
                gameUpdateKeyboard();
                ballHandler();
                // Check for winners
                if (PlayerScore == MAX_SCORE)
                {
                    Winner = 1;
                    gameState = false;
                }
                if (CompScore == MAX_SCORE)
                {
                    Winner = 2;
                    gameState = false;
                }
            }            
            if (!gameState)
            {
                menuUpdateKeyboard();
            }
            
            

            // TODO: Add your update logic here
            // MOVE BALL
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

            if (gameState)
            {
                drawField();
                drawScores();
                drawObjects();
            }
            if (!gameState)
            {
                drawMenu();
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }

        /// <summary>
        /// Ball movement and Collision detection handled here
        /// </summary>
        private void ballHandler()
        {
            // Ensure within horizontal field dimensions
            // Ensure within vertical field dimensions
            if ((Ball.position.X > 0 && Ball.position.X < viewportRect.Right - Ball.getWidth()) && (Ball.position.Y > ScoreBar && Ball.position.Y < viewportRect.Bottom - Ball.getHeight()))
            {
                // Left bat collision check
                // If adjacent to bat or partly past bat
                if ((Ball.position.X <= EDGE + LeftBat.getWidth() && Ball.position.X != 0) 
                    && (Ball.position.Y > LeftBat.position.Y - Ball.getHeight() 
                        && Ball.position.Y < LeftBat.position.Y + LeftBat.getHeight() + Ball.getHeight()))
                {
                    // Reverse horizontal
                    Ball.velocity.X *= -1;
                }

                // Right bat collision check
                // If adjacent or partly past bat
                if (Ball.position.X >= viewportRect.Right - EDGE - RightBat.getWidth()
                    && (Ball.position.Y > RightBat.position.Y - Ball.getHeight()
                        && Ball.position.Y < RightBat.position.Y + RightBat.getHeight() + Ball.getHeight()))
                {
                    // Reverse horizontal
                    Ball.velocity.X *= -1;
                }
            }
            
            // If ball above or at scorebar
            if (Ball.position.Y <= ScoreBar)
            {
                Ball.position.Y = ScoreBar;
                Ball.velocity.Y *= -1;
            }

            // If ball bellow or at bottom
            if (Ball.position.Y >= viewportRect.Bottom - Ball.getHeight())
            {
                Ball.position.Y = viewportRect.Bottom - Ball.getHeight();
                Ball.velocity.Y *= -1;
            }

            // If ball goes past player bat
            if (Ball.position.X == 0)
            {
                CompScore++;
                Ball.position = InitBallPos;
            }
            
            // If ball goes past computer bat
            if (Ball.position.X == viewportRect.Right - Ball.getWidth())
            {
                PlayerScore++;
                Ball.position = InitBallPos;
            }

            // ensure it draws within lines
            Ball.position += Ball.velocity;
        }

        /// <summary>
        /// Scores drawn from the center of their string to ensure they do not move
        /// throghout gameplay
        /// </summary>
        private void drawScores()
        {
            // Computer Score
            Vector2 FontOrigin = Font1.MeasureString(CompScore.ToString())/2;
            spriteBatch.DrawString(Font1, CompScore.ToString(),
                CompScorePos, Color.Yellow, 0, FontOrigin, 1.0f, SpriteEffects.None, 0.5f);
            // Player Score
            FontOrigin = Font1.MeasureString(PlayerScore.ToString())/2;
            spriteBatch.DrawString(Font1, PlayerScore.ToString(),
                PlayerScorePos, Color.Yellow, 0, FontOrigin, 1.0f, SpriteEffects.None, 0.5f);
        }

        private void drawObjects()
        {
            spriteBatch.Draw(Ball.sprite, Ball.position, Color.White);
            spriteBatch.Draw(LeftBat.sprite, LeftBat.position, Color.White);
            spriteBatch.Draw(RightBat.sprite, RightBat.position, Color.White);
        }

        private void drawField()
        {
            for (int i = ScoreBar; i < graphics.GraphicsDevice.Viewport.Height; i++)
            {
                spriteBatch.Draw(lineTexture, new Vector2(graphics.GraphicsDevice.Viewport.Width/2 - 2, i), Color.White);
            }
            for (int j = 0; j < graphics.GraphicsDevice.Viewport.Width; j += 4)
            {
                spriteBatch.Draw(lineTexture, new Vector2(j, ScoreBar - 1), Color.White);
            }
        }

        private void drawMenu()
        {
            spriteBatch.DrawString(Font1, "Press Enter To Begin...", new Vector2(100,100), Color.White);
            if(Winner == 1)
                spriteBatch.DrawString(Font1, "You Win!", new Vector2(100, 150), Color.LimeGreen);
            if(Winner == 2)
                spriteBatch.DrawString(Font1, "Computer Wins!", new Vector2(100, 150), Color.Red);      
        }

        private void gameUpdateKeyboard()
        {
            KeyboardState keybState = Keyboard.GetState();

            //Move down for arrow down
            if (keybState.IsKeyDown(Keys.Down))
            {
                LeftBat.position.Y += 5;
            }

            //Move up for arrow up
            if (keybState.IsKeyDown(Keys.Up))
            {
                LeftBat.position.Y -= 5;
            }

            //Stop bat from falling off the side
            if (LeftBat.position.Y < ScoreBar)
            {
                LeftBat.position.Y = ScoreBar;
            }
            if (LeftBat.position.Y > (graphics.GraphicsDevice.Viewport.Height - LeftBat.getHeight()))
            {
                LeftBat.position.Y = graphics.GraphicsDevice.Viewport.Height - LeftBat.getHeight();
            }
        }

        private void menuUpdateKeyboard()
        {
            KeyboardState keybState = Keyboard.GetState();

            // press enter to start a game
            if (keybState.IsKeyDown(Keys.Enter))
            {
                // Reset winner and scores
                Winner = 0;
                PlayerScore = 0;
                CompScore = 0;
                gameState = true;
            }
        }
    }
}

// TODO:    Add bat 'hit' speed boost effect
//          Computer AI - Move once ball past halfway