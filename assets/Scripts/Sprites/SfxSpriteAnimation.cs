using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SfxSpriteAnimation : BaseSpriteAnimation {

    public int Type = E.SIMPLESINGLE, Shape = GEOMETRY.CIRCLE;
    public List<List<Point>> points;

    public SfxSpriteAnimation(){
    //  sheetname = sheet;
    }

    public override void Startstuff(){
        Kind = "Effects";
    }

	void Update(){
        //Debug.Log(MaxLoops);
        ChangeSprite(CurrentFrame);
        Timer += TimeMaster.GeneralTiming * Time.deltaTime;

        if (Timer >= FrameTimer){
            Timer = 0;
            CurrentFrame += 1;
            if(CurrentFrame == LastFrame){
                Looped += 1;
                if(MaxLoops == Looped){ //this.
                    // PiecesMaster.Actors -= 1;
                    Destroy(this.transform.parent.gameObject);
                }else{
                    CurrentFrame = 0;
                }
            }
        }
        
        gameObject.GetComponent<SpriteRenderer>().color = new Color(r, g, b, a);
	}
     
     void ChangeSprite(int index)
     {
         Sprite sprite = sprites[index];
         sr.sprite = sprite;
     }
 
     void ChangeSpriteByName( string name )
     {
        // Sprite sprite = sprites[array.IndexOf(names, name)];
        // sr.sprite = sprite;
     }


}