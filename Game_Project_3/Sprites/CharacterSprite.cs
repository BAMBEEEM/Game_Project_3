using Game_Project_3.Collisions;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game_Project_3.Enums;
using Microsoft.Xna.Framework.Audio;

namespace Game_Project_3.Sprites
{

    /// <summary>
    /// A class representing a slime ghost
    /// </summary>
    public class CharacterSprite
    {
        private GamePadState gamePadState;

        private KeyboardState keyboardState;

        private Texture2D texture;

        public Vector2 CurrentPosition = new Vector2(600, 200) * 1.6f;

        private Vector2 lastPosition;


        private bool flipped;

        private BoundingRectangle bounds = new BoundingRectangle(new Vector2(600 - 54, 200 - 56) * 1.6f, 54 * 1.6f, 56 * 1.6f);

        private int _animationFrame;

        private double _flippingTimer;

        private double _animationTimer;

        private double _soundTimer;

        private StepSound _stepSound = StepSound.Left;

        private SoundEffect[] _steps = new SoundEffect[3];

        private float _flippingSpeed = 0.35f;

        public bool Stopped = false;

        private float _animationSpeed = 0.1f;

        private bool _standing = true;

        public bool Poisoned = false;

        // they will be instantiated in Game Screen class using ScreenManager's Graphics Viewport
        public float MinOffsetX { get; set; }
        public float MaxOffsetX { get; set; }
        public float MinOffsetY { get; set; }
        public float MaxOffsetY { get; set; }



        public float CharBonusSpeed = 1f;

        private float _charSpeedLimiter;

        /// <summary>
        /// The bounding volume of the sprite
        /// </summary>
        public BoundingRectangle Bounds => bounds;

        /// <summary>
        /// The color to blend with the ghost
        /// </summary>
        public Color Color { get; set; } = Color.White;

        /// <summary>
        /// Loads the sprite texture using the provided ContentManager
        /// </summary>
        /// <param name="content">The ContentManager to load with</param>
        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("ninja");


            _steps[0] = content.Load<SoundEffect>("step1");
            _steps[1] = content.Load<SoundEffect>("step2");
            _steps[2] = content.Load<SoundEffect>("step3");
        }

        /// <summary>
        /// Updates the sprite's position based on user input
        /// </summary>
        /// <param name="gameTime">The GameTime</param>
        public void Update(GameTime gameTime)
        {
            gamePadState = GamePad.GetState(0);
            keyboardState = Keyboard.GetState();


            #region GamePad Input

            if (!Stopped)
            {
                if (gamePadState.ThumbSticks.Left.Y > 0) //up
                {
                    CurrentPosition += gamePadState.ThumbSticks.Left * new Vector2(2, -2) * 1.6f * CharBonusSpeed;
                    _flippingTimer += gameTime.ElapsedGameTime.TotalSeconds;

                    if (_flippingTimer > _flippingSpeed)
                    {
                        if (flipped)
                            flipped = false;
                        else
                            flipped = true;

                        _flippingTimer -= _flippingSpeed;
                    }
                }

                if (gamePadState.ThumbSticks.Left.Y < 0) //down
                {
                    CurrentPosition += gamePadState.ThumbSticks.Left * new Vector2(2, -2) * 1.6f * CharBonusSpeed;
                    _flippingTimer += gameTime.ElapsedGameTime.TotalSeconds;

                    if (_flippingTimer > _flippingSpeed)
                    {
                        if (flipped)
                            flipped = false;
                        else
                            flipped = true;

                        _flippingTimer -= _flippingSpeed;
                    }
                }


                if (gamePadState.ThumbSticks.Left.X < 0) //left
                {
                    CurrentPosition += gamePadState.ThumbSticks.Left * new Vector2(2, -2) * 1.6f * CharBonusSpeed;
                    if (gamePadState.ThumbSticks.Left.Y > 0 || gamePadState.ThumbSticks.Left.Y < 0)
                    {
                        // 0.75 bevause it's impossible to joystick vector to be (1,1), max it could be is (0.75,0.75)
                        CurrentPosition -= gamePadState.ThumbSticks.Left * 0.75f * new Vector2(2, -2) * 1.6f * CharBonusSpeed;

                        if (_flippingTimer > _flippingSpeed)
                        {
                            if (flipped)
                                flipped = false;
                            else
                                flipped = true;

                            _flippingTimer -= _flippingSpeed;
                        }
                    }
                    else
                        flipped = true;
                }

                if (gamePadState.ThumbSticks.Left.X > 0) //right
                {
                    CurrentPosition += gamePadState.ThumbSticks.Left * new Vector2(2, -2) * 1.6f * CharBonusSpeed;
                    if (gamePadState.ThumbSticks.Left.Y > 0 || gamePadState.ThumbSticks.Left.Y < 0)
                    {
                        // 0.75 bevause it's impossible to joystick vector to be (1,1), max it could be is (0.75,0.75)
                        CurrentPosition -= gamePadState.ThumbSticks.Left * 0.75f * new Vector2(2, -2) * 1.6f * CharBonusSpeed;

                        if (_flippingTimer > _flippingSpeed)
                        {
                            if (flipped)
                                flipped = false;
                            else
                                flipped = true;

                            _flippingTimer -= _flippingSpeed;
                        }
                    }
                    else
                        flipped = false;
                }
            }
            #endregion

            #region Keyboard Input

            if (!Stopped)
            {
                // Apply keyboard movement
                if (keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.W))
                {
                    //if (position.Y > 170)
                    CurrentPosition += new Vector2(0, -2) * 1.6f * CharBonusSpeed;

                    _flippingTimer += gameTime.ElapsedGameTime.TotalSeconds;

                    if (_flippingTimer > _flippingSpeed)
                    {
                        if (flipped)
                            flipped = false;
                        else
                            flipped = true;

                        _flippingTimer -= _flippingSpeed;
                    }
                }
                if (keyboardState.IsKeyDown(Keys.Down) || keyboardState.IsKeyDown(Keys.S))
                {
                    //if (position.Y < 453)
                    CurrentPosition += new Vector2(0, 2) * 1.6f * CharBonusSpeed;

                    _flippingTimer += gameTime.ElapsedGameTime.TotalSeconds;

                    if (_flippingTimer > _flippingSpeed)
                    {
                        if (flipped)
                            flipped = false;
                        else
                            flipped = true;

                        _flippingTimer -= _flippingSpeed;
                    }

                }
                if (keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.A))
                {
                    //if (position.X > 20)

                    CurrentPosition += new Vector2(-2, 0) * 1.6f * CharBonusSpeed;
                    if (keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.W)
                        || keyboardState.IsKeyDown(Keys.Down) || keyboardState.IsKeyDown(Keys.S))
                    {

                        if (_flippingTimer > _flippingSpeed)
                        {
                            if (flipped)
                                flipped = false;
                            else
                                flipped = true;

                            _flippingTimer -= _flippingSpeed;
                        }
                    }
                    else
                        flipped = true;
                }
                if (keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.D))
                {
                    //if (position.X < 780)
                    CurrentPosition += new Vector2(2, 0) * 1.6f * CharBonusSpeed;
                    if (keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.W)
                        || keyboardState.IsKeyDown(Keys.Down) || keyboardState.IsKeyDown(Keys.S))
                    {

                        if (_flippingTimer > _flippingSpeed)
                        {
                            if (flipped)
                                flipped = false;
                            else
                                flipped = true;

                            _flippingTimer -= _flippingSpeed;
                        }
                    }
                    else
                        flipped = false;
                }


                if (!(keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.W)
                        || keyboardState.IsKeyDown(Keys.Down) || keyboardState.IsKeyDown(Keys.S)
                        || keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.D)
                        || keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.A))
                        && gamePadState.ThumbSticks.Left == new Vector2(0, 0))
                {
                    _standing = true;
                }
                else _standing = false;
            }
            #endregion

            //to limit the sprite from getting out of map
            #region Position Offset


            if (CurrentPosition.X < 20 * 1.6f)
                CurrentPosition.X += 20 * 1.6f - CurrentPosition.X;

            if (CurrentPosition.X > (MaxOffsetX + 300))
                CurrentPosition.X -= CurrentPosition.X - (MaxOffsetX + 300);

            if (CurrentPosition.Y < 170 * 1.6f)
                CurrentPosition.Y += 170 * 1.6f - CurrentPosition.Y;

            if (CurrentPosition.Y > (MaxOffsetY + 10))
                CurrentPosition.Y -= CurrentPosition.Y - (MaxOffsetY +10);

            #endregion

            //Update the bounds
            bounds.X = CurrentPosition.X - 43.2f ; //1.6f
            bounds.Y = CurrentPosition.Y - 48f;    //1.6f

            #region Step Sound
            _soundTimer += gameTime.ElapsedGameTime.TotalMilliseconds;
            _charSpeedLimiter = (CharBonusSpeed - 1) / 4.5f;
            if (_soundTimer * (1 + _charSpeedLimiter) > 300)
            {
                if (!_standing && !Stopped)
                {
                    _steps[(int)_stepSound].Play();
                    if (_stepSound == StepSound.Left)
                        _stepSound = StepSound.LightRight;
                    else
                        _stepSound = StepSound.Left;
                    
                }
                _soundTimer = 0;
            }
            #endregion
        }



        /// <summary>
        /// Draws the sprite using the supplied SpriteBatch
        /// </summary>
        /// <param name="gameTime">The game time</param>
        /// <param name="spriteBatch">The spritebatch to render with</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (Poisoned) Color = Color.Green;
            _charSpeedLimiter = (CharBonusSpeed - 1) / 16;

            _animationTimer += gameTime.ElapsedGameTime.TotalSeconds * (1 + _charSpeedLimiter);
            if (!Stopped)
            {
                if (_animationTimer > _animationSpeed)
                {
                    _animationFrame++;
                    if (_standing == true)
                        _animationFrame = 6;

                    else if (_animationFrame > 5)
                    {
                        _animationFrame = 0;
                    }



                    _animationTimer -= _animationSpeed;
                }
            }
            else // stop at animation frame #3 when dead (looks the best) or stay standing if wasn't moving
            {
                if (_standing == true)
                    _animationFrame = 6;
                else
                    _animationFrame = 3;
            }



            var source = new Rectangle(_animationFrame * 36, 0, 36, 40);

            SpriteEffects spriteEffects = (flipped) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            spriteBatch.Draw(texture, CurrentPosition, source, Color, 0, new Vector2(18, 20), 1.5f * 1.6f, spriteEffects, 0);
        }
    }
}
