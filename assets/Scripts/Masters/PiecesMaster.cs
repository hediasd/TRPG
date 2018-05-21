using UnityEngine;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

public class PiecesMaster : MonoBehaviour {

	public GameObject cellEff, cellMon, cellDmg, cellTrr;
	public GameObject chooser, effPieces, monPieces, txtPieces;
	public GameState stackTop;
	ChooserMaster chooserMaster;
	public static Dictionary<int, GameObject> MonPiecesList = new Dictionary<int, GameObject>();

	public static int Actors = 0;

	void Start () {		
		chooserMaster = chooser.GetComponent<ChooserMaster>();
	}
	
	void Update () {
		MonsterSpriteAnimation.timer += Time.deltaTime;
	}

	public void Cleanup(){

		foreach (Transform child in effPieces.transform) {
			GameObject.Destroy(child.gameObject);
		}
		foreach (Transform child in monPieces.transform) {
			GameObject.Destroy(child.gameObject);
		}
		foreach (Transform child in txtPieces.transform) {
			GameObject.Destroy(child.gameObject);
		}
		MonPiecesList.Clear();

	}

	public static GameObject MonsterGameObject(Monster mon){
		return MonPiecesList[mon.ID];
	}

	public void Updater(){
		switch (stackTop.Selecter){
			case E.CHOOSER:
				if(Input.GetKeyDown("up")){
					chooserMaster.Move(0f, 0f, 1f);
				}
				if(Input.GetKeyDown("down")){
					chooserMaster.Move(0f, 0f, -1f);
				}		
				if(Input.GetKeyDown("left")){
					chooserMaster.Move(-1f, 0f, 0f);
				}		
				if(Input.GetKeyDown("right")){
					chooserMaster.Move(1f, 0f, 0f);
				}				
				//if(Input.GetKeyDown("down") && undefined < 5) undefined++;
				//if(Input.GetKeyDown("up") && undefined > 0) undefined--;
			break;
		}
	}

	public void WalkTo(GameObject go, Point to, List<Point> PointPath){
		BattleMaster.Acting++;
		go.GetComponent<Piece>().Walk(to, PointPath);
	}

	public GameObject SpawnMonsterPiece(Monster mon, Point p){

		GameObject SpawnedMonsterPiece = (GameObject) Instantiate(cellMon, new Vector3(p.x, 0, p.z), Quaternion.Euler (0, 0, 0));
		/* //////////////////////////////////// */
		MonsterSpriteAnimation msa = SpawnedMonsterPiece.transform.GetChild(0).GetComponent<MonsterSpriteAnimation>();
		msa.Sheetname = mon.Texture;
		msa.Startup();

		if(mon.PaletteA != null){		
			ColorSwap cs = SpawnedMonsterPiece.transform.GetChild(0).GetComponent<ColorSwap>();
			cs.news.Add(mon.PaletteA_);
			cs.news.Add(mon.PaletteB_);
			cs.Startup(msa.sprites[0]);
		}else{
			Destroy(SpawnedMonsterPiece.GetComponent<ColorSwap>());
		}
		//Monster monscript = newmon.AddComponent<Monster>();
		//Monster monscript = new Monster();
		//foreach (FieldInfo fi in mon.GetType().GetFields()){
		//	fi.SetValue(monscript, fi.GetValue(mon));
		//}
		SpawnedMonsterPiece.GetComponent<MonsterHolder>().Monster = mon;		
		SpawnedMonsterPiece.name = "(Mon) " + mon.Name;
		SpawnedMonsterPiece.transform.SetParent(monPieces.transform, false);
		MonPiecesList.Add(mon.ID, SpawnedMonsterPiece);

		//monscript.cellMon = newmon;
		return SpawnedMonsterPiece;
	}

	public void SpawnDamageText(GameObject MonsterPiece, string Text){
		
			//GameObject monpiece = MonsterGameObject(DamageInstance.TargetMonster);
			Point at = new Point(MonsterPiece);
			GameObject damageText;
			damageText = (GameObject) Instantiate(cellDmg, new Vector3(1+at.x, 0.2f, at.z), Quaternion.Euler (18, -45, 0));
			
			damageText.name = "(Txt) " + "Damage " + Text;//DamageInstance.FinalDamage;
			ShakingText st = damageText.transform.GetComponent<ShakingText>();
			damageText.transform.SetParent(txtPieces.transform, false);
			st.Startup(""+Text);

	}

	public void SpawnAnimation(PieceSpell ps){
		BattleMaster.Acting++;
		StartCoroutine(Actor(ps));		
	}

	public void SpawnSfxEffect(SfxSpriteAnimation sfx, Point to, Point fr){
		GameObject effect;
		effect = (GameObject) Instantiate(cellEff, new Vector3(to.x, 0, to.z), Quaternion.Euler (0, 0, 0));
		
		//	effect = (GameObject) Instantiate(cellEff, new Vector3(fr.x, 0, fr.z), Quaternion.Euler (0, 0, 0));

		SfxSpriteAnimation spa = effect.transform.GetChild(0).GetComponent<SfxSpriteAnimation>();
		foreach (FieldInfo fi in sfx.GetType().GetFields()){
			fi.SetValue(spa, fi.GetValue(sfx));
		}

		//effect.transform.GetChild(0).GetComponent<SfxSpriteAnimation>().Startup();
		spa.Startup();
		effect.name = "(Eff) " + sfx.Sheetname;
		effect.transform.SetParent(effPieces.transform, false);

		//if(fr != null) GoTo(effect, to, spa);
	}

	public void SpawnTerrain(Terrain trr, Point p){

		GameObject SpawnedTerrain = (GameObject) Instantiate(cellTrr, new Vector3(p.x, 0, p.z), Quaternion.Euler (-90, 0, 0));
		/* //////////////////////////////////// */
		MonsterSpriteAnimation msa = SpawnedTerrain.transform.GetChild(0).GetComponent<MonsterSpriteAnimation>();
		msa.Sheetname = trr.Texture;
		msa.Startup();

		if(trr.PaletteA != null){		
			ColorSwap cs = SpawnedTerrain.transform.GetChild(0).GetComponent<ColorSwap>();
			cs.news.Add(trr.PaletteA_);
			cs.news.Add(trr.PaletteB_);
			cs.Startup(msa.sprites[0]);
		}else{
			Destroy(SpawnedTerrain.GetComponent<ColorSwap>());
		}
		
		SpawnedTerrain.GetComponent<TerrainHolder>().Terrain = trr;		
		SpawnedTerrain.name = "(Trr) " + trr.Name;
		SpawnedTerrain.transform.SetParent(monPieces.transform, false);

	}

	public IEnumerator Actor(PieceSpell ps)
    {
		//Animation animation = Grimoire.Animations[0];

		GameObject SpellAnimation = Instantiate(Resources.Load("Animation/Fire Ball")) as GameObject;

		if(SpellAnimation.GetComponent<Piece>() == null){
			SpellAnimation.AddComponent<Piece>();
		}

		Point target = ps.to;
		Point fr = new Point(ps.who);

		SpellAnimation.transform.localPosition = new Vector3(fr.x, 0, fr.z);

		List<Point> shape = ps.sp.EffectShapePoints(fr, target);
		
		//Queue<SfxSpriteAnimation> EffectQueue = new Queue<SfxSpriteAnimation>();
		
		/*
				foreach (SfxSpriteAnimation sfx in animation.EffectList)
				{
					List<List<Point>> fragmented_shape = ps.sp.FragmentedShape(fr, target, shape, sfx.Shape);
					sfx.points = fragmented_shape;
					/*foreach (List<Point> lp in fragmented_shape)
					{
						foreach (Point p in lp)
						{
							Debug.Log(p.x + " " + p.z + "    ");
						}
						Debug.Log("\n ");
					}
					EffectQueue.Enqueue(sfx);
				}

				while(EffectQueue.Count > 0)
				{
					SfxSpriteAnimation sfx = EffectQueue.Dequeue();
					if(sfx.Type == E.WAVES){
						foreach (List<Point> points in sfx.points)
						{
							foreach (Point p in points)
							{
								SpawnSfxEffect(sfx, p, fr);
							}
							yield return TimeMaster.WaitSeconds(0.2f);
						}
					}else{
						SpawnSfxEffect(sfx, target, fr);
					}
					if(sfx.Step) continue;

					while(effPieces.transform.childCount > 0){
						yield return TimeMaster.WaitSeconds(0.05f);
					}
				}
				
		 */

        yield return TimeMaster.WaitSeconds(0.25f);
		//Actors--;
		BattleMaster.Acting--;
    }


}
