using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class SaveLoad {
	
	public static List<Level> savedLevels = new List<Level>();

	//it's static so we can call it from anywhere
	public static void Save() {
		BinaryFormatter bf = new BinaryFormatter();

		//Application.persistentDataPath is a string, so if you wanted you can put that into debug.log if you want to know where save games are located
		FileStream file = File.Create (Application.persistentDataPath + "/savedLevels.gd"); //you can call it anything you want
		bf.Serialize(file, SaveLoad.savedLevels);
		file.Close();
	}   
	
	public static void Load() {
		if(File.Exists(Application.persistentDataPath + "/savedLevels.gd")) {
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/savedLevels.gd", FileMode.Open);
			SaveLoad.savedLevels = (List<Level>)bf.Deserialize(file);
			file.Close();
		}
	}



}