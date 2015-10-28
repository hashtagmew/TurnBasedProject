using UnityEngine;
using UnityEditor;

using System.Collections;
using System.Collections.Generic;

using System.Xml;
using System.Xml.Linq;
using System.Linq;

using UnityEngine.UI;

using System.Text;

public static class DiskLoadManager {

	static private XDocument s_xmlDoc;

	static public void LoadUnitStats(out GameUnit gu, string path) {
		gu = null;
	}

}
