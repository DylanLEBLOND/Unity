using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Text;
using System;
using System.IO;
using System.Xml.Serialization;
using ShogiUtils;

namespace ShogiData {

	/// <summary>
	/// Classe sauvegarde des scores.
	/// </summary>

	public class ScoreSave {

		/// <summary>
		/// Chemin du fichier des scores.
		/// </summary>
		public static string path = "data/score/";

		/// <summary>
		/// Nom du ficher des scores.
		/// </summary>
		public static string filename = path + "highscores.xml";

		// méthodes

		/// <summary>
		/// Création d'un nouveau dossier si il n'y à pas de dossier de sauvegarde des scores.
		/// </summary>
		public static void createDirectoryIfNotExists() {
			if (!Directory.Exists(path))
				Directory.CreateDirectory(path);
			if (!Directory.Exists("data/save/"))
				Directory.CreateDirectory("data/save/");
		}

		/// <summary>
		/// Sauvegarde du score.
		/// </summary>
		/// <param name="s">Un score.</param>
		public static void save(Score s) {
			List<Score> scores;
			XmlSerializer serializer = new XmlSerializer(typeof(List<Score>));

			// le fichier n'existe pas encore
			if (!File.Exists(filename)) {
				scores = new List<Score>();
				scores.Add(s);
				using (FileStream fs = File.Open(filename, FileMode.Create, FileAccess.Write)) {
					serializer.Serialize(fs, scores);
				}
			}

			// le fichier existe déjà
			else {
				using (FileStream fs = File.Open(filename, FileMode.Open, FileAccess.Read)) {
					scores = (List<Score>) serializer.Deserialize(fs);
					scores.Add(s);
				}
				File.WriteAllText(filename, string.Empty);
				using (FileStream fs = File.Open(filename, FileMode.Open, FileAccess.Write)) {
					serializer.Serialize(fs, scores);
				}
			}
		}


		/// <summary>
		/// Chargement des socres.
		/// </summary>
		public static List<Score> load() {
			List<Score> scores = null;
			XmlSerializer serializer = new XmlSerializer(typeof(List<Score>));
			
			// le fichier existe
			if (File.Exists(filename)) {
				using (FileStream fs = File.Open(filename, FileMode.Open, FileAccess.Read)) {
					scores = (List<Score>) serializer.Deserialize(fs);
				}
			}
			truncate(ref scores);
			return scores;
		}

		/// <summary>
		/// Trie des scores.
		/// </summary>
		/// <param name="list">Une liste de scores.</param>
		public static void sortDesc(ref List<Score> list) {
			if (list != null) {
				var d = list.OrderByDescending(x => x.score).ToList();
				list = d.ToList();
			}
		}

		/// <summary>
		/// Sauvegarde du joueur, temps de jeu et nombre de mouvements adverses.
		/// </summary>
		/// <param name="player">Un joueur.</param>
		/// <param name="timer">Un timer.</param>
		/// <param name="opponentMoves">Le nombre de mouvement du joueur adverse.</param>
		public static void save(Player player, int opponentMoves)
		{
			if (player != null && player.getPlayerType() == PlayerType.PLAYER && _GameConfig.instance.gameMode == GameMode.PLAYER_VS_AI)
			{
				save(new Score(player.getName(), _GameManager.instance.player1Score, 10.ToString(),  _GameManager.instance.player1MovesCount + opponentMoves));
			}
		}

		/// <summary>
		/// Tronque la liste des score aux 50 meilleurs.
		/// </summary>
		/// <param name="list">Une liste de scores.</param>
		public static void truncate(ref List<Score> list) {
			int limit = 50;
			if (list != null && list.Count > limit) {
				sortDesc(ref list);
				for (int i=limit+1; i<list.Count; i++) {
					list.Remove(list[i]);
				}
			}
		}
	}

	/// <summary>
	/// Classe score.
	/// </summary>

	[Serializable]
	public class Score {
		
		// attributs
		/// <summary>
		/// Nom du joueur.
		/// </summary>
		public String name;

		/// <summary>
		/// Valeur du score.
		/// </summary>
		public int score;

		/// <summary>
		/// Temps de jeu.
		/// </summary>
		public String time;

		/// <summary>
		/// Nombre de mouvements.
		/// </summary>
		public int moves;

		// constructeurs

		/// <summary>
		/// Initialise une nouvelle instance de <see cref="ShogiData.Score"/>.
		/// </summary>
		public Score() {}

		/// <summary>
		/// Initialise une nouvelle instance de <see cref="ShogiData.Score"/> en fonction d'un nom de joueur, d'une valeur de score, d'un temps de jeu et d'un nombre de mouvements.
		/// </summary>
		public Score(String name, int score, String time, int moves) {
			this.name = name;
			this.score = score;
			this.time = time;
			this.moves = moves;
		}
	}


	/// <summary>
	/// Classe sauvegarde de partie.
	/// </summary>
	/// 
	[Serializable]
	public class GameSave
	{
		/// <summary>
		/// Indentifiant de sauvegarde.
		/// </summary>
		[XmlAttribute]
		public int id;

		/// <summary>
		/// Nom du joueur.
		/// </summary>
		[XmlAttribute]
		public string playerName;

		/// <summary>
		/// Nombre de mouvements.
		/// </summary>
		[XmlAttribute]
		public int playerMovesCount;

		/// <summary>
		/// Date.
		/// </summary>
		[XmlAttribute]
		public string date;

		/// <summary>
		/// Temps.
		/// </summary>
		[XmlAttribute]
		public string time;

		/// <summary>
		/// Nom du ficher.
		/// </summary>
		[XmlAttribute]
		public string fileName;

		/// <summary>
		/// Liste des mouvements.
		/// </summary>
		[XmlArrayItem("Move")]
		public List<Move> moves;
		//public Button clickEvent;
		
		
		//[NonSerialized()] 
		/// <summary>
		/// Retour sur repirse de mouvement.
		/// </summary>
		private Move redo;

		/// <summary>
		/// Mode de jeu
		/// </summary>
		[XmlAttribute] 
		public GameMode gameMode;
		/// <summary>
		/// Initialise une nouvelle instance de <see cref="ShogiData.GameSave"/>.
		/// </summary>
		public GameSave() {
			fileName="Nothing.xml";
		}

		/// <summary>
		/// Initialise une nouvelle instance de <see cref="ShogiData.GameSave"/> en fonction d'un nom de ficher.
		/// </summary>
		public GameSave(string filename) {
			this.fileName = filename;
		}

		/// <summary>
		/// Affecte un retour sur reprise de mouvement.
		/// </summary>
		/// <param name="move">Move.</param>
		public void setRedo(Move move) {
			this.redo = move;
		}

		/// <summary>
		/// Retourne un retour sur reprise de mouvement.
		/// </summary>
		/// <returns>Un retour sur reprise de mouvement.</returns>
		public Move getRedo() {
			return this.redo;
		}

		/// <summary>
		/// Affecte une liste de mouvements aux mouvements de la sauvegarde.
		/// </summary>
		/// <param name="moves">Une liste de mouvements.</param>
		public void setMoves(List<Move> moves) {
			this.moves = moves;
		}

		/// <summary>
		/// Retourne la liste des mouvements.
		/// </summary>
		/// <returns>Retour de la liste des mouvements.</returns>
		public List<Move> getMoves() {
			return this.moves;
		}
		
		/// <summary>
		/// Sauvegarde la partie dans un fichier.
		/// </summary>
		/// <param name="filename">Nom de ficher.</param>
		public void save(string filename){
			XmlSerializer serializer = new XmlSerializer(typeof(GameSave));
			StreamWriter save = new StreamWriter(filename);
			serializer.Serialize (save, this);
			save.Close();
		}

		/// <summary>
		/// Chargement d'une partie.
		/// </summary>
		/// <param name="filename">Nom du fichier.</param>
		public static GameSave load(string filename){
			XmlSerializer deserializer = new XmlSerializer(typeof(GameSave));
			StreamReader load = new StreamReader(filename);
			GameSave myGameSave = new GameSave ();
			myGameSave = (GameSave)deserializer.Deserialize(load);
			load.Close ();
			return myGameSave;
		}

		/// <summary>
		/// Jouer tous les mouvements de la liste.
		/// </summary>
		public void playAllMoves() {
			foreach (Move move in moves) {
				move.play();
			}
		}
	
		/// <summary>
		/// Ajoute un nouveau mouvement à la liste.
		/// </summary>
		/// <param name="mov">Mov.</param>
		public void Add(Move mov)
		{
			if (moves == null) {
				moves = new List<Move> ();
				moves.Add(mov);
			}
			else
				moves.Add(mov);
		}
	}

	/// <summary>
	/// Classe liste de sauvegardes du jeu.
	/// </summary>
	[Serializable]
	public class GameSaves:List<GameSave>
	{

		/// <summary>
		/// Sauvegardes vers un chemin spéficifié.
		/// </summary>
		/// <param name="chemin">Un chemin.</param>
		public void save(string chemin)
		{
			XmlSerializer serializer = new XmlSerializer(typeof(GameSaves));
			StreamWriter ecrivain = new StreamWriter(chemin);
			serializer.Serialize(ecrivain, this);
			ecrivain.Close();
		}

		/// <summary>
		/// Chargements à partir d'un chemin particulier.
		/// </summary>
		/// <param name="chemin">Un chemin.</param>
		public static GameSaves load(string chemin)
		{
			XmlSerializer deserializer = new XmlSerializer(typeof(GameSaves));
			StreamReader lecteur = new StreamReader(chemin);
			GameSaves p = (GameSaves)deserializer.Deserialize(lecteur);
			lecteur.Close();
			
			return p;
		}
	}

}