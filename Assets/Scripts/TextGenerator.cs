using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TextGenerator : MonoBehaviour {

	public Text storyText;
	public Text nameText;
	public TextAsset bodyText;
	public TextAsset adjectives;
	public TextAsset places;
	public TextAsset colors;
	public TextAsset nouns;
	public TextAsset actions;
	public TextAsset locations;
	public TextAsset descriptives;
	public TextAsset negatives;
	public TextAsset positives;
	public TextAsset materials;
	public TextAsset animals;
	public TextAsset good;
	public TextAsset herbs;
	public TextAsset feelings;
	public string[] bodyArray;
	public string[] adjArray;
	public string[] placeArray;
	public string[] colorArray;
	public string[] nounArray;
	public string[] actArray;
	public string[] locArray;
	public string[] descriptArray;
	public string[] animalArray;
	public string[] goodArray;
	public string[] posArray;
	public string[] herbArray;
	public string[] negArray;
	public string[] materialArray;
	public string[] feelArray;
	public string[] pronouns = { "she", "he", "they" };
	public string[] prenouns = { "her", "his", "their" };
	public string[] prenounsex = { "she is", "he is", "they are" };
	public string[] pronoun = { "her", "him", "them" };
	public string[] weapons = { "wand", "axe", "hat", "dress", "fork", "sword"  };

	public PixelCharacter character;


	// Use this for initialization
	void Start () {

		bodyArray = (bodyText.text.Split ('\n'));
		adjArray = (adjectives.text.Split ('\n'));
		placeArray = (places.text.Split ('\n'));
		colorArray = (colors.text.Split ('\n'));
		nounArray = (nouns.text.Split ('\n'));
		actArray = (actions.text.Split ('\n'));
		locArray = (locations.text.Split ('\n'));
		descriptArray = (descriptives.text.Split ('\n'));
		animalArray = (animals.text.Split ('\n'));
		goodArray = (good.text.Split ('\n'));
		posArray = (positives.text.Split ('\n'));
		herbArray = (herbs.text.Split ('\n'));
		negArray  = (negatives.text.Split ('\n'));
		materialArray = (materials.text.Split ('\n'));
		feelArray = (feelings.text.Split ('\n'));


	}

	public void Generate (){

		character.Draw ();

		string bodyPart = bodyArray [Random.Range (0, bodyArray.Length)];
		string adjective = adjArray [Random.Range (0, adjArray.Length)];
		string colour = colorArray [Random.Range (0, colorArray.Length)];
		string noun = nounArray [Random.Range (0, nounArray.Length)];
		string act = actArray [Random.Range (0, actArray.Length)];
		string prenoun = prenouns [Random.Range (0, prenouns.Length)];
		string prenounex = prenounsex [Random.Range (0, prenounsex.Length)];
		string location = locArray [Random.Range (0, locArray.Length)];
		string descript = descriptArray [Random.Range (0, descriptArray.Length)];
		string material = materialArray [Random.Range (0, materialArray.Length)];
		string good = goodArray [Random.Range (0, goodArray.Length)];
		string animal = animalArray [Random.Range (0, animalArray.Length)];
		string positive = posArray [Random.Range (0, posArray.Length)];
		string place = placeArray [Random.Range (0, placeArray.Length)];
		string feeling = feelArray [Random.Range (0, feelArray.Length)];
		string negative = negArray [Random.Range (0, negArray.Length)];
		string herb = herbArray [Random.Range (0, herbArray.Length)];
		string weapon = weapons [Random.Range (0, weapons.Length)];

		nameText.text = noun + " princess";

		int storypick = (Random.Range (1, 6));

		if (storypick == 1) {

			storyText.text = noun + " princess is a " + adjective + " individual who lives in a place called " + location +
			". Best known for " + prenoun + " " + colour + " " + bodyPart + ", which manifests magical powers, " +
			"all the better to " + act + " with others.";
		}
		if (storypick == 2) {
		
			storyText.text = noun + " princess is a " + descript + " person, who brews magical tea with " + colour + " " + material +
			". " + prenounex + " best friends with a " + good + " " + animal + " and enjoys being " + positive +
			" all over the kingdom of " + location + ".";

		} if (storypick == 3) {

			storyText.text = noun + " princess lives " + place + " " + descript + " mountain in the "
				+ negative + " province of " + location + ". " + prenounex + " in love with a large " + colour + 
				" " + animal + ", and they often fight off " +  negative + " " + herb + " fairies together.";
		} if (storypick == 4) {
		
			storyText.text = noun + " princess loves to show off " + prenoun + " " + colour + " castle, nestled in the mountains of " +
			location + ". Together with Lord " + animal + " they like to create sculptures that make you feel " + feeling +
			".";
			
		}if (storypick == 5) {

			storyText.text = noun + " princess has a " + positive + " " + weapon + " with which " + prenounex + " learning to cast spells over an army of " +
				animal + "s.";
		}
		
		}
	
				
		

		}

	



