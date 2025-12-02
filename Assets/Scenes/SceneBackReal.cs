using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneBackReal: MonoBehaviour
{
    void OnTriggerEnter(Collider other){
    	SceneManager.LoadScene(0);
		
    }
}
