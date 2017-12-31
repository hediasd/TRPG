using UnityEngine;
using System.Collections;
using System.Collections.Generic; 

public class Piece : MonoBehaviour {


    void Start () {
	}

    float t = 0.0f;

    public void Walk(Point to, bool end = true){
        Point here = new Point(this.gameObject);
        List<Point> path = Environment.GetPath(here, to, true);

        foreach (Point p in path)
        {
            here = here + p;
        }
        StartCoroutine(Transition(path, 3, end, null));
    }
    //public void Fly(List<Point> moves, bool end = true, SfxSpriteAnimation animate = null){
    //    StartCoroutine(Transition(moves, 1.5f, end, animate));
    //}

    /*IEnumerator Transition(float x, float z)
    {
        
        Vector3 startingPos = transform.position;
        t = 0.0f;
        //transitionDuration
        while (t < 1.0f)
        {
            t += Time.deltaTime * (Time.timeScale / 0.3f); ///
            transform.position = Vector3.Lerp(startingPos, startingPos + new Vector3(x, 0f, z), t);
            //transform.LookAt(target.position - new Vector3(0,1.5f,0));
            yield return 0;
        }
    }*/
    
    IEnumerator Transition(List<Point> moves, float speed, bool end, SfxSpriteAnimation animate)
    {
        //Lock ck = new Lock(E.NONE);
        //BattleMaster.AddLock(ck);

        for (int i = 0; i < moves.Count; i++)
        {
            Vector3 startingPos = transform.position;
            t = 0.0f;
            
            while (t < 1.0f)
            {
                t += Time.deltaTime * (Time.timeScale / (speed / 10)); ///
                transform.position = Vector3.Lerp(startingPos, startingPos + new Vector3(moves[i].x, 0f, moves[i].z), t);
                //transform.LookAt(target.position - new Vector3(0,1.5f,0));
                yield return 0;
            }
        }

        //if(animate != null) animate.animating = true;
        yield return new WaitForSeconds(0.15f);
        if(end) BattleMaster.Acting = false;
        
        //BattleMaster.ReleaseLock(ck);
    }
}
