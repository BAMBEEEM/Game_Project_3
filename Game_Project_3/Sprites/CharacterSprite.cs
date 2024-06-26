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
using Game_Project_3.Misc;
using SharpDX.DirectWrite;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks.Dataflow;
using Game_Project_3.ParticleManagement;

namespace Game_Project_3.Sprites
{

    /// <summary>
    /// A class representing a slime ghost
    /// </summary>
    public class CharacterSprite
    {
        private GamePadState gamePadState;

        private KeyboardState keyboardState;

        private Texture2D _charTexture;

        public Vector2 CurrentPosition = new Vector2(600, 200) * 1.6f;

        private bool flipped;

        private BoundingRectangle bounds = new BoundingRectangle(new Vector2(600 - 54, 200 - 56) * 1.6f, 54 * 1.6f, 15.3f * 1.6f);

        private int _animationFrame;

        private double _flippingTimer;

        private double _animationTimer;

        private double _soundTimer;

        private StepSound _stepSound = StepSound.Left;

        private SoundEffect[] _steps = new SoundEffect[5];

        private SoundEffect _dashSound;
        private float _flippingSpeed = 0.35f;

        public bool Stopped = false;

        private float _animationSpeed = 0.1f;

        private bool _standing = true;

        public bool Poisoned = false;

        public bool Slowed = false;

        public bool WinManeuver = false;

        public bool LossManeuver = false;

/*        private Vector2 _lossDirection = RandomHelper.NextDirection();


        private Vector2 _lossAcceleration = new Vector2(RandomHelper.NextFloat(-0.3f, 0.3f), RandomHelper.NextFloat(-0.21f, 0.2f));

        private Vector2 _lossVelocity = new Vector2(0, RandomHelper.NextFloat(1,1.2f));

        private float _lossManeuverTime = 0;

        public Vector2 LossPosition;*/

        public float Stamina = 100;

        // they will be instantiated in Game Screen class using ScreenManager's Graphics Viewport
        public float MinOffsetX { get; set; }
        public float MaxOffsetX { get; set; }
        public float MinOffsetY { get; set; }
        public float MaxOffsetY { get; set; }



        public float CharBonusSpeed = 1f;

        private float _charSpeedLimiter;

        double ClickTimer;
        const double TimerDelay = 500;

        private bool _isCharging;
        private float _timeSinceDash;

        public bool Dead = false;

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
            _charTexture = content.Load<Texture2D>("ninja");


            _steps[0] = content.Load<SoundEffect>("step1");
            _steps[1] = content.Load<SoundEffect>("step2");
            _steps[2] = content.Load<SoundEffect>("step3");
            _steps[3] = content.Load<SoundEffect>("step1mud");
            _steps[4] = content.Load<SoundEffect>("step2mud");

            _dashSound = content.Load<SoundEffect>("dashsound");
        }

        float _lastSpacePressTime = 0;
        KeyboardState _previousKeyboardState;
        GamePadState _previousGamePadState;

        float dashtime = 0;
        private float _staminaChargeTimer = 0;
        public void Update(GameTime gameTime)
        {
           float initSpeed = CurrentPosition.X; //debug
            if (keyboardState.IsKeyDown(Keys.K))
                Dead = true;

            if (WinManeuver == true)
            {
                CurrentPosition += new Vector2(2, 0) * 1.6f * CharBonusSpeed;
                if (CurrentPosition.Y < 385)
                    CurrentPosition += new Vector2(0, 2) * 1.6f * CharBonusSpeed;
            else if (CurrentPosition.Y > 389) CurrentPosition += new Vector2(0, -2) * 1.6f * CharBonusSpeed;
            }

            if (LossManeuver == true)
            {
/*                if (_lossManeuverTime < 600)
                {
                    _lossVelocity += _lossAcceleration;
                    CurrentPosition += _lossVelocity;
                    _lossManeuverTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                }*/
                /*                CurrentPosition += new Vector2(2, 0) * 1.6f * CharBonusSpeed;
                                if (CurrentPosition.Y < LossPosition.Y+5)
                                    CurrentPosition += new Vector2(0, 2) * 1.6f * CharBonusSpeed;
                                else if (CurrentPosition.Y > LossPosition.Y+5) CurrentPosition += new Vector2(0, -2) * 1.6f * CharBonusSpeed;

                                if (CurrentPosition.X < LossPosition.X)
                                    CurrentPosition += new Vector2(2, 0) * 1.6f * CharBonusSpeed;
                                else if (CurrentPosition.X > LossPosition.X) CurrentPosition += new Vector2(-2, 0) * 1.6f * CharBonusSpeed;*/
            }


            _timeSinceDash += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (_timeSinceDash > DifficultySettings.StaminaDelay)
            {
                _staminaChargeTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                if (Stamina < 100)
                {
                    if (_staminaChargeTimer > DifficultySettings.StaminaRate && !Stopped)
                    {
                        Stamina += 2.5f;
                        _staminaChargeTimer = 0;
                    }
                }
            }

             

            _previousKeyboardState = keyboardState;
            _previousGamePadState = gamePadState;
             keyboardState = Keyboard.GetState();
            gamePadState = GamePad.GetState(0);
            float currentTime = (float)gameTime.TotalGameTime.TotalMilliseconds;


            if (dashtime > 0)
                dashtime -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            Rectangle source = new Rectangle(0, 0, 224, 160);

            // Check for double press of space within 500ms
            if ( (keyboardState.IsKeyDown(Keys.Space) && _previousKeyboardState.IsKeyUp(Keys.Space)) 
                || (gamePadState.IsButtonDown(Buttons.A) && _previousGamePadState.IsButtonUp(Buttons.A)))
            {
                float timeSinceLastPress = currentTime - _lastSpacePressTime;
                if (timeSinceLastPress < 500 && Stamina >= DifficultySettings.StaminaUsePerDash && !Stopped)
                {
                    _timeSinceDash = 0;
                    // Perform dash action
                    Stamina -= DifficultySettings.StaminaUsePerDash;
                    dashtime = 250;
                    // Reset the timer
                    _lastSpacePressTime = 0;
                    _dashSound.Play();
                }
                else
                {
                    _lastSpacePressTime = currentTime;
                }
            }
            //dashtime = 250; //for debug

            float footstepSoundTimer;
            if (!Slowed)
            {
                CharBonusSpeed = DifficultySettings.CharSpeedWithoutSlow + (6.1f * (dashtime / 250));
                footstepSoundTimer = 300;
            }
            else
            {
                CharBonusSpeed = DifficultySettings.CharSpeedWithSlow + (4.5f * (dashtime / 250));
                footstepSoundTimer = 500;
            }





            #region GamePad Input

            if (!Stopped)
            {
                CurrentPosition += gamePadState.ThumbSticks.Left  * new Vector2(2, -2) * 1.6f * CharBonusSpeed;

                if (gamePadState.ThumbSticks.Left.Y != 0) //up or down
                {
                    _flippingTimer += gameTime.ElapsedGameTime.TotalSeconds;

                    if (_flippingTimer > _flippingSpeed)
                    {
                        if (flipped)
                            flipped = false;
                        else
                            flipped = true;

                        _flippingTimer -= _flippingSpeed;
                    }

                        CurrentPosition.X += (Math.Sign(gamePadState.ThumbSticks.Left.X) - gamePadState.ThumbSticks.Left.X) * 2 * 1.6f * CharBonusSpeed;
                    

                }
            }
            else
            {
                flipped = false;
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
                        && gamePadState.ThumbSticks.Left == new Vector2(0, 0) 
                        && !(WinManeuver || LossManeuver))
                {    
                    
                    _standing = true;
                }
                else _standing = false;
            }
            else
            {
                flipped = false;
            }
            #endregion

            //to limit the sprite from getting out of map
            #region Position Offset


            /*            if (CurrentPosition.X < 20 * 1.6f)
                            CurrentPosition.X += 20 * 1.6f - CurrentPosition.X;

                        if (CurrentPosition.X > (MaxOffsetX + 300))
                            CurrentPosition.X -= CurrentPosition.X - (MaxOffsetX + 300);*/

            if (CurrentPosition.Y < 170)
                CurrentPosition.Y += 170 - CurrentPosition.Y;

            if (CurrentPosition.Y > (MaxOffsetY - 110))
                CurrentPosition.Y -= CurrentPosition.Y - (MaxOffsetY - 110);

            #endregion

            //Update the bounds
            bounds.X = CurrentPosition.X - 43.2f; //1.6f
            bounds.Y = CurrentPosition.Y + 31f;    //1.6f
            #region Step Sound
            _soundTimer += gameTime.ElapsedGameTime.TotalMilliseconds;
            _charSpeedLimiter = (CharBonusSpeed - 1) / 4.5f;
            if (_soundTimer * (1 + _charSpeedLimiter) > footstepSoundTimer)
            {
                if (!_standing && !Stopped)
                {
                    _steps[(int)_stepSound].Play();
                    if (Slowed) _steps[(int)_stepSound+2].Play();
                    if (_stepSound == StepSound.Left)
                        _stepSound = StepSound.LightRight;
                    else
                        _stepSound = StepSound.Left;
                    
                }
                _soundTimer = 0;
            }
            #endregion
            float finalspeed = CurrentPosition.X - initSpeed;
        }



        /// <summary>
        /// Draws the sprite using the supplied SpriteBatch
        /// </summary>
        /// <param name="gameTime">The game time</param>
        /// <param name="spriteBatch">The spritebatch to render with</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            _charSpeedLimiter = (CharBonusSpeed - 1) / 16;

            if (Slowed) Color = Color.SandyBrown;



            _animationTimer += gameTime.ElapsedGameTime.TotalSeconds * (1 + _charSpeedLimiter);
            if (!Dead)
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
                Color = Color.SkyBlue;

                if (_animationTimer > _animationSpeed)
                {
                    if (_animationFrame == 15)
                       ; //do nothing
                    
                    else if (_animationFrame < 6)
                        _animationFrame = 7;

                    else 
                    {
                        _animationFrame++;
                    }




                    _animationTimer -= _animationSpeed;
                }
            }



            var source = new Rectangle(_animationFrame * 46, 0, 46, 40);

            SpriteEffects spriteEffects = (flipped) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            spriteBatch.Draw(_charTexture, CurrentPosition, source, Color, 0, new Vector2(18, 20), 1.5f * 1.6f, spriteEffects, 0.2f);
        }
    }
}
