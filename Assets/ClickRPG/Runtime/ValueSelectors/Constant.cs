using System;
using UnityEngine;

namespace ClickRPG.ValueSelectors
{
    [Serializable]
    public sealed class Constant<T> : IValueSelector<T>
    {
        [SerializeField]
        private T value = default!;

        public T Value => value;
    }
}
