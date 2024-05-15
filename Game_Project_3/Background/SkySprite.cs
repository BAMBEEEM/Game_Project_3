using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Project_3.Background
{
    /// <summary>
    /// A class representing the static sky background.
    /// </summary>
    public class SkySprite
    {
        private Texture2D texture;

        private Vector2 position = new Vector2(0, 0);

        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("clearsky");
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, null, Color.White, 0, new Vector2(8, 0), (float)1.485 * 1.52f, SpriteEffects.None, 0);
        }
    }
}
