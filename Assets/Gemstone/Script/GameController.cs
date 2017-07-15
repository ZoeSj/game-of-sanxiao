using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
	public GemStone gemstone;
	public int rowNum=7;//行数
	public int columNum=10;//列数
	public ArrayList gemstoneList;
	public ArrayList matchesGemstoneList;//相同的宝石列表
	// Use this for initialization
	public AudioClip match3Clip;//匹配的声音
	public AudioClip swapClip;//宝石交换的声音
	public AudioClip errorClip;//交换出错的提示音
	void Start () {

		gemstoneList = new ArrayList ();
		matchesGemstoneList = new ArrayList ();
		for (int rowIndex=0;rowIndex<rowNum;rowIndex++){
			ArrayList temp= new ArrayList ();
			for (int columIndex=0;columIndex <columNum;columIndex++){
				GemStone c = AddGemstone(rowIndex,columIndex);
				temp.Add (c);
			}
			gemstoneList.Add (temp);
		}
		if (CheckHorizontalMatches () || CheckVerticalMatches ()) {
			RemoveMatches ();

		} 
	} 
	public GemStone AddGemstone(int rowIndex,int columIndex){
		GemStone c = Instantiate (gemstone) as GemStone;
		c.transform.parent = this.transform;
		c.GetComponent<GemStone> ().RandomCreateGemstoneBg ();
		c.GetComponent<GemStone> ().UpdatePosition (rowIndex, columIndex);
		return c;
	}
	private GemStone currentGemStone;
	public void Select(GemStone c){
		if (currentGemStone == null) {
			currentGemStone = c;
			currentGemStone.isSelected=true;
			return;
		} else {
			if(Mathf.Abs(currentGemStone.rowIndex-c.rowIndex)+Mathf.Abs(currentGemStone.columIndex-c.columIndex)==1){
				StartCoroutine (ExangeAndMatches(currentGemStone,c));
		}else{
			audio.PlayOneShot(errorClip);
		}
		currentGemStone.isSelected=false;
		currentGemStone = null;
	}
}
	IEnumerator ExangeAndMatches(GemStone c1,GemStone c2){
		Exchange (c1, c2);
		yield return new WaitForSeconds (0.5f);
		if (CheckHorizontalMatches () || CheckVerticalMatches ()) {
			RemoveMatches ();
		} else {
			Debug.Log("没有检测到相同，交换回来！");
			Exchange(c1,c2);
		}
	}
	void RemoveMatches(){
		for (int i=0; i<matchesGemstoneList.Count; i++) {
			GemStone c = matchesGemstoneList [i] as GemStone;
			RemoveGemstone (c);

		}
		matchesGemstoneList = new ArrayList ();
		StartCoroutine (WaitForCheckMatchesAgin ());
	}
	IEnumerator WaitForCheckMatchesAgin(){
		    
			yield return new WaitForSeconds (0.5f);
			if (CheckHorizontalMatches () || CheckVerticalMatches ()) {
			RemoveMatches ();
		} 
		}


	void RemoveGemstone(GemStone c){
		c.Dispose();
		audio.PlayOneShot (match3Clip);
		for (int i = c.rowIndex+1; i<rowNum; i++) {
			GemStone temGemstone = GetGemStone (i,c.columIndex);
			temGemstone.rowIndex--;
			SetGemStone(temGemstone.rowIndex,temGemstone.columIndex,temGemstone);
			temGemstone.UpdatePosition(temGemstone.rowIndex,temGemstone.columIndex);
		}
		GemStone newGemstone = AddGemstone (rowNum, c.columIndex);
		newGemstone.rowIndex--;
		SetGemStone (newGemstone.rowIndex, newGemstone.columIndex,newGemstone);
		newGemstone.UpdatePosition (newGemstone.rowIndex, newGemstone.columIndex);
	}

	public GemStone GetGemStone(int rowIndex,int columIndex){
		ArrayList temp = gemstoneList [rowIndex] as ArrayList;
		GemStone c = temp [columIndex] as GemStone;
		return c;
	}
	public void SetGemStone(int rowIndex,int  columIndex,GemStone c){
		ArrayList temp = gemstoneList [rowIndex] as ArrayList;
		temp [columIndex] = c;
	}
	public void Exchange (GemStone c1,GemStone c2){
		audio.PlayOneShot (swapClip);
		SetGemStone (c1.rowIndex, c1.columIndex, c2);
		SetGemStone (c2.rowIndex, c2.columIndex, c1);
		//huan hang hao 
		int tempRowIndex;
		tempRowIndex = c1.rowIndex;
		c1.rowIndex = c2.rowIndex;
		c2.rowIndex = tempRowIndex;
		//huan lie  hao
		int tempColumIndex;
		tempColumIndex = c1.columIndex;
		c1.columIndex = c2.columIndex;
		c2.columIndex = tempColumIndex;

		c1.UpdatePosition (c1.rowIndex, c1.columIndex);
		c2.UpdatePosition (c2.rowIndex, c2.columIndex);
	}
	// Update is called once per frame
	bool CheckHorizontalMatches(){
		bool isMatches = false;
		for (int rowIndex=0; rowIndex<rowNum; rowIndex++) {
			for(int columIndex=0;columIndex<columNum-2;columIndex++){
				if(
					(GetGemStone(rowIndex,columIndex).gemstoneType==GetGemStone(rowIndex,columIndex+1).gemstoneType)
				  &&(GetGemStone(rowIndex,columIndex).gemstoneType==GetGemStone(rowIndex,columIndex+2).gemstoneType)
					)
				{
					Debug.Log("发现行相同的宝石");
					AddMatches (GetGemStone(rowIndex,columIndex));
					AddMatches (GetGemStone(rowIndex,columIndex+1));
					AddMatches (GetGemStone(rowIndex,columIndex+2));
						isMatches=true;
				}
			}
		
		}
		return isMatches;
	}
	bool CheckVerticalMatches(){
		bool isMatches = false;
		for (int columIndex=0; columIndex<columNum; columIndex++) {
			for(int rowIndex=0;rowIndex<rowNum-2;rowIndex++){
				if(
					(GetGemStone(rowIndex,columIndex).gemstoneType==GetGemStone(rowIndex+1,columIndex).gemstoneType)
					&&(GetGemStone(rowIndex,columIndex).gemstoneType==GetGemStone(rowIndex+2,columIndex).gemstoneType)
					)
				{
					Debug.Log("发现列相同的宝石");
					AddMatches (GetGemStone(rowIndex,columIndex));
					AddMatches (GetGemStone(rowIndex+1,columIndex));
					AddMatches (GetGemStone(rowIndex+2,columIndex));
					isMatches=true;
				}
			}
			
		}
		return isMatches;
	}
	void AddMatches(GemStone c){
		if (matchesGemstoneList == null)
			matchesGemstoneList = new ArrayList ();

		int index=matchesGemstoneList.IndexOf (c);
		if (index==-1)
		matchesGemstoneList.Add (c);//把相同的宝石放入列表

	}

	void Update () {
	
	}
}
