using Game_Project_3.Collisions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Game_Project_3.Background
{
    /// <summary>
    /// This class represents the stars in the sky, inncluding animation.
    /// </summary>
    /// <remarks>
    /// The stars animation changes by color, variant(different-looking stars),
    /// fade-in/fade-out speed, placement.
    /// </remarks>
    public class StarSprite
    {
        private string _variableAnimationSpeed;

        private float _animationSpeed = 0.22f;

        private double _animationTimer;

        private int _animationFrame;

        private int _animationVarient;

        private Texture2D _texture;

        private Random rng = new Random();

        private Vector2 _position = new Vector2(-999, -999);

        private BoundingRectangle bounds;

        public BoundingRectangle Bounds => bounds;

        private int _animationColor;

        private Color[] _colors = {Color.White, new Color(237, 213, 141),
                                new Color(122, 245, 153), new Color(107, 173, 245),
                                new Color(87, 204, 245)};


        public void LoadContent(ContentManager content)
        {
            _texture = content.Load<Texture2D>("stars");
        }

        public void Update(GameTime gameTime)
        {

        }

        /// <summary>
        /// Forces it to reset by reappearing at a different place
        /// </summary>
        public void Reset()
        {
            _animationFrame = 11;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            _animationTimer += gameTime.ElapsedGameTime.TotalSeconds;

            if (_animationTimer > _animationSpeed)
            {
                _animationFrame++;
                /*
                                if (rng.Next(1,2) == 2) // for randomness
                                    animationFrame++;
                */
                if (_animationFrame > 10)
                {
                    _animationFrame = 0;
                    _animationVarient = rng.Next(0, 8);
                    _animationColor = rng.Next(0, 5);
                    _position.X = rng.Next(10, 780);
                    _position.Y = rng.Next(10, 470);

                    bounds = new BoundingRectangle(new Vector2(_position.X - 13.365f, _position.Y) * 1.52f, 9 * 1.485f * 1.52f, 9 * 1.485f * 1.52f);

                    int a = rng.Next(-4, 4);
                    if (a < 0) //if negative, add "-" sign to the text, and flip out variable "a".
                        _variableAnimationSpeed = $"-0.0{-a}";
                    else
                        _variableAnimationSpeed = $"0.0{a}";

                    _animationSpeed = 0.22f + (float)Convert.ToDouble(_variableAnimationSpeed);
                }



                //if (animationVar)

                _animationTimer -= _animationSpeed;
            }

            var source = new Rectangle(_animationFrame * 9, _animationVarient * 9, 9, 9);

            spriteBatch.Draw(_texture, _position, source, _colors[_animationColor], 0, new Vector2(8, 0), (float)1.485 * 1.52f, SpriteEffects.None, 1);

        }



    }
}
