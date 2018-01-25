using UnityEngine;
using System.Collections;
using System.Collections.Generic; 

public class Piece : MonoBehaviour {


    void Start () {
	}

    float t = 0.0f;

    public void Walk(Point to, List<Point> PointPath, bool end = true){
        Point here = new Point(this.gameObject);

        StartCoroutine(Transition(PointPath, 3, end, null));
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
        List<Point> UnitaryMovementPoints = new List<Point>();
        for (int i = 0; i < moves.Count-1; i++)
        {
            UnitaryMovementPoints.Add(new Point(moves[i+1] - moves[i]));
        }

        for (int i = 0; i < UnitaryMovementPoints.Count; i++)
        {
            Vector3 startingPos = transform.position;
            t = 0.0f;
            
            while (t < 1.0f)
            {
                t += TimeMaster.GeneralTiming * Time.deltaTime * (Time.timeScale / (speed / 10)); ///
                transform.position = Vector3.Lerp(startingPos, startingPos + new Vector3(UnitaryMovementPoints[i].x, 0f, UnitaryMovementPoints[i].z), t);
                //transform.LookAt(target.position - new Vector3(0,1.5f,0));
                yield return 0;
            }
        }

        //if(animate != null) animate.animating = true;
        TimeMaster.WaitSeconds(0.15f);
        if(end) BattleMaster.Acting = false;
        
        //BattleMaster.ReleaseLock(ck);
    }
}
