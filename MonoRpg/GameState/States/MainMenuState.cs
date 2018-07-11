using System.Net;
using Client;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoRpg.Components;
using MonoRpg.Components.UI;

namespace MonoRpg.GameState.States
{
    public enum ConnectionState
    {
        SERVER_SELECT,
        LOGIN,
        REGISTER
    }

    public interface IMainMenuState : IGameState
    {
    }

    public class MainMenuState : BaseGameState, IMainMenuState
    {
        #region Field Region

        Texture2D background;
        SpriteFont spriteFont;
        MenuComponent serverSelectionMenuComponent;
        LoginComponent loginComponent;
        ConnectionState connectionState;
        #endregion

        #region Property Region
        #endregion

        #region Constructor Region

        public MainMenuState(Game game) : base(game)
        {
            game.Services.AddService(typeof(IMainMenuState), this);
        }

        #endregion

        #region Method Region

        public override void Initialize()
        {
            connectionState = ConnectionState.SERVER_SELECT;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteFont = Game.Content.Load<SpriteFont>(@"Fonts\InterfaceFont");
            background = Game.Content.Load<Texture2D>(@"GameScreens\MenuScreen");

            // server selection menu
            Texture2D serverSelectionButtonTexture = Game.Content.Load<Texture2D>(@"UI\Buttons\Button_Wood1");

            string[] serverSelectionMenuItems = { "AERICAN", "XENA", "EXIT" };

            serverSelectionMenuComponent = new MenuComponent(Game, spriteFont, serverSelectionButtonTexture, serverSelectionMenuItems, 5);
            serverSelectionMenuComponent.HiliteBackgroundColor = Color.Yellow;
            serverSelectionMenuComponent.HiliteForegroundColor = Color.Yellow;

            Vector2 serverSelectionMenuPosition = new Vector2();
            serverSelectionMenuPosition.Y = Game1.TargetHeight / 2;
            serverSelectionMenuPosition.X = (Game1.TargetWidth - serverSelectionMenuComponent.Width) / 2;

            serverSelectionMenuComponent.Postion = serverSelectionMenuPosition;

            // login dialog
            Texture2D loginRegisterBackgroundTexture = Game.Content.Load<Texture2D>(@"UI\Windows\Window_Wood1");

            loginComponent = new LoginComponent(Game, spriteFont, loginRegisterBackgroundTexture);

            Vector2 loginRegisterPosition = new Vector2();
            loginRegisterPosition.Y = Game1.TargetHeight / 2;
            loginRegisterPosition.X = (Game1.TargetWidth - loginComponent.Width) / 2;

            loginComponent.Position = loginRegisterPosition;

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            switch (connectionState)
            {
                case ConnectionState.SERVER_SELECT:
                    serverSelectionMenuComponent.Update(gameTime);

                    if (Xin.CheckKeyReleased(Keys.Space) || Xin.CheckKeyReleased(Keys.Enter) || (serverSelectionMenuComponent.MouseOver && Xin.CheckMouseReleased(MouseButtons.Left)))
                    {
                        if (serverSelectionMenuComponent.SelectedIndex == 0)
                        {
                            Xin.FlushInput();
                            ConnectToServer(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 2013));
                            //GameRef.GamePlayState.SetUpNewGame();
                            //GameRef.GamePlayState.StartGame();
                            //manager.PushState((GamePlayState)GameRef.GamePlayState, PlayerIndexInControl);
                        }
                        else if (serverSelectionMenuComponent.SelectedIndex == 1)
                        {
                            Xin.FlushInput();
                            GameRef.SetWindowResolution(400, 300);
                            //ConnectToServer(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 2014));



                            //GameRef.GamePlayState.LoadExistingGame();
                            //GameRef.GamePlayState.StartGame();
                            //manager.PushState((GamePlayState)GameRef.GamePlayState, PlayerIndexInControl);
                        }
                        else if (serverSelectionMenuComponent.SelectedIndex == 2)
                        {
                            Game.Exit();
                        }
                    }
                    break;
                case ConnectionState.LOGIN:
                    loginComponent.Update(gameTime);
                    break;
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            GameRef.SpriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, GameRef.ScaleMatrix);

            GameRef.SpriteBatch.Draw(background, new Vector2(0, 0), Color.White);

            GameRef.SpriteBatch.End();

            base.Draw(gameTime);

            GameRef.SpriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, GameRef.ScaleMatrix);

            switch (connectionState)
            {
                case ConnectionState.SERVER_SELECT:
                    serverSelectionMenuComponent.Draw(gameTime, GameRef.SpriteBatch);
                    break;
                case ConnectionState.LOGIN:
                    loginComponent.Draw(gameTime, GameRef.SpriteBatch);
                    break;
            }

            GameRef.SpriteBatch.End();
        }

        private void ConnectToServer(IPEndPoint endpoint)
        {
            GameRef.GameSocketClient = new GameClient(endpoint);
            GameRef.GameSocketClient.SocketOpen += GameSocketClient_SocketOpen;
            GameRef.GameSocketClient.SocketClosed += GameRef.GameSocketClient_SocketClosed;
            GameRef.GameSocketClient.Start();
        }        

        private void GameSocketClient_SocketOpen(object sender, System.EventArgs e)
        {
            connectionState = ConnectionState.LOGIN;
        }
        #endregion
    }
}
