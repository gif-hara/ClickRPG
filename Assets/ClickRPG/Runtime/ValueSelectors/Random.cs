using System;
using UnityEngine;

namespace ClickRPG.ValueSelectors
{
    [Serializable]
    public abstract class Random<T> : IValueSelector<T>
    {
        [SerializeField]
        protected T min = default!;

        [SerializeField]
        protected T max = default!;

        public abstract T Value { get; }
    }

    [Serializable]
    public sealed class RandomInt : Random<int>
    {
        public override int Value => UnityEngine.Random.Range(min, max + 1);
    }

    [Serializable]
    public sealed class RandomFloat : Random<float>
    {
        public override float Value => UnityEngine.Random.Range(min, max);
    }
}
