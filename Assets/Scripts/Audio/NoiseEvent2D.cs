using System.Collections.Generic;
using UnityEngine;

namespace SchoolPanicRoguelike.Audio
{
    public class NoiseEvent2D
    {
        public Vector2 Position { get; private set; }
        public float Volume { get; private set; }
        public float Radius { get; private set; }
        public float ExpireTime { get; private set; }

        private static readonly List<NoiseEvent2D> ActiveEvents = new List<NoiseEvent2D>();

        private NoiseEvent2D(Vector2 position, float radius, float volume, float duration)
        {
            Position = position;
            Radius = radius;
            Volume = volume;
            ExpireTime = Time.time + duration;
        }

        public static void EmitNoise(Vector2 position, float radius, float volume = 1f, float duration = 1.5f)
        {
            CleanupExpired();
            ActiveEvents.Add(new NoiseEvent2D(position, radius, volume, duration));
        }

        public static IEnumerable<NoiseEvent2D> GetActiveEvents()
        {
            CleanupExpired();
            return ActiveEvents.ToArray();
        }

        private static void CleanupExpired()
        {
            float time = Time.time;
            ActiveEvents.RemoveAll(e => e.ExpireTime <= time);
        }
    }
}
