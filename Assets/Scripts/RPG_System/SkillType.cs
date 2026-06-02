using UnityEngine;

public enum SkillType { Damage, Heal, Buff }

[CreateAssetMenu(fileName = "Nueva Habilidad", menuName = "RPG/Habilidad")]
public class SkillData : ScriptableObject
{
    public string skillName;
    public string description;
    public int manaCost;
    public int power;
    public SkillType type;

    public void Execute(BattleUnit user, BattleUnit target)
    {
        Debug.Log($"{user.unitName} usa {skillName}!");

        switch (type)
        {
            case SkillType.Damage:
                // Habilidad mágica/fuerte que ignora parte de la defensa
                int damage = user.baseAttack + power;
                target.TakeDamage(damage);
                break;

            case SkillType.Heal:
                // Cura al propio usuario
                user.Heal(power);
                break;

            case SkillType.Buff:
                // Aquí podrías alterar estadísticas temporalmente
                Debug.Log("Efecto de estado aplicado (No implementado en este esqueleto)");
                break;
        }
    }
}
