using Game_Project_3.Collisions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game_Project_3.Misc;

namespace Game_Project_3.Sprites
{
    /// <summary>
    /// A class representing the main tidal wave.
    /// </summary>
    public class MainTidalWave
    {
        private Texture2D texture;

        public Vector2 Position = new(-610, 60);

        private BoundingRectangle[] _bounds = 
            {
            new BoundingRectangle(new Vector2(0, 140), 550-70, 16),
            new BoundingRectangle(new Vector2(0, 140), 554-70, 61),
            new BoundingRectangle(new Vector2(0, 140), 554-70, 69.5f),
            new BoundingRectangle(new Vector2(0, 140), 599-70, 94f),
            new BoundingRectangle(new Vector2(0, 140), 599-70, 47f),
            new BoundingRectangle(new Vector2(0, 140), 619-70, 90),
            new BoundingRectangle(new Vector2(0, 140), 654-70, 50),
            new BoundingRectangle(new Vector2(0, 140), 684-70, 66)
    }; 

        public BoundingRectangle [] Bounds  => _bounds; 

        public Color Color = Color.White;


        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("wave");
        }

        public void Update(GameTime gameTime)
        {
            Position += new Vector2(DifficultySettings.WaveSpeed, 0);



            if (Position.X > 12500)
                Position.X -= Position.X - 12500;

            _bounds[0].X = Position.X - 218+315+70; 
            _bounds[0].Y = Position.Y + 142;    

            _bounds[1].X = Position.X - 245 + 315 + 70;  
            _bounds[1].Y = Position.Y + 158;     

            _bounds[2].X = Position.X - 263 + 315 + 70;  
            _bounds[2].Y = Position.Y + 218;     

            _bounds[3].X = Position.X - 321 + 315 + 70;  
            _bounds[3].Y = Position.Y + 286;     

            _bounds[4].X = Position.X - 321 + 315 + 70;  
            _bounds[4].Y = Position.Y + 377;     

            _bounds[5].X = Position.X - 326 + 315 + 70;  
            _bounds[5].Y = Position.Y + 421;     

            _bounds[6].X = Position.X - 334 + 315 + 70;  
            _bounds[6].Y = Position.Y + 509;     

            _bounds[7].X = Position.X - 330 + 315 + 70;  
            _bounds[7].Y = Position.Y + 555;     
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Position, null, Color, 0, new Vector2(0, 0), (float)2.75, SpriteEffects.None, 0.1f);
        }
    }
}
