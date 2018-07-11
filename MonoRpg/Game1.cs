using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoRpg.Components;
using MonoRpg.GameState;
using MonoRpg.GameState.States;
using Client;
using System;
using MonoRpg.Components.Utility;

namespace MonoRpg
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public static Rectangle screenRectangle;
        public const int TargetWidth = 1280; //1920
        public const int TargetHeight = 720; //1080
        Matrix scaleMatrix;

        GameStateManager gameStateManager;
        ITitleIntroState titleIntroState;
        IMainMenuState mainMenuState;
        IGamePlayState gamePlayState;

        GameClient gameSocketClient;

        public GraphicsDeviceManager Graphics
        {
            get { return graphics; }
        }
        public SpriteBatch SpriteBatch
        {
            get { return spriteBatch; }
        }
        public Matrix ScaleMatrix
        {
            get { return scaleMatrix; }
        }

        public ITitleIntroState TitleIntroState
        {
            get { return titleIntroState; }
        }
        public IMainMenuState MainMenuState
        {
            get { return mainMenuState; }
        }
        public IGamePlayState GamePlayState
        {
            get { return gamePlayState; }
        }

        public GameClient GameSocketClient
        {
            get { return gameSocketClient; }
            set { gameSocketClient = value;  }
        }

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.SynchronizeWithVerticalRetrace = false; //vsync
            graphics.PreferredBackBufferFormat = SurfaceFormat.Alpha8;
            IsFixedTimeStep = false;
            if (IsFixedTimeStep)
                TargetElapsedTime = TimeSpan.FromMilliseconds(1000.0f / 60);

            SetWindowResolution();
            IsMouseVisible = true;

            gameStateManager = new GameStateManager(this);
            Components.Add(gameStateManager);

            titleIntroState = new TitleIntroState(this);
            mainMenuState = new MainMenuState(this);
            gamePlayState = new GamePlayState(this);
            gameStateManager.ChangeState((TitleIntroState)titleIntroState, PlayerIndex.One);
        }

        public void SetWindowResolution(int windowWidth = TargetWidth, int windowHeight = TargetHeight)
        {
            graphics.PreferredBackBufferWidth = windowWidth;
            graphics.PreferredBackBufferHeight = windowHeight;
            graphics.ApplyChanges();

            float scaleWidth = graphics.GraphicsDevice.Viewport.Width / (float)TargetWidth;
            float scaleHeight = graphics.GraphicsDevice.Viewport.Height / (float)TargetHeight;

            scaleMatrix = Matrix.CreateScale(scaleWidth, scaleHeight, 1);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            Components.Add(new Xin(this));
            Components.Add(new FrameRateCounter(this));

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

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            GameSocketClient?.Stop();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            base.Draw(gameTime);
        }

        internal void GameSocketClient_SocketClosed(object sender, EventArgs e)
        {
            // back to server select if connection to server is lost, to reconnect or select a different server
            gameStateManager.ChangeState((MainMenuState)mainMenuState, PlayerIndex.One);
        }
    }
}
