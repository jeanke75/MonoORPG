using ClassLibrary.Packets.Server;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoRpg.GameState;
using MonoRpg.GameState.States;

namespace MonoRpg.Components.UI
{
    public class LoginComponent
    {
        #region Fields

        Game1 GameRef;
        SpriteFont spriteFont;

        int width;
        int height;
        Vector2 position;

        InputBox usernameInput;
        InputBox passwordInput;
        string errorMessage;

        Texture2D texture;

        #endregion Fields

        #region Properties

        public int Width
        {
            get { return width; }
        }
        public int Height
        {
            get { return height; }
        }
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        #endregion Properties

        #region Constructors

        public LoginComponent(Game game, SpriteFont spriteFont, Texture2D texture)
        {
            GameRef = (Game1)game;
            this.spriteFont = spriteFont;
            this.texture = texture;
            width = texture.Width;
            height = texture.Height;

            Texture2D inputTexture = game.Content.Load<Texture2D>(@"UI\InputBoxes\Input_Black");
            usernameInput = new InputBox(game, spriteFont, inputTexture);
            usernameInput.MaxLength = 20;
            usernameInput.IsFocussed = true;

            passwordInput = new InputBox(game, spriteFont, inputTexture);
            passwordInput.MaxLength = 20;

            errorMessage = "";
        }

        #endregion Constructors

        #region Methods

        public void Update(GameTime gameTime)
        {
            usernameInput.Update(gameTime);
            passwordInput.Update(gameTime);

            if (Xin.CheckKeyReleased(Keys.Tab))
            {
                if (!usernameInput.IsFocussed && !passwordInput.IsFocussed)
                {
                    usernameInput.IsFocussed = true;
                }
                else
                {
                    bool tempUserFocussed = usernameInput.IsFocussed;
                    usernameInput.IsFocussed = passwordInput.IsFocussed;
                    passwordInput.IsFocussed = tempUserFocussed;
                }
            }
            else if (Xin.CheckMouseReleased(MouseButtons.Left) || Xin.CheckKeyReleased(Keys.Enter))
            {
                usernameInput.IsFocussed = false;
                passwordInput.IsFocussed = false;

                if (usernameInput.Text.Length == 0 || passwordInput.Text.Length == 0)
                    errorMessage = "Please fill in your credentials.";
                else
                {
                    errorMessage = "";
                    GameRef.GameSocketClient.LoginMessageReceived += GameSocketClient_Login;
                    GameRef.GameSocketClient.SendMessage(new ClassLibrary.Packets.Client.cLogin() { Username = usernameInput.Text, Password = passwordInput.Text });
                }
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);

            int offsetLeft = 25;
            int offsetTop = 50;

            // labels
            Vector2 usernamePosition = position + new Vector2(offsetLeft, offsetTop);
            spriteBatch.DrawString(spriteFont, "Username :", position + new Vector2(offsetLeft, offsetTop), Color.White);

            Vector2 passwordPosition = usernamePosition + new Vector2(0, 40);
            spriteBatch.DrawString(spriteFont, "Password :", passwordPosition, Color.White);

            // inputs
            usernameInput.Position = usernamePosition + new Vector2(100, -8);
            usernameInput.Draw(gameTime, spriteBatch);

            passwordInput.Position = passwordPosition + new Vector2(100, -8);
            passwordInput.Draw(gameTime, spriteBatch);

            // errorMessage
            Vector2 errorPosition = passwordPosition + new Vector2(0, 80);
            spriteBatch.DrawString(spriteFont, errorMessage, errorPosition, Color.Red);
        }

        private void GameSocketClient_Login(svLogin login)
        {
            if (login.Success)
            {
                GameRef.GamePlayState.SetUpGame(login);
                ((IStateManager)GameRef.Services.GetService(typeof(IStateManager))).PushState((GamePlayState)GameRef.GamePlayState, 0);
            }
            else
            {
                errorMessage = login.Success.ToString();
            }
        }

        #endregion Methods

        #region Virtual Methods
        #endregion Virtual Methods
    }
}
