using UnityEngine;
using Random = UnityEngine.Random;

namespace TarodevController {
    public class PlayerAnimator : MonoBehaviour {

        [SerializeField] private Animator _anim;
        private IPlayerController _player;

        void Awake() => _player = GetComponentInParent<IPlayerController>();

        void Update() {
            if (_player == null) return;

            if (_player.Input.X != 0) transform.localScale = new Vector3(_player.Input.X > 0 ? 1 : -1, 1, 1);

            if (_player.JumpingThisFrame) 
            {
                _anim.SetTrigger("Jump");
            }
            if(_player.Input.X > 0 || _player.Input.X < 0)
            {
                _anim.SetTrigger("Run");
            }
            if(_player.Input.X == 0)
            {
                _anim.SetTrigger("Idle");
            }
        } 
    }
}