using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoRpg.Components.UI
{
    public class InputBox
    {
        #region Field Region

        protected Game1 GameRef;
        
        SpriteFont spriteFont;
        Texture2D texture;

        int width;
        int height;
        Vector2 position;

        int maxLength;
        string text;

        bool isFocussed;

        TimeSpan elapsed;

        #endregion

        #region Property Region

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

        public int MaxLength
        {
            get { return maxLength; }
            set
            {
                if (value < 0)
                    maxLength = 0;
                else
                    maxLength = value;
            }
        }
        public string Text
        {
            get { return text; }
        }

        public bool IsFocussed
        {
            get { return isFocussed; }
            set { isFocussed = value; }
        }

        #endregion

        #region Constructor Region

        public InputBox(Game game, SpriteFont spriteFont, Texture2D texture)
        {
            GameRef = (Game1)game;

            this.spriteFont = spriteFont;
            this.texture = texture;

            width = texture.Width;
            height = texture.Height;

            maxLength = 0;
            text = "";

            isFocussed = false;
        }

        #endregion

        #region Method Region

        public void Update(GameTime gameTime)
        {
            if (isFocussed)
            {
                elapsed += gameTime.ElapsedGameTime;

                if (text.Length > 0 && Xin.CheckKeyReleased(Keys.Back))
                {
                    text = text.Substring(0, text.Length - 1);
                }
                else if (maxLength == 0 || text.Length < maxLength)
                {
                    bool upperCase = Xin.KeyboardState.IsKeyDown(Keys.LeftShift) ||
                                     Xin.KeyboardState.IsKeyDown(Keys.RightShift) ||
                                     Xin.KeyboardState.CapsLock;
                                   
                    
                    for (int keyValue = (int)Keys.A; keyValue <= (int)Keys.Z; keyValue++)
                    {
                        Keys key = (Keys)keyValue;
                        if (Xin.CheckKeyReleased(key))
                        {
                            string letter = key.ToString();
                            text += (upperCase ? letter : letter.ToLower());
                            return;
                        }
                    }
                    if ((upperCase && Xin.CheckKeyReleased(Keys.D0)) || (Xin.KeyboardState.NumLock && Xin.CheckKeyReleased(Keys.NumPad0)))
                    {
                        text += "0";
                    }
                    else if ((upperCase && Xin.CheckKeyReleased(Keys.D1)) || (Xin.KeyboardState.NumLock && Xin.CheckKeyReleased(Keys.NumPad1)))
                    {
                        text += "1";
                    }
                    else if ((upperCase && Xin.CheckKeyReleased(Keys.D2)) || (Xin.KeyboardState.NumLock && Xin.CheckKeyReleased(Keys.NumPad2)))
                    {
                        text += "2";
                    }
                    else if ((upperCase && Xin.CheckKeyReleased(Keys.D3)) || (Xin.KeyboardState.NumLock && Xin.CheckKeyReleased(Keys.NumPad3)))
                    {
                        text += "3";
                    }
                    else if ((upperCase && Xin.CheckKeyReleased(Keys.D4)) || (Xin.KeyboardState.NumLock && Xin.CheckKeyReleased(Keys.NumPad4)))
                    {
                        text += "4";
                    }
                    else if ((upperCase && Xin.CheckKeyReleased(Keys.D5)) || (Xin.KeyboardState.NumLock && Xin.CheckKeyReleased(Keys.NumPad5)))
                    {
                        text += "5";
                    }
                    else if ((upperCase && Xin.CheckKeyReleased(Keys.D6)) || (Xin.KeyboardState.NumLock && Xin.CheckKeyReleased(Keys.NumPad6)))
                    {
                        text += "6";
                    }
                    else if ((upperCase && Xin.CheckKeyReleased(Keys.D7)) || (Xin.KeyboardState.NumLock && Xin.CheckKeyReleased(Keys.NumPad7)))
                    {
                        text += "7";
                    }
                    else if ((upperCase && Xin.CheckKeyReleased(Keys.D8)) || (Xin.KeyboardState.NumLock && Xin.CheckKeyReleased(Keys.NumPad8)))
                    {
                        text += "8";
                    }
                    else if ((upperCase && Xin.CheckKeyReleased(Keys.D9)) || (Xin.KeyboardState.NumLock && Xin.CheckKeyReleased(Keys.NumPad9)))
                    {
                        text += "9";
                    }
                }
            }
            else
            {
                // when tabbing fast between inputboxes this will make sure the caret is visible on entry
                elapsed = gameTime.ElapsedGameTime;
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // background
            spriteBatch.Draw(texture, position, Color.White);

            // text
            Vector2 textPosition = new Vector2(position.X + 5, position.Y + (texture.Height - spriteFont.LineSpacing) / 2);
            spriteBatch.DrawString(spriteFont, text, textPosition, Color.White);

            // caret
            if (IsFocussed && Math.Sin(elapsed.TotalSeconds * 6) > 0)
                spriteBatch.DrawString(spriteFont, "|", textPosition + new Vector2(spriteFont.MeasureString(text).X, 0), Color.White);
            //Color caretColor = new Color(1f, 1f, 1f) * (float)Math.Abs(Math.Sin(elapsed.TotalSeconds * 6));
            //spriteBatch.DrawString(spriteFont, "|", textPosition + new Vector2(textSize.Length() + spriteFont.Spacing, 0), caretColor); 
        }

        #endregion
    }
}
