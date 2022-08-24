using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBehaviour : MonoBehaviour
{
    public GameObject Black_Rook;
    public GameObject White_Rook;
    public GameObject Black_Knight;
    public GameObject White_Knight;
    public GameObject Black_Bishop;
    public GameObject White_Bishop;
    public GameObject Black_King;
    public GameObject White_King;
    public GameObject Black_Queen;
    public GameObject White_Queen;
    public GameObject Black_Pawn;
    public GameObject White_Pawn;


    // Reference to the piece in the square.
    [HideInInspector]
    public GameObject currentPiece;
    public short tileId;
    public bool isActive;
    public Color originalColor;

    // Variables for changing piece square
    public GameObject targetedPiece;
    public GameObject targetedPieceTile;

    public void Start()
    {
        originalColor = gameObject.GetComponent<SpriteRenderer>().material.color;

        Black_Rook = Resources.Load("Prefabs/Black_Rook") as GameObject;
        Black_Bishop = Resources.Load("Prefabs/Black_Bishop") as GameObject;
        Black_Knight = Resources.Load("Prefabs/Black_Knight") as GameObject;
        Black_Queen = Resources.Load("Prefabs/Black_Queen") as GameObject;
        Black_King = Resources.Load("Prefabs/Black_King") as GameObject;        
        Black_Pawn = Resources.Load("Prefabs/Black_Pawn") as GameObject;

        White_Rook = Resources.Load("Prefabs/White_Rook") as GameObject;
        White_Bishop = Resources.Load("Prefabs/White_Bishop") as GameObject;
        White_Knight = Resources.Load("Prefabs/White_Knight") as GameObject;
        White_Queen = Resources.Load("Prefabs/White_Queen") as GameObject;
        White_King = Resources.Load("Prefabs/White_King") as GameObject;        
        White_Pawn = Resources.Load("Prefabs/White_Pawn") as GameObject;

        gameObject.AddComponent<BoxCollider>();

        LoadPiece();
    }

    private void Update()
    {
        if (isActive)
        {
            var myColor = new Color(0.84f,0.8f,0.34f,1.0f);
            gameObject.GetComponent<SpriteRenderer>().material.color = myColor;
        }
        else if (!isActive)
        {
            gameObject.GetComponent<SpriteRenderer>().material.color = originalColor;
        }
    }

    public void OnMouseDown() 
    {
        if (!isActive)
        {
            try
            {
                Debug.Log(currentPiece.name);
                switch(currentPiece.name)
                {
                    case "white_pawn": WhitePawn_ShowLegalMoves(GameModel.White_Pieces); break;
                    case "black_pawn": BlackPawn_ShowLegalMoves(GameModel.Black_Pieces); break;
                    case "white_rook": break;
                    case "black_rook": break;
                    case "white_knight": Knight_ShowLegalMoves(GameModel.White_Pieces); break;
                    case "black_knight": Knight_ShowLegalMoves(GameModel.Black_Pieces); break;
                    case "white_bishop": break;
                    case "black_bishop": break; 
                    case "white_queen": break;
                    case "black_queen": break;
                    case "white_king": King_ShowLegalMoves(GameModel.White_Pieces); break;
                    case "black_king": King_ShowLegalMoves(GameModel.Black_Pieces); break;
                }
            }
            catch (System.NullReferenceException)
            {
                return;
            }
        }
        else // When a square is active
        {   
            if (gameObject.transform.childCount != 0) // If there is a piece in that square destroy it
            {
                Object.Destroy(gameObject.transform.GetChild(0).gameObject);   
            }         

            switch(targetedPiece.name)
            {
                case "white_pawn":
                    SpawnPiece("p","w");
                    GameModel.UpdateGameModel(targetedPieceTile.GetComponent<TileBehaviour>().tileId,tileId,"white_pawn");
                    GenerateBoard.SetAllInactive();
                    break;
                case "black_pawn": 
                    SpawnPiece("p","b");
                    GameModel.UpdateGameModel(targetedPieceTile.GetComponent<TileBehaviour>().tileId,tileId,"black_pawn");
                    GenerateBoard.SetAllInactive();
                    break;
                case "white_rook": break;
                case "black_rook": break;
                case "white_knight": 
                    SpawnPiece("n","w"); 
                    // UpdateGameModel(tileToNull, tileToSet, pieceCode);
                    GameModel.UpdateGameModel(targetedPieceTile.GetComponent<TileBehaviour>().tileId,tileId,"white_knight"); // the 2 places + the piece occupying
                    GenerateBoard.SetAllInactive();                     
                    break;
                case "black_knight": 
                    SpawnPiece("n","b");
                    GameModel.UpdateGameModel(targetedPieceTile.GetComponent<TileBehaviour>().tileId,tileId,"black_knight");
                    GenerateBoard.SetAllInactive(); 
                    break;
                case "white_bishop": break;
                case "black_bishop": break; 
                case "white_queen": break;
                case "black_queen": break;
                case "white_king": 
                    SpawnPiece("k","w");
                    GameModel.UpdateGameModel(targetedPieceTile.GetComponent<TileBehaviour>().tileId,tileId,"white_king");
                    GenerateBoard.SetAllInactive(); 
                    break;
                case "black_king": 
                    SpawnPiece("k","b");
                    GameModel.UpdateGameModel(targetedPieceTile.GetComponent<TileBehaviour>().tileId,tileId,"black_king");
                    GenerateBoard.SetAllInactive(); 
                    break;
            }
            // This line takes care of removing the previous square piece.
            Object.Destroy(targetedPieceTile.transform.GetChild(0).gameObject);
        }
        
    }

    // Pending to change the model when this operation is created.
    public void SpawnPiece(string piece,string color)
    {
        piece = piece.ToLower();
        color = color.ToLower();
        var parentTransform = gameObject.GetComponent<Transform>();
        Vector3 spawnPos = parentTransform.position + new Vector3(0.0f,0.0f,-1.0f);
        
        if (color == "b")
        {
            switch (piece)
            {
                case "k":
                    Instantiate(Black_King,spawnPos,Quaternion.identity,parentTransform);
                    currentPiece = Black_King;
                    break;
                case "q":
                    Instantiate(Black_Queen,spawnPos,Quaternion.identity,parentTransform);
                    currentPiece = Black_Queen;
                    break;
                case "b":
                    Instantiate(Black_Bishop,spawnPos,Quaternion.identity,parentTransform);
                    currentPiece = Black_Bishop;     
                    break;           
                case "n":
                    Instantiate(Black_Knight,spawnPos,Quaternion.identity,parentTransform);
                    currentPiece = Black_Knight;
                    break;
                case "r":
                    Instantiate(Black_Rook,spawnPos,Quaternion.identity,parentTransform);
                    currentPiece = Black_Rook;
                    break;
                case "p":
                    Instantiate(Black_Pawn,spawnPos,Quaternion.identity,parentTransform);
                    currentPiece = Black_Pawn;
                    break;
            }
        }
        if (color == "w")
        {
            switch (piece)
            {
                case "k":
                    Instantiate(White_King,spawnPos,Quaternion.identity,parentTransform);
                    currentPiece = White_King;
                    break;
                case "q":
                    Instantiate(White_Queen,spawnPos,Quaternion.identity,parentTransform);
                    currentPiece = White_Queen;
                    break;
                case "b":
                    Instantiate(White_Bishop,spawnPos,Quaternion.identity,parentTransform);
                    currentPiece = White_Bishop;
                    break;                
                case "n":
                    Instantiate(White_Knight,spawnPos,Quaternion.identity,parentTransform);
                    currentPiece = White_Knight;
                    break;
                case "r":
                    Instantiate(White_Rook,spawnPos,Quaternion.identity,parentTransform);
                    currentPiece = White_Rook;
                    break;
                case "p":
                    Instantiate(White_Pawn,spawnPos,Quaternion.identity,parentTransform);
                    currentPiece = White_Pawn;
                    break;
            }
        }
    }

    void LoadPiece()
    {
        if ((GameModel.White_Pawns & ((ulong)1 << tileId)) != 0)
        {
            SpawnPiece("p","w");
        }
        else if ((GameModel.Black_Pawns & ((ulong)1 << tileId)) != 0)
        {
            SpawnPiece("p","b");
        }
        else if ((GameModel.Black_Knights & ((ulong)1 << tileId)) != 0)
        {
            SpawnPiece("n","b");
        }
        else if ((GameModel.White_Knights & ((ulong)1 << tileId)) != 0)
        {
            SpawnPiece("n","w");
        }
        else if ((GameModel.Black_Bishops & ((ulong)1 << tileId)) != 0)
        {
            SpawnPiece("b","b");
        }
        else if ((GameModel.White_Bishops & ((ulong)1 << tileId)) != 0)
        {
            SpawnPiece("b","w");
        }
        else if ((GameModel.Black_Rooks & ((ulong)1 << tileId)) != 0)
        {
            SpawnPiece("r","b");
        }
        else if ((GameModel.White_Rooks & ((ulong)1 << tileId)) != 0)
        {
            SpawnPiece("r","w");
        }
        else if ((GameModel.Black_Queens & ((ulong)1 << tileId)) != 0)
        {
            SpawnPiece("q","b");
        }
        else if ((GameModel.White_Queens & ((ulong)1 << tileId)) != 0)
        {
            SpawnPiece("q","w");
        }
        else if ((GameModel.Black_Kings & ((ulong)1 << tileId)) != 0)
        {
            SpawnPiece("k","b");
        }
        else if ((GameModel.White_Kings & ((ulong)1 << tileId)) != 0)
        {
            SpawnPiece("k","w");
        }
    }
    // It colors the square where that piece can move. It also sets the active property so a piece can move
    void Knight_ShowLegalMoves(ulong ownSide)
    {
        ulong knight_pos = ((ulong)1 << tileId);
        ulong res = GameModel.knight_legal_moves(knight_pos,ownSide); 
        foreach (var tile in GenerateBoard.GameTiles)
        {
            TileBehaviour externalTile = tile.GetComponent<TileBehaviour>();
            if ( (res & ((ulong)1 << externalTile.tileId) ) != 0 ) // That tile has to be active
            {                
                externalTile.isActive = true;
                externalTile.targetedPiece = currentPiece; // The piece we plan to move
                externalTile.targetedPieceTile = gameObject;
            }
            else
            {
                externalTile.isActive = false;
            }
        }        
    }

    void King_ShowLegalMoves(ulong ownSide)
    {
        ulong king_pos = ((ulong)1 << tileId);
        ulong res = GameModel.king_legal_moves(king_pos,ownSide);   

        foreach (var tile in GenerateBoard.GameTiles)
        {
            TileBehaviour externalTile = tile.GetComponent<TileBehaviour>();
            if ( (res & ((ulong)1 << externalTile.tileId) ) != 0 ) // That tile has to be active
            {                
                externalTile.isActive = true;
                externalTile.targetedPiece = currentPiece; // The piece we plan to move
                externalTile.targetedPieceTile = gameObject;
            }
            else
            {
                externalTile.isActive = false;
            }
        }        
    }

    void WhitePawn_ShowLegalMoves(ulong ownSide)
    {
        ulong pawn_pos = ((ulong)1 << tileId);
        ulong res = GameModel.white_pawn_legal_moves(pawn_pos,ownSide);   

        foreach (var tile in GenerateBoard.GameTiles)
        {
            TileBehaviour externalTile = tile.GetComponent<TileBehaviour>();
            if ( (res & ((ulong)1 << externalTile.tileId) ) != 0 ) // That tile has to be active
            {                
                externalTile.isActive = true;
                externalTile.targetedPiece = currentPiece; // The piece we plan to move
                externalTile.targetedPieceTile = gameObject;
            }
            else
            {
                externalTile.isActive = false;
            }
        }        
    }

    void BlackPawn_ShowLegalMoves(ulong ownSide)
    {
        ulong pawn_pos = ((ulong)1 << tileId);
        ulong res = GameModel.black_pawn_legal_moves(pawn_pos,ownSide);   

        foreach (var tile in GenerateBoard.GameTiles)
        {
            TileBehaviour externalTile = tile.GetComponent<TileBehaviour>();
            if ( (res & ((ulong)1 << externalTile.tileId) ) != 0 ) // That tile has to be active
            {                
                externalTile.isActive = true;
                externalTile.targetedPiece = currentPiece; // The piece we plan to move
                externalTile.targetedPieceTile = gameObject;
            }
            else
            {
                externalTile.isActive = false;
            }
        }        
    }

}
