using UnityEngine;
using Random = UnityEngine.Random;

namespace TarodevController
{
    public class PlayerAnimator : MonoBehaviour
    {


        [SerializeField] private Animator _anim;
        private IPlayerController _player;

        private Animation currentAnimation = Animation.Idle;

        void Awake() => _player = GetComponentInParent<IPlayerController>();

        void Update()
        {
            if (_player == null) return;
            if (Time.timeScale == 0) return;

            if (_player.Input.X != 0) transform.localScale = new Vector3(_player.Input.X > 0 ? 1 : -1, 1, 1);

            if (_player.JumpingThisFrame)
            {
                SetAnimation(Animation.Jump);
                return;
                //_anim.SetTrigger("Jump");
            }

            if (_player.Grounded && (_player.Input.X > 0 || _player.Input.X < 0))
            {
                SetAnimation(Animation.Run);
                //_anim.SetTrigger("Run");
            }
            if (_player.Grounded && (_player.Input.X == 0))
            {
                SetAnimation(Animation.Idle);
                //_anim.SetTrigger("Idle");
            }

            

            if (!_player.Grounded && !_player.LandingThisFrame && _player.RawMovement.y < 0)
            {
                SetAnimation(Animation.Fall);
                //_anim.SetTrigger("Fall");
            }
            if (!_player.Grounded && _player.LandingThisFrame)
            {
                SetAnimation(Animation.Land);
                //_anim.SetTrigger("Fall");
            }
        }

        private void SetAnimation(Animation anim)
        {
            if (currentAnimation != anim)
            {
                switch (anim)
                {
                    case Animation.Idle:
                        _anim.SetTrigger("Idle");
                        break;
                    case Animation.Run:
                        _anim.SetTrigger("Run");
                        break;
                    case Animation.Jump:
                        _anim.SetTrigger("Jump");
                        break;
                    case Animation.Fall:
                        _anim.SetTrigger("Fall");
                        break;
                    case Animation.Land:
                        _anim.SetTrigger("Land");
                        break;
                    default:
                        break;
                }
                currentAnimation = anim;
            }

        }

    }

    public enum Animation
    {
        Idle,
        Run,
        Jump,
        Fall,
        Land
    }
}