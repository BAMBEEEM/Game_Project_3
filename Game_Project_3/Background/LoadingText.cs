using Game_Project_3.Collisions;
using Game_Project_3.StateManagement;
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



    public class LoadingText
    {

        private double _animationTimer;

        private SpriteFont _font;
        public void LoadContent(ContentManager content)
        {
            _font = content.Load<SpriteFont>("retro");
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {

            _animationTimer += gameTime.ElapsedGameTime.TotalSeconds;

            if (_animationTimer >= 0f)
            {
                spriteBatch.DrawString(_font, "Loading.", new Vector2(513, 310), Color.White);
            }
            if (_animationTimer >= 0.44f)
            {
                spriteBatch.DrawString(_font, "Loading..", new Vector2(513, 310), Color.White);
            }
            if (_animationTimer >= 0.88f)
            {
                spriteBatch.DrawString(_font, "Loading...", new Vector2(513, 310), Color.White);
            }
            if (_animationTimer >= 1.22f) _animationTimer = 0;





        }

    }
}
