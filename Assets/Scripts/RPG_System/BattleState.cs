using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }

public class BattleManager : MonoBehaviour
{
    public BattleState state;

    public BattleUnit playerUnit;
    public BattleUnit enemyUnit;
    public TextMeshProUGUI vidaActualPlayer;
    public TextMeshProUGUI manaActualPlayer;
    public GameObject ventanaAcciones;
    public GameObject flechaSelecEnemy;
    public GameObject[] flechas;
    public GameObject btnReset;
    public Animator _playerAnimator;
    public Animator _enemyAnimator;
    public LectorDeTXT lectorTxt;

    private int index = 0;
    private bool _isActing = false;
    private bool _ejeEnUso = false;
    private string textoEsperado = "Poder: 1\nDefensa: 1";



    void Start()
    {
        
        vidaActualPlayer.text = playerUnit.currentHp.ToString();
        manaActualPlayer.text = playerUnit.currentMana.ToString();
        state = BattleState.START;
        StartCoroutine(SetupBattle());
        
    }

    void Update()
    {
        string contenidoActual = File.ReadAllText(PlayerPrefs.GetString("RutaArchivoRPG"));
        if (textoEsperado != contenidoActual)
        {
            lectorTxt.CargarDatosDesdeTxt();
        }
        
        if (_isActing && state == BattleState.PLAYERTURN)
        {
            float valorVertical = Input.GetAxisRaw("Vertical");

            if (valorVertical != 0)
            {
                if (!_ejeEnUso)
                {
                    if (valorVertical > 0) EjecutarAccionArriba();
                    else if (valorVertical < 0) EjecutarAccionAbajo();

                    _ejeEnUso = true;
                }
            }
            else
            {
                _ejeEnUso = false;
            }

           
            if (Input.GetButtonDown("Submit"))
            {
                _isActing = false;
                ventanaAcciones.SetActive(false);

                
                if (index == 0)
                {
                    StartCoroutine(PlayerAttack());
                }
                else if (index == 1)
                {
                    StartCoroutine(PlayerUseSkill());
                }
                else
                {
                    StartCoroutine(PlayerDefend());
                }
             

                index = 0;
                MostrarFlechas();
            }
        }
       
        else if (!_isActing && state == BattleState.PLAYERTURN)
        {
            if (Input.GetButtonDown("Submit"))
            {
                ventanaAcciones.SetActive(true);
                _isActing = true;
            }
        }
    }

    IEnumerator SetupBattle()
    {
        yield return new WaitForSeconds(1f);

        PlayerTurn();
    }

    void PlayerTurn()
    {
        flechaSelecEnemy.SetActive(true);
        state = BattleState.PLAYERTURN;
        Debug.Log("Es el turno del jugador. Elige una acción.");
        
    }


    IEnumerator PlayerAttack()
    {
        _playerAnimator.SetTrigger("Atack1");
        playerUnit.AttackTarget(enemyUnit);
        _enemyAnimator.SetTrigger("getDamage");
        yield return new WaitForSeconds(2f);

        CheckBattleStatus();
    }

    IEnumerator PlayerDefend()
    {
        playerUnit.Defend();
        yield return new WaitForSeconds(2f);

        state = BattleState.ENEMYTURN;
        StartCoroutine(EnemyTurn());
    }

    IEnumerator PlayerUseSkill()
    {
        if (playerUnit.currentMana >= 5)
        {
            playerUnit.UseMana(5);
            _playerAnimator.SetTrigger("Atack2");
            playerUnit.UseSkill(enemyUnit);
            yield return new WaitForSeconds(1.05f);
            _enemyAnimator.SetTrigger("getDamage");
            manaActualPlayer.text = playerUnit.currentMana.ToString();
        }
        else
        {
            Debug.Log("No hay suficiente maná");
            yield break; 
        }

        yield return new WaitForSeconds(1f);
        CheckBattleStatus();
    }

    IEnumerator EnemyTurn()
    {
        ventanaAcciones.SetActive(false);
        flechaSelecEnemy.SetActive(false);
        Debug.Log("Turno del enemigo");
        yield return new WaitForSeconds(1f);
       

        int currentLife = playerUnit.currentHp;
        _enemyAnimator.SetTrigger("attack");
        enemyUnit.AttackTarget(playerUnit);
        


        if (currentLife != playerUnit.currentHp)
        {
            yield return new WaitForSeconds(1.10f);
            _playerAnimator.SetTrigger("getDamage");
            vidaActualPlayer.text = playerUnit.currentHp.ToString();
        }

        yield return new WaitForSeconds(2f);

        CheckBattleStatus();
    }

    void CheckBattleStatus()
    {
        if (enemyUnit.isDead)
        {
            _enemyAnimator.SetTrigger("die");
            state = BattleState.WON;
            EndBattle();
        }
        else if (playerUnit.isDead)
        {
            _playerAnimator.SetTrigger("die");
            state = BattleState.LOST;
            EndBattle();
        }
        else
        {
            
            if (state == BattleState.PLAYERTURN)
            {
                state = BattleState.ENEMYTURN;
                StartCoroutine(EnemyTurn());
            }
            else
            {
                playerUnit.ResetDefense();
                PlayerTurn();
            }
        }
    }

    void EndBattle()
    {
        if (state == BattleState.WON) Debug.Log("Ganaste la batalla");
        else if (state == BattleState.LOST) {
            btnReset.SetActive(true);
        }
    }

    void EjecutarAccionArriba()
    {
        index--;
        if (index < 0) index = flechas.Length - 1;
        MostrarFlechas();


    }
    void EjecutarAccionAbajo()
    {
        index++;
        if (index > flechas.Length - 1) index = 0;
        MostrarFlechas();

    }

    void MostrarFlechas()
    {
        for (int i = 0; i < flechas.Length; i++)
        {
            if (i != index) flechas[i].SetActive(false);
            else flechas[i].SetActive(true);
        }
    }

    public void ResetearCombate()
    {
        
        btnReset.SetActive(false);

       
        int escenaActual = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(escenaActual);
    }
}

