using UnityEngine;
using System.Collections;

public static class PixelCharacterDrawTool {

	public static void DrawFromPixelTexture(Texture2D to, PixelTexture from, Color col, Vector2 start){
		//start is bottom left

		for(int y = 0; y < from.height; y++){
			for(int x = 0; x < from.width; x++){
				if(from.GetPixel(x,y).a == 0f){
					continue;
				}

				Color pixelColor = col;
				if(from.GetPixel(x,y).val < 0.45f){
					pixelColor *= (1f - (0.5f - from.GetPixel(x,y).val));
					pixelColor.a = 1f;
				}
				else if(from.GetPixel(x,y).val > 0.55f){
					pixelColor *= (1f + (from.GetPixel(x,y).val - 0.5f));
					pixelColor.a = 1f;
				}

				to.SetPixel(x + (int)start.x, y + (int)start.y, pixelColor);
			}
		}
	}

	public static Color RandomColor(){
		return new Color(Random.Range(0f,1f), Random.Range(0f,1f), Random.Range(0f,1f), 1f);
	}

	public static Color GreyColor(float val){
		return new Color(val, val, val, 1f);
	}


	public static Texture2D SetupTexture(PixelTexture head, PixelTexture body, PixelTexture legs){
		int height = head.height + body.height + legs.height;
		int width = 0;
		if(head.width >= body.width && head.width >= legs.width){
			width = head.width;
		}
		else if(body.width >= head.width && body.width >= legs.width){
			width = body.width;
		}
		else {
			width = legs.width;
		}

		Texture2D texture = new Texture2D(width, height, TextureFormat.RGBA32, false);
		texture.filterMode = FilterMode.Point;
		texture.wrapMode = TextureWrapMode.Clamp;

		for(int y = 0; y < height; y++){
			for(int x = 0; x < width; x++){
				texture.SetPixel(x,y, new Color(1f,1f,1f,0f));
			}
		}

		return texture;
	}

	private static string SaveImageName(string name) {
		string path = Application.dataPath + "/Pixel Character Builder/Saved Textures/";
		int n = 0;
		string comparer = path+name+"_"+n+".png";

		bool looping = true;
		while(looping){
			bool sameNameFound = false;
			foreach(string file in System.IO.Directory.GetFiles(path, "*.png")) {
				if(file == comparer){
					n++;
					sameNameFound = true;
					comparer = path+name+"_"+n+".png";;
					break;
				}
			}
			looping = sameNameFound;
		}

		return comparer;
	}
	
	public static void Save(Texture2D texture, string name) {
		byte[] bytes = texture.EncodeToPNG();
		string filename = SaveImageName(name);
		System.IO.File.WriteAllBytes(filename, bytes);
	}

}