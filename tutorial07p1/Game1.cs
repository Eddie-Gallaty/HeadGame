using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using tutorial07p1.Sprites;
using tutorial07p1.Models;
namespace tutorial07p1
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        //adding the screen size vars
        public static int ScreenWidth;
        public static int ScreenHeight;
        public static Random Random;

        private List<Sprite> _sprites;
        private float _timer;
        private bool HasStarted = false;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            
            Random = new Random();
            ScreenHeight = _graphics.PreferredBackBufferHeight;
            ScreenWidth = _graphics.PreferredBackBufferWidth;

        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            Restart();
        }

        private void Restart()
        {
            var headTexture = Content.Load<Texture2D>("head");
            _sprites = new List<Sprite>()
            {
                new Player(headTexture)
                {
                    Position = new Vector2((ScreenWidth / 2) - (headTexture.Width / 2), ScreenHeight - headTexture.Height),
                    Input = new Input()
                    {
                        Left = Keys.A,
                        Right = Keys.D,
                    },

                    Speed = 10f,
                },
            };

            HasStarted = false;
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                HasStarted = false;
                //changing to true will turn the pause feature off
                //HasStarted = true;
            }
            else 
            {
                HasStarted= true;
            }
            //the below is like a pause before the space button is pressed. commenting out to play with making an actual pause

            if (!HasStarted)
            {
                return;
            }

            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            foreach (var sprite in _sprites)
            {
                sprite.Update(gameTime, _sprites);
            }

            if(_timer > 0.25f)
            {
                _timer = 0;
                 _sprites.Add(new Bomb(Content.Load<Texture2D>("bullet")));
            }

            for(int i = 0; i < _sprites.Count; i++)
            {
                var sprite = _sprites[i];
                if (sprite.IsRemoved)
                {
                    _sprites.RemoveAt(i);
                    i--;
                }

                if (sprite is Player)
                {
                    var player = sprite as Player;
                    if (player.HasDied)
                    {
                        Restart();
                    }
                }
            }

            

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();

            foreach (var sprite in _sprites)
            {
                sprite.Draw(_spriteBatch);
            }
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}