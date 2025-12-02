using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneGym: MonoBehaviour
{
    void OnTriggerEnter(Collider other){
    	SceneManager.LoadScene(3);
		
    }
}
