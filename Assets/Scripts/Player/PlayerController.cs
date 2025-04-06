using UnityEngine;

public abstract class PlayerController : MonoBehaviour
{
    public abstract bool IsMoving { get; }
    public abstract bool IsDashing { get; }  

}
