using UnityEngine;
using System.Collections;

public class GemStone : MonoBehaviour {
	public int rowIndex=0;
	public int columIndex=0;
	public float xoffset=-5.0f;
	public float yoffset=-2.0f;
	public GameObject[] gemstoneBgs;
	public int gemstoneType;

	private GameObject gemstoneBg;
	private GameController gameController;
	private SpriteRenderer spriteRenderer;
	public bool isSelected{
		set{
			if(value)
				spriteRenderer.color=Color.red;
		    else 
				spriteRenderer.color=Color.white;
		}
	}

	// Use this for initialization

	void Start () {
		gameController = GameObject.Find ("GameController").GetComponent<GameController> ();
		spriteRenderer = gemstoneBg.GetComponent<SpriteRenderer> ();
	}
	
	/// <summary>
	/// 更新宝石的位置
	/// </summary>
	/// <param name="_rowIndex">行号</param>
	/// <param name="_columIndex">列号</param>
	public void UpdatePosition (int _rowIndex,int _columIndex) {
		rowIndex = _rowIndex;
		columIndex = _columIndex;
		this.transform.position = new Vector3 (columIndex*1.2f+xoffset, rowIndex*1.2f+yoffset, 0);
	
	}
	/// <summary>
	/// 随机产生宝石的背景
	/// </summary>
	public void RandomCreateGemstoneBg(){
		gemstoneType =Random.Range (0 , gemstoneBgs.Length);
		gemstoneBg =Instantiate (gemstoneBgs[gemstoneType]) as GameObject;
		gemstoneBg.transform.parent = this.transform;
	}
	void OnMouseDown(){
		gameController.Select (this);

}
public void Dispose(){
		Destroy (this.gameObject);
		Destroy (gemstoneBg.gameObject);
		gameController = null;
		//销毁被点击的宝石
	}
}
