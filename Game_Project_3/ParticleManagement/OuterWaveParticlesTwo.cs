using System;
using System.Collections.Generic;
using Game_Project_3.Misc;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game_Project_3.ParticleManagement
{
    public class OuterWaveParticlesTwo : ParticleSystem
    {
        Rectangle _source;
        List<Vector2> _bezierControlPoints;

        private float _lastSpawnTime;
        private float _spawnRate;
        public Vector2 BigWavePosition;
        private Vector2 totalRandom;



        public OuterWaveParticlesTwo(Game game, Rectangle source) : base(game, 400)
        {
            _source = source;



            // Define the control points to match the wave shape
            _bezierControlPoints = new List<Vector2>
            {
                new Vector2(0, 740),                     // Start point
                new Vector2(40, 130),       // Control point (peak of the wave)
                new Vector2(400, 130)                  // End point
            };
        }

        protected override void InitializeConstants()
        {
            textureFilename = "wave2";
            minNumParticles = 3;
            maxNumParticles = 3;
        }

        protected override void InitializeParticle(ref Particle p, Vector2 where)
        {
            float t = 0;  // Random position along the Bézier curve
            Vector2 position = CalculateBezierPoint(t, _bezierControlPoints) + BigWavePosition;
            Vector2 velocity = CalculateBezierVelocity(t, _bezierControlPoints);

            p.Initialize(position, velocity, Vector2.Zero, Color.White,
                scale: RandomHelper.NextFloat(1f, 1.2f), lifetime: 6.8f);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            float currentTime = (float)gameTime.TotalGameTime.TotalSeconds;
            if (currentTime - _lastSpawnTime > _spawnRate)
            {
                AddParticles(_source);
                _lastSpawnTime = currentTime;

                _spawnRate = 0.1f;
            }
        }

        private Vector2 CalculateBezierPoint(float t, List<Vector2> controlPoints)
        {
            int n = controlPoints.Count - 1;
            Vector2 point = Vector2.Zero;

            for (int i = 0; i <= n; i++)
            {
                float binomialCoefficient = BinomialCoefficient(n, i);
                float oneMinusT = (float)Math.Pow(1 - t, n - i);
                float tPower = (float)Math.Pow(t, i);
                point += binomialCoefficient * oneMinusT * tPower * controlPoints[i];
            }

            return point;
        }

        private Vector2 CalculateBezierVelocity(float t, List<Vector2> controlPoints)
        {
            int n = controlPoints.Count - 1;
            Vector2 velocity = Vector2.Zero;

            for (int i = 0; i <= n; i++)
            {
                float binomialCoefficient = BinomialCoefficient(n - 1, i);
                float oneMinusT = (float)Math.Pow(1 - t, (n - 1) - i);
                float tPower = (float)Math.Pow(t, i);
                Vector2 pointDifference = i < n ? (controlPoints[i + 1] - controlPoints[i]) : Vector2.Zero;
                velocity += binomialCoefficient * oneMinusT * tPower * pointDifference;
            }

            return velocity;
        }

        private float BinomialCoefficient(int n, int k)
        {
            float result = 1;
            for (int i = 1; i <= k; i++)
            {
                result *= (n - (k - i)) / (float)i;
            }
            return result;
        }

        protected override void UpdateParticle(ref Particle particle, float dt)
        {
            base.UpdateParticle(ref particle, dt);
            totalRandom += new Vector2(RandomHelper.NextFloat(-0.01f, 0.01f), RandomHelper.NextFloat(-0.01f, 0.01f));
            float normalizedLifetime = particle.TimeSinceStart / (particle.Lifetime - 3.5f);
            float t = 1 * normalizedLifetime;  // Random position along the Bézier curve
            if (t < 1)
            {
                
                particle.Position = CalculateBezierPoint(t, _bezierControlPoints) + BigWavePosition + totalRandom;
                var temp = particle.Position;
                particle.Position = temp + totalRandom;
            }

            else
            {
                if (particle.TimeSinceStart < 3.32f)
                {
                    particle.Position = CalculateBezierPoint(t, _bezierControlPoints) + BigWavePosition + totalRandom;
                }
                totalRandom = Vector2.Zero;

                    particle.Position += new Vector2(2, RandomHelper.NextFloat(-0.6f, 0.8f));

                if (BigWavePosition.X != 12500)
                    particle.Position += new Vector2(DifficultySettings.WaveSpeed, 0);

                particle.Velocity = Vector2.Zero;

            }
            particle.Color = Color.White;

        }

        /*        public override void Draw(GameTime gameTime, )
                {
                    // tell sprite batch to begin, using the spriteBlendMode specified in
                    // initializeConstants
                    spriteBatch.Begin(blendState: blendState);

                    foreach (Particle p in particles)
                    {
                        // skip inactive particles
                        if (!p.Active)
                            continue;

                        spriteBatch.Draw(texture, p.Position, null, p.Color,
                            p.Rotation, origin, p.Scale, SpriteEffects.None, 0.0f);
                    }

                    spriteBatch.End();

                    base.Draw(gameTime);
                }*/
    }
}
