using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace group_14_assignment7;

public class AsteroidsSpawn
{
    Queue<Asteroid> asteroids;
    Texture2D[] asteroidTextures;
    Texture2D[] tailTextures;

    int screenWidth = 1000;
    int screenHeight = 800;
    int spawnPadding = 80;

    public bool stopGame;

    private float totalTime;

    public AsteroidsSpawn(Texture2D[] _asteroidTextures, Texture2D[] _tailTextures)
    {
        asteroids = new Queue<Asteroid>();
        asteroidTextures = _asteroidTextures;
        tailTextures = _tailTextures;
        stopGame = false;
        totalTime = 0;
    }

    public void StopAsteroids()
    {
        stopGame = true;
    }

    public void beginAsteroids(GameTime gametime)
    {
        if (stopGame)
            return;

        totalTime += (float)gametime.ElapsedGameTime.TotalSeconds;
        int difficulty = Math.Clamp((int)totalTime, 1, 7);

        int currentCount = asteroids.Count;

        for (int i = 0; i < currentCount; i++)
        {
            Asteroid asteroid = asteroids.Dequeue();
            asteroid.Animate(gametime);

            if (asteroid.isAlive)
            {
                asteroids.Enqueue(asteroid);
            }
        }

        while (asteroids.Count < difficulty)
        {
            spawnRandom(difficulty);
        }
    }

    public void DrawAsteroids(SpriteBatch spriteBatch)
    {
        foreach (Asteroid asteroid in asteroids)
        {
            asteroid.Draw(spriteBatch);
        }
    }

    public void ResetAll()
    {
        asteroids.Clear();
        totalTime = 0f;
        stopGame = false;
    }

    public void spawnRandom(int difficulty)
    {
        if (asteroids.Count < difficulty)
        {
            int index = Random.Shared.Next(asteroidTextures.Length);

            Texture2D chosenBodyTexture = asteroidTextures[index];
            Texture2D chosenTailTexture = tailTextures[index];

            int sizeInt = Random.Shared.Next(1, 20);

            Vector2 spawn = GetRandomBorderPosition();
            Vector2 target = new Vector2(Random.Shared.Next(100, 900), Random.Shared.Next(100, 700));
            bool curved = Random.Shared.Next(2) == 0;

            asteroids.Enqueue(new Asteroid(
                chosenBodyTexture,
                chosenTailTexture,
                    1f + sizeInt * 0.3f,
                spawn,
                100f/sizeInt,
                target,
                curved
            ));
        }
    }
    

    private Vector2 GetRandomBorderPosition()
    {
        int side = Random.Shared.Next(4);

        switch (side)
        {
            case 0:
                return new Vector2(-spawnPadding, Random.Shared.Next(0, screenHeight));

            case 1:
                return new Vector2(screenWidth + spawnPadding, Random.Shared.Next(0, screenHeight));

            case 2:
                return new Vector2(Random.Shared.Next(0, screenWidth), -spawnPadding);

            default:
                return new Vector2(Random.Shared.Next(0, screenWidth), screenHeight + spawnPadding);
        }
    }
}