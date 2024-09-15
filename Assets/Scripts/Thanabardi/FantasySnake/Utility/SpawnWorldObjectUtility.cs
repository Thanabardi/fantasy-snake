using System;
using System.Collections.Generic;
using System.Linq;
using Thanabardi.FantasySnake.Core.FantasySnakeSO;
using Thanabardi.FantasySnake.Core.GameCharacter;
using Thanabardi.FantasySnake.Core.GameWorld;
using Thanabardi.FantasySnake.Core.System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Thanabardi.FantasySnake.Utility
{
    public class SpawnWorldObjectUtility : MonoBehaviour
    {

        [SerializeField, Min(1)]
        int initHeroSpawnNumber = 1;
        [SerializeField, Min(1)]
        int initMonsterSpawnNumber = 1;

        [SerializeField]
        private GameObject[] _obstraclePrefabs;

        [SerializeField]
        private Character[] _characterPrefabs;

        [SerializeField]
        private SpawnChance[] _spawnChances;

        private GridManager _gridManager;

        private Dictionary<CharacterClassSO, Dictionary<Type, List<Character>>> _characterClassDict;
        private CharacterClassSO[] _characterClasses;
        private float _maxClassSpawnProb = 0;
        private float _maxSpawnChances = 0;

        private void Awake()
        {
            _gridManager = FindObjectOfType<GridManager>();
            InitializeCharacterDict();

            _characterClasses = _characterClassDict.Keys.OrderBy(c => c.SpawnRate).ToArray();
            _maxClassSpawnProb = _characterClasses.Last()?.SpawnRate ?? 0;

            _spawnChances = _spawnChances.OrderBy(c => c.Chance).ToArray();
            _maxSpawnChances = _spawnChances.Last()?.Chance ?? 0;
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

        private void Start()
        {
            for (int i = 0; i < initHeroSpawnNumber; i++)
            {
                SpawnCharacter(typeof(Hero));
            }
            for (int i = 0; i < initMonsterSpawnNumber; i++)
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
                    Debug.Log(spawnChance.SpawnNumber);
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
            gridTile ??= _gridManager.GetRandomEmptyTile();
            float randomVar = Random.Range(0f, 1f);
            foreach (CharacterClassSO characterclass in _characterClasses)
            {
                if (randomVar <= characterclass.SpawnRate / _maxClassSpawnProb)
                {
                    List<Character> characterPrefabs = _characterClassDict[characterclass][type];
                    Character characterPrefab = characterPrefabs[Random.Range(0, characterPrefabs.Count)];
                    Character character = Instantiate(characterPrefab, gridTile.transform.position, Quaternion.identity);
                    _gridManager.PlaceWorldItem(character, gridTile);
                    return character;
                }
            }
            return null;
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