using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectCup : MonoBehaviour
{
    public void Setcup(string _cupName)
    {
        //loadScene(_cupName);
        Debug.Log(_cupName);
    }
    public void SetDifficulty(string _difficulty)
    {
        //save(_difficulty);
        Debug.Log(_difficulty);
    }
    public void LoadScene(string nextscene)
    {
        SceneManager.LoadScene(nextscene);
    }
}
