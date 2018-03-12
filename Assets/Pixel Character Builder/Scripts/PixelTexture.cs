using UnityEngine;
using System.Collections;

[System.Serializable]
public class PixelTexture {

	public Pixel[] texture;
	public int width;
	public int height;
	public bool isNull;

	[System.Serializable]
	public struct Pixel {
		public float val;
		public float a;

		public Pixel(float val, float a){
			this.val = val;
			this.a = a;
		}
	}

	public PixelTexture(){
		isNull = true;
	}

	public PixelTexture(int width, int height){
		this.width = width;
		this.height = height;
		texture = new Pixel[width*height];
		for(int y = 0; y < height; y++){
			for(int x = 0; x < width; x++){
				SetPixel(x, y, new Pixel(0f, 0f));
			}
		}
		isNull = false;
	}
		
	public void SetPixel(int x, int y, Pixel pixel){
		texture[y * width + x] = pixel;
	}

	public Pixel GetPixel(int x, int y){
		return texture[y * width + x];
	}

	public void SetToNull(){
		isNull = true;
	}
}