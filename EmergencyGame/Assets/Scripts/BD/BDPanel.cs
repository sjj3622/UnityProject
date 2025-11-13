using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BDPanel : MonoBehaviour
{
    
   public void AgainClick()
    {
        BDgpManager.gameState = "BDStart";
        SceneManager.LoadScene("BleedingGamepalying");
    }

    public void ExitClick()
    {
        SceneManager.LoadScene("Bleeding");
    }

}
