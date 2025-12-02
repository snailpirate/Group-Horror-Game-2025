using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneUpsideDown : MonoBehaviour
{
    void OnTriggerEnter(Collider other){
    	SceneManager.LoadScene(2);
		
    }
}
