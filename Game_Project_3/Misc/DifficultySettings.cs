using Game_Project_3.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game_Project_3.ParticleManagement;

namespace Game_Project_3.Misc
{
    public static class DifficultySettings
    {
        public static Difficulty SetDifficulty = Difficulty.Easy;

        public static float WaveSpeed = 0f;

        public static float CharSpeedWithSlow;

        public static float CharSpeedWithoutSlow;

        public static float StaminaRate;

        public static float StaminaDelay;

        public static int MudPerSection;

        public static float StaminaUsePerDash;

        public static float InnerWaveParticleTwoDirectionX;

        public static float InnerWaveParticleOneAccelerationX;

        public static float InnerWaveParticleTwoAccelerationX;

        public static float DrownTime;

        public static void InitializeDifficulty()
        {
            if (SetDifficulty == Difficulty.Easy)
            {
                MudPerSection = 2;

                WaveSpeed = 3.8f;
                CharSpeedWithSlow = 0.75f;

                CharSpeedWithoutSlow = 0.95f;

                StaminaDelay = 750;

                StaminaRate = 175;

                StaminaUsePerDash = 12.5f;

                DrownTime = 2000;
                InnerWaveParticleOneAccelerationX = -60;

                InnerWaveParticleTwoDirectionX = 0.8f;
                InnerWaveParticleTwoAccelerationX = -50;

            }
            else if (SetDifficulty == Difficulty.Normal)
            {
                MudPerSection = 3;

                WaveSpeed = 4.4f;
                CharSpeedWithSlow = 0.71f;

                CharSpeedWithoutSlow = 1.065f;

                StaminaDelay = 600;

                StaminaRate = 150;

                StaminaUsePerDash = 25;

                DrownTime = 1500;

                InnerWaveParticleOneAccelerationX = -50;

                InnerWaveParticleTwoDirectionX = 0.3f;
                InnerWaveParticleTwoAccelerationX = -60;
            }
            else
            {
                MudPerSection = 4;

                WaveSpeed = 5.3f;
                CharSpeedWithSlow = 0.65f;

                CharSpeedWithoutSlow = 1.14f;

                StaminaDelay = 500;

                StaminaRate = 125;

                StaminaUsePerDash = 25;

                DrownTime = 1250;

                InnerWaveParticleOneAccelerationX = -105;

                InnerWaveParticleTwoDirectionX = 0.3f;
                InnerWaveParticleTwoAccelerationX = -115;
            }
        }

    }
}
