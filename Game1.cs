using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
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
    private float _rockCD;
    private float _lastWave;
    private float _lastShoot;
    private Player player;
    private Vector2 _mousePosition;
    private CollisionManager _collisionManager;
    private List<Bullet> _bullets = new List<Bullet>();
    private List<Rock> _rocks = new List<Rock>();
    public GameTime gameTime;
    

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
        _collisionManager = new CollisionManager();
        _bulletSpeed = 8.0f;
        _shootCD = 0.2f; //in seconds
        _rockCD = 45f;

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
       
        


        //rock
        Texture2D rTexture = Content.Load<Texture2D>("largeRock");
        for(int i = 0; i < 10; i++)
        {
            Rock rock = new Rock(player.Position, "large", rTexture);
            rock.Origin = new Vector2(rock.Texture.Width / 2f, rock.Texture.Height / 2f);
            _rocks.Add(rock);
        }
        
    
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
        UpdateRocks();
        CheckRocks();

        BulletCheck();

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

    private void BulletCheck()
    {
        List<Bullet> bulletsCopy = _bullets.ToList();
        List<Rock> rocksCopy = _rocks.ToList();
        foreach(Bullet bullet in bulletsCopy)
        {
            foreach(Rock rock in rocksCopy)
            {   
                //Console.WriteLine(_collisionManager.CollideCheck(_bulletTexture,bullet.Location,rock.Texture,rock.Position));
                
                if(_collisionManager.CollideCheck(_bulletTexture,bullet.Location,rock.Texture,rock.Position))
                {
                    _rocks.Remove(rock);
                    _bullets.Remove(bullet);
                }
            }
        }
    }

    private void CheckRocks() //check rock collisions with each other 
{
	List<Rock> rocksCopy = _rocks.ToList();
	foreach(Rock rock1 in rocksCopy)
	{
		foreach(Rock rock2 in rocksCopy)
		{
			if(rock1 == rock2)
			{
				continue;	
			}
				if(_collisionManager.CollideCheck(rock1.Texture,rock1.Position, 
                                    rock2.Texture, rock2.Position))
			    {
				_collisionManager.RockCollide(rock1,rock2);
			    }
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

    private void UpdateRocks()
    {
        List<Rock> rocksCopy = _rocks.ToList();
        foreach(Rock rock in rocksCopy)
        {
            
            Vector2 nDirection = Vector2.Normalize(rock.Direction);
            rock.Position = rock.Position - nDirection * rock.Speed;
        }
        for(int i = 0; i <= _rocks.Count - 1;i++)
        {
            if(i%2 == 0)
            {
                _rocks[i].Rotate(0.5f);
            }
            else
            {
                _rocks[i].Rotate(-0.5f);
            }
        }
    }

    private void UpdateWave()
    {

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

        foreach(Rock rock in _rocks)
        {
            _spriteBatch.Draw(rock.Texture,rock.Position, null, Color.White,
                            rock.Rotation,rock.Origin,
                            1.0f,SpriteEffects.None,0f);
        }

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
