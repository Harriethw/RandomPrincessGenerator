using UnityEngine;
using System.Collections;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(PixelCharacter))] 

public class PixelCharacterEditor : Editor {

	private PixelCharacter character;
	private static int numOfCharacters = 1;
	private static float defaultLabelWidth;
	private static bool headStyleFoldout = false;
	private static bool bodyStyleFoldout = false;
	private static bool legsStyleFoldout = false;
	GUIStyle myFoldoutStyle;

	private void OnEnable(){
		character = (PixelCharacter)target;
		defaultLabelWidth = EditorGUIUtility.labelWidth;
	}

	private void SetFoldoutStyle(){
		myFoldoutStyle = new GUIStyle(EditorStyles.foldout);
		Color myStyleColor = Color.black;
		myFoldoutStyle.fontStyle = FontStyle.Bold;
		myFoldoutStyle.normal.textColor = myStyleColor;
		myFoldoutStyle.onNormal.textColor = myStyleColor;
		myFoldoutStyle.hover.textColor = myStyleColor;
		myFoldoutStyle.onHover.textColor = myStyleColor;
		myFoldoutStyle.focused.textColor = myStyleColor;
		myFoldoutStyle.onFocused.textColor = myStyleColor;
		myFoldoutStyle.active.textColor = myStyleColor;
		myFoldoutStyle.onActive.textColor = myStyleColor;
	}

	public override void OnInspectorGUI(){
		SerializedObject obj = this.serializedObject;
		obj.Update();

		SetFoldoutStyle();

		EditorGUILayout.Space();
		EditorGUILayout.LabelField("Shape Creation", EditorStyles.boldLabel);
		if(GUILayout.Button("Open Shape Creator")){
			ShapeCreatorWindow.ShowWindow(character);
		}

		EditorGUILayout.Space();
		EditorGUILayout.LabelField("Character Generation", EditorStyles.boldLabel);
		if(obj.FindProperty("head").FindPropertyRelative("shape").FindPropertyRelative("isNull").boolValue 
			|| obj.FindProperty("body").FindPropertyRelative("shape").FindPropertyRelative("isNull").boolValue 
			|| obj.FindProperty("legs").FindPropertyRelative("shape").FindPropertyRelative("isNull").boolValue 
			|| obj.FindProperty("skinColors").arraySize == 0) {
				EditorGUILayout.HelpBox("All Body Parts And A Skin Color Is Required Meanwhile The Rest Is Optional", MessageType.Warning);
		}
		else{
			EditorGUILayout.BeginHorizontal();
			if(GUILayout.Button("Build")){
				character.Draw();
			}
			if(character.GetComponent<SpriteRenderer>().sprite != null){
				if(GUILayout.Button("Save Texture")){
					foreach(PixelCharacter p in targets){
						PixelCharacterDrawTool.Save(p.GetComponent<SpriteRenderer>().sprite.texture, p.gameObject.name);
					}
				}
			}
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Multiple Characters", EditorStyles.centeredGreyMiniLabel);
			EditorGUILayout.BeginHorizontal();
			numOfCharacters = EditorGUILayout.IntField("Amount", numOfCharacters);
			if(GUILayout.Button("Build")){
				character.Draw();
				float xSpacing = character.GetComponent<SpriteRenderer>().bounds.size.x * 1.2f;
				float ySpacing = character.GetComponent<SpriteRenderer>().bounds.size.y * 1.2f;
				int rows = (int)Mathf.Sqrt(numOfCharacters);
				int columns = Mathf.CeilToInt(numOfCharacters / (float)rows);
				int n = 1;
				GameObject clones = new GameObject(character.name + " Clones");
				for(int y = 0; y < rows; y++){
					for(int x = 0; x < columns; x++){
						if(n <= numOfCharacters){
							Vector3 pos = new Vector3(character.gameObject.transform.position.x + xSpacing*2 + xSpacing*x, character.gameObject.transform.position.y - ySpacing*y, 0f);
							GameObject clone = Instantiate(character.gameObject, pos, Quaternion.identity) as GameObject;
							clone.transform.parent = clones.transform;
							clone.name = character.name/* + " " + (n)*/;
							clone.GetComponent<PixelCharacter>().Draw();
							n++;
						}
					}
				}
				character.Draw();
			}
			EditorGUILayout.EndHorizontal();
		}

		EditorGUILayout.Space();
		EditorGUILayout.LabelField("Body Parts", EditorStyles.boldLabel);
		EditorGUIUtility.labelWidth = 60;
		ShowBodyPart(character.head);
		ShowBodyPart(character.body);
		ShowBodyPart(character.legs);
		EditorGUIUtility.labelWidth = defaultLabelWidth;

		EditorGUILayout.Space();
		EditorGUILayout.LabelField("Skin Colors");
		ShowColorArray(obj.FindProperty("skinColors"));

		GUILayout.Space(20);
		EditorGUILayout.Space();
		headStyleFoldout = EditorGUILayout.Foldout(headStyleFoldout, "Head Style", myFoldoutStyle);
		if(headStyleFoldout){
			ShowBodyPartStyles(character.head, obj.FindProperty("head"), ref character.tempHeadLayerName);
		}
		GUILayout.Space(20);
		EditorGUILayout.Space();
		bodyStyleFoldout = EditorGUILayout.Foldout(bodyStyleFoldout, "Body Style", myFoldoutStyle);
		if(bodyStyleFoldout){
			ShowBodyPartStyles(character.body, obj.FindProperty("body"), ref character.tempBodyLayerName);
		}
		GUILayout.Space(20);
		EditorGUILayout.Space();
		legsStyleFoldout = EditorGUILayout.Foldout(legsStyleFoldout, "Legs Style", myFoldoutStyle);
		if(legsStyleFoldout){
			ShowBodyPartStyles(character.legs, obj.FindProperty("legs"), ref character.tempLegsLayerName);
		}

		obj.ApplyModifiedProperties();
	}

	private void ShowBodyPart(PixelCharacter.BodyPart bodyPart){
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField(bodyPart.name);

		if(bodyPart.shape.isNull){
			if(GUILayout.Button("Open Creator")){
				ShapeCreatorWindow.ShowWindow(character);
			}
		}
		else{
			if(GUILayout.Button("Edit")){
				ShapeCreatorWindow.ShowWindow(character, bodyPart);
			}
			if(GUILayout.Button("Delete")){
				character.DeleteBodyPart(bodyPart);
			}
		}
		EditorGUILayout.EndHorizontal();
	}

	private void ShowBodyPartStyles(PixelCharacter.BodyPart bodyPart, SerializedProperty bodyPartProp, ref string tempNewStyleString){
		EditorGUILayout.LabelField("New Style Layer", EditorStyles.centeredGreyMiniLabel);
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.Space();
		EditorGUIUtility.labelWidth = 40;
		tempNewStyleString = EditorGUILayout.TextField("Name", tempNewStyleString);
		if(GUILayout.Button("Add Layer")){
			bodyPart.styleLayers.Add(new PixelCharacter.StyleLayer(tempNewStyleString));
			tempNewStyleString = "";
		}
		EditorGUILayout.Space();
		EditorGUILayout.EndHorizontal();
		EditorGUIUtility.labelWidth = defaultLabelWidth;
		GUILayout.Space(20);
		for(int i = 0; i < bodyPartProp.FindPropertyRelative("styleLayers").arraySize; i++){
			EditorGUILayout.LabelField(bodyPart.styleLayers[i].name, EditorStyles.centeredGreyMiniLabel);
			PixelCharacter.StyleLayer temp = bodyPart.styleLayers[i];
			temp.drawProbability = EditorGUILayout.Slider("Draw Probability", temp.drawProbability, 0f, 1f);
			bodyPart.styleLayers[i] = temp;
			EditorGUILayout.Space();
			ShowColorArray(bodyPartProp.FindPropertyRelative("styleLayers").GetArrayElementAtIndex(i).FindPropertyRelative("colors"));
			EditorGUILayout.Space();
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.Space();
			if(i != bodyPartProp.FindPropertyRelative("styleLayers").arraySize - 1){
				if(GUILayout.Button("Move Down")){
					bodyPart.styleLayers[i] = bodyPart.styleLayers[i+1];
					bodyPart.styleLayers[i+1] = temp;	
				}
			}
			if(i != 0){
				if(GUILayout.Button("Move Up")){
					bodyPart.styleLayers[i] = bodyPart.styleLayers[i-1];
					bodyPart.styleLayers[i-1] = temp;
				}
			}
			if(GUILayout.Button("Remove Layer")){
				bodyPart.styleLayers.RemoveAt(i);
			}
			EditorGUILayout.Space();
			EditorGUILayout.EndHorizontal();
			GUILayout.Space(20);
		}
	}

	private void ShowColorArray(SerializedProperty prop){
		if(prop.arraySize == 0){
			prop.arraySize += 1;
			prop.GetArrayElementAtIndex(prop.arraySize-1).colorValue = PixelCharacterDrawTool.RandomColor();
		}

		for(int i = 0; i < prop.arraySize; i++){
			EditorGUILayout.PropertyField(prop.GetArrayElementAtIndex(i), new GUIContent("   ● Color " + (i+1)));
		}

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.Space();
		if(GUILayout.Button("Add Color")){
			prop.arraySize += 1;
			prop.GetArrayElementAtIndex(prop.arraySize-1).colorValue = PixelCharacterDrawTool.RandomColor();
		}
		if(prop.arraySize >= 2){
			if(GUILayout.Button("Remove Color")){
				prop.arraySize -= 1;
			}
		}

		EditorGUILayout.Space();
		EditorGUILayout.EndHorizontal();
	}
}
