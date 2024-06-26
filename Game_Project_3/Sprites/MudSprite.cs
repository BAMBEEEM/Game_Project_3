using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game_Project_3.Collisions;
using Game_Project_3.ParticleManagement;

namespace Game_Project_3.Sprites
{

    /// <summary>
    /// A class representing a singular mud sprite
    /// </summary>
    public class MudSprite
    {

        private int _animationVarient;

        private Texture2D _texture;

        public Vector2 Position;

        private BoundingRectangle _bounds;

        public BoundingRectangle Bounds => _bounds;

        public float Closeness;

        public float Section = 2;

        public Color Color = Color.Gray;

        private Rectangle _source;

        /// <summary>
        /// Sets the mud source depending on its variation and draws its bound
        /// </summary>
        public void SetSource()
        {
            switch (_animationVarient)
            {
                case 0:
                    _source = new Rectangle(0, 0, 223, 160);
                    _bounds = new BoundingRectangle(new Vector2(Position.X - 10, Position.Y - 0f), _source.Width, _source.Height);
                    break;
                case 1:
                    _source = new Rectangle(0, 173, 184, 152);
                    _bounds = new BoundingRectangle(new Vector2(Position.X - 14, Position.Y - 0f), _source.Width, _source.Height);
                    break;
                case 2:
                    _source = new Rectangle(0, 340, 226, 122);
                    _bounds = new BoundingRectangle(new Vector2(Position.X - 13, Position.Y - 0f), _source.Width, _source.Height);
                    break;
                case 3:
                    _source = new Rectangle(0, 472, 68, 136);
                    _bounds = new BoundingRectangle(new Vector2(Position.X - 9, Position.Y - 0f), _source.Width, _source.Height);
                    break;
                case 4:
                    _source = new Rectangle(0, 627, 239, 82);
                    _bounds = new BoundingRectangle(new Vector2(Position.X - 9, Position.Y - 0f), _source.Width - 7, _source.Height * 0.7f);
                    break;
                case 5:
                    _source = new Rectangle(0, 736, 240, 68);
                    _bounds = new BoundingRectangle(new Vector2(Position.X - 9, Position.Y + 15), 228, 51);
                    break;
                case 6:
                    _source = new Rectangle(0, 816, 280, 108);
                    _bounds = new BoundingRectangle(new Vector2(Position.X - 9, Position.Y), _source.Width, _source.Height);
                    break;
                case 7:
                    _source = new Rectangle(0, 954, 187, 83);
                    _bounds = new BoundingRectangle(new Vector2(Position.X, Position.Y + 13), 180, 70);
                    break;
                case 8:
                    _source = new Rectangle(0, 1050, 314, 102);
                    _bounds = new BoundingRectangle(new Vector2(Position.X - 9, Position.Y), _source.Width, _source.Height);
                    break;
                case 9:
                    _source = new Rectangle(0, 1167, 336, 152);
                    _bounds = new BoundingRectangle(new Vector2(Position.X - 9, Position.Y), _source.Width, _source.Height - 15);
                    break;

            }
        }

        /// <summary>
        /// Helper method to respawn if two muds fall at the same place
        /// </summary>
        public void Respawn()
        {
            Position.X = RandomHelper.NextFloat(0, 900) + (Section * 900);
            Position.Y = RandomHelper.NextFloat(180, 660);
            _animationVarient = RandomHelper.Next(0, 10);
            SetSource();
        }

        public void LoadContent(ContentManager content)
        {
            _texture = content.Load<Texture2D>("mud");
            _animationVarient = RandomHelper.Next(0, 10);

            Position.X = RandomHelper.NextFloat(0, 900) + (Section * 900);

            if (_source.Height < 110)
                Position.Y = RandomHelper.NextFloat(185, 539);
            else Position.Y = RandomHelper.NextFloat(185, 493);
            SetSource();

        }


        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Position, _source, Color, 0, new Vector2(8, 0), (float)1, SpriteEffects.None, 0.2f);
        }
    }
}
