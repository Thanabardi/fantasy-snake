using UnityEngine;
namespace Thanabardi.FantasySnake.Core.GameWorld.GamePotion
{
    public abstract class Potion : WorldItem
    {
        [SerializeField]
        private Color _potionColor = Color.white;
        public Color PotionColor => _potionColor;

        [SerializeField, Min(0)]
        private float _spawnRate;
        public float SpawnRate => _spawnRate;

        private const string _EMISSION_COLOR = "_EmissionColor";
        private const int _EMISSION_INTENSITY = 4;

        private void Start()
        {
            MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();
            if (TryGetComponent(out Light light))
            {
                light.color = _potionColor;
            }
            Material material = meshRenderer.material;
            material.SetColor(_EMISSION_COLOR, _potionColor * Mathf.Pow(2, _EMISSION_INTENSITY));
        }
    }
}