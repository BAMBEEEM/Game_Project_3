using Game_Project_3.StateManagement;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Game_Project_3.Background;
using Game_Project_3.GameButtons;
using SharpDX.Direct2D1;
using Microsoft.Xna.Framework.Audio;
using System.Reflection.Metadata;

namespace Game_Project_3.Screens
{
    public class MenuScreen : GameScreen
    {

        ContentManager _content;

        Texture2D _texture;
        VideoPlayer _player;
        bool _isPlaying = false;
        InputAction _skip;

        private SkySprite _sky;
        private NorthernLightSprite _northernLight;
        private StarSprite[] _stars = new StarSprite[7];
        private MenuWood _wood;
        private StartButton _startButton;
        private QuitButton _quitButton;
        private LoadingText _loadingText;


        private SpriteFont _arial;
        private InputState _inputState;
        private Song _menuSong;

        private InputAction _menuUp;
        private InputAction _menuDown;
        private InputAction _menuSelect;

        private float _time;
        private bool _isTransitioning = false;
        private float _timeSinceTransition;



        TimeSpan introProgress;
        //Song _menuSong;



        /*        public MenuScreen() 
                {
                    _player = new VideoPlayer();
                    _skip = new InputAction(new Buttons[] { Buttons.A }, new Keys[] { Keys.Enter, Keys.Space }, true);
                }*/

        /*        private void PlayGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
                {
                    ExitScreen();
                    var loadingScreen = new GameplayScreen();
                    ScreenManager.AddScreen(loadingScreen, PlayerIndex.One);

                } //here we have our own quit game etc.*/

        public override void Activate()
        {
            if (_content == null)
            {
                _content = new ContentManager(ScreenManager.Game.Services, "Content");
            }
            _sky = new SkySprite();
            _northernLight = new NorthernLightSprite();
            for (int i = 0; i < 7; i++)
            {
                _stars[i] = new StarSprite();
            }

            _startButton = new StartButton();
            _quitButton = new QuitButton();
            _inputState = new InputState();
            _wood = new MenuWood();
            _loadingText = new LoadingText();

            _sky.LoadContent(_content);
            _northernLight.LoadContent(_content);
            for (int i = 0; i < 7; i++)
            { _stars[i].LoadContent(_content); }
            _loadingText.LoadContent(_content);



            _wood.LoadContent(_content);


            _startButton.LoadContent(_content);
            _quitButton.LoadContent(_content);

            /*
                        _menuSong = _content.Load<Song>("MenuSongWithLoop");
                        MediaPlayer.IsRepeating = true;
                        MediaPlayer.Play(_menuSong);*/

            _menuSong = _content.Load<Song>("MenuSong");


            _menuUp = new InputAction(
                new[] { Buttons.DPadUp, Buttons.LeftThumbstickUp },
                new[] { Keys.Up, Keys.W }, true);
            _menuDown = new InputAction(
                new[] { Buttons.DPadDown, Buttons.LeftThumbstickDown },
                new[] { Keys.Down, Keys.S }, true);
            _menuSelect = new InputAction(
                new[] { Buttons.A, Buttons.Start },
                new[] { Keys.Enter, Keys.Space }, true);

            MediaPlayer.Play(_menuSong);

        }



        public override void HandleInput(GameTime gameTime, InputState input)
        {
            if (_inputState.PriorMouseState.Position != _inputState.CurrentMouseState.Position)
            {
                _startButton.IsSelected = false;
                _startButton.Shade = Color.White;
                _quitButton.IsSelected = false;
                _quitButton.Shade = Color.White;
            }
            else
            {
                if (_startButton.IsSelected || _quitButton.IsSelected)
                {

                    ScreenManager.Game.IsMouseVisible = false;
                }
                else ScreenManager.Game.IsMouseVisible = true;

            }



            if (_quitButton.Bounds.CollidesWith(_inputState.Cursor) && _inputState.Clicked)
                _quitButton.InitialClick = true;

            if (_quitButton.Bounds.CollidesWith(_inputState.Cursor) && _quitButton.InitialClick)
            {
                if (_inputState.Clicking)
                {
                    _quitButton.Shade = Color.DarkGray;

                }
                else
                {
                    _quitButton.Shade = Color.White;
                }
                if (Mouse.GetState().LeftButton == ButtonState.Released)
                {
                    _quitButton.Shade = Color.DarkGray;
                    _quitButton.InitialClick = false;
                    QuitGame();
                }
            }
            else
            {
                if (_inputState.CurrentMouseState.LeftButton == ButtonState.Released)
                {
                    _quitButton.InitialClick = false;
                }
                _quitButton.Shade = Color.White;
            }

            if (_startButton.Bounds.CollidesWith(_inputState.Cursor) && _inputState.Clicked)
                _startButton.InitialClick = true;

            if (_startButton.Bounds.CollidesWith(_inputState.Cursor) && _startButton.InitialClick)
            {
                if (_inputState.Clicking)
                {
                    _startButton.Shade = Color.DarkGray;

                }
                else
                {
                    _startButton.Shade = Color.White;
                }
                if (_inputState.CurrentMouseState.LeftButton == ButtonState.Released)
                {
                    _startButton.InitialClick = false;
                    LoadTransition(gameTime);
                }

            }
            else
            {
                if (_inputState.CurrentMouseState.LeftButton == ButtonState.Released)
                {
                    _startButton.InitialClick = false;
                }
                _startButton.Shade = Color.White;
            }

            PlayerIndex playerIndex;


            if (_menuUp.Occurred(input, ControllingPlayer, out playerIndex))
            {
                if (_startButton.IsSelected)
                {
                    _startButton.IsSelected = false;
                    _quitButton.IsSelected = true;
                }
                else if (_quitButton.IsSelected)
                {
                    _startButton.IsSelected = true;
                    _quitButton.IsSelected = false;
                }
                else
                {
                    _startButton.IsSelected = true;
                    _quitButton.IsSelected = false;
                }

            }

            if (_menuDown.Occurred(input, ControllingPlayer, out playerIndex))
            {

                if (_startButton.IsSelected)
                {
                    _startButton.IsSelected = false;
                    _quitButton.IsSelected = true;
                }
                else if (_quitButton.IsSelected)
                {
                    _startButton.IsSelected = true;
                    _quitButton.IsSelected = false;
                }
                else
                {
                    _startButton.IsSelected = false;
                    _quitButton.IsSelected = true;
                }


            }

            if (_menuSelect.Occurred(input, ControllingPlayer, out playerIndex))
            {
                if (_startButton.IsSelected)
                {
                    _startButton.Shade = Color.DarkGray;
                    LoadTransition(gameTime);
                }
                else
                {
                    _quitButton.Shade = Color.DarkGray;
                    QuitGame();
                }
            }
        }

        void StartGame()
        {
            ScreenManager.Game.ResetElapsedTime();
            var gameplayScreen = new ExperimentalGame();
            ScreenManager.AddScreen(gameplayScreen, PlayerIndex.One);
            MediaPlayer.Stop();
            ExitScreen();
        }

        void LoadTransition(GameTime gameTime)
        {
            _time = 0;
            _isTransitioning = true;
            _timeSinceTransition = 0;
        }

        void QuitGame()
        {
            ScreenManager.Game.Exit();
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {


            _timeSinceTransition += gameTime.ElapsedGameTime.Milliseconds;

            introProgress += gameTime.ElapsedGameTime;

            if (_isTransitioning && _timeSinceTransition > 3000)
            {
                StartGame();
            }

            if (introProgress.TotalMilliseconds >= _menuSong.Duration.TotalMilliseconds - 18)
            {
                introProgress = TimeSpan.Zero;
                MediaPlayer.Stop();
                MediaPlayer.Play(_menuSong);
            }

            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);





            _inputState.Update();

            /*if (_inputState.Exit == true)
            { Exit(); }*/
            // TODO: Add your update logic here

            //balls[2].Position += _inputManager.Direction;

            // TODO: Add your update logic here

            foreach (StarSprite star in _stars)
            {
                if (star.Bounds.CollidesWith(_wood.Bounds))

                {
                    star.Reset();
                }
                star.Update(gameTime);

            }
        }

        public override void Deactivate()
        {
            /*            _player.Pause();
                        _isPlaying = false;*/
        }

        public override void Draw(GameTime gameTime)
        {

            var _spriteBatch = ScreenManager.SpriteBatch;
            _spriteBatch.Begin();

            _sky.Draw(_spriteBatch);
            _northernLight.Draw(_spriteBatch);


            _loadingText.Draw(_spriteBatch, gameTime);


                for (int i = 0; i < 7; i++)
                {
                    _stars[i].Draw(gameTime, _spriteBatch);
                }
            _spriteBatch.End();

            Matrix cameraUpTransform = Matrix.Identity;

            if (_isTransitioning)
            {
                _time += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                Matrix cameraUpTranslation = Matrix.CreateTranslation(0, -0.6f * _time, 0);
                cameraUpTransform = cameraUpTranslation;
            }

            _spriteBatch.Begin(transformMatrix: cameraUpTransform);

            _wood.Draw(_spriteBatch);

            _startButton.Draw(_spriteBatch);

            _quitButton.Draw(_spriteBatch);
            _spriteBatch.End();




        }
    }
}
