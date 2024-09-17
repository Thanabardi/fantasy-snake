using System;
using System.Collections;
using System.Collections.Generic;
using Thanabardi.FantasySnake.Core.GameSystem;
using Thanabardi.FantasySnake.Core.GameWorld.GameCharacter;
using Thanabardi.FantasySnake.Core.NewInputSystem;
using UnityEngine;

namespace Thanabardi.FantasySnake.Utility.Animation
{
    public class CharacterAnimationUtility : MonoBehaviour
    {
        private Animator _animator;

        private Character _character;
        private Queue<IEnumerator> _animationQueue;
        private string _currentAnimationState;
        private bool _isPlaying;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _character = GetComponent<Character>();
            _animationQueue = new();
        }

        private void OnEnable()
        {
            _character.OnGetHit += OnGetHitHandler;
            _character.OnAttack += OnAttackHandler;
            _character.OnDied += OnDiedHandler;
        }

        private void OnDisable()
        {
            _character.OnGetHit -= OnGetHitHandler;
            _character.OnAttack -= OnAttackHandler;
            _character.OnDied -= OnDiedHandler;
        }

        private void OnAttackHandler()
        {
            PlayAnimationQueue(TriggerActionAnimation(_animator, CharacterState.ATTACK, _currentAnimationState, () =>
            {
                SoundManager.Instance.RandomPlaySoundOneshot(SoundManager.Instance.HitSFX);
            }));
        }

        private void OnGetHitHandler(int damage)
        {
            if (damage > 0)
            {
                PlayAnimationQueue(TriggerActionAnimation(_animator, CharacterState.HURT, _currentAnimationState));
            }
        }

        private void OnDiedHandler()
        {
            // move character slightly to the front
            Vector3 position = _character.transform.position;
            _character.transform.position = new Vector3(position.x, position.y, position.z - 0.01f);
            PlayAnimationQueue(TriggerActionAnimation(_animator, CharacterState.DEAD, _currentAnimationState, () =>
            {
                SoundManager.Instance.RandomPlaySoundOneshot(SoundManager.Instance.DeadSFX);
            }, () =>
            {
                Destroy(_character.gameObject);
            }));
        }

        private void StopAnimation(bool isStop)
        {
            _animator.enabled = !isStop;
        }

        private void ChangeAnimationState(Animator animator, string newState, ref string currentState)
        {
            if (currentState == newState) { return; }
            animator.Play(newState);
            currentState = newState;
        }

        private IEnumerator TriggerActionAnimation(Animator animator, string newState, string currentState, Action OnPlay = null, Action OnFinished = null)
        {
            OnPlay?.Invoke();
            ChangeAnimationState(animator, newState, ref currentState);
            float animationLenght = animator.GetCurrentAnimatorStateInfo(0).length;
            _isPlaying = true;
            InputManager.Instance.InputMaster.Gameplay.Disable();
            yield return new WaitForSeconds(animationLenght);
            InputManager.Instance.InputMaster.Gameplay.Enable();
            _isPlaying = false;
            ChangeAnimationState(animator, CharacterState.IDLE, ref currentState);
            PlayAnimationQueue();
            OnFinished?.Invoke();
        }

        private void PlayAnimationQueue(IEnumerator playIEnumerator = null)
        {
            // add new animation 
            if (playIEnumerator != null) { _animationQueue.Enqueue(playIEnumerator); }

            // start animation in queue
            if (!_isPlaying && _animationQueue.Count > 0)
            {
                StartCoroutine(_animationQueue.Dequeue());
            }
        }

        protected static class CharacterState
        {
            public const string IDLE = "Idle";
            public const string ATTACK = "Attack";
            public const string DEAD = "Dead";
            public const string HURT = "Hurt";
        }
    }
}