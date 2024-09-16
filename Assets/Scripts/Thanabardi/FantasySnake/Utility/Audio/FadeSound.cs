using System;
using System.Collections;
using UnityEngine;

namespace Thanabardi.FantasySnake.Utility.Audio
{
    public static class FadeSound
    {
        private const int _LOG_FADE_STEP = 100;
        private const int _LOG_ROLL_OFF_SCALE = 1;
        private const int _LOG_ROLL_OFF_MIN_DISTANCE = 1;

        public static IEnumerator Fade(AudioSource audioSource, float targetIntensity, int step = _LOG_FADE_STEP, Action callback = null)
        {
            float volumeLevel = audioSource.volume;
            float newVolume, level;
            //logarithmically increasing or decreasing volume
            for (int i = Mathf.CeilToInt(step / 10f); i <= step; i++)
            {
                level = (10f / step) * i;
                if (targetIntensity > volumeLevel)
                {
                    // increse volume
                    newVolume = volumeLevel + MathF.Log10(level);
                    if (newVolume >= targetIntensity) { break; }
                }
                else if (targetIntensity < volumeLevel)
                {
                    // decreas volume
                    newVolume = volumeLevel - MathF.Log10(level);
                    if (newVolume <= targetIntensity) { break; }
                }
                else { break; }
                audioSource.volume = newVolume;
                yield return new WaitForFixedUpdate();
            }
            audioSource.volume = targetIntensity;
            callback?.Invoke();
        }

        public static float LogarithmicRollOff(float distance)
        {
            // return the audio amplitude base on distance
            return Mathf.Clamp01(_LOG_ROLL_OFF_MIN_DISTANCE * (1 / (1 + _LOG_ROLL_OFF_SCALE * (Mathf.Abs(distance) - 1))));
        }
    }
}