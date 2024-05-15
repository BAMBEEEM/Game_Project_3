using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game_Project_3.Collisions;

namespace Game_Project_3.Sprites
{

    public class MushroomSprite
    {
        public bool Collected { get; set; } = false;

        private string _variableAnimationSpeed;

        private float _animationSpeed = 0.22f;

        private double _animationTimer;

        private int _antiCloseness = 3;

        private int _animationVarient;


        private Texture2D _texture;

        private Random rng = new Random();

        public Vector2 Position = new Vector2(-999, -999);

        private BoundingRectangle _bounds;

        public BoundingRectangle Bounds => _bounds;

        public float Closeness;

        private Color _color = new Color(20, 20, 20);

        private bool _poisonous = false;

        public bool Poisonous => _poisonous;

        public void Respawn()
        {
            Position.X = rng.Next(13, 790) * 1.6f;
            Position.Y = rng.Next(190, 424) * 1.6f;
        }

        public void LoadContent(ContentManager content)
        {
            _texture = content.Load<Texture2D>("mushroom1wcolor");
            _animationVarient = rng.Next(0, 3);
            if (_animationVarient == 2)
                _poisonous = true;
            //_animationColor = rng.Next(0, 5);
            Position.X = rng.Next(13, 790) * 1.6f;
            Position.Y = rng.Next(190, 424) * 1.6f;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (Collected) return; // it breaks the draw method and the mushroom is "removed"
            _animationTimer += gameTime.ElapsedGameTime.TotalSeconds;

            /*if (_animationTimer > _animationSpeed)
            {*/
            /*
                            if (rng.Next(1,2) == 2) // for randomness
                                animationFrame++;
            */



            _bounds = new BoundingRectangle(new Vector2(Position.X - 15 * 1.6f + 1.39f, Position.Y - 1.39f), 15 * 1.485f * 1.6f, 15 * 1.485f * 1.6f);

/*            int a = rng.Next(-4, 4);
            if (a < 0) //if negative, add "-" sign to the text, and flip out variable "a".
                _variableAnimationSpeed = $"-0.0{-a}";
            else
                _variableAnimationSpeed = $"0.0{a}";

            _animationSpeed = 0.22f + (float)Convert.ToDouble(_variableAnimationSpeed);




            //if (animationVar)

            _animationTimer -= _animationSpeed;
            //}*/

            var source = new Rectangle(0, _animationVarient * 15, 15, 15);



            if (Closeness < 105 * 1.6f)
            {
                _antiCloseness = ((int)(105 * 1.6f) - (int)Closeness);
                _color = new Color(_antiCloseness * 2, _antiCloseness * 2, _antiCloseness * 2);
            }
            else
            {
                _color = new Color(20, 20, 20);
            }

            spriteBatch.Draw(_texture, Position, source, _color, 0, new Vector2(8, 0), (float)1.485 * 1.6f, SpriteEffects.None, 1);
        }
    }
}
