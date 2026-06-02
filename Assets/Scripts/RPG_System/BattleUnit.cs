using UnityEngine;

public class BattleUnit : MonoBehaviour
{
    public string unitName;
    public int maxHp;
    public int currentHp;
    public int maxMana;
    public int currentMana;

    public int baseAttack;
    public int baseDefense;

    public bool isDead => currentHp <= 0;
    private bool isDefending = false;

    void Awake()
    {
        currentHp = maxHp;
        currentMana = maxMana;
    }

    public void AttackTarget(BattleUnit target)
    {
        Debug.Log($"{unitName} ataca a {target.unitName}!");
        target.TakeDamage(baseAttack);
    }

    public void UseSkill(BattleUnit target)
    {
        Debug.Log($"{unitName} ataca a {target.unitName}!");
        int extraDmg = baseAttack * 2;
       
        target.TakeDamage(baseAttack + extraDmg);
    }

    public void TakeDamage(int damage)
    {
        int finalDamage = damage - baseDefense;

        if (isDefending)
        {
            finalDamage /= 2; // Reduce el daño a la mitad si se defiende
            Debug.Log($"{unitName} está defendiéndose. ¡Daño mitigado!");
        }

        finalDamage = Mathf.Max(finalDamage, 1); // Al menos hace 1 de daño
        currentHp -= finalDamage;
        currentHp = Mathf.Max(currentHp, 0);

        Debug.Log($"{unitName} recibe {finalDamage} de daño. Vida restante: {currentHp}");
    }

    public void Defend()
    {
        isDefending = true;
        Debug.Log($"{unitName} se prepara para defenderse.");
    }

    public void ResetDefense()
    {
        isDefending = false;
    }

    public void UseMana(int amount)
    {
        currentMana -= amount;
    }

   
}
