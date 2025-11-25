using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitch : MonoBehaviour
{
    void OnTriggerEnter(Collider other){
    	SceneManager.LoadScene(1);
		
    }
}
