using Game_Project_3.Collisions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Project_3.GameButtons
{
    /// <summary>
    /// A class that represents the menu buttons.
    /// </summary>
    public class QuitButton
    {
        private BoundingRectangle bounds = new BoundingRectangle(new Vector2(305 + 16 - 19.0625f, 240) * 1.52f, 92 * 2.3f * 1.52f, 31 * 2.3f * 1.52f);
        private Texture2D _texture;
        private Vector2 _position = new Vector2(305 + 16, 240) * 1.52f;

        public bool IsSelected = false;
        /// <summary>
        /// The bounding rectangle of the QuitButton
        /// </summary>
        public BoundingRectangle Bounds => bounds;
        public Color Shade = Color.White;
        public bool InitialClick = false;


        public void LoadContent(ContentManager content)
        {
            _texture = content.Load<Texture2D>("woodgui");
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (IsSelected == true)
                Shade = Color.Thistle;



            spriteBatch.Draw(_texture, _position, new Rectangle(130, 545, 92, 31), Shade, 0, new Vector2(8, 0), 2.3f * 1.52f, SpriteEffects.None, 1);
        }
    }
}
