using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Xml.Serialization;
using Tools;

namespace Tools
{
	public class SerializeNode
	{
		public static void Serialize (Node node, string fileName)
		{
			XmlSerializer xmlNode = new XmlSerializer (typeof (Node));

			StreamWriter streamNode = new StreamWriter (fileName, false);
			xmlNode.Serialize (streamNode, node);
			streamNode.Close ();
		}

		public static void SerializeList (List<Node> list, string fileName)
		{
			XmlSerializer xmlNode = new XmlSerializer (typeof (List<Node>));

			StreamWriter streamNode = new StreamWriter (fileName, false);
			xmlNode.Serialize (streamNode, list);
			streamNode.Close ();
		}

		public static Node Deserialize (string fileName)
		{
			XmlSerializer xmlNode = new XmlSerializer (typeof (Node));

			StreamReader revertStream = new StreamReader (fileName);
			Node obj = (Node)xmlNode.Deserialize (revertStream);

			revertStream.Close ();
			return obj;
		}

		public static List<Node> DeserializeList (string fileName)
		{
			XmlSerializer xmlNode = new XmlSerializer (typeof (List<Node>));

			StreamReader revertStream = new StreamReader (fileName);
			List<Node> obj = (List<Node>)xmlNode.Deserialize (revertStream);

			revertStream.Close ();

			return obj;
		}
	}
}

