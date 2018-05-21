using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MonsterSpriteAnimation : BaseSpriteAnimation {

    //bool started = true;
	public static float timer = 0;
	static int IndividualCurrentFrame = 0;

    public override void Startstuff(){
        Kind = "Monsters";
        FrameTimer = 0.2f;
    }

	void Update(){

        //Debug.Log(MaxLoops);
        ChangeSprite(base.CurrentFrame);
        Timer += TimeMaster.GeneralTiming * Time.deltaTime;

        if (Timer >= FrameTimer){
            Timer = 0;
			CurrentFrame += 1;
			if(CurrentFrame == 4) CurrentFrame = 0;
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