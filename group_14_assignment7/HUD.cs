using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace group_14_assignment7;

public class HUD
{
    private int _lives;
    private int _score;

    private SpriteFont _font;
    private Texture2D _heartFull;
    private Texture2D _heartEmpty;

    private float _timeAccumulator;
    private float _scale = 0.15f;
    
    private bool _justReset;

    private enum GameState
    {
        Playing,
        GameOver
    }

    private GameState _currentState;

    private int _screenWidth;
    private int _screenHeight;

    public HUD(SpriteFont font, Texture2D heartFull, Texture2D heartEmpty, int screenWidth, int screenHeight)
    {
        _font = font;
        _heartFull = heartFull;
        _heartEmpty = heartEmpty;

        _screenWidth = screenWidth;
        _screenHeight = screenHeight;

        Reset();
    }

    public void Reset()
    {
        _lives = 3;
        _score = 0;
        _timeAccumulator = 0f;
        _currentState = GameState.Playing;

        _justReset = true; 
    }
    
    public bool ConsumeResetFlag()
    {
        if (_justReset)
        {
            _justReset = false;
            return true;
        }
        return false;
    }

    public void AddScore(int amount)
    {
        if (_currentState == GameState.Playing)
            _score += amount;
    }

    public void LoseLife()
    {
        if (_currentState != GameState.Playing) return;

        _lives--;

        if (_lives <= 0)
        {
            _lives = 0;
            _currentState = GameState.GameOver;
        }
    }

    public void Update(GameTime gameTime)
    {
        if (_currentState == GameState.Playing)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _timeAccumulator += dt;

            if (_timeAccumulator >= 1f)
            {
                _score += 1;
                _timeAccumulator -= 1f;
            }
        }
        else if (_currentState == GameState.GameOver)
        {
            KeyboardState ks = Keyboard.GetState();

            if (ks.IsKeyDown(Keys.P))
            {
                Reset();
            }
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();

        if (_currentState == GameState.Playing)
        {
            DrawScore(spriteBatch);
            DrawLives(spriteBatch);
        }
        else if (_currentState == GameState.GameOver)
        {
            DrawGameOver(spriteBatch);
        }

        spriteBatch.End();
    }

    private void DrawScore(SpriteBatch spriteBatch)
    {
        string scoreText = $"Score: {_score}";
        spriteBatch.DrawString(_font, scoreText, new Vector2(20, 20), Color.White);
    }

    private void DrawLives(SpriteBatch spriteBatch)
    {
        int heartSpacing = 10;
        int heartWidth = (int)(_heartFull.Width * _scale);
        int totalHearts = 3;

        int totalWidth = totalHearts * heartWidth + (totalHearts - 1) * heartSpacing;

        int startX = _screenWidth - totalWidth - 20;
        int y = 20;

        for (int i = 0; i < totalHearts; i++)
        {
            Texture2D texture = i < _lives ? _heartFull : _heartEmpty;

            int x = startX + i * (heartWidth + heartSpacing);
            

            spriteBatch.Draw(
                texture,
                new Vector2(x, y),
                null,
                Color.White,
                0f,
                Vector2.Zero,
                _scale,
                SpriteEffects.None,
                0f
            );
        }

        string livesText = $"Lives Remaining: {_lives}";
        Vector2 textSize = _font.MeasureString(livesText);

        spriteBatch.DrawString(
            _font,
            livesText,
            new Vector2(_screenWidth - textSize.X - 20, y + heartWidth + 5),
            Color.White
        );
    }

    private void DrawGameOver(SpriteBatch spriteBatch)
    {
        string gameOverText = "GAME OVER";
        string scoreText = $"Score: {_score}";
        string restartText = "Press P to Play Again";

        Vector2 goSize = _font.MeasureString(gameOverText);
        Vector2 scoreSize = _font.MeasureString(scoreText);
        Vector2 restartSize = _font.MeasureString(restartText);

        Vector2 center = new Vector2(_screenWidth / 2f, _screenHeight / 2f);

        spriteBatch.DrawString(_font, gameOverText, center - new Vector2(goSize.X / 2, 100), Color.White);
        spriteBatch.DrawString(_font, scoreText, center - new Vector2(scoreSize.X / 2, 20), Color.White);
        spriteBatch.DrawString(_font, restartText, center - new Vector2(restartSize.X / 2, -60), Color.White);
    }
    
    public bool IsGameOver()
    {
        return _currentState == GameState.GameOver;
    }
}