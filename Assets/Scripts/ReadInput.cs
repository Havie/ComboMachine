using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//namespace Animancer.Examples { };

public class ReadInput : MonoBehaviour
{
    [SerializeField] private int _combo;

    private const int _COMBOMAX_N =7;

    private Animancer.Examples.Events.AnimancerInput _animancer;
    private ActionCharacter _ac;
    private CharacterState _state;


    // Start is called before the first frame update
    void Start()
    {
        SetUpAnimancer();
        SetUpActionChar();
        SetUpState();
    }
    private void SetUpAnimancer()
    {
        _animancer = this.GetComponent<Animancer.Examples.Events.AnimancerInput>();

        if (_animancer == null)
            Debug.LogError("Cant find Animancer on " + this.gameObject);
    }
    private void SetUpActionChar()
    {
        _ac = this.GetComponent<ActionCharacter>();
        if (_ac == null)
            Debug.LogError("Cant find Action Character");
    }
    private void SetUpState()
    {
        _state = this.GetComponent<CharacterState>();
        if (_state == null)
            Debug.LogError("Cant Find Char State for : " + this.gameObject);

    }
    // Update is called once per frame
    void Update()
    {
        ReadMovement();

        if (ReadNormal())
            HandleNormal();
        else if (ReadStrong())
            HandleStrong();
    }
    public void ClearCombo() { _combo = 0; }
    private void ReadMovement()
    {
        float hori = Input.GetAxis("Horizontal");
        float vert = Input.GetAxis("Vertical");
        if (_ac)
         _ac.PlayerMovement4(hori, vert, _state.CheckCommittment());


        if ((Mathf.Abs(hori) > 1e-5 || Mathf.Abs(vert) > 1e-5))
        {
           if(_state.CanMove())
                UpdateAnimancer(0); // move
        }
        else
        {
            if(_state.CanMove())
                UpdateAnimancer(-1); // idle
        }
    }
    private bool ReadNormal()
    {
        return (Input.GetMouseButtonDown(0) || (Input.GetKeyUp(KeyCode.JoystickButton2)));
    }
    private bool ReadStrong()
    {
        return (Input.GetMouseButtonDown(1) || (Input.GetKeyUp(KeyCode.JoystickButton3)));
    }

    private void HandleNormal()
    {

        if (_state.CanAttack())
        {
            if (_combo < _COMBOMAX_N)
            {
               UpdateAnimancer(++_combo);
               _state.setState(CharacterState.eSTATE.ATTACKING);
               _state.CloseWindow();
            }
        }
    }
    private void HandleStrong()
    {

        if (_state.CanAttack())
        {
            if (_combo < _COMBOMAX_N+10) // meh?
            {
                UpdateAnimancer(_combo += 10);
                _state.setState(CharacterState.eSTATE.ATTACKING);
                _state.CloseWindow();
            }
        }
    }
    private void UpdateAnimancer(int id)
    {
        if (_animancer)
            _animancer.PlayAnim(id);
    }
}
