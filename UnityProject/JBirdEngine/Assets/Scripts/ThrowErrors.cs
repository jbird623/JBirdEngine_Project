using UnityEngine;
using System.Collections;

public class ThrowErrors : MonoBehaviour {

    void Start () {
        Debug.LogError("Deleting System32.");
        Debug.LogWarning("Warning: Universe currently imploding. Please advise.");
        Debug.LogError("Error: Divide by 0. Are you trying to kill us all?");
        Debug.LogError("This error message doesn't actually indicate any errors, it just wants attention.");
        Debug.LogWarning("No one actually likes you. I'm sorry. Do you want a hug?");
        Debug.LogWarning("Everyone knows your secret. Run.");
        Debug.LogError("I'm out of witty error messages.");
    }

}
