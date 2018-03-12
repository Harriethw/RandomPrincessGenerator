using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(SpriteRenderer))]
[System.Serializable]
public class PixelCharacter : MonoBehaviour {

	[System.Serializable]
	public struct BodyPart {
		public string name;
		public PixelTexture shape;
		public List<StyleLayer> styleLayers;

		public BodyPart(string name){
			this.name = name;
			shape = new PixelTexture();
			styleLayers = new List<StyleLayer>();
		}
	}

	[System.Serializable]
	public struct StyleLayer {
		public string name;
		public List<PixelTexture> styles;
		public Color[] colors;
		public float drawProbability;

		public StyleLayer(string name){
			this.name = name;
			styles = new List<PixelTexture>();
			colors = new Color[0];
			drawProbability = 1f;
		}
	}

	public Texture2D texture;

	public BodyPart head = new BodyPart("Head");
	public BodyPart body = new BodyPart("Body");
	public BodyPart legs = new BodyPart("Legs");
	public Color[] skinColors;

	private Vector2[] startPoints = new Vector2[3];

	public string tempHeadLayerName = "";
	public string tempBodyLayerName = "";
	public string tempLegsLayerName = "";

	public void Draw(){
		if(GetComponent<SpriteRenderer>().sprite != null){
			Texture2D.DestroyImmediate(GetComponent<SpriteRenderer>().sprite.texture);
			Sprite.DestroyImmediate(GetComponent<SpriteRenderer>().sprite);
		}

		texture = PixelCharacterDrawTool.SetupTexture(head.shape, body.shape, legs.shape);
		GetStartPoints();

		Color skinCol = skinColors[Random.Range(0, skinColors.Length)];
		DrawBodyPartWithStyles(head, startPoints[2], skinCol);
		DrawBodyPartWithStyles(body, startPoints[1], skinCol);
		DrawBodyPartWithStyles(legs, startPoints[0], skinCol);
		
		texture.Apply();
		GetComponent<SpriteRenderer>().sprite = Sprite.Create(texture, new Rect(new Vector2(0f,0f), new Vector2(texture.width, texture.height)), new Vector2(0.5f, 0.0f));
	}

	private void DrawBodyPartWithStyles(BodyPart bodyPart, Vector2 startPoint, Color skinCol){
		PixelCharacterDrawTool.DrawFromPixelTexture(texture, bodyPart.shape, skinCol, startPoint);
		for(int i = 0; i < bodyPart.styleLayers.Count; i++){
			if(bodyPart.styleLayers[i].drawProbability >= Random.Range(0f, 1f)){
				PixelTexture style = bodyPart.styleLayers[i].styles[Random.Range(0, bodyPart.styleLayers[i].styles.Count)];
				Color col = bodyPart.styleLayers[i].colors[Random.Range(0, bodyPart.styleLayers[i].colors.Length)];
				PixelCharacterDrawTool.DrawFromPixelTexture(texture, style, col, startPoint);
			}
		}
	}

	private void GetStartPoints(){
		startPoints[0] = new Vector2(Mathf.Floor((texture.width - legs.shape.width) / 2f), 0f);
		startPoints[1] = new Vector2(Mathf.Floor((texture.width - body.shape.width) / 2f), legs.shape.height);
		startPoints[2] = new Vector2(Mathf.Floor((texture.width - head.shape.width) / 2f), legs.shape.height + body.shape.height);
	}

	public void DeleteBodyPart(BodyPart bodyPart){
		bodyPart.shape.SetToNull();
		bodyPart.styleLayers.Clear();
	}
}
