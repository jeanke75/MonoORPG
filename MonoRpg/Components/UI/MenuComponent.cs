using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoRpg.Components.UI
{
    public class MenuComponent
    {
        #region Fields
        Game1 GameRef;
        SpriteFont spriteFont;

        readonly List<string> menuItems = new List<string>();
        int selectedIndex = -1;
        bool mouseOver;

        int width;
        int height;
        int menuSpacing;

        Color normalBackgroundColor = Color.White;
        Color normalForegroundColor = Color.White;
        Color hiliteBackgroundColor = Color.White;
        Color hiliteForegroundColor = Color.Red;

        Texture2D texture;

        Vector2 position;

        #endregion Fields

        #region Properties

        public Vector2 Postion
        {
            get { return position; }
            set { position = value; }
        }

        public int Width
        {
            get { return width; }
        }

        public int Height
        {
            get { return height; }
        }

        public int SelectedIndex
        {
            get { return selectedIndex; }
            set
            {
                selectedIndex = (int)MathHelper.Clamp(
                        value,
                        0,
                        menuItems.Count - 1);
            }
        }

        public Color NormalBackgroundColor
        {
            get { return normalBackgroundColor; }
            set { normalBackgroundColor = value; }
        }

        public Color NormalForegroundColor
        {
            get { return normalForegroundColor; }
            set { normalForegroundColor = value; }
        }

        public Color HiliteBackgroundColor
        {
            get { return hiliteBackgroundColor; }
            set { hiliteBackgroundColor = value; }
        }

        public Color HiliteForegroundColor
        {
            get { return hiliteForegroundColor; }
            set { hiliteForegroundColor = value; }
        }

        public bool MouseOver
        {
            get { return mouseOver; }
        }

        #endregion Properties

        #region Constructors

        public MenuComponent(Game game, SpriteFont spriteFont, Texture2D texture)
        {
            this.GameRef = (Game1)game;
            this.mouseOver = false;
            this.spriteFont = spriteFont;
            this.texture = texture;
            this.menuSpacing = 10;
        }

        public MenuComponent(Game game, SpriteFont spriteFont, Texture2D texture, string[] menuItems, int menuSpacing) : this(game, spriteFont, texture)
        {
            selectedIndex = 0;

            foreach (string s in menuItems)
            {
                this.menuItems.Add(s);
            }

            this.menuSpacing = menuSpacing;

            MeassureMenu();
        }

        #endregion Constructors

        #region Methods

        public void SetMenuItems(string[] items)
        {
            menuItems.Clear();
            menuItems.AddRange(items);
            MeassureMenu();

            selectedIndex = 0;
        }

        private void MeassureMenu()
        {
            width = texture.Width;
            height = 0;

            foreach (string s in menuItems)
            {
                Vector2 size = spriteFont.MeasureString(s);

                if (size.X > width)
                    width = (int)size.X;

                height += texture.Height + menuSpacing;
            }

            height -= menuSpacing;
        }

        public void Update(GameTime gameTime)
        {
            Vector2 menuPosition = position;
            Point p = Xin.MouseState.Position;

            Rectangle buttonRect;
            mouseOver = false;

            for (int i = 0; i < menuItems.Count; i++)
            {
                buttonRect = new Rectangle((int)(menuPosition.X * GameRef.ScaleMatrix.Scale.X), (int)(menuPosition.Y * GameRef.ScaleMatrix.Scale.Y), (int)(texture.Width * GameRef.ScaleMatrix.Scale.X), (int)(texture.Height * GameRef.ScaleMatrix.Scale.Y));

                if (buttonRect.Contains(p))
                {
                    selectedIndex = i;
                    mouseOver = true;
                }

                menuPosition.Y += texture.Height + menuSpacing;
            }

            if (!mouseOver && (Xin.CheckKeyReleased(Keys.Up)))
            {
                selectedIndex--;
                if (selectedIndex < 0)
                    selectedIndex = menuItems.Count - 1;
            }
            else if (!mouseOver && (Xin.CheckKeyReleased(Keys.Down)))
            {
                selectedIndex++;
                if (selectedIndex > menuItems.Count - 1)
                    selectedIndex = 0;
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Vector2 menuPosition = position;
            Color myBackgroundColor;
            Color myForegroundColor;

            for (int i = 0; i < menuItems.Count; i++)
            {
                if (i == SelectedIndex)
                {
                    myBackgroundColor = HiliteBackgroundColor;
                    myForegroundColor = HiliteForegroundColor;
                }
                else
                {
                    myBackgroundColor = NormalBackgroundColor;
                    myForegroundColor = NormalForegroundColor;
                }

                spriteBatch.Draw(texture, menuPosition, myBackgroundColor);

                Vector2 textSize = spriteFont.MeasureString(menuItems[i]);

                Vector2 textPosition = menuPosition + new Vector2((int)(texture.Width - textSize.X) / 2, (int)(texture.Height - textSize.Y) / 2);
                spriteBatch.DrawString(spriteFont, menuItems[i], textPosition, myForegroundColor);

                menuPosition.Y += texture.Height + menuSpacing;
            }
        }

        #endregion Methods

        #region Virtual Methods
        #endregion Virtual Methods
    }
}
