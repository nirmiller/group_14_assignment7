using System.Collections.Generic;
using System.Net.Http.Headers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace group_14_assignment7;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    // background 
    private Texture2D _background; 
    
    // screen info 
    private float _screenWidth = 1000;
    private float _screenHeight = 800;
    
    // asteroids 
    private AsteroidsSpawn asteroidSpawner;
    Texture2D[] asteroidBodies;
    Texture2D[] asteroidTails;
    
    // ship 
    private Texture2D _ship;
    private Texture2D _shot;
    private Spaceship _spaceship;
    
    // gui
    private HUD _hud;
    private SpriteFont _font;
    private Texture2D _heartFull;
    private Texture2D _heartEmpty;
    
    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        
        _graphics.PreferredBackBufferHeight = 800;
        _graphics.PreferredBackBufferWidth = 1000;
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
        // background
        _background = Content.Load<Texture2D>("imgs/background");
        
        // asteroids 
        asteroidBodies = new Texture2D[]
        {
            Content.Load<Texture2D>("imgs/meteor_body"),
            Content.Load<Texture2D>("imgs/comet_body")
        };

        asteroidTails = new Texture2D[]
        {
            Content.Load<Texture2D>("imgs/meteor_tail"),
            Content.Load<Texture2D>("imgs/comet_tail")
        };

        asteroidSpawner = new AsteroidsSpawn(asteroidBodies, asteroidTails);
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // ship 
        _ship = Content.Load<Texture2D>("imgs/largerRocket");
        _shot = Content.Load<Texture2D>("imgs/shot");
        _spaceship = new Spaceship(screenWidth: _screenWidth,
            screenHeight: _screenHeight,
            spaceshipTexture: _ship,
            shipSpeed: 150f,
            shot: _shot,
            shotSpeed: 250f);
        
        // gui
        _font = Content.Load<SpriteFont>("fonts/Arial"); // make sure you added this in Content
        _heartFull = Content.Load<Texture2D>("imgs/heart");
        _heartEmpty = Content.Load<Texture2D>("imgs/missing_heart");
        
        _hud = new HUD(_font, _heartFull, _heartEmpty, (int)_screenWidth, (int)_screenHeight);
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();
        
        // ship 
        _spaceship.Update(gameTime);
        
        // asteroid
        List<(Vector2 pos, float radius)> collisionPoints = new();

        collisionPoints.Add((_spaceship.getPosition(), 40f)); // tweak this

        List<Vector2> fireShots = _spaceship.GetShotPositions();
        var asteroids = asteroidSpawner.GetAsteroids();

        for (int i = fireShots.Count - 1; i >= 0; i--)
        {
            Vector2 shotPos = fireShots[i];

            foreach (var asteroid in asteroids)
            {
                if (!asteroid.isAlive) continue;

                if (asteroid.IsColliding(shotPos, 4f)) // same radius you used
                {
                    asteroid.Kill();                 // destroy asteroid
                    _spaceship.RemoveShotAt(i);      // remove shot
                    _hud.AddScore(3);                // add score

                    break; // prevents collateral hits
                }
            }
        }

        for (int i = 0; i < fireShots.Count; i++)
        {
            collisionPoints.Add((fireShots[i], 8f)); 
        }

        // pass to asteroid system
        var (shipHit, kills) = asteroidSpawner.beginAsteroids(gameTime, collisionPoints);
        
        if (shipHit)
        {
            _hud.LoseLife();

            if (!_hud.IsGameOver())
            {
                _spaceship.Reset();
                asteroidSpawner.ResetAll();
            }
        }
        
        // gui
        _hud.Update(gameTime);
        if (_hud.ConsumeResetFlag())
        {
            _spaceship.Reset();
            asteroidSpawner.ResetAll();
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        // background
        _spriteBatch.Begin();
        _spriteBatch.Draw(_background,
            new Vector2(0, 0),
            Color.White);
        _spriteBatch.End();
        
        // asteroid 
        asteroidSpawner.DrawAsteroids(_spriteBatch);
        
        // ship
        _spaceship.Draw(_spriteBatch);
    
        // gui
        _hud.Draw(_spriteBatch);

        base.Draw(gameTime);
    }
}