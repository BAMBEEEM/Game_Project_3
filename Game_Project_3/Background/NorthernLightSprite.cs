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
    /// This class represents the northern light, inncluding animation.
    /// </summary>
    /// <remarks>
    /// The northern light animation works by opacity and sprite flipping tricks.
    /// </remarks>
    public class NorthernLightSprite
    {
        private Texture2D texture;

        private float opacity = 0f;

        private bool opacityIncreasing = true;

        private SpriteEffects flipped = SpriteEffects.None;

        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("northernlight");
        }




        public void Draw(SpriteBatch spriteBatch)
        {
            if ((opacity < 1f && opacityIncreasing) || opacity < 0f)
            {
                opacity += 0.008f;
                opacityIncreasing = true;
            }
            else
            {
                opacity -= 0.008f;
                opacityIncreasing = false;
            }

            if (opacity <= 0f)
                if (flipped == SpriteEffects.None)
                    flipped = SpriteEffects.FlipHorizontally;
                else
                    flipped = SpriteEffects.None;

            spriteBatch.Draw(texture, new Vector2(), null, Color.White * opacity, 0, new Vector2(8, 0), 1.485f * 1.52f, flipped, 1);
        }
    }
}
