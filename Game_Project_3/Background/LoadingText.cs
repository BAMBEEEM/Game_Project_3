using Game_Project_3.Collisions;
using Game_Project_3.StateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Project_3.Background
{



    public class LoadingText
    {
        private Texture2D _spaceBarButton;
        private Texture2D _buttonA;

        private double _animationTimer;

        private SpriteFont _font;
        public void LoadContent(ContentManager content)
        {
            _font = content.Load<SpriteFont>("retro");
            _spaceBarButton = content.Load<Texture2D>("spacebar");
            _buttonA = content.Load<Texture2D>("buttonA");

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



            spriteBatch.DrawString(_font, "Hint: Press             (or     ) twice to dash!", new Vector2(98, 660), Color.White);
            spriteBatch.Draw(_spaceBarButton, new Vector2(391, 662), new Rectangle(0,0,202,46), Color.White, 0, new Vector2(0, 0), (float)1, SpriteEffects.None, 0.17f);
            spriteBatch.Draw(_buttonA, new Vector2(699, 650), new Rectangle(0, 0, 70, 70), Color.White, 0, new Vector2(0, 0), (float)1, SpriteEffects.None, 0.74f);



        }

    }
}
