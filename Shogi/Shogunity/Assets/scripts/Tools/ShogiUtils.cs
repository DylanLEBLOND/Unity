using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Text;
using System;
using System.IO;
using System.Xml.Serialization;

namespace ShogiUtils
{

	/// <summary>
	/// Classe contenant les couleurs des surbrillances.
	/// </summary>
	public static class HighlightColor
	{
		public static Color red = new Color(0.76f, 0.28f, 0.19f);
		public static Color blue = new Color(0.27f, 0.31f, 0.63f);
		public static Color green = new Color(0.30f, 0.70f, 0.39f);
	}

	/// <summary>
	/// Enumération caractérisant les environs locaux.
	/// </summary>
	public enum Neighbor
	{
		TOP, LEFT, BOTTOM, RIGHT,
		TOP_LEFT, TOP_RIGHT, BOTTOM_RIGHT, BOTTOM_LEFT
	};

	/// <summary>
	/// Enumération caractérisant la couleur des joueurs.
	/// </summary>
	public enum GameColor
	{
		SENTE, GOTE
	};

	/// <summary>
	/// Enumération caractérisant les modes de jeu.
	/// </summary>
	public enum GameMode
	{
		PLAYER_VS_PLAYER, PLAYER_VS_AI, AI_VS_AI, PLAYER_VS_PLAYER_NETWORK, UNDEFINED
	};

	/// <summary>
	/// Enumération caractérisant le type de joueur.
	/// </summary>
	public enum PlayerType
	{
		PLAYER, GLADIATOR, PREDATOR, TERMINATOR, SECRET, UNDEFINED
	};

	/// <summary>
	/// Classe représentative d'un joueur.
	/// </summary>

	/// <summary>
	/// Classe de coordonnées bidimentionnelle.
	/// </summary>

	public class Coordinates {

		/// <summary>
		/// Coordonnée x.
		/// </summary>
		public int x;

		/// <summary>
		/// Coordonnée y.
		/// </summary>
		public int y;


		/// <summary>
		/// Nouvelle instance de <see cref="ShogiUtils.Coordinates"/>.
		/// </summary>
		public Coordinates() {
			this.x = 0;
			this.y = 0;
		}

		/// <summary>
		/// Nouvelle instance de <see cref="ShogiUtils.Coordinates"/> en fonction d'une autre instance.
		/// </summary>
		public Coordinates(Coordinates coord) {
			x = coord.x;
			y = coord.y;
		}

		/// <summary>
		/// Nouvelle instance de <see cref="ShogiUtils.Coordinates"/> en fonction d'un x et d'un y passés en paramètre.
		/// </summary>
		public Coordinates(int x, int y) {
			this.x = x;
			this.y = y;
		}

		/// <summary>
		/// Définit un nouveau couple x et y.
		/// </summary>
		/// <param name="x">Coordonée x.</param>
		/// <param name="y">Coordonnée y.</param>
		public void set(int x, int y) {
			this.x = x;
			this.y = y;
		}

		/// <summary>
		/// Définit un nouveau couple x et y en fonction d'autres coordonnées.
		/// </summary>
		/// <param name="coord">Coordonées.</param>
		public void set(Coordinates coord) {
			this.x = coord.x;
			this.y = coord.y;
		}

		/// <summary>
		/// Vérifie si les coordonnées sont comprises dans une surface.
		/// </summary>
		/// <returns><c>vrai</c>, Si comprises dedans, <c>faux</c> sinon.</returns>
		/// <param name="top">Y max.</param>
		/// <param name="left">X Min.</param>
		/// <param name="bottom">Y Min.</param>
		/// <param name="right">X Max.</param>
		public bool isInsideBorders(int top=8, int left=0, int bottom=0, int right=8) {
			return (y<=top) && (x>=left) && (y>=bottom) && (x<=right);
		}

		/// <summary>
		/// Teste si deux coordonnées sont égales.
		/// </summary>
		/// <param name="coord">Coordonnées à tester.</param>
		public bool equals(Coordinates coord) {
			return (coord.x == x) && (coord.y == y);
		}

		/// <summary>
		/// Coordonnées voisines
		/// </summary>
		/// <returns>Les coordonées voisines.</returns>
		/// <param name="neighbor">Sens relative vers où regarder.</param>
		/// <param name="color">Couleur de joueur.</param>
		public Coordinates getNeighbor(Neighbor neighbor, GameColor color) {
			int a = (color == GameColor.SENTE) ? 1 : -1;
			switch(neighbor) {
			case Neighbor.TOP:
				return new Coordinates(x, y+a);
			case Neighbor.LEFT:
				return new Coordinates(x-1, y);
			case Neighbor.BOTTOM:
				return new Coordinates(x, y-a);
			case Neighbor.RIGHT:
				return new Coordinates(x+1, y);
			case Neighbor.TOP_LEFT:
				return new Coordinates(x-1, y+a);
			case Neighbor.TOP_RIGHT:
				return new Coordinates(x+1, y+a);
			case Neighbor.BOTTOM_RIGHT:
				return new Coordinates(x+1, y-a);
			case Neighbor.BOTTOM_LEFT:
				return new Coordinates(x-1, y-a);
			default:
				return new Coordinates(0, 0);
			}
		}

		/// <summary>
		/// Retourne une <see cref="System.String"/> qui représent le <see cref="ShogiUtils.Coordinates"/> courant.
		/// </summary>
		/// <returns>Une <see cref="System.String"/> qui représent le <see cref="ShogiUtils.Coordinates"/> courant.</returns>
		public override string ToString () {
			return string.Format ("(x=" + x + ", y=" + y + ")");
		}

		/// <summary>
		/// L'index unidimentionnel en fonction de ces coordonnées.
		/// </summary>
		/// <returns>The index.</returns>
		public int getIndex() {
			return 9*y + x;
		}

		/// <summary>
		/// Supprime les doublons d'une liste de coordonnées.
		/// </summary>
		/// <returns>Une liste de coordonnées sans doublons.</returns>
		/// <param name="coordinates">Une liste de coordonnées.</param>
		public static List<Coordinates> removeDuplicates(List<Coordinates> coordinates) {
			var d = coordinates.GroupBy (a => new {a.x, a.y}).ToList();
			return d.Select (group => group.First()).ToList();
		}

		/// <summary>
		/// Affichage dans la console de d'un liste de coordonnées.
		/// </summary>
		/// <param name="coordinates">Liste de coordonnées.</param>
		public static void debugList(List<Coordinates> coordinates) {
			StringBuilder sb = new StringBuilder ();
			foreach(Coordinates c in coordinates) {
				sb.Append(c.ToString() + "\n");
			}
			Debug.Log(sb.ToString());
		}
	}

	/// <summary>
	/// Classe caractérisant un mouvement.
	/// </summary>

	[Serializable]
	public class  Move{

		/// <summary>
		/// Nom du joueur.
		/// </summary>
		[XmlAttribute]
		public string playerName;

		/// <summary>
		/// Identifiant de la pièce
		/// </summary>
		[XmlAttribute]
		public int tokenID;

		/// <summary>
		/// Coordonnée de départ x.
		/// </summary>
		[XmlAttribute]
		public int startCoordX;

		/// <summary>
		/// Coordonnée de départ y.
		/// </summary>
		[XmlAttribute]
		public int startCoordY;

		/// <summary>
		/// Coordonnée de destination x.
		/// </summary>
		[XmlAttribute]
		public int destinationCoordX;

		/// <summary>
		/// Coordonnée de destination y.
		/// </summary>
		[XmlAttribute]
		public int destinationCoordY;

		/// <summary>
		/// Etat de promotion de la pièce.
		/// </summary>
		[XmlAttribute]
		public bool isPromoted;
		/*[NonSerialized]
		public Token token;
		[NonSerialized]
		public Player player;
		[NonSerialized]
		public Box startBox;
		[NonSerialized]
		public Box destinationBox;*/

		/// <summary>
		/// Initialise une nouvelle instance de <see cref="ShogiUtils.Move"/>.
		/// </summary>

		public Move() {}

		/// <summary>
		/// Initialise une nouvelle instance de <see cref="ShogiUtils.Move"/> en fonction de :
		/// 	- Une pièce.
		/// 	- Un joueur.
		/// 	- Une case de départ.
		/// 	- Une case d'arrivée.
		/// </summary>
//		public Move(Token token,Player player,Box startBox, Box destinationBox)
//		{
//			this.tokenID = token.id;
//			this.playerName = player.name;
//			this.startCoordX = startBox.coord.x;
//			this.startCoordY = startBox.coord.y;
//			this.destinationCoordX = destinationBox.coord.x;
//			this.destinationCoordY = destinationBox.coord.y;
//		}

	/*	public Move(Player player,Token token,Box startBox,Box destinationBox){
			this.player = player;
			this.token = token;
			this.startBox = startBox;
			this.destinationBox = destinationBox;
		}*/

		/// <summary>
		/// Initialise une nouvelle instance de <see cref="ShogiUtils.Move"/> en fonction de :
		/// 	- Un nom de joueur.
		/// 	- Un état de promotion.
		/// 	- Un identifiant de pièce.
		/// 	- De coordonnées de départ.
		/// 	- De coordonnées d'arrivée.
		/// </summary>

		public Move(string playerName,bool isPromoted,int tokenID,Coordinates startCoord,Coordinates destinationCoord){
			this.playerName = playerName;
			this.tokenID = tokenID;
			this.startCoordX = startCoord.x;
			this.startCoordY = startCoord.y;
			this.destinationCoordX = destinationCoord.x;
			this.destinationCoordY = destinationCoord.y;
			this.isPromoted = isPromoted;
		}

		/// <summary>
		/// Joue le mouvement sur le jeu.
		/// </summary>

		public void play()
		{
			#if false
			List<Token> tokens = _GameManager.instance.tokens;
			foreach (Token t in tokens) {
				if(t.id==this.tokenID){
					//Promotion
					if(this.startCoordX==this.destinationCoordX && this.startCoordY==this.destinationCoordY && this.isPromoted){
						t.promote();
						Debug.Log ("Move done : id="+tokenID+" promotion");
					}
					else if(this.startCoordX==this.destinationCoordX && this.startCoordY==this.destinationCoordY && this.isPromoted==false){
						Debug.Log ("Move done : id="+tokenID+" but no promotion");
					}
					else if(this.destinationCoordX==(-100) && this.destinationCoordY==(-100)){
						Debug.Log ("Capture move : id="+tokenID);
					}
					
					else{
						//t.OnMouseDown();
						t.selected=true;
						_GameManager.selectedToken=t;
						foreach(Box b in _GameManager.instance.boxes){
							if(b.coord.x==destinationCoordX && b.coord.y==destinationCoordY){
								_GameManager.instance.moveToken(b);
								Debug.Log ("Move done : id="+tokenID+" destX="+destinationCoordX+" destY="+destinationCoordY);
							}
						}					
					}
				}
			}
			Debug.Log (this.ToString ());
			#endif
		}

		/// <summary>
		/// Retourne une <see cref="System.String"/> qui représente le <see cref="ShogiUtils.Move"/> courant.
		/// </summary>
		/// <returns>Une <see cref="System.String"/> qui représente le  <see cref="ShogiUtils.Move"/> courant.</returns>
		override public string ToString()
		{
			string str = "is tostring from Move id : "+this.tokenID.ToString();
			str += "\n playerName: " + this.playerName;
			str += "\n startCoordX: " + this.startCoordX.ToString();
			str += "\n startCoordY: " + this.startCoordY.ToString();
			str += "\n destCoordX: " + this.destinationCoordX.ToString ();;
			str += "\n destCoordY: " + this.destinationCoordY.ToString();
			return str;
		}
		               
	}

	public class Pair<T, K> {
		
		public T e1;
		public K e2;
		
		public Pair(T e1, K e2) {
			this.e1 = e1;
			this.e2 = e2;
		}
	}
}