<<<<<<< HEAD
using System.Collections;
=======
ï»¿using System.Collections;
>>>>>>> 22ff9ed88032a66f3cc0fc3a068ec87347d487ea
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{

<<<<<<< HEAD
    public void StartClick() // °ÔÀÓ ½ºÅ¸Æ® ¹öÆ°
    {
        SceneManager.LoadScene("WorldMap");
    }

    public void ExitClick() // °ÔÀÓ Á¾·á ¹öÆ°
    {
        Application.Quit(); // À¯´ÏÆ¼ °ÔÀÓ Á¾·á
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // À¯´ÏÆ¼ ¿¡µðÅÍÀÇ ½ÃÀÛ´ÜÃßºÎºÐ
=======
    public void StartClick()   //ê²Œìž„ ìŠ¤íƒ€íŠ¸ ë²„íŠ¼
    {
        SceneManager.LoadScene("WorldMap");

    }

    public void ExitClick()   // ê²Œìž„ ì¢…ë£Œ ë²„íŠ¼
    {
        Application.Quit(); // ìœ ë‹ˆí‹° ê²Œìž„ ì¢…ë£Œ

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;  //ìœ ë‹ˆí‹° ì—ë””í„°ì˜ ì‹œìž‘ë‹¨ì¶”ë¶€ë¶„
>>>>>>> 22ff9ed88032a66f3cc0fc3a068ec87347d487ea
#endif
    }


<<<<<<< HEAD
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
=======
    void Start()
    {

    }


    void Update()
    {

    }
}
>>>>>>> 22ff9ed88032a66f3cc0fc3a068ec87347d487ea
