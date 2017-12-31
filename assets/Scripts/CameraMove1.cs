using UnityEngine;
using System.Collections;

public class CameraMove1 : MonoBehaviour {

	// Use this for initialization

	
	public float transitionDuration = 1.5f;
	public Transform target;
    float t = 1.0f;
    float code = 0.0f;

    void Start () {
	    
	}
    
    void Update(){
        if(transitionDuration < 1.5f){
            transitionDuration += 0.1f;
        }
    }

    public void goOn(){
        if(t >= 1.0f){
        StartCoroutine(Transition());
        }
    }

    public void goTo(GameObject go){
        float codex = Random.Range(0.0f, 1000.0f);
        code = codex;
        target = go.transform;
        if(transitionDuration >= 0.5f) transitionDuration = 0.2f;
        StartCoroutine(Transition(0.0f, -2f*0.25f, 1.0f, codex)); //-1.5f
    }

    IEnumerator Transition()
    {
        t = 0.0f;
        Vector3 startingPos = transform.position;
        while (t < 1.0f)
        {
            t += Time.deltaTime * (Time.timeScale / transitionDuration);

            transform.position = Vector3.Lerp(startingPos, target.position + new Vector3(4f,1.5f,0f), t); //
            transform.LookAt(target.position - new Vector3(0,1.5f,0));
            yield return 0;
        }
    }

        IEnumerator Transition(float x, float y, float z, float cod)
    {
        t = 0.0f;
        Vector3 startingPos = transform.position;
        while (t < 1.0f && code==cod)
        {
            t += Time.deltaTime * (Time.timeScale / transitionDuration);

            transform.position = Vector3.Lerp(startingPos, target.position + new Vector3(x, y, z), t);
            //transform.LookAt(target.position);// - new Vector3(0,1.5f,0));
            yield return 0;
        }
    }

}
