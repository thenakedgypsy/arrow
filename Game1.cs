using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace mono;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private Texture2D _upTexture;
    private Texture2D _rightTexture;
    private Texture2D _downTexture;
    private Texture2D _leftTexture;
    private Texture2D _currentTexture;
    private Vector2 _position;
    private Rectangle rect;
    private float _rotation = 0;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _upTexture = Content.Load<Texture2D>("man");
        _rightTexture = Content.Load<Texture2D>("manRight");
        _leftTexture = Content.Load<Texture2D>("manLeft");
        _downTexture = Content.Load<Texture2D>("manDown");
        _position = new Vector2(100,100);
        _currentTexture = _upTexture;

        // TODO: use this.Content to load your game content here
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
        {
            Exit();
        }

        UpdateMovement();

           // TODO: Add your update logic here

        base.Update(gameTime);
    }

    private void UpdateMovement()
    {
        if (Keyboard.GetState().IsKeyDown(Keys.D))
        {   
            _position.X += 2; 
            _currentTexture = _rightTexture;
        }
        if (Keyboard.GetState().IsKeyDown(Keys.W))
        {
            _position.Y -= 2;
            _currentTexture = _upTexture;
        }

        if (Keyboard.GetState().IsKeyDown(Keys.A))
        {   
            _position.X -= 2; 
            _currentTexture = _leftTexture;
        }
        if (Keyboard.GetState().IsKeyDown(Keys.S))
        {
            _position.Y += 2;
            _currentTexture = _downTexture;
        }
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();

        _spriteBatch.Draw(_currentTexture,_position, Color.White);

        _spriteBatch.End();
        // TODO: Add your drawing code here

        base.Draw(gameTime);
    }
}
