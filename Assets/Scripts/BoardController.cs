using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardController : MonoBehaviour
{
	public int width;
	public int height;

	public GameObject tilePrefab;
    public GameObject moveFramePrefab;

	private GameObject [,] tiles;
	private GameObject [,] tilePieces;

    private GameObject [,] moveFrames;

    private int [,] noclicked;

    private bool [,] clickable;

	public GameObject[] pieces;

    private GameObject scoreboard;

    private GameObject panel;

	// Green 160 200 160
	// Gray 200 200 200 tilebackground // Gray 180 180 180 tileframe //

    //public Color tileframeGreen = new Color(160f/255f, 200f/255f, 160f/255f);
    //public Color tileframeGray = new Color(180f/255f, 180f/255f, 180f/255f);

    public Color tileGray = new Color(160/255f, 160f/255f, 160f/255f);
    public Color tileBronze = new Color(205f/255f, 127f/255f, 50f/255f);
    public Color tileSilver = new Color(192f/255f, 192f/255f, 192f/255f);
    public Color tileGold = new Color(212f/255f, 175f/255f, 55f/255f);

    public Color tileGreen = new Color(64f/255f, 255f/255f, 64f/255f);

    void Start()
    {
        tiles = new GameObject[width, height];
        tilePieces = new GameObject[width, height];
        noclicked = new int[width, height];
        clickable = new bool[width, height];

        scoreboard = GameObject.Find("scoreboard");

        moveFrames = new GameObject[width, height];

        SetupBoard();

        panel = GameObject.Find("Panel");
        panel.active = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void createMoveFrame(int i, int j){
        if(noclicked[i, j] < 3){
                moveFrames[i, j] = Instantiate(moveFramePrefab, tiles[i, j].transform.position,
                    Quaternion.identity);
                moveFrames[i, j].transform.parent = tiles[i, j].transform;
                clickable[i, j] = true;
        }
  
    }

    public void clearMoveFrames(){
        for (int i = 0; i < height; i++) {
            for (int j = 0; j < width; j++) {
                Destroy(moveFrames[i, j]);
            }
        }
    }

    public void checkClickable(){
        int noOfClickables = 0;
        for (int i = 0; i < height; i++) {
            for (int j = 0; j < width; j++) {
                if (clickable[i, j]) {
                    noOfClickables += 1;
                }
            }
        }
        int score = calculateScore();
        scoreboard.GetComponent<Text>().text = score + "";

        if (noOfClickables > 0) {
            //Debug.Log("Number of clickables: " + noOfClickables);
        }
        else {
            Debug.Log("GameOver. Score: " + score);
            panel.active = true;
            panel.transform.Find("finalscore").GetComponent<Text>().text = score + "";
            int gold = 0;
            int silver = 0;
            int bronze = 0;
            for (int o = 0; o < width; o++) {
                for (int p = 0; p < width; p++) {
                    if (noclicked[o, p] == 1) {
                        bronze++;
                    }
                    if (noclicked[o, p] == 2) {
                        silver++;
                    }
                    if (noclicked[o, p] == 3) {
                        gold++;
                    }
                }
            }
            panel.transform.Find("finalgold").GetComponent<Text>().text = "x " + gold;
            panel.transform.Find("finalsilver").GetComponent<Text>().text = "x " + silver;
            panel.transform.Find("finalbronze").GetComponent<Text>().text = "x " + bronze;
        }
    }

    public int calculateScore() {
        int score = 0;
        for (int i = 0; i < height; i++) {
            for (int j = 0; j < width; j++) {
                score += noclicked[i, j] * noclicked[i, j] * 5;
            }
        }
        return score;
    }

    private void SetupBoard() {
        float marginx = 0.025f;
        float marginy = 0.025f;
    	for (int i = 0; i < width; i++) {
    		for (int j = 0; j < height; j++) {
    			Vector2 tmpPos = new Vector2(i+marginx*i, j+marginy*j);
    			tiles[i, j] = Instantiate(tilePrefab, tmpPos, Quaternion.identity);
    			tiles[i, j].transform.parent = this.transform;
    			tiles[i, j].name = "(" + i + ", " + j + ")";
    			int tileFlip = Random.Range(0, 4);
    			SpriteRenderer tileBackground = tiles[i, j].transform.Find("tilebackground")
                    .GetComponent<SpriteRenderer>();
    			SpriteRenderer tileFrame = tiles[i, j].transform.Find("tileframe")
                    .GetComponent<SpriteRenderer>();

    			switch(tileFlip) {
    				case 0:
    					tileBackground.flipX = false;
    					tileBackground.flipY = false;
    					break;
    				case 1:
    					tileBackground.flipX = true;
    					tileBackground.flipY = false;
    					break;
    				case 2:
    					tileBackground.flipX = false;
    					tileBackground.flipY = true;
    					break;
    				case 3:
    					tileBackground.flipX = true;
    					tileBackground.flipY = true;
    					break;
    			}
    			tileFrame.flipX = tileBackground.flipX;
    			tileFrame.flipY = tileBackground.flipY;

    			tiles[i, j].GetComponent<TileController>().SetIJ(i, j);

    			InitPiece(i, j);
                clickable[i, j] = true;
    		}
    	}
    }

    public void PieceClicked(int i, int j) {
        //panel.active = true;
        if(panel.active) {
            return;
        }
        if (clickable[i, j] && noclicked[i, j] < 3){
            noclicked[i, j] += 1;
            clearFrameIllum();
            clearMoveFrames();
            GameObject clickedPiece = tilePieces[i, j];
            GameObject clickedTile = tiles[i, j];
            string clickedtag = clickedPiece.tag;
            MakeAllUnclickable();
            IllumNextPiece(i, j, clickedtag);
            Destroy(clickedPiece);
            if (noclicked[i, j] < 3) {
                InitPiece(i, j);
            } 
        }
        checkClickable();
    }

    public void MakeAllUnclickable(){
        for (int i = 0; i < width; i++){
            for (int j = 0; j < height; j++){
                clickable[i, j] = false;
            }
        }
    }

    public void IllumNextPiece(int i, int j, string clickedtag) {
        int horivertmove = 0;
        int diagmove = 0;
        switch(clickedtag) {
            case "one":
                horivertmove = 1;
                diagmove = 0;
                break;
            case "two":
                horivertmove = 2;
                diagmove = 1;
                break;
            case "three":
                horivertmove = 3;
                diagmove = 1;
                break;
            case "four":
                horivertmove = 4;
                diagmove = 2;
                break;
        }
        int[] horivertstates = new int[] {i+horivertmove, j, i-horivertmove, j, i, j+horivertmove, 
                                        i, j-horivertmove};
        int[] diagstates = new int[] {i+diagmove, j+diagmove, i+diagmove, j-diagmove, i-diagmove,
                                        j+diagmove, i-diagmove, j-diagmove};
        for (int q = 0; q < 8; q+=2){
            int h1 = horivertstates[q];
            int h2 = horivertstates[q+1];
            if (h1 > -1 && h1 < height && h2 > -1 && h2 < height){
                //tiles[h1, h2].transform.Find("tileframe")
                //    .GetComponent<SpriteRenderer>().color = tileGreen;
                createMoveFrame(h1, h2);
            }
            int d1 = diagstates[q];
            int d2 = diagstates[q+1];
            if (d1 != i && d2 != j){
                if (d1 > -1 && d1 < height && d2 > -1 && d2 < height){
                    //tiles[d1, d2].transform.Find("tileframe")
                    //    .GetComponent<SpriteRenderer>().color = tileGreen;
                    createMoveFrame(d1, d2);
            }
            }
        }
    }

    public void clearFrameIllum() {
        for (int i = 0; i < height; i++) {
            for (int j = 0; j < width; j++) {
                switch(noclicked[i, j]){
                    case 0:
                        tiles[i, j].transform.Find("tilebackground")
                            .GetComponent<SpriteRenderer>().color = tileGray;
                        break;
                    case 1:
                        tiles[i, j].transform.Find("tilebackground")
                            .GetComponent<SpriteRenderer>().color = tileBronze;
                        break;
                    case 2:
                        tiles[i, j].transform.Find("tilebackground")
                            .GetComponent<SpriteRenderer>().color = tileSilver;
                        break;
                    case 3:
                        tiles[i, j].transform.Find("tilebackground")
                            .GetComponent<SpriteRenderer>().color = tileGold;
                        break;
                }
                tiles[i, j].transform.Find("tileframe")
                    .GetComponent<SpriteRenderer>().color = tileGray;
            }
        }
    }

    private void InitPiece(int i, int j) {
    	int pieceindex = Random.Range(0, pieces.Length);
    	GameObject piece = Instantiate(pieces[pieceindex], tiles[i, j].transform.position, 
            Quaternion.identity);
    	piece.transform.parent = tiles[i, j].transform;
    	piece.name = tiles[i, j].name;
    	tilePieces[i, j] = piece;
    }
}
