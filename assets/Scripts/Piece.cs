using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour {

    void Start () { }

    float t = 0.0f;

    public void Walk (Point to, List<Point> PointPath, bool end = true) {
        PiecesMaster.MonstersActing++;
        Point here = new Point (this.gameObject);
        StartCoroutine (Transition (PointPath, 3, end));
    }

    public void Fly (List<Point> moves, bool end = true) {
        PiecesMaster.MonstersActing++;
        StartCoroutine (Transition (moves, 1.5f, end));
    }

    public void ParticleFly (Point from, Point to, float speed = 3) {
        PiecesMaster.MonstersActing++;
        StartCoroutine (ParticleTransition (from, to, speed));
    }

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

    IEnumerator Transition (List<Point> moves, float speed, bool end) {
        List<Point> UnitaryMovementPoints = new List<Point> ();
        for (int i = 0; i < moves.Count - 1; i++) {
            UnitaryMovementPoints.Add (new Point (moves[i + 1] - moves[i]));
        }

        for (int i = 0; i < UnitaryMovementPoints.Count; i++) {
            Vector3 startingPos = transform.position;
            t = 0.0f;

            while (t < 1.0f) {
                t += TimeMaster.GeneralTiming * Time.deltaTime * (Time.timeScale / (speed / 10)); ///
                //transform.position = Vector3.Lerp(startingPos, startingPos + new Vector3(UnitaryMovementPoints[i].x, 0f, UnitaryMovementPoints[i].z), t);
                transform.position = Vector3.MoveTowards (startingPos, startingPos + new Vector3 (UnitaryMovementPoints[i].x, 0f, UnitaryMovementPoints[i].z), t);

                //transform.LookAt(target.position - new Vector3(0,1.5f,0));
                yield return 0;
            }
        }

        //if(animate != null) animate.animating = true;
        TimeMaster.WaitSeconds (0.15f);
        if (end) PiecesMaster.MonstersActing--;

        //BattleMaster.ReleaseLock(ck);
    }

    IEnumerator ParticleTransition (Point from, Point to, float speed) {

        Vector3 startingPos = new Vector3 (from.x, 0, from.z);
        Vector3 goalPos = new Vector3 (to.x, 0, to.z);

        float distance = Point.Distance (from, to);
        t = 0.0f;
        
        while (t < distance - 0.2f) {
            //t += TimeMaster.GeneralTiming * Time.deltaTime * (Time.timeScale / (speed / 10)); ///
            t += TimeMaster.GeneralTiming * Time.deltaTime * Time.timeScale * speed;
            //transform.position = Vector3.Lerp(startingPos, startingPos + new Vector3(UnitaryMovementPoints[i].x, 0f, UnitaryMovementPoints[i].z), t);
            transform.localPosition = Vector3.MoveTowards (startingPos, goalPos, t);

            //transform.LookAt(target.position - new Vector3(0,1.5f,0));
            yield return 0;

        }

        //if(animate != null) animate.animating = true;
        //Destroy(GetComponentInChildren<ParticleSystem>());

        // 
        PiecesMaster.MonstersActing--;
        GetComponentInChildren<ParticleSystem> ().Stop (false, ParticleSystemStopBehavior.StopEmittingAndClear); // = 0;
        TimeMaster.WaitSeconds (0.35f);

        //TimeMaster.WaitSeconds (0.35f);
        //;

    }

}