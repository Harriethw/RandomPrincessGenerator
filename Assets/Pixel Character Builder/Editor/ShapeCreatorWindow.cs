using UnityEngine;
using System.Collections;
using UnityEditor;

public class ShapeCreatorWindow : EditorWindow {

	private static PixelCharacter character;
	private static PixelCharacter.BodyPart bodyPart;
	private static string shapeName;
	private static PixelTexture shape;
	private static bool variablesSet = false;

	private static int pixelSize = 20;
	private static int shapePosX = 25;
	private static int shapePosY = 25;
	private static int spacing = 25;
	private static int bodyPartSectionSpacing = 50;
	private static int bodyPartSectionWidth = 200;

	private static int headShapeWidth = 5;
	private static int headShapeHeight = 5;
	private static int bodyShapeWidth = 5;
	private static int bodyShapeHeight = 5;
	private static int legShapeWidth = 5;
	private static int legShapeHeight = 5;

	private static int colorPickerPosY;
	private static Color selectedCol = new Color(0.5f, 0.5f, 0.5f, 1f);
	private static int selectedColIndex = 5;

	private static Vector2 headScrollPosition = Vector2.zero;
	private static Vector2 bodyScrollPosition = Vector2.zero;
	private static Vector2 legsScrollPosition = Vector2.zero;

	public static void ShowWindow(PixelCharacter character, PixelCharacter.BodyPart bodyPart){
		EditorWindow.GetWindow(typeof(ShapeCreatorWindow), false, "Shape Creator", true);
		ShapeCreatorWindow.character = character;
		SetVariables(bodyPart, bodyPart.shape, bodyPart.name);
	}

	public static void ShowWindow(PixelCharacter character){
		EditorWindow.GetWindow(typeof(ShapeCreatorWindow), false, "Shape Creator", true);
		ShapeCreatorWindow.character = character;
		variablesSet = false;
	}

	void OnGUI(){
		if(variablesSet){
			DrawShape(shape, shapePosX, shapePosY, shapeName);
			if(shape != bodyPart.shape){
				DrawShape(bodyPart.shape, shapePosX + shape.width*pixelSize + spacing, shapePosY, bodyPart.name);
				DrawBodyPartTileHovering(shapePosX + shape.width*pixelSize + spacing, shapePosY);
				DrawPersonPreview(shapePosX + shape.width*pixelSize + spacing + bodyPart.shape.width*pixelSize + spacing);
			}
			else{
				DrawEmptyShape(shapePosX + shape.width*pixelSize + spacing, shapePosY, shape.width*pixelSize, shape.height*pixelSize);
				DrawPersonPreview(shapePosX + (shape.width*pixelSize + spacing)*2);
			}
			DrawColorPicker(shape.height);
			PaintShape();
		}
		else{
			DrawEmptyShape(shapePosX, shapePosY, 12*pixelSize, 12*pixelSize);
			DrawEmptyShape(shapePosX + 12*pixelSize + spacing, shapePosY, 12*pixelSize, 12*pixelSize);
			DrawPersonPreview(shapePosX + (12*pixelSize + spacing)*2);
			DrawColorPicker(12);
		}

		SelectColor();
		DrawBodyPartSections();

		Repaint();
	}

	private static void SetVariables(PixelCharacter.BodyPart bodyPart, PixelTexture shape, string shapeName){
		ShapeCreatorWindow.bodyPart = bodyPart;
		ShapeCreatorWindow.shape = shape;
		ShapeCreatorWindow.shapeName = shapeName;
		variablesSet = true;
	}

	private void DrawBox(Rect rect, Color col){
		Handles.color = col;
		Handles.DrawLine(new Vector2(rect.xMin, rect.yMin), new Vector2(rect.xMax, rect.yMin));
		Handles.DrawLine(new Vector2(rect.xMax, rect.yMin), new Vector2(rect.xMax, rect.yMax));
		Handles.DrawLine(new Vector2(rect.xMax, rect.yMax), new Vector2(rect.xMin, rect.yMax));
		Handles.DrawLine(new Vector2(rect.xMin, rect.yMax), new Vector2(rect.xMin, rect.yMin));
	}

	private void DrawShape(PixelTexture shape, int posX, int posY, string label){
		GUI.Label(new Rect(posX, posY-20, 200, 20), label, EditorStyles.boldLabel);

		for(int y = 0; y < shape.height; y++){
			for(int x = 0; x < shape.width; x++){
				Rect rect = new Rect(posX + pixelSize*x, posY + pixelSize*(shape.height-y-1), pixelSize, pixelSize);

				if(shape.GetPixel(x,y).a == 0f){
					DrawBox(rect, Color.white);
					continue;
				}

				GUI.color = PixelCharacterDrawTool.GreyColor(shape.GetPixel(x,y).val);
				GUI.DrawTexture(rect, EditorGUIUtility.whiteTexture);
				DrawBox(rect, Color.white);
			}
		}

		DrawBox(new Rect(posX, posY, shape.width*pixelSize, shape.height*pixelSize), Color.white);
	}

	private void DrawEmptyShape(int posX, int posY, int width, int height){
		DrawBox(new Rect(posX, posY, width, height), Color.white);
		GUI.Label(new Rect(posX + (width/2)-20, posY + (height/2)-20, 200, 20), "Empty");
	}

	private void DrawColorPicker(int texHeight){
		colorPickerPosY = shapePosY + (texHeight*pixelSize) + 5;
		GUI.color = Color.white;
		GUI.Label(new Rect(shapePosX, colorPickerPosY, 12*pixelSize, 20), "Color Picker", EditorStyles.centeredGreyMiniLabel);

		colorPickerPosY += 20;

		Rect colorRect = new Rect(shapePosX, colorPickerPosY, pixelSize, pixelSize);
		Color col = new Color(1f, 1f, 1f, 1f);
		for(int i = 0; i <= 10; i++){
			GUI.color = col;
			GUI.DrawTexture(colorRect, EditorGUIUtility.whiteTexture);
			DrawBox(colorRect, Color.white);
			colorRect.x += pixelSize;
			col -= new Color(0.1f, 0.1f, 0.1f, 0f);
		}

		DrawBox(colorRect, Color.white); // eraser

		Rect selectedRect = new Rect(shapePosX + selectedColIndex*pixelSize, colorPickerPosY, pixelSize, pixelSize);
		DrawBox(selectedRect, Color.black);

		GUI.Label(new Rect(shapePosX - 10, colorPickerPosY + 20, 200, 20), "White");
		GUI.Label(new Rect(shapePosX + (pixelSize*5) - 5, colorPickerPosY + 20, 200, 20), "Gray");
		GUI.Label(new Rect(shapePosX + (pixelSize*10) - 7, colorPickerPosY + 20, 200, 20), "Black");
		GUI.Label(new Rect(shapePosX + (pixelSize*11) - 10, colorPickerPosY - 20, 200, 20), "Eraser");
	}

	private void SelectColor(){
		if(Event.current.type == EventType.MouseDown && Event.current.button == 0){
			Vector2 clickPos = Event.current.mousePosition;
			Rect colorPickerRect = new Rect(shapePosX, colorPickerPosY, pixelSize*12, pixelSize);

			if(colorPickerRect.Contains(clickPos)){
				for(int i = 0; i <= 10; i++){
					Rect rect = new Rect(shapePosX + (i*pixelSize), colorPickerPosY, pixelSize, pixelSize);
					if(rect.Contains(clickPos)){
						selectedCol = new Color(1f-(i*0.1f), 1f-(i*0.1f), 1f-(i*0.1f), 1f);
						selectedColIndex = i;
						return;
					}
				}
				selectedCol = new Color(0f, 0f, 0f, 0f);
				selectedColIndex = 11;
			}

		}
	}

	private void PaintShape(){
		if(Event.current.type == EventType.MouseDown && Event.current.button == 0 || Event.current.type == EventType.MouseDrag && Event.current.button == 0){
			Vector2 clickPos = Event.current.mousePosition;
			Rect shapeRect = new Rect(shapePosX, shapePosY, shape.width*pixelSize, shape.height*pixelSize);
			if(shapeRect.Contains(clickPos)){
				int x = Mathf.FloorToInt(((clickPos.x-shapePosX)*shape.width / (shape.width * pixelSize)));
				int y = Mathf.FloorToInt(((clickPos.y-shapePosY)*shape.height / (shape.height * pixelSize)));
				y = shape.height-y-1;
				if(selectedCol.a == 0f){
					shape.SetPixel(x, y, new PixelTexture.Pixel(selectedCol.grayscale, 0f));
					return;
				}
				shape.SetPixel(x, y, new PixelTexture.Pixel(selectedCol.grayscale, 1f));
			}
		}
	}

	private void DrawBodyPartTileHovering(int bodyPartPosX, int bodyPartPosY){
		Vector2 mousePos = Event.current.mousePosition;
		Rect shapeRect = new Rect(shapePosX, shapePosY, shape.width*pixelSize, shape.height*pixelSize);
		if(shapeRect.Contains(mousePos)){
			int x = Mathf.FloorToInt(((mousePos.x-shapePosX)*shape.width / (shape.width * pixelSize)));
			int y = Mathf.FloorToInt(((mousePos.y-shapePosY)*shape.height / (shape.height * pixelSize)));
			y = shape.height-y-1;

			Rect rect = new Rect(bodyPartPosX + pixelSize*x, bodyPartPosY + pixelSize*(shape.height-y-1), pixelSize, pixelSize);
			DrawBox(rect, Color.red);
		}
	}

	private void DrawPersonPreview(int previewPosX){
		if(character.head.shape.isNull || character.body.shape.isNull || character.legs.shape.isNull){
			return;
		}

		if(bodyPartSectionWidth*3+bodyPartSectionSpacing*4 > previewPosX){
			previewPosX = bodyPartSectionWidth*3+bodyPartSectionSpacing*4;
		}

		GUI.Label(new Rect(previewPosX, shapePosY-20, 200, 20), "Preview", EditorStyles.boldLabel);

		int width = 0;
		if(character.head.shape.width >= character.body.shape.width && character.head.shape.width >= character.legs.shape.width){
			width = character.head.shape.width;
		}
		else if(character.body.shape.width >= character.head.shape.width && character.body.shape.width >= character.legs.shape.width){
			width = character.body.shape.width;
		}
		else {
			width = character.legs.shape.width;
		}

		int posY = shapePosY;
		DrawShape(character.head.shape, previewPosX + pixelSize*((width-character.head.shape.width)/2), posY, "");
		posY += character.head.shape.height*pixelSize;
		DrawShape(character.body.shape, previewPosX + pixelSize*((width-character.body.shape.width)/2), posY, "");
		posY += character.body.shape.height*pixelSize;
		DrawShape(character.legs.shape, previewPosX + pixelSize*((width-character.legs.shape.width)/2), posY, "");
	}

	private void DrawBodyPartSections(){
		int posY = colorPickerPosY + spacing*2;

		Handles.color = Color.black;
		Handles.DrawLine(new Vector2(shapePosX, posY), new Vector2(bodyPartSectionWidth*3 + bodyPartSectionSpacing*3, posY));
		GUI.color = Color.white;

		posY += spacing;

		if(character.head.shape.isNull){
			DrawNoBodyPartShapeCase(shapePosX, posY, ref character.head, ref headShapeWidth, ref headShapeHeight);
		}
		else{
			DrawBodyPartSection(shapePosX, posY, ref character.head, ref character.tempHeadLayerName, ref headScrollPosition);
		}

		if(character.body.shape.isNull){
			DrawNoBodyPartShapeCase(shapePosX + bodyPartSectionWidth + bodyPartSectionSpacing, posY, ref character.body, ref bodyShapeWidth, ref bodyShapeHeight);
		}
		else{
			DrawBodyPartSection(shapePosX + bodyPartSectionWidth + bodyPartSectionSpacing, posY, ref character.body, ref character.tempBodyLayerName, ref bodyScrollPosition);
		}

		if(character.legs.shape.isNull){
			DrawNoBodyPartShapeCase(shapePosX + bodyPartSectionWidth*2 + bodyPartSectionSpacing*2, posY, ref character.legs, ref legShapeWidth, ref legShapeHeight);
		}
		else{
			DrawBodyPartSection(shapePosX + bodyPartSectionWidth*2 + bodyPartSectionSpacing*2, posY, ref character.legs, ref character.tempLegsLayerName, ref legsScrollPosition);
		}
	}

	private void DrawBodyPartSection(int posX, int posY, ref PixelCharacter.BodyPart bodyPart, ref string tempLayerName, ref Vector2 scrollPosition){
		float viewHeight = 20*2 + spacing*3;
		for(int i = 0; i < bodyPart.styleLayers.Count; i++){
			viewHeight += spacing*3;
			for(int s = 0; s < bodyPart.styleLayers[i].styles.Count; s++){
				viewHeight += spacing;
			}
		}

		scrollPosition = GUI.BeginScrollView(new Rect(posX, posY, bodyPartSectionWidth+20, 300), scrollPosition, new Rect(0, 0, bodyPartSectionWidth, viewHeight));
		posX = 0;
		posY = 0;

		GUI.Label(new Rect(posX, posY, bodyPartSectionWidth, 20), bodyPart.name + " Section", EditorStyles.boldLabel);
		posY += 20;
		//GUI.Label(new Rect(posX, posY, bodyPartSectionWidth, 20), "Body Part", EditorStyles.centeredGreyMiniLabel);
		//posY += spacing;
		GUI.Label(new Rect(posX, posY, bodyPartSectionWidth/3, 20), "● " + bodyPart.name);
		if(GUI.Button(new Rect(posX + bodyPartSectionWidth*(1f/3f), posY, bodyPartSectionWidth/3, 15), "Edit")){
			SetVariables(bodyPart, bodyPart.shape, bodyPart.name);
		}
		if(GUI.Button(new Rect(posX + bodyPartSectionWidth*(2f/3f), posY, bodyPartSectionWidth/3, 15), "Delete")){
			if(ShapeCreatorWindow.bodyPart.shape == bodyPart.shape){
				variablesSet = false;
			}
			character.DeleteBodyPart(bodyPart);
		}
		posY += spacing;
		GUI.Label(new Rect(posX, posY, bodyPartSectionWidth, 20), "New Style Layer", EditorStyles.centeredGreyMiniLabel);
		posY += spacing;
		GUI.Label(new Rect(posX, posY, 50, 15), "Name");
		tempLayerName = GUI.TextField(new Rect(posX + 50, posY, bodyPartSectionWidth - 50, 15), tempLayerName);
		posY += 20;
		if(GUI.Button(new Rect(posX, posY, bodyPartSectionWidth, 15), "Add Layer")){
			bodyPart.styleLayers.Add(new PixelCharacter.StyleLayer(tempLayerName));
			tempLayerName = "";
		}
		posY += spacing;
		for(int i = 0; i < bodyPart.styleLayers.Count; i++){
			GUI.Label(new Rect(posX, posY, bodyPartSectionWidth, 20), bodyPart.styleLayers[i].name, EditorStyles.centeredGreyMiniLabel);
			posY += spacing;
			for(int s = 0; s < bodyPart.styleLayers[i].styles.Count; s++){
				GUI.Label(new Rect(posX, posY, bodyPartSectionWidth/3, 20), "● " + bodyPart.styleLayers[i].name + " " + (s+1));
				if(GUI.Button(new Rect(posX + bodyPartSectionWidth*(1f/3f), posY, bodyPartSectionWidth/3, 15), "Edit")){
					SetVariables(bodyPart, bodyPart.styleLayers[i].styles[s], bodyPart.styleLayers[i].name + " " + (s+1));
				}
				if(GUI.Button(new Rect(posX + bodyPartSectionWidth*(2f/3f), posY, bodyPartSectionWidth/3, 15), "Delete")){
					if(shape == bodyPart.styleLayers[i].styles[s]){
						variablesSet = false;
					}
					bodyPart.styleLayers[i].styles.RemoveAt(s);
				}
				posY += spacing;
			}

			if(GUI.Button(new Rect(posX, posY, bodyPartSectionWidth, 15), "Create New Shape")){
				bodyPart.styleLayers[i].styles.Add(new PixelTexture(bodyPart.shape.width, bodyPart.shape.height));
				SetVariables(bodyPart, bodyPart.styleLayers[i].styles[bodyPart.styleLayers[i].styles.Count-1], bodyPart.styleLayers[i].name + " " + bodyPart.styleLayers[i].styles.Count);
			}
			posY += spacing;
			if(GUI.Button(new Rect(posX, posY, bodyPartSectionWidth/3f, 15), "Down")){
				PixelCharacter.StyleLayer temp = bodyPart.styleLayers[i];
				bodyPart.styleLayers[i] = bodyPart.styleLayers[i+1];
				bodyPart.styleLayers[i+1] = temp;
			}
			if(GUI.Button(new Rect(posX + bodyPartSectionWidth*(1f/3f), posY, bodyPartSectionWidth/3f, 15), "Up")){
				PixelCharacter.StyleLayer temp = bodyPart.styleLayers[i];
				bodyPart.styleLayers[i] = bodyPart.styleLayers[i-1];
				bodyPart.styleLayers[i-1] = temp;
			}
			if(GUI.Button(new Rect(posX + bodyPartSectionWidth*(2f/3f), posY, bodyPartSectionWidth/3f, 15), "Remove")){
				bodyPart.styleLayers.RemoveAt(i);
			}
			posY += spacing;
		}

		GUI.EndScrollView();
	}

	private void DrawNoBodyPartShapeCase(int posX, int posY, ref PixelCharacter.BodyPart bodyPart, ref int width, ref int height){
		GUI.Label(new Rect(posX, posY, bodyPartSectionWidth, 20), bodyPart.name + " Section", EditorStyles.boldLabel);
		posY += 20;
		GUI.Label(new Rect(posX, posY, bodyPartSectionWidth, 20), "A Body Part Shape Is Required");
		posY += 20;
		if(GUI.Button(new Rect(posX, posY, bodyPartSectionWidth, 20), "Create New Shape")){
			bodyPart.shape = new PixelTexture(width, height);
			SetVariables(bodyPart, bodyPart.shape, bodyPart.name);
		}
		EditorGUIUtility.labelWidth = 50;
		posY += 25;
		width = EditorGUI.IntField(new Rect(posX, posY, bodyPartSectionWidth, 20), "Width", width);
		posY += 25;
		height = EditorGUI.IntField(new Rect(posX, posY, bodyPartSectionWidth, 20), "Height", height);
	}
}
