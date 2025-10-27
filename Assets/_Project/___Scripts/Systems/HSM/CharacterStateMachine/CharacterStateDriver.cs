using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace HSM
{

    public class CharacterStateDriver : MonoBehaviour
    {
        private StateMachine _machine;
        private State _root;

        [Header("Context"), Space(10)]
        public CharacterContext Context = new CharacterContext();
        
        [Header("Character Attributes"), Space(10)]
        [SerializeField] private Animator _characterAnimator;
        [SerializeField] private LayerMask _groundLayer;
        [SerializeField] private Rigidbody _rigidBody;
        [SerializeField] private Transform _groundCheck;
        [SerializeField] private float _groundCheckRadius = 0.2f; 
        [SerializeField] private bool _drawGizmos = true;
        
        private string _lastPath;

        private void OnDrawGizmosSelected()
        {
            if (_drawGizmos && _groundCheck != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(_groundCheck.position, _groundCheckRadius);
            }
        }

        private void Awake()
        {
            if (_rigidBody == null)
                Debug.LogError("PlayerStateDriver needs a RigidBody");
            _rigidBody.constraints = (RigidbodyConstraints)80;

            Context.Rigidbody = _rigidBody;
            Context.Animator = _characterAnimator;
            
            _root = new CharacterRootState(null, Context);
            StateMachineBuilder builder = new(_root);
            _machine = builder.Build();
        }
        
        private void Update()
        {
            Context.Move = new Vector3(InputManager.Instance.GetMoveDirection().x, 0f, InputManager.Instance.GetMoveDirection().y);
            Context.Grounded = Physics.CheckSphere(_groundCheck.position, _groundCheckRadius, _groundLayer);
            
            _machine.Tick(Time.deltaTime);
            
            string path = StatePath(_machine.Root.Leaf());
            if (path != _lastPath)
            {
                Debug.Log("State : " + path);
                _lastPath = path;
            }
        }

        private void FixedUpdate()
        {
            Vector3 bodyVelocity = _rigidBody.linearVelocity;
            bodyVelocity.x = Context.Velocity.x;
            bodyVelocity.z = Context.Velocity.z;
            _rigidBody.linearVelocity = bodyVelocity;
        }

        static string StatePath(State s)
        {
            return string.Join(" > ", s.PathToRoot().Reverse().Select(state => state.GetType().Name));
        }
    }
}
