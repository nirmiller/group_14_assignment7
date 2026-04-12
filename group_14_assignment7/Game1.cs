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
    
    // astroids 
    private AsteroidsSpawn asteroidSpawner;
    Texture2D[] asteroidBodies;
    Texture2D[] asteroidTails;
    
    // spaceship 
    private Texture2D _ship;
    private Texture2D _shot;
    
    // gui

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
        
        // astroids 
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

        // spaceship 
        _ship = Content.Load<Texture2D>("imgs/rocket");
        _shot = Content.Load<Texture2D>("imgs/shot");
    
    
        // gui
        
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();
        
        // astroid
        asteroidSpawner.beginAsteroids(gameTime);
        
        // spaceship 
    
    
        // gui

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
        
        // astroid 
        asteroidSpawner.DrawAsteroids(_spriteBatch);
        
        // spaceship 
    
    
        // gui

        base.Draw(gameTime);
    }
}