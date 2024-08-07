using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;





namespace mono;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private MouseState _mouseState;
    private Texture2D _playerTexture;
    private Texture2D _bulletTexture;
    private float _bulletSpeed;
    private float _shootCD;
    private float _lastShoot;
    private float _playerRotation;
    private Vector2 _playerMoveDirection;
    private float _playerSpeed;
    private Vector2 _playerOrigin;
    private Vector2 _playerPosition;
    private Vector2 _mousePosition;
    public GameTime gameTime;

    private List<Bullet> _bullets = new List<Bullet>();
    


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
        _playerSpeed = 2.0f;
        _bulletSpeed = 12.0f;
        _shootCD = 0.5f; //in seconds
    }

    protected override void LoadContent()
    {
        _mouseState = Mouse.GetState();
        _mousePosition = new Vector2(_mouseState.X,_mouseState.Y);
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _playerTexture = Content.Load<Texture2D>("tri");
        _bulletTexture = Content.Load<Texture2D>("bull");
        _playerPosition = new Vector2(100,100);
        _playerOrigin = new Vector2(_playerTexture.Width / 2f, _playerTexture.Height / 2f);
        _lastShoot = 0f;

        // TODO: use this.Content to load your game content here
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
        {
            Exit();
        }
        
        
        UpdateMouse();
        UpdateRotation();
        UpdateMovement();
        if(Keyboard.GetState().IsKeyDown(Keys.Space))
        {            
            Shoot(gameTime);
        }
        UpdateBullets();

        base.Update(gameTime);
    }

    private void UpdateMovement()
    {
        Vector2 nDirection = Vector2.Normalize(_playerMoveDirection);
        Vector2 strafeDirection = new Vector2((float)Math.Cos(_playerRotation + MathHelper.PiOver2), //x
                                            (float)Math.Sin(_playerRotation + MathHelper.PiOver2)); //y
        Vector2 nStrafeDirection = Vector2.Normalize(strafeDirection);
        
        KeyboardState keyState = Keyboard.GetState();
        if(keyState.IsKeyDown(Keys.W))
        {
            if(keyState.IsKeyDown(Keys.D) || keyState.IsKeyDown(Keys.A))
            {
            _playerPosition = _playerPosition + nDirection * (_playerSpeed / 1.5f);
            }
            else{
            _playerPosition = _playerPosition + nDirection * (_playerSpeed + 0.5f);
            }
        }
        if(keyState.IsKeyDown(Keys.S))
        {
            if(keyState.IsKeyDown(Keys.D) || keyState.IsKeyDown(Keys.A))
            {
            _playerPosition = _playerPosition - nDirection * (_playerSpeed / 1.5f);
            }
            else{
            _playerPosition = _playerPosition - nDirection * (_playerSpeed - 0.5f);
            }
        }        
        if(keyState.IsKeyDown(Keys.D))
        {
            if (keyState.IsKeyDown(Keys.W) || keyState.IsKeyDown(Keys.S))
            {
                _playerPosition = _playerPosition + nStrafeDirection * (_playerSpeed / 1.5f);
            }
            else
            {
                _playerPosition = _playerPosition + nStrafeDirection * _playerSpeed;
            }
        }
        if(keyState.IsKeyDown(Keys.A))
        {
            if (keyState.IsKeyDown(Keys.W) || keyState.IsKeyDown(Keys.S))
            {
                _playerPosition = _playerPosition - nStrafeDirection * (_playerSpeed / 1.5f);
            }
            else
            {
                _playerPosition = _playerPosition - nStrafeDirection * _playerSpeed;
            }
        }
    }

    private void UpdateRotation()
    {
        _playerMoveDirection = _mousePosition - _playerPosition;
        _playerRotation = (float)Math.Atan2(_playerMoveDirection.Y, _playerMoveDirection.X);
    }

    private void Shoot(GameTime gameTime)
    {
        float currentTime = (float)gameTime.TotalGameTime.TotalSeconds;
        if(currentTime - _lastShoot >= _shootCD)
        {
        Bullet bullet = new Bullet(_playerMoveDirection, _playerPosition);
        _bullets.Add(bullet);
        _lastShoot = currentTime;
        }
    }

    private void UpdateMouse()
    {
        _mouseState = Mouse.GetState();
        _mousePosition = new Vector2(_mouseState.X,_mouseState.Y);
    }

    private void UpdateBullets()
    {
        foreach(Bullet bullet in _bullets)
        {
            Vector2 nDirection = Vector2.Normalize(bullet.Direction);
            bullet.Location = bullet.Location + nDirection * _bulletSpeed;
        }
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        _spriteBatch.Begin();

        _spriteBatch.Draw(_playerTexture,
            _playerPosition, null, Color.White, 
            _playerRotation, _playerOrigin, 1.0f, 
            SpriteEffects.None, 0f);

        foreach(Bullet bullet in _bullets)
        {
            _spriteBatch.Draw(_bulletTexture, bullet.Location, Color.White);
        }

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
