using Code.Common.Entity;
using Code.Infrastructure.Identifiers;
using UnityEngine;
using Zenject;

namespace Code.Infrastructure.View
{
    public class SelfInitializeEntityView : MonoBehaviour
    {
        [SerializeField] private EntityBehaviour _entityBehaviour;

        private IIdentifierService _identifierService;

        [Inject]
        private void Construct(IIdentifierService identifierService)
        {
            _identifierService = identifierService;
        }

        private void Awake()
        {
            GameEntity entity = CreateEntity.Empty();
            entity.AddId(_identifierService.Next());

            _entityBehaviour.SetEntity(entity);
        }
    }
}