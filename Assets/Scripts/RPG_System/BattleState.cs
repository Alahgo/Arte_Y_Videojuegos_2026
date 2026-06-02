using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }

public class BattleManager : MonoBehaviour
{
    public BattleState state;

    public BattleUnit playerUnit;
    public BattleUnit enemyUnit;

    void Start()
    {
        state = BattleState.START;
        StartCoroutine(SetupBattle());
    }

    IEnumerator SetupBattle()
    {
        yield return new WaitForSeconds(1f);

        PlayerTurn();
    }

    void PlayerTurn()
    {
        state = BattleState.PLAYERTURN;
        Debug.Log("Es el turno del jugador. Elige una acción.");
        // Aquí se activaría la UI de botones para el jugador
    }

    public void OnAttackButton()
    {
        if (state != BattleState.PLAYERTURN) return;
        StartCoroutine(PlayerAttack());
    }

    public void OnDefendButton()
    {
        if (state != BattleState.PLAYERTURN) return;
        StartCoroutine(PlayerDefend());
    }

    public void OnSkillButton(SkillData skill)
    {
        if (state != BattleState.PLAYERTURN) return;
        StartCoroutine(PlayerUseSkill(skill));
    }

    IEnumerator PlayerAttack()
    {
        // El jugador ataca al enemigo
        playerUnit.AttackTarget(enemyUnit);
        yield return new WaitForSeconds(1f);

        CheckBattleStatus();
    }

    IEnumerator PlayerDefend()
    {
        playerUnit.Defend();
        yield return new WaitForSeconds(1f);

        state = BattleState.ENEMYTURN;
        StartCoroutine(EnemyTurn());
    }

    IEnumerator PlayerUseSkill(SkillData skill)
    {
        if (playerUnit.currentMana >= skill.manaCost)
        {
            playerUnit.UseMana(skill.manaCost);
            skill.Execute(playerUnit, enemyUnit);
        }
        else
        {
            Debug.Log("¡No hay suficiente maná!");
            yield break; // Permite al jugador elegir otra cosa
        }

        yield return new WaitForSeconds(1f);
        CheckBattleStatus();
    }

    IEnumerator EnemyTurn()
    {
        Debug.Log("Turno del enemigo...");
        yield return new WaitForSeconds(1f);

        // IA enemiga simple: Ataca si el jugador no está defendido, o aleatorio
        enemyUnit.AttackTarget(playerUnit);
        yield return new WaitForSeconds(1f);

        CheckBattleStatus();
    }

    void CheckBattleStatus()
    {
        if (enemyUnit.isDead)
        {
            state = BattleState.WON;
            EndBattle();
        }
        else if (playerUnit.isDead)
        {
            state = BattleState.LOST;
            EndBattle();
        }
        else
        {
            // Resetear estados temporales (como la defensa) antes de cambiar de turno
            if (state == BattleState.PLAYERTURN)
            {
                state = BattleState.ENEMYTURN;
                StartCoroutine(EnemyTurn());
            }
            else
            {
                playerUnit.ResetDefense(); // Quitar escudo al empezar su turno
                PlayerTurn();
            }
        }
    }

    void EndBattle()
    {
        if (state == BattleState.WON) Debug.Log("¡Ganaste la batalla!");
        else if (state == BattleState.LOST) Debug.Log("Fuiste derrotado...");
    }
}

