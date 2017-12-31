using UnityEngine;
using System.Collections;
using System.Collections.Generic; 

public class TextDialog : TextBubble {

	public void Startup () {
		textMesh = transform.Find("Text").gameObject.GetComponent<TextMesh>();
		StartCoroutine(Transition());
	}

    IEnumerator Transition() {

		for (int m = max; m < textPileup.Length; m += max)
		{
			if(textPileup[m] != ' '){
				int k = m - 1;
				while(k > 0){
					if(textPileup[k] == ' '){
						textPileup = textPileup.Remove(k, 1);
						textPileup = textPileup.Insert(k, "\n");
						break;
					}
					k--;
				}
			}else{
				//textPileup = textPileup.Remove(m, 2);
				//textPileup = textPileup.Insert(m, "\n");
			}
		}

		words = textPileup.ToCharArray();

        for (int i = 0; i < textPileup.Length; i++)
        {
			float t = 0.0f;
            bool go = true; 
            while (t < 1.0f)
            {
                t += Time.deltaTime * (Time.timeScale / 0.05f); ///
				if(go) { 
					//textPileup += words[i];
					textMesh.text = textPileup;
					go = false;
				}
				yield return 0;
            }
        }
    }
	
}
