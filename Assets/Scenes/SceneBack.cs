using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneBack : MonoBehaviour
{
    void OnTriggerEnter(Collider other){
    	SceneManager.LoadScene(0);
		
    }
}
