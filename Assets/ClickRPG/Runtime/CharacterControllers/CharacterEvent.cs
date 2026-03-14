namespace ClickRPG.CharacterControllers
{
    public static class CharacterEvent
    {
        public enum EventType
        {
            Died,
            DamageToken,
        }

        public readonly struct Died
        {
        }

        public readonly struct DamageToken
        {
            public int Damage { get; }

            public DamageToken(int damage)
            {
                Damage = damage;
            }
        }
    }
}
