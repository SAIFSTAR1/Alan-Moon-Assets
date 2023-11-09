using UnityEngine;


public class Weapon : MonoBehaviour
{
    public string Type;
    public virtual void Attack(float dir){}
}