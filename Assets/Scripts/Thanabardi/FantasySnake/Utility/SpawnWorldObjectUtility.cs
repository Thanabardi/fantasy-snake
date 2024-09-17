using System;
using System.Collections.Generic;
using System.Linq;
using Thanabardi.FantasySnake.Core.FantasySnakeSO;
using Thanabardi.FantasySnake.Core.GameWorld.GameCharacter;
using Thanabardi.FantasySnake.Core.GameWorld;
using UnityEngine;
using Random = UnityEngine.Random;
using Thanabardi.FantasySnake.Core.GameWorld.GamePotion;
using Thanabardi.FantasySnake.Core.GameSystem;

namespace Thanabardi.FantasySnake.Utility
{
    public class SpawnWorldObjectUtility : MonoBehaviour
    {
        [Header("Spawn Configuration")]
        [SerializeField, Min(1)]
        private int _initHeroSpawnNumber = 1;
        [SerializeField, Min(1)]
        private int _initMonsterSpawnNumber = 1;
        [SerializeField]
        private SpawnChance[] _spawnChances;

        [Space(20)]
        [SerializeField]
        private Character[] _characterPrefabs;
        [SerializeField]
        private Potion[] _potionPrefabs;

        private GridManager _gridManager;

        private Dictionary<CharacterClassSO, Dictionary<Type, List<Character>>> _characterClassDict;
        private CharacterClassSO[] _characterClasses;
        private float _maxClassSpawnProb = 0;
        private float _maxPotionSpawnProb = 0;
        private float _maxSpawnChances = 0;

        private void Awake()
        {
            try
            {
                // load configuration with minimum spawn number of 1
                _initHeroSpawnNumber = Mathf.Max(1, GameConfig.ConfigData.InitHeroSpawnNumber);
                _initMonsterSpawnNumber = Mathf.Max(1, GameConfig.ConfigData.InitMonsterSpawnNumber);
                _spawnChances = GameConfig.ConfigData.SpawnChance;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
            _gridManager = FindObjectOfType<GridManager>();
            InitializeCharacterDict();
            InitializeSpawnRate();
        }

        private void Start()
        {
            for (int i = 0; i < _initHeroSpawnNumber; i++)
            {
                SpawnCharacter(typeof(Hero));
            }
            for (int i = 0; i < _initMonsterSpawnNumber; i++)
            {
                SpawnCharacter(typeof(Monster));
            }
        }

        public void SpawnCharacters(Type type)
        {
            float randomVar = Random.Range(0f, 1f);
            foreach (SpawnChance spawnChance in _spawnChances)
            {
                if (randomVar <= spawnChance.Chance / _maxSpawnChances)
                {
                    for (int i = 0; i < spawnChance.SpawnNumber; i++)
                    {
                        SpawnCharacter(type);
                    }
                    return;
                }
            }
        }

        public Character SpawnCharacter(Type type, GridTile gridTile = null)
        {
            if (!gridTile && !_gridManager.TryGetRandomEmptyTile(out gridTile))
            {
                return null;
            }
            float randomVar = Random.Range(0f, 1f);
            foreach (CharacterClassSO characterclass in _characterClasses)
            {
                if (randomVar <= characterclass.SpawnRate / _maxClassSpawnProb)
                {
                    List<Character> characterPrefabs = _characterClassDict[characterclass][type];
                    Character characterPrefab = characterPrefabs[Random.Range(0, characterPrefabs.Count)];
                    Character character = Instantiate(characterPrefab, gridTile.transform.position, Quaternion.identity);
                    _gridManager.AddItemOnTile(character, gridTile);
                    return character;
                }
            }
            return null;
        }

        public Potion SpawnPotion(GridTile gridTile = null)
        {
            if (!gridTile && !_gridManager.TryGetRandomEmptyTile(out gridTile))
            {
                return null;
            }
            float randomVar = Random.Range(0f, 1f);
            foreach (Potion potionPrefab in _potionPrefabs)
            {
                if (randomVar <= potionPrefab.SpawnRate / _maxPotionSpawnProb)
                {
                    Vector3 position = gridTile.transform.position;
                    Potion potion = Instantiate(potionPrefab, new Vector3(position.x, position.y + 0.1f, position.z), Quaternion.identity);
                    _gridManager.AddItemOnTile(potion, gridTile);
                    return potion;
                }
            }
            return null;
        }

        private void InitializeCharacterDict()
        {
            _characterClassDict = new();
            foreach (Character character in _characterPrefabs)
            {
                // check is the dictionary contains the character class
                if (_characterClassDict.TryGetValue(character.CharacterClass, out var characterDict))
                {
                    // check is character class contains character type
                    if (characterDict.TryGetValue(character.GetType(), out var characterList))
                    {
                        characterList.Add(character);
                    }
                    else { characterDict.Add(character.GetType(), new() { character }); }
                }
                else { _characterClassDict.Add(character.CharacterClass, new() { { character.GetType(), new() { character } } }); }
            }
        }

        private void InitializeSpawnRate()
        {
            // sort and get max rate for each spawn items
            _characterClasses = _characterClassDict.Keys.OrderBy(c => c.SpawnRate).ToArray();
            _maxClassSpawnProb = _characterClasses.Last()?.SpawnRate ?? 0;

            _potionPrefabs = _potionPrefabs.OrderBy(c => c.SpawnRate).ToArray();
            _maxPotionSpawnProb = _potionPrefabs.Last()?.SpawnRate ?? 0;

            _spawnChances = _spawnChances.OrderBy(c => c.Chance).ToArray();
            _maxSpawnChances = _spawnChances.Last()?.Chance ?? 0;
        }

        [Serializable]
        public class SpawnChance
        {
            [Min(0)]
            public float Chance;
            [Min(1)]
            public int SpawnNumber = 1;
        }
    }
}