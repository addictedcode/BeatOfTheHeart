using UnityEngine;

public class Forte : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private int health = 4;
    [SerializeField] private int attackDamage = 5;
    [SerializeField] private int reflectDamage = 2;

    [Header("Super")]
    [SerializeField] private int attackAddSuper = 3;
    [SerializeField] private int reflectAddSuper = 10;
    [SerializeField] private int superMeterValue = 0;

    private readonly int maxSuperMeter = 100;

    #region Actions
    public void Move(int dir)
    {
        GameManager.Instance.TileManager.MoveToTile(dir);
        SFXManager.Instance.PlayOneShot("Jump");
    }

    public void Attack()
    {
        GameManager.Instance.Minotaur.TakeDamage(attackDamage);
        AddSuperMeterValue(attackAddSuper);
        SFXManager.Instance.PlayOneShot("Attack");
    }

    public void Reflect()
    {
        AddSuperMeterValue(reflectAddSuper);
        SFXManager.Instance.PlayOneShot("Reflect");
    }

    public void Stutter()
    {

    }
    #endregion

    #region Health
    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0) Death();

        SFXManager.Instance.PlayOneShot("Hurt");
    }

    private void Death()
    {
        GameManager.Instance.EndGame(true);
    }
    #endregion

    private void AddSuperMeterValue(int value)
    {
        superMeterValue = Mathf.Clamp(superMeterValue += value, 0, maxSuperMeter);
    }
}
