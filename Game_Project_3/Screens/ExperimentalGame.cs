using System;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Game_Project_3.StateManagement;
using static System.TimeZoneInfo;
using Game_Project_3.Background;
using Game_Project_3.Sprites;
using System.Collections.Generic;
using System.Reflection.Metadata;
using Game_Project_3.Collisions;
using SharpDX.Direct2D1;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;
using System.DirectoryServices.ActiveDirectory;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrayNotify;
using Game_Project_3.ParticleManagement;
using Game_Project_3.Misc;
using System.Drawing.Text;

namespace Game_Project_3.Screens
{
    // This screen implements the actual game logic. It is just a
    // placeholder to get the idea across: you'll probably want to
    // put some more interesting gameplay in here!
    public class ExperimentalGame : GameScreen
    {
        private ContentManager _content;
        /*        private SpriteFont _gameFont;

                private Vector2 _playerPosition = new Vector2(100, 100);
                private Vector2 _enemyPosition = new Vector2(100, 100);

                private readonly Random _random = new Random();

                private float _pauseAlpha;
                private readonly InputAction _pauseAction;

                private GraphicsDeviceManager graphics;*/

        private bool _hasBegun = false;
        private bool _lost = false;

        private CharacterSprite _mainCharacter;
        private SpriteFont spriteFont;
        private ForestSprite _forest;
        private ForestFrontLayerSprite _forestFrontLayer;
        private MainTidalWave _wave;
        private MudSprite[] _mud = new MudSprite[13*DifficultySettings.MudPerSection];
        private StaminaBarSprite _staminaSprite;

        private bool _won = false;

        private TimeSpan _elapsedTime = new TimeSpan();

        private Color _timeColor;

        private float _shakeTime;

        private float _drownTime;

        private bool _isDrowning;

        private float _startTimer = 1850;

        TimeSpan introProgress;
        Song _ingameSong;


        OuterWaveParticlesOne _outerWaveEffectOne;
        OuterWaveParticlesTwo _outerWaveEffectTwo;

        InnerWaveParticlesOne _innerWaveEffectOne;
        InnerWaveParticlesTwo _innerWaveEffectTwo;

        public ExperimentalGame()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
            _hasBegun = true;
            /*
                        _pauseAction = new InputAction(
                            new[] { Buttons.Start, Buttons.Back },
                            new[] { Keys.Back }, true);*/
        }




        // Load graphics content for the game
        public override void Activate()
        {
            if (_content == null)
                _content = new ContentManager(ScreenManager.Game.Services, "Content");



            _forest = new ForestSprite();
            _forestFrontLayer = new ForestFrontLayerSprite();
            _wave = new MainTidalWave();
            _staminaSprite = new StaminaBarSprite();





            // TODO: Add your initialization logic here
            for (int i = 0; i < 13 * DifficultySettings.MudPerSection; i++)
            {
                _mud[i] = new MudSprite();
                _mud[i].Section = 1 + (i / DifficultySettings.MudPerSection);
            }

            foreach (MudSprite mud in _mud)
            {
                mud.LoadContent(_content);
            }

            _mainCharacter = new CharacterSprite();

            _mainCharacter.LoadContent(_content);

            _mainCharacter.MaxOffsetX = (ScreenManager.GraphicsDevice.Viewport.Width) ;

            _mainCharacter.MaxOffsetY = (ScreenManager.GraphicsDevice.Viewport.Height) ;



            spriteFont = _content.Load<SpriteFont>("retro");
            _forest.LoadContent(_content);
            _forestFrontLayer.LoadContent(_content);
            _staminaSprite.LoadContent(_content);
            _wave.LoadContent(_content);

            _outerWaveEffectOne = new OuterWaveParticlesOne(this.ScreenManager.Game, new Rectangle((int)_wave.Position.X - 7, (int)_wave.Position.Y - 137, 0, 0));
            _outerWaveEffectTwo = new OuterWaveParticlesTwo(this.ScreenManager.Game, new Rectangle((int)_wave.Position.X, (int)_wave.Position.Y - 130, 0, 0));

            _innerWaveEffectOne = new InnerWaveParticlesOne(this.ScreenManager.Game, new Rectangle((int)_wave.Position.X + 60, (int)_wave.Position.Y + 430, 240, 203));
            _innerWaveEffectTwo = new InnerWaveParticlesTwo(this.ScreenManager.Game, new Rectangle((int)_wave.Position.X + 60, (int)_wave.Position.Y + 430, 180, 233));

            ScreenManager.Game.Components.Add(_outerWaveEffectOne);
            ScreenManager.Game.Components.Add(_outerWaveEffectTwo);
            ScreenManager.Game.Components.Add(_innerWaveEffectOne);
            ScreenManager.Game.Components.Add(_innerWaveEffectTwo);




            /*            // _gameFont = _content.Load<SpriteFont>("gamefont");

                        // A real game would probably have more content than this sample, so
                        // it would take longer to load. We simulate that by delaying for a
                        // while, giving you a chance to admire the beautiful loading screen.
                        Thread.Sleep(1000);

                        // once the load has finished, we use ResetElapsedTime to tell the game's
                        // timing mechanism that we have just finished a very long frame, and that
                        // it should not try to catch up.*/
            ScreenManager.Game.ResetElapsedTime();

            _ingameSong = _content.Load<Song>("IngameSong");



            MediaPlayer.Play(_ingameSong);
        }

        /*        public override void Deactivate()
                {
                    base.Deactivate();
                }*/

        /*        public override void Unload()
                {
                    _content.Unload();
                }*/

        // This method checks the GameScreen.IsActive property, so the game will
        // stop updating when the pause menu is active, or if you tab away to a different application.
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            _startTimer -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (_startTimer > 18)
            {
                _mainCharacter.Stopped = true;
            }
            else if(_startTimer > -18) _mainCharacter.Stopped = false;


            if (_mainCharacter.CurrentPosition.X > 13200)
                _won = true;

            _mainCharacter.Color = Color.White; // default color
            _mainCharacter.Update(gameTime);
            _staminaSprite.Stamina = _mainCharacter.Stamina;
            _wave.Update(gameTime);
            //very good for testing: if (CollisionHelper.Collides(new BoundingRectangle(Mouse.GetState().Position.X, Mouse.GetState().Position.Y, 1, 1), _mud.Bounds))

            int loopCount = 0;
            foreach (BoundingRectangle b in _wave.Bounds)
            {
                
                if (_mainCharacter.Bounds.CollidesWith(b))
                {
                    _drownTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                    _isDrowning = true;
                    if (_drownTime >= DifficultySettings.DrownTime)
                    {
                        _lost = true;
/*                        _mainCharacter.LossPosition = new Vector2(_wave.Position.X + 200, _wave.Position.Y + 300);
*/                    }
                    break;
                }
                else
                {
                    if (loopCount == 7)
                    {
                        _drownTime = 0;
                        _isDrowning = false;
                    }
                } 

                loopCount++;

            }
            foreach (MudSprite mud in _mud)
                if (mud.Bounds.CollidesWith(_mainCharacter.Bounds))
                {
                    _mainCharacter.Slowed = true;
                    //_mud.Color = Color.Red;
                    break;
                }
                else _mainCharacter.Slowed = false;


            _outerWaveEffectOne.BigWavePosition = new Vector2(_wave.Position.X - 7, _wave.Position.Y - 137);
            _outerWaveEffectTwo.BigWavePosition = new Vector2(_wave.Position.X, _wave.Position.Y - 130);

            _innerWaveEffectOne.BigWavePosition = new Vector2(_wave.Position.X + 60, _wave.Position.Y + 680);
            _innerWaveEffectTwo.BigWavePosition = new Vector2(_wave.Position.X + 60, _wave.Position.Y + 680);


            introProgress += gameTime.ElapsedGameTime;
            TimeSpan songDurationLeft = TimeSpan.Zero;
            if (introProgress >= _ingameSong.Duration - TimeSpan.FromMilliseconds(85))
            {
                songDurationLeft.Add(TimeSpan.FromMilliseconds(introProgress.TotalMilliseconds - _ingameSong.Duration.TotalMilliseconds));
            }

            if (introProgress.TotalMilliseconds >= _ingameSong.Duration.TotalMilliseconds - 18)
            {
                introProgress = TimeSpan.Zero;
                MediaPlayer.Stop();
                MediaPlayer.Play(_ingameSong);
            }
            songDurationLeft = TimeSpan.Zero;


            // responsible for time limit
            while (!_hasBegun) gameTime.ElapsedGameTime = new TimeSpan();
            _elapsedTime += gameTime.ElapsedGameTime;
            if (_elapsedTime.TotalMilliseconds < 18)
            {

                for (int i = 0; i < 13 * DifficultySettings.MudPerSection; i += DifficultySettings.MudPerSection)
                {
                    // if normal or hard difficulty
                    if (DifficultySettings.SetDifficulty == Enums.Difficulty.Normal || DifficultySettings.SetDifficulty == Enums.Difficulty.Hard)
                    {
                        while (CollisionHelper.Collides(_mud[i].Bounds, _mud[i + 1].Bounds))
                        {
                            _mud[i + 1].Respawn();
                        }
                        while (CollisionHelper.Collides(_mud[i].Bounds, _mud[i + 2].Bounds))
                            _mud[i].Respawn();

                        while (CollisionHelper.Collides(_mud[i + 1].Bounds, _mud[i + 2].Bounds))
                            _mud[i + 2].Respawn();
                    }
                    else // if easy
                    {
                        while (CollisionHelper.Collides(_mud[i].Bounds, _mud[i + 1].Bounds))
                        {
                            _mud[i + 1].Respawn();
                        }
                    }
                }
                _lost = false;
            }
            // responsible for time limit
            if (_won)
            {
                _mainCharacter.WinManeuver = true;
                _mainCharacter.Stopped = true; // charecter stops when game ends.
            }

            else if (_lost)
            {
                _mainCharacter.LossManeuver = true;
                _mainCharacter.Stopped = true; // charecter stops when game ends.
                _mainCharacter.Dead = true; // charecter dies.

            }

 


            /*
                        // Gradually fade in or out depending on whether we are covered by the pause screen.
                        if (coveredByOtherScreen)
                            _pauseAlpha = Math.Min(_pauseAlpha + 1f / 32, 1);
                        else
                            _pauseAlpha = Math.Max(_pauseAlpha - 1f / 32, 0);

                        if (IsActive)
                        {
                            // Apply some random jitter to make the enemy move around.
                            const float randomization = 10;

                            _enemyPosition.X += (float)(_random.NextDouble() - 0.5) * randomization;
                            _enemyPosition.Y += (float)(_random.NextDouble() - 0.5) * randomization;

                            // Apply a stabilizing force to stop the enemy moving off the screen.
                            var targetPosition = new Vector2(
                                ScreenManager.GraphicsDevice.Viewport.Width / 2 - _gameFont.MeasureString("Insert Gameplay Here").X / 2,
                                200);

                            _enemyPosition = Vector2.Lerp(_enemyPosition, targetPosition, 0.05f);

                            // This game isn't very fun! You could probably improve
                            // it by inserting something more interesting in this space :-)
                        }*/

            base.Update(gameTime, otherScreenHasFocus, false);
        }

        // Unlike the Update method, this will only be called when the gameplay screen is active.
        public override void HandleInput(GameTime gameTime, InputState input)
        {
            /*            if (input == null)
                            throw new ArgumentNullException(nameof(input));

                        // Look up inputs for the active player profile.
                        int playerIndex = (int)ControllingPlayer.Value;

                        var keyboardState = input.CurrentKeyboardStates[playerIndex];
                        var gamePadState = input.CurrentGamePadStates[playerIndex];

                        // The game pauses either if the user presses the pause button, or if
                        // they unplug the active gamepad. This requires us to keep track of
                        // whether a gamepad was ever plugged in, because we don't want to pause
                        // on PC if they are playing with a keyboard and have no gamepad at all!
                        bool gamePadDisconnected = !gamePadState.IsConnected && input.GamePadWasConnected[playerIndex];

                        PlayerIndex player;
                        if (_pauseAction.Occurred(input, ControllingPlayer, out player) || gamePadDisconnected)
                        {
                           // ScreenManager.AddScreen(new PauseMenuScreen(), ControllingPlayer);
                        }
                        else
                        {
                            // Otherwise move the player position.
                            var movement = Vector2.Zero;

                            if (keyboardState.IsKeyDown(Keys.Left))
                                movement.X--;

                            if (keyboardState.IsKeyDown(Keys.Right))
                                movement.X++;

                            if (keyboardState.IsKeyDown(Keys.Up))
                                movement.Y--;

                            if (keyboardState.IsKeyDown(Keys.Down))
                                movement.Y++;

                            var thumbstick = gamePadState.ThumbSticks.Left;

                            movement.X += thumbstick.X;
                            movement.Y -= thumbstick.Y;

                            if (movement.Length() > 1)
                                movement.Normalize();

                            _playerPosition += movement * 8f;
                        }*/
        }

        public override void Draw(GameTime gameTime)
        {
            _shakeTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            var spriteBatch = ScreenManager.SpriteBatch;

            //GraphicsDevice.Clear(Color.Transparent);

            //Calculate our offset vector
            float playerX = MathHelper.Clamp(_mainCharacter.CurrentPosition.X, 630, 13300);
            float offsetX = 630 - playerX;


            /*            Matrix zoomTranslation = Matrix.CreateTranslation(-1280 / 2f, -720 / 2f, 0);
                        Matrix zoomTransform = zoomTranslation * Matrix.CreateScale(0.85f) * Matrix.Invert(zoomTranslation);
            */
            // Background
            spriteBatch.Begin();
            _staminaSprite.Draw(spriteBatch);
            spriteBatch.End();
            CameraSettings.transform = Matrix.CreateTranslation(offsetX, 0, 0) * CameraSettings.WaveShakeEffect(_shakeTime) * CameraSettings.DrownShakeEffect(_shakeTime, _isDrowning);


            spriteBatch.Begin(transformMatrix: Matrix.CreateTranslation(offsetX, 0, 0) * CameraSettings.DrownShakeEffect(_shakeTime, _isDrowning));
            _forest.Draw(spriteBatch);
            for (int i = 0; i < 13 * DifficultySettings.MudPerSection; i++)
                _mud[i].Draw(spriteBatch);
/*            if (!_mainCharacter.Dead)
*/                _mainCharacter.Draw(gameTime, spriteBatch);

            spriteBatch.End();


            spriteBatch.Begin(transformMatrix: Matrix.CreateTranslation(offsetX, 0, 0) * CameraSettings.WaveShakeEffect(_shakeTime) * CameraSettings.DrownShakeEffect(_shakeTime, _isDrowning));
            _wave.Draw(spriteBatch);
            _outerWaveEffectOne.Draw(spriteBatch);
            _outerWaveEffectTwo.Draw(spriteBatch);
            _innerWaveEffectOne.Draw(spriteBatch);
            _innerWaveEffectTwo.Draw(spriteBatch);

            spriteBatch.End();

            spriteBatch.Begin(transformMatrix: Matrix.CreateTranslation(offsetX, 0, 0) * CameraSettings.DrownShakeEffect(_shakeTime, _isDrowning));

            _forestFrontLayer.Draw(spriteBatch);

/*            if (_mainCharacter.Dead)
                _mainCharacter.Draw(gameTime, spriteBatch);*/


            spriteBatch.End();


            // TODO: Add your drawing code here
            spriteBatch.Begin();
            _staminaSprite.Draw(spriteBatch);

            if (_startTimer > 0)
            {
                spriteBatch.DrawString(spriteFont, "RUN! -->", new Vector2(355, 75) * 1.6f, Color.White);
            }

            if (_won)
            {
                spriteBatch.DrawString(spriteFont, "You win!", new Vector2(335, 195) * 1.6f, Color.White);
            }
            else if (_lost)
            {
                spriteBatch.DrawString(spriteFont, "You Drowned!", new Vector2(288, 195) * 1.6f, Color.White);
            }
            spriteBatch.End();





            base.Draw(gameTime);
            /*
                // This game has a blue background. Why? Because!
                ScreenManager.GraphicsDevice.Clear(ClearOptions.Target, Color.CornflowerBlue, 0, 0);

                // Our player and enemy are both actually just text strings.
                var spriteBatch = ScreenManager.SpriteBatch;

                spriteBatch.Begin();

    *//*            spriteBatch.DrawString(_gameFont, "// TODO", _playerPosition, Color.Green);
                spriteBatch.DrawString(_gameFont, "Insert Gameplay Here",
                                       _enemyPosition, Color.DarkRed);*//*

                spriteBatch.End();

                // If the game is transitioning on or off, fade it out to black.
                if (TransitionPosition > 0 || _pauseAlpha > 0)
                {
                    float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, _pauseAlpha / 2);

                    ScreenManager.FadeBackBufferToBlack(alpha);
                }*/
        }
    }
}
