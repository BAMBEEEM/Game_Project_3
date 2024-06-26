﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Game_Project_3.Misc;

namespace Game_Project_3.ParticleManagement
{

    /// <summary>
    /// A class representing Inner Particle Effect for the tidal wave, Variation #2
    /// </summary>
    public class InnerWaveParticlesTwo : ParticleSystem
    {
        private Rectangle _source;
        private float _lastSpawnTime;
        private float _spawnRate;
        public Vector2 BigWavePosition;

        public InnerWaveParticlesTwo(Game game, Rectangle source) : base(game, 25)
        {
            _source = source;
            _spawnRate = 1; // spawn rate in seconds (NOT MILLISECOND)
        }

        protected override void InitializeConstants()
        {
            textureFilename = "wave4";
            minNumParticles = 1;
            maxNumParticles = 1;
            blendState = BlendState.Additive;
            DrawOrder = AdditiveBlendDrawOrder;
        }

        protected override void InitializeParticle(ref Particle p, Vector2 where)
        {
            Vector2 direction = new Vector2(RandomHelper.NextFloat(0.3f, DifficultySettings.InnerWaveParticleTwoDirectionX), -1);
            float velocity;
            Vector2 acceleration;
            
                velocity = 50;
                acceleration = new Vector2(DifficultySettings.InnerWaveParticleTwoAccelerationX, RandomHelper.NextFloat(-85, -75));

            p.Initialize(
                (direction * velocity)+BigWavePosition,
                direction * velocity,
                acceleration,
                Color.Black,
                scale: RandomHelper.NextFloat(0.5f, 0.7f),
                rotation: RandomHelper.NextFloat(5.9f, 6.1f),
                angularVelocity: 0.1f,
                lifetime: 2.8f

            );
        }

        protected override void UpdateParticle(ref Particle particle, float dt)
        {
            base.UpdateParticle(ref particle, dt);

            if (BigWavePosition.X < 12560)
                particle.Position += new Vector2(DifficultySettings.WaveSpeed, 0)*2;
            else
                particle.Position += new Vector2(DifficultySettings.WaveSpeed, 0);

            float normalizedLifetime = particle.TimeSinceStart / particle.Lifetime;

            float alpha = 4 * (normalizedLifetime) * (1 - normalizedLifetime);
            particle.Color = Color.AliceBlue * alpha;

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            float currentTime = (float)gameTime.TotalGameTime.TotalSeconds;
            if (currentTime - _lastSpawnTime > _spawnRate)
            {
                AddParticles(_source);
                _lastSpawnTime = currentTime;

                _spawnRate = RandomHelper.NextFloat(1.5f, 2f);
            }
        }
    }
}
