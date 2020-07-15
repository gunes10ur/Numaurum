using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour
{
	private bool isMouseOver = false;
	private bool isMouseDown = false;

	public int indexI;
	public int indexJ;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseEnter() {
    	isMouseOver = true;
    }

    private void OnMouseExit() {
    	isMouseOver = false;
    }

    private void OnMouseDown() {
    	isMouseDown = true;
    }

    private void OnMouseUp() {
    	if (isMouseDown && isMouseOver) {
    		NotifyMaster();
    	}
    }

    public void SetIJ(int i, int j) {
    	indexI = i;
    	indexJ = j;
    }

    private void NotifyMaster() {
    	this.transform.parent.GetComponent<BoardController>().PieceClicked(indexI, indexJ);
    }
}
