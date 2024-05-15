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

namespace Game_Project_3.Screens
{
    // This screen implements the actual game logic. It is just a
    // placeholder to get the idea across: you'll probably want to
    // put some more interesting gameplay in here!
    public class GameplayScreen : GameScreen
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

        private int[] _mushroomsLeft = new int[_maxLevel];
        private int _finalScore;
        private List<MushroomSprite>[] _mushrooms = new List<MushroomSprite>[_maxLevel];

        private bool _calculated = false;

        private int level = 1;

        private bool _won = false;

        private TimeSpan time;

        private TimeSpan _extraTime;

        private TimeSpan _elapsedTime = new TimeSpan();

        private Color _timeColor;

        private const int _maxLevel = 7;


        enum MusicState { None, Intro, Loop };
        MusicState currentState;


        TimeSpan introProgress;
        Song _ingameSong;

        SoundEffect _ending70ms;
        SoundEffect _ending80ms;
        SoundEffect _pickupSound;
        public GameplayScreen()
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

            // TODO: Add your initialization logic here
            for (int i = 0; i < _maxLevel; i++)
            {
                _mushrooms[i] = new List<MushroomSprite>();
                _mushrooms[i].Capacity = 5 + i;

                for (int j = 0; j < _mushrooms[i].Capacity; j++)
                {
                    _mushrooms[i].Add(new MushroomSprite());
                }
            }

            _finalScore = 0;

            _mainCharacter = new CharacterSprite();

            _mainCharacter.LoadContent(_content);
            spriteFont = _content.Load<SpriteFont>("retro");
            _forest.LoadContent(_content);
            foreach (List<MushroomSprite> LMushroom in _mushrooms)
                foreach (MushroomSprite mushroom in LMushroom) mushroom.LoadContent(_content);
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
            _ending70ms = _content.Load<SoundEffect>("ending70ms");
            _ending80ms = _content.Load<SoundEffect>("ending80ms");
            _pickupSound = _content.Load<SoundEffect>("PickupSound");


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

            _mainCharacter.Color = Color.White; // default color
            _mainCharacter.Update(gameTime);


            introProgress += gameTime.ElapsedGameTime;
            TimeSpan songDurationLeft = TimeSpan.Zero;
            if (introProgress >= _ingameSong.Duration - TimeSpan.FromMilliseconds(85))
            {
                songDurationLeft.Add(TimeSpan.FromMilliseconds(introProgress.TotalMilliseconds - _ingameSong.Duration.TotalMilliseconds));
            }
            if (songDurationLeft.Milliseconds > 75) 
            {

                    _ending80ms.Play(); 
            }
            else if (songDurationLeft.Milliseconds > 1) _ending70ms.Play();
            if (introProgress.TotalMilliseconds >= _ingameSong.Duration.TotalMilliseconds -18)
            {
                introProgress = TimeSpan.Zero;
                MediaPlayer.Stop();
                MediaPlayer.Play(_ingameSong);
            }
            songDurationLeft = TimeSpan.Zero;


            // responsible for time limit
            while (!_hasBegun) gameTime.ElapsedGameTime = new TimeSpan();
            double extratime = (level * 8) + level;
            _elapsedTime += gameTime.ElapsedGameTime;
            if (_elapsedTime.TotalMilliseconds < 18)
            {
                foreach (MushroomSprite mushroom in _mushrooms[level - 1])
                {
                    while (CollisionHelper.Closeness(mushroom.Position, _mainCharacter.CurrentPosition) < 85 * 1.6f)
                    {
                        mushroom.Respawn();
                    }
                }
                _lost = false;
            }
            // responsible for time limit
            if (!_won && !_lost)
            {
                time = (System.TimeSpan.FromSeconds(5) + System.TimeSpan.FromSeconds(extratime)) - _elapsedTime;
            }
            else
            {
                _mainCharacter.Stopped = true; // charecter stops when game ends.
            }

            if (time < System.TimeSpan.FromSeconds(0)) // game is lost if time ends.
                _lost = true;

            // Calculates number of mushrooms left that are not toxic
            // Also resets and respawn the mushroom if it is poisonous and too close another mushroom
            if (!_calculated)
            {
                for (int i = 0; i < _maxLevel; i++)
                {
                    _mushroomsLeft[i] = 5 + i;

                    for (int j = 0; j < _mushrooms[i].Capacity; j++)
                    {

                        if (_mushrooms[i][j].Poisonous)
                        {
                            _mushroomsLeft[i]--;
                            int k = j - 1;

                            while (k > 0)
                            {
                                k--;
                                if (CollisionHelper.Closeness(_mushrooms[i][j].Position, _mushrooms[i][k].Position) < 100 * 1.6f)
                                {
                                    _mushrooms[i][j].Respawn();
                                    k = j - 1;
                                }
                            }
                        }
                    }
                }
                _calculated = true;
            }

            List<MushroomSprite> currentLMushroom = _mushrooms[level - 1];

            // needed to calculate light dimming in MushroomSprite
            foreach (MushroomSprite mushroom in currentLMushroom)
            {
                mushroom.Closeness = CollisionHelper.Closeness(mushroom.Position - new Vector2(-11.1375f, -11.1375f),
                    _mainCharacter.CurrentPosition);
            }

            //Detect and process collisions
            foreach (MushroomSprite mushroom in currentLMushroom)
            {
                if (!mushroom.Collected && mushroom.Bounds.CollidesWith(_mainCharacter.Bounds) && !_lost && _elapsedTime.TotalMilliseconds > 70)
                {
                    if (!mushroom.Poisonous)
                    {
                        _mainCharacter.Color = Color.Gold;
                        mushroom.Collected = true;
                        _pickupSound.Play();
                        _mushroomsLeft[level - 1]--;
                        _finalScore++;
                        _mainCharacter.CharBonusSpeed += 0.016f; //become faster with every mushroom
                    }
                    else
                    {
                        _mainCharacter.Poisoned = true;
                        _lost = true;
                    }
                }


            }

            //advances to next level
            if (_mushroomsLeft[level - 1] == 0 && level < _maxLevel)
            {
                level++;
                _extraTime = gameTime.TotalGameTime + System.TimeSpan.FromSeconds(1);
                foreach (MushroomSprite mushroom in _mushrooms[level - 1])
                {
                    while (CollisionHelper.Closeness(mushroom.Position, _mainCharacter.CurrentPosition) < 120 * 1.6f)
                    {
                        mushroom.Respawn();
                    }
                }
            }
            else if (level == _maxLevel && _mushroomsLeft[level - 1] == 0)
            {
                _won = true;
            }

            // makes time green to allude time increase
            if (_extraTime > gameTime.TotalGameTime)

                _timeColor = new Color(75, 200, 75);
            else
                _timeColor = Color.White;


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
            var spriteBatch = ScreenManager.SpriteBatch;

            //GraphicsDevice.Clear(Color.Transparent);
            List<MushroomSprite> currentLMushroom = _mushrooms[level - 1];

            // TODO: Add your drawing code here
            ScreenManager.SpriteBatch.Begin();
            _forest.Draw(spriteBatch);
            _mainCharacter.Draw(gameTime, spriteBatch);
            foreach (MushroomSprite mushroom in currentLMushroom) mushroom.Draw(gameTime, spriteBatch);
            if (_won)
            {
                spriteBatch.DrawString(spriteFont, "You win!", new Vector2(335, 195) * 1.6f, Color.White);
                spriteBatch.DrawString(spriteFont, $"Final Score: {_finalScore}", new Vector2(275, 220) * 1.6f, Color.White);
            }
            else if (!_lost)
            {
                //outlining text
                spriteBatch.DrawString(spriteFont, $"Mushrooms left: {_mushroomsLeft[level - 1]}", new Vector2(239, 60) * 1.6f, Color.Black);
                spriteBatch.DrawString(spriteFont, $"Mushrooms left: {_mushroomsLeft[level - 1]}", new Vector2(241, 60) * 1.6f, Color.Black);
                spriteBatch.DrawString(spriteFont, $"Mushrooms left: {_mushroomsLeft[level - 1]}", new Vector2(240, 59) * 1.6f, Color.Black);
                spriteBatch.DrawString(spriteFont, $"Mushrooms left: {_mushroomsLeft[level - 1]}", new Vector2(240, 61) * 1.6f, Color.Black);
                spriteBatch.DrawString(spriteFont, $"Mushrooms left: {_mushroomsLeft[level - 1]}", new Vector2(240, 60) * 1.6f, Color.White);
                spriteBatch.DrawString(spriteFont, $"Time Left: {time:ss'.'ff}", new Vector2(264, 20) * 1.6f, Color.Black);
                spriteBatch.DrawString(spriteFont, $"Time Left: {time:ss'.'ff}", new Vector2(266, 20) * 1.6f, Color.Black);
                spriteBatch.DrawString(spriteFont, $"Time Left: {time:ss'.'ff}", new Vector2(265, 19) * 1.6f, Color.Black);
                spriteBatch.DrawString(spriteFont, $"Time Left: {time:ss'.'ff}", new Vector2(265, 21) * 1.6f, Color.Black);
                spriteBatch.DrawString(spriteFont, $"Time Left: {time:ss'.'ff}", new Vector2(265, 20) * 1.6f, _timeColor);
            }
            else if (_lost)
            {
                Vector2 drawPosition; // to adjust text placement and center it
                if (_finalScore < 10) drawPosition = new Vector2(310, 195) * 1.6f;
                else drawPosition = new Vector2(320, 195) * 1.6f;


                spriteBatch.DrawString(spriteFont, "You lost!", drawPosition, Color.White);
                spriteBatch.DrawString(spriteFont, $"Final Score: {_finalScore}", new Vector2(275, 220) * 1.6f, Color.White);
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
