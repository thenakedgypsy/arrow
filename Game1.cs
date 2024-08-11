using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace mono;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private MouseState _mouseState;
    private Texture2D _bulletTexture;
    private Texture2D _midRockTexture;
    private Texture2D _smallRockTexture;
    private Texture2D _largeRockTexture;
    private Texture2D _debrisTexture;
    private Texture2D _shipDebrisTexture;
    private SoundEffect _explodeSound;
    private SoundEffect _destroySound;
    private SoundEffect _laserSound;
    private Song _theme;
    private float _bulletSpeed;
    private float _shootCD;
    private float _waveCD;
    private float _lastWave;
    private int _waveNumber;
    private float _lastShoot;
    private SpriteFont font;
    private Player player;
    private Vector2 _mousePosition;
    private CollisionManager _collisionManager;
    private List<Bullet> _bullets = new List<Bullet>();
    private List<Rock> _rocks = new List<Rock>();
    private List<Debris> _debris = new List<Debris>();
    public GameTime gameTime;
    private int _score;
    private Vector2 goPosition;
    

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
        _shootCD = 0.4f; //in seconds
        _lastShoot = 0f;
        
        _waveCD = 15f; //in seconds
        _lastWave = -15f;
        _waveNumber = 20;
       
        _score = 0;

        goPosition = new Vector2(880,0);

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
        

        _midRockTexture = Content.Load<Texture2D>("midRock");
        
        _smallRockTexture = Content.Load<Texture2D>("smallRock");
       
        _largeRockTexture = Content.Load<Texture2D>("largeRock");

        _debrisTexture = Content.Load<Texture2D>("debris");

        _shipDebrisTexture = Content.Load<Texture2D>("debris2");

        _destroySound = Content.Load<SoundEffect>("destroy");

        _explodeSound = Content.Load<SoundEffect>("explode");

        _laserSound = Content.Load<SoundEffect>("pew");

        font = Content.Load<SpriteFont>("KarenFat");
        
        _theme = Content.Load<Song>("music");
        MediaPlayer.Play(_theme);

        
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
        {
            Exit();
        }
        CheckPlayer();
        UpdateMouse();
        player.UpdateRotation(_mousePosition);
        UpdateMovement();
        UpdateShoot(gameTime);
        UpdateBullets();
        UpdateRocks();
        CheckRocks();
        UpdateWave(gameTime);
        CheckBullets();
        UpdateDebris();
        

        base.Update(gameTime);
    }

    private void UpdateWave(GameTime gameTime)
{    
    if(player.Alive)
    {
        float currentTime = (float)gameTime.TotalGameTime.TotalSeconds; 
        if(currentTime - _lastWave >= _waveCD) //if CD has passed
        {
            _waveNumber++; //need a property in game.cs for this and _waveCD
            _lastWave = currentTime;
            while(_rocks.Count < _waveNumber)
            {
                Rock rock = new Rock(player.Position,"large");
                rock.Texture = _largeRockTexture;
                rock.Origin = new Vector2(rock.Texture.Width / 2f, rock.Texture.Height / 2f);
                _rocks.Add(rock);
            }
            
        }
    }
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
        
        if(Keyboard.GetState().IsKeyDown(Keys.Space) && player.Alive)
        {
            float currentTime = (float)gameTime.TotalGameTime.TotalSeconds;
            if(currentTime - _lastShoot >= _shootCD)
            {
            Bullet bullet = new Bullet(player.MoveDirection, player.Position);
            _bullets.Add(bullet);
            _lastShoot = currentTime;
            _laserSound.Play();
            }
        }        
    }

    private void CheckBullets()
    {
        List<Bullet> bulletsCopy = _bullets.ToList();
        List<Rock> rocksCopy = _rocks.ToList();
        List<Debris> newDebris = new List<Debris>();
        foreach(Bullet bullet in bulletsCopy)
        {
            foreach(Rock rock in rocksCopy)
            {                   
                if(_collisionManager.CollideCheck(_bulletTexture,bullet.Location,rock.Texture,rock.Position))
                {
                    if(rock.Size == "large" || rock.Size == "mid")
                    {
                        List<Rock> tRocks = rock.Fragment();
                        _rocks.Add(tRocks[0]);
                        _rocks.Add(tRocks[1]);
                    }
                    newDebris = rock.Explode(_debrisTexture);
                    foreach(Debris debris in newDebris)
                    {
                        _debris.Add(debris);
                    }
                    _score += rock.Score;
                    _rocks.Remove(rock);
                    _bullets.Remove(bullet);
                    _destroySound.Play();
                    
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

    private void CheckPlayer()
    {
        if(player.Alive)
        {
            List<Rock> rocksCopy = _rocks.ToList();
            foreach(Rock rock in rocksCopy)
            {
                if(_collisionManager.CollideCheck(player.Texture, player.Position, rock.Texture, rock.Position))
                {
                    player.Alive = false;
                    _explodeSound.Play();
                    List<Debris> newDebris = player.Explode(_shipDebrisTexture);
                    foreach(Debris debris in newDebris)
                    {
                        _debris.Add(debris);
                    }
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
    
    private void UpdateDebris()
    {
        List<Debris> debrisCopy = _debris.ToList();
        foreach(Debris debris in debrisCopy)
        {
            Vector2 nDirection = Vector2.Normalize(debris.Direction);
            debris.Position += nDirection * (_bulletSpeed / 2);
            if(debris.Position.Y < -10 || debris.Position.Y > 1080)
            {
                _debris.Remove(debris);
            }
            else if(debris.Position.X < -10 || debris.Position.X > 1920)
            {
                _debris.Remove(debris);
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
            if(rock.Position.Y < -100 || rock.Position.Y > 1180)
            {
            _rocks.Remove(rock);
            }
            else if(rock.Position.X < -100 || rock.Position.X > 2020)
            {
            _rocks.Remove(rock);
            }
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

    private void UpdateGameOver()
    {
        if(goPosition.Y < 540)
        {
            goPosition.Y = goPosition.Y + 0.5f * _bulletSpeed;
        }
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        _spriteBatch.Begin();

        if(player.Alive)
        {
        _spriteBatch.Draw(player.Texture,
                        player.Position, null, Color.White, 
                        player.Rotation, player.Origin, 1.0f, 
                        SpriteEffects.None, 0f);
        }

        foreach(Bullet bullet in _bullets)
        {
            _spriteBatch.Draw(_bulletTexture, bullet.Location, Color.White);
        }

        foreach(Rock rock in _rocks)
        {
            if(rock.Size == "mid")
            {
                rock.Texture = _midRockTexture;
                rock.Score = 2;
            }
            if(rock.Size == "small")
            {
                rock.Texture = _smallRockTexture;
                rock.Score = 3;
            }
            _spriteBatch.Draw(rock.Texture,rock.Position, null, Color.White,
                            rock.Rotation,rock.Origin,
                            1.0f,SpriteEffects.None,0f);
        }

        foreach(Debris debris in _debris)
        {

            _spriteBatch.Draw(debris.Texture, debris.Position, null, Color.White);
        }

        _spriteBatch.DrawString(font, $"SCORE: {_score}", new Vector2(1720, 50), Color.White);

        if(!player.Alive)
        {
        _spriteBatch.DrawString(font, $"GAME OVER", goPosition, Color.White);
        UpdateGameOver();
        }
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
