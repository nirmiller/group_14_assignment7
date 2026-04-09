using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace group_14_assignment7;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private AsteroidsSpawn asteroidSpawner;
    Texture2D[] asteroidBodies;
    Texture2D[] asteroidTails;

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
        asteroidBodies = new Texture2D[]
        {
            Content.Load<Texture2D>("meteor_body"),
            Content.Load<Texture2D>("comet_body")
        };

        asteroidTails = new Texture2D[]
        {
            Content.Load<Texture2D>("meteor_tail"),
            Content.Load<Texture2D>("comet_tail")
        };

        asteroidSpawner = new AsteroidsSpawn(asteroidBodies, asteroidTails);
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // TODO: use this.Content to load your game content here
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        // TODO: Add your drawing code here

        base.Draw(gameTime);
    }
}