using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MonsterSpriteAnimation : BaseSpriteAnimation {

     bool started = true;
	 public static float timer = 0;
	 static int default_currFrame = 0;

    public override void Startstuff(){
        Kind = "Monsters";
    }

	void Update(){
        if(started){

		ChangeSprite(default_currFrame);
				
		if (timer >= 0.2f){
            timer = 0;
			default_currFrame += 1;
			if(default_currFrame == 4) default_currFrame = 0;
		}
        
        gameObject.GetComponent<SpriteRenderer>().color = new Color(r, g, b, a);

        }
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