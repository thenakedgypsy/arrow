﻿using System;
using System.Collections.Generic;
using System.Linq;
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
    private Texture2D _bulletTexture;
    private float _bulletSpeed;
    private float _shootCD;
    private float _lastShoot;
    private Player player;
    private Rock rock;
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

        _graphics.PreferredBackBufferWidth = 1920;
        _graphics.PreferredBackBufferHeight = 1080;
        _graphics.ApplyChanges();
        _bulletSpeed = 8.0f;
        _shootCD = 0.2f; //in seconds

        base.Initialize();

    }

    protected override void LoadContent()
    {
        //initialise some other bits
        _mouseState = Mouse.GetState();
        _mousePosition = new Vector2(_mouseState.X,_mouseState.Y);

        _spriteBatch = new SpriteBatch(GraphicsDevice);

        Texture2D pTexture = Content.Load<Texture2D>("tri");
        player = new Player(pTexture);
        player.Origin = new Vector2(player.Texture.Width / 1.5f, player.Texture.Height / 2f);

        _bulletTexture = Content.Load<Texture2D>("bull");
        _lastShoot = 0f;

        Texture2D rTexture = Content.Load<Texture2D>("largeRock");
        rock = new Rock(player.Position, "large", rTexture);
        rock.Origin = new Vector2(rock.Texture.Width / 2f, rock.Texture.Height / 2f);
    
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
        {
            Exit();
        }
        
        UpdateMouse();
        player.UpdateRotation(_mousePosition);
        UpdateMovement();
        UpdateShoot(gameTime);
        UpdateBullets();
        rock.Rotate(0.5f);

        base.Update(gameTime);
    }

    private void UpdateMovement()
    {
        Vector2 nDirection = Vector2.Normalize(player.MoveDirection);
        Vector2 strafeDirection = new Vector2((float)Math.Cos(player.Rotation + MathHelper.PiOver2), //x
                                            (float)Math.Sin(player.Rotation + MathHelper.PiOver2)); //y
        Vector2 nStrafeDirection = Vector2.Normalize(strafeDirection);
        
        KeyboardState keyState = Keyboard.GetState();
        if(keyState.IsKeyDown(Keys.W))
        {
            if(keyState.IsKeyDown(Keys.D) || keyState.IsKeyDown(Keys.A))
            {
            player.Position = player.Position + nDirection * (player.Speed / 1.5f);
            }
            else{
            player.Position = player.Position + nDirection * (player.Speed + 0.5f);
            }
        }
        if(keyState.IsKeyDown(Keys.S))
        {
            if(keyState.IsKeyDown(Keys.D) || keyState.IsKeyDown(Keys.A))
            {
            player.Position = player.Position - nDirection * (player.Speed / 1.5f);
            }
            else{
            player.Position = player.Position - nDirection * player.Speed;
            }
        }        
        if(keyState.IsKeyDown(Keys.D))
        {
            if (keyState.IsKeyDown(Keys.W) || keyState.IsKeyDown(Keys.S))
            {
                player.Position = player.Position + nStrafeDirection * (player.Speed / 1.5f);
            }
            else
            {
                player.Position = player.Position + nStrafeDirection * player.Speed;
            }
        }
        if(keyState.IsKeyDown(Keys.A))
        {
            if (keyState.IsKeyDown(Keys.W) || keyState.IsKeyDown(Keys.S))
            {
                player.Position = player.Position - nStrafeDirection * (player.Speed / 1.5f);
            }
            else
            {
                player.Position = player.Position - nStrafeDirection * player.Speed;
            }
        }
    }

    private void UpdateShoot(GameTime gameTime)
    {
        if(Keyboard.GetState().IsKeyDown(Keys.Space))
        {
            float currentTime = (float)gameTime.TotalGameTime.TotalSeconds;
            if(currentTime - _lastShoot >= _shootCD)
            {
            Bullet bullet = new Bullet(player.MoveDirection, player.Position);
            _bullets.Add(bullet);
            _lastShoot = currentTime;
            }
        }
    }

    private void UpdateMouse()
    {
        _mouseState = Mouse.GetState();
        _mousePosition = new Vector2(_mouseState.X,_mouseState.Y);
    }

    private void UpdateBullets()
    {
        List<Bullet> bulletsCopy  = _bullets.ToList();
        foreach(Bullet bullet in bulletsCopy)
        {
            Vector2 nDirection = Vector2.Normalize(bullet.Direction);
            bullet.Location = bullet.Location + nDirection * _bulletSpeed;
            if(bullet.Location.Y < 0 || bullet.Location.Y > 1080)
            {
            _bullets.Remove(bullet);
            }
            else if(bullet.Location.X < 0 || bullet.Location.X > 1920)
            {
            _bullets.Remove(bullet);
            }
        } 
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        _spriteBatch.Begin();

        _spriteBatch.Draw(player.Texture,
                        player.Position, null, Color.White, 
                        player.Rotation, player.Origin, 1.0f, 
                        SpriteEffects.None, 0f);

        foreach(Bullet bullet in _bullets)
        {
            _spriteBatch.Draw(_bulletTexture, bullet.Location, Color.White);
        }

        _spriteBatch.Draw(rock.Texture,rock.Location,null, Color.White,
                            rock.Rotation,rock.Origin,
                            1.0f,SpriteEffects.None,0f);

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
