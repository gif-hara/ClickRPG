using ClickRPG.CharacterControllers.Brains;
using UnityEngine;

namespace ClickRPG.CharacterControllers
{
    public sealed class Character : MonoBehaviour
    {
        [SerializeReference, SubclassSelector]
        private ICharacterBrain brain = null!;

        private readonly CharacterBrainController brainController = new();

        void Awake()
        {
            brainController.Setup(this, brain);
        }
    }
}
