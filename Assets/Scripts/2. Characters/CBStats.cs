using UnityEngine;

//script per le statistiche e i lvl del player
public class CBStats : MonoBehaviour
{
    [Header("Cavaliere Bianco Stats")]

    //il lvl del player e suoi punti esperienza
    public int cbLVL = 1;
    [SerializeField] private int cbCurrentEXP = 0;
    public int cbToNextLVL = 100;

    //la sua forza (quanti danni fa ai nemici, quali armi può usare)
    [SerializeField] private int cbStrenght = 1;
    [SerializeField] private float cbDMG = 10;
    //la sua difesa (quanto vengono minimizzati i danni fisici e magici dei nemici)
    [SerializeField] private int cbDefense = 1;
    [SerializeField] private float cbPhysicalDEF = 10;
    [SerializeField] private float cbMagicalDEF = 10;
    //la sua destrezza (quanto è veloce quando corre, quando attacca e quando schiva)
    [SerializeField] private int cbDexterity = 1;
    [SerializeField] private float cbSpeed = 10;
    [SerializeField] private float cbAttackSpeed = 1;
    [SerializeField] private float cbDodgeSpeed = 1;
    //la sua costituzione (quanta vita ha, quanta stamina ha)
    [SerializeField] private int cbSConstitution = 1;
    public int cbCurrentHP = 50;
    public int cbMaxHP = 50;
    [SerializeField] private int cbCurrentSTA = 100;
    public int cbMaxSTA = 100;
}
