using UnityEngine;

public class MonsterHolder : MonoBehaviour {

	public Color MonsterColorA, MonsterColorB;
	public int Team;
	public string Races;
	public MonsterInstance HeldMonster;

	void Start()
	{
		MonsterThinker Thinker = GetComponent<MonsterThinker> ();
		if(Thinker == null){
			gameObject.AddComponent<MonsterThinker>();
        }
	}

	void Update () {

		if (Time.frameCount % TimeMaster.GeneralFrameWaitingInterval == 0) {
			MonsterColorA = HeldMonster.PaletteA;
			MonsterColorB = HeldMonster.PaletteB;
			Team = HeldMonster.Team;
			Races = "";
			foreach (int MonsterRace in HeldMonster.Races) {
				Races += RACE.GetRaceName (MonsterRace) + " ";
			}
		}

	}

}