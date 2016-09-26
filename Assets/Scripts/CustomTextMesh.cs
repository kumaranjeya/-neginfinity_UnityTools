
using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class CustomTextMesh : MonoBehaviour{
	TextGenerator _textGen = null;//new TextGenerator();
	TextGenerationSettings genSettings = new TextGenerationSettings();
	Mesh textMesh = null;
	MeshFilter meshFilter = null;
	MeshRenderer meshRenderer = null;
	[TextArea(3, 50)]
	[SerializeField] string _text = "Text";
	string prevText;

	TextGenerator textGen{
		get{
			if (_textGen == null){
				_textGen = new TextGenerator();
			}
			return _textGen;
		}
	}

	void requestCharacters(string characters){
		if (font == null)
			return;

		font.RequestCharactersInTexture(characters);
	}

	public Font font{
		get{
			if (textSettings.font == null){
				textSettings.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
				requestCharacters(text);
				textGen.Invalidate();
			}
			return textSettings.font;
		}
		set{
			if (value == textSettings.font)
				return;
			textSettings.font = value;
			requestCharacters(_text);
			textGen.Invalidate();
		}
	}

	public string text{
		get{
			return _text;
		}
		set{
			if (value == _text)
				return;			
			_text = text;
		}
	}

	[System.Serializable]
	public class TextSettings{
		public Font font;
		public Material fontMaterial;
		public float pixelsPerUnit = 256.0f;
		public bool richText = false;
		public bool alignByGeometry = false;
		public float textWidth = 1.0f;
		public float textHeight = 1.0f;
		public int fontSize = 14;
		public int minDynamicSize = 2;
		public int maxDynamicSize = -1;
		public TextAnchor anchor = TextAnchor.MiddleCenter;
		public TextAnchor alignment = TextAnchor.MiddleCenter;
		public Color color = Color.white;
		public bool bestFit = true;
		public int lineSpacing = 1;
		public HorizontalWrapMode horizontalWrap = HorizontalWrapMode.Wrap;
		public VerticalWrapMode verticalWrap = VerticalWrapMode.Truncate;
		public FontStyle fontStyle = FontStyle.Normal;

		public override bool Equals(object obj){
			if (obj == null)
				return false;
			TextSettings other = obj as TextSettings;
			if ((object)other == null)
				return false;

			return (fontMaterial == other.fontMaterial) &&
			(pixelsPerUnit == other.pixelsPerUnit) &&
			(richText == other.richText) &&
			(alignByGeometry == other.alignByGeometry) &&
			(font == other.font) &&
			(textWidth == other.textWidth) &&
			(textHeight == other.textHeight) &&
			(fontSize == other.fontSize) &&
			(minDynamicSize == other.minDynamicSize) &&
			(maxDynamicSize == other.maxDynamicSize) &&
			(anchor == other.anchor) &&
			(alignment == other.alignment) &&
			(color == other.color) &&
			(bestFit == other.bestFit) &&
			(lineSpacing == other.lineSpacing) &&
			(horizontalWrap == other.horizontalWrap) &&
			(verticalWrap == other.verticalWrap) &&
			(fontStyle == other.fontStyle);
		}

		protected static int beginHash(){
			return 17;
		}

		protected static void combineHash<T>(ref int prevHash, T obj){
			int nextHash = 0;
			if ((object)obj != null)
				nextHash = obj.GetHashCode();
			prevHash = prevHash * 23 + nextHash;
		}

		public override int GetHashCode(){
			int hash = beginHash();
			combineHash(ref hash, fontMaterial);
			combineHash(ref hash, pixelsPerUnit);
			combineHash(ref hash, richText);
			combineHash(ref hash, alignByGeometry);
			combineHash(ref hash, font);
			combineHash(ref hash, textWidth);
			combineHash(ref hash, textHeight);
			combineHash(ref hash, fontSize);
			combineHash(ref hash, minDynamicSize);
			combineHash(ref hash, maxDynamicSize);
			combineHash(ref hash, anchor);
			combineHash(ref hash, alignment);
			combineHash(ref hash, color);
			combineHash(ref hash, bestFit);
			combineHash(ref hash, lineSpacing);
			combineHash(ref hash, horizontalWrap);
			combineHash(ref hash, verticalWrap);
			return hash;
		}

		public void assignFrom(TextSettings other){
			fontMaterial = other.fontMaterial;
			font = other.font;
			pixelsPerUnit = other.pixelsPerUnit;
			richText = other.richText;
			alignByGeometry = other.alignByGeometry;
			textWidth = other.textWidth;
			textHeight = other.textHeight;
			fontSize = other.fontSize;
			minDynamicSize = other.minDynamicSize;
			maxDynamicSize = other.maxDynamicSize;
			anchor = other.anchor;
			alignment = other.alignment;
			color = other.color;
			bestFit = other.bestFit;
			lineSpacing = other.lineSpacing;
			horizontalWrap = other.horizontalWrap;
			verticalWrap = other.verticalWrap;
			fontStyle = other.fontStyle;
		}
	};

	[SerializeField] public TextSettings textSettings = new TextSettings();
	[SerializeField] TextSettings _prevTextSettings = new TextSettings();

	bool isDirty{
		get{
			return (prevText != _text) ||
				(textSettings != _prevTextSettings);
		}
	}

	void onFontTextureChanged(Font newFont){
		if (newFont == font)
			return;
		textGen.Invalidate();
		rebuildText();
	}

	void fillGenSettings(){
		if ((font != null) && (font.dynamic)){
			genSettings.fontSize = textSettings.fontSize;
			genSettings.resizeTextMinSize = textSettings.minDynamicSize;
			genSettings.resizeTextMaxSize = (textSettings.maxDynamicSize > 0) ? textSettings.maxDynamicSize: textSettings.fontSize;
		}

		genSettings.generationExtents = new Vector2(textSettings.textWidth * textSettings.pixelsPerUnit, 
			textSettings.textHeight * textSettings.pixelsPerUnit);

		genSettings.alignByGeometry = textSettings.alignByGeometry;
		genSettings.color = textSettings.color;
		genSettings.font = textSettings.font;
		genSettings.fontStyle = textSettings.fontStyle;
		genSettings.lineSpacing = textSettings.lineSpacing;
		var anchorPoint = getAnchorPoint();
		genSettings.pivot = anchorPoint;
		genSettings.resizeTextForBestFit = textSettings.bestFit;
		genSettings.richText = textSettings.richText;
		genSettings.scaleFactor = 1.0f;
		genSettings.textAnchor = textSettings.alignment;//textSettings.anchor;
		genSettings.horizontalOverflow = textSettings.horizontalWrap;
		genSettings.verticalOverflow = textSettings.verticalWrap;
	}

	void rebuildText(){
		if (meshFilter == null){
			meshFilter = GetComponent<MeshFilter>();
			if (!meshFilter){
				enabled = false;
				Debug.LogWarningFormat("No mesh filter found on {0}", gameObject.name);
				return;
			}
		}

		if (!textMesh){
			textMesh = new Mesh();
			textMesh.name = "Text Mesh";
			textMesh.hideFlags = HideFlags.HideAndDontSave;
		}

		if (!meshRenderer){
			meshRenderer = GetComponent<MeshRenderer>();
		}

		meshFilter.sharedMesh = textMesh;

		fillGenSettings();
		textGen.Populate(_text, genSettings);

		var genVerts = textGen.verts;

		var numVerts = genVerts.Count;

		var verts = new Vector3[numVerts];
		var normals = new Vector3[numVerts];
		var uv0 = new Vector2[numVerts];
		var uv1 = new Vector2[numVerts];
		var colors = new Color32[numVerts];
		var tangents = new Vector4[numVerts];

		int numIndexes = (numVerts / 4)*6;
		var indexes = new int[numIndexes];

		float scale = 1.0f;
		if (textSettings.pixelsPerUnit > 0.0f)
			scale = 1.0f/textSettings.pixelsPerUnit;

		int indexOffset = 0;
		for(int i = 0; i < numVerts; i++){
			var pos = genVerts[i].position * scale;
			pos.x = -pos.x;
			verts[i] = pos;//genVerts[i].position * scale;
			normals[i] = genVerts[i].normal;
			uv0[i] = genVerts[i].uv0;
			uv1[i] = genVerts[i].uv1;
			colors[i] = genVerts[i].color;
			tangents[i] = genVerts[i].tangent;
			if ((i & 3) == 3){
				int baseIndex = i - 3;
				indexes[indexOffset++] = baseIndex + 0;
				indexes[indexOffset++] = baseIndex + 1;
				indexes[indexOffset++] = baseIndex + 2;
				indexes[indexOffset++] = baseIndex + 0;
				indexes[indexOffset++] = baseIndex + 2;
				indexes[indexOffset++] = baseIndex + 3;
			}
		}

		textMesh.Clear();
		textMesh.vertices = verts;
		textMesh.triangles = indexes;
		textMesh.normals = normals;
		textMesh.uv = uv0;
		textMesh.uv2 = uv1;
		textMesh.colors32 = colors;
		textMesh.tangents = tangents;

		meshFilter.sharedMesh = textMesh;

		var fontMaterial = textSettings.fontMaterial;			
		if (!fontMaterial && font){
			fontMaterial = font.material;
		}

		if (meshRenderer){
			meshRenderer.sharedMaterial = fontMaterial;
		}

		_prevTextSettings.assignFrom(textSettings);
		prevText = text;		
	}

	void OnEnable(){		
		Font.textureRebuilt += onFontTextureChanged;
		textGen.Invalidate();
		rebuildText();
	}

	void OnDisable(){
		Font.textureRebuilt -= onFontTextureChanged;
		if (Application.isEditor){
			GetComponent<MeshFilter>().sharedMesh = null;
			DestroyImmediate(textMesh);
			textMesh = null;
		}
	}

	Vector2 getAnchorPoint(){
		switch(textSettings.anchor){
			case(TextAnchor.LowerLeft):
				return new Vector2(0.0f, 0.0f);
			case(TextAnchor.LowerCenter):
				return new Vector2(0.5f, 0.0f);
			case(TextAnchor.LowerRight):
				return new Vector2(1.0f, 0.0f);
			case(TextAnchor.MiddleLeft):
				return new Vector2(0.0f, 0.5f);
			case(TextAnchor.MiddleCenter):
				return new Vector2(0.5f, 0.5f);
			case(TextAnchor.MiddleRight):
				return new Vector2(1.0f, 0.5f);
			case(TextAnchor.UpperLeft):
				return new Vector2(0.0f, 1.0f);
			case(TextAnchor.UpperCenter):
				return new Vector2(0.5f, 1.0f);
			case(TextAnchor.UpperRight):
				return new Vector2(1.0f, 1.0f);
		}
		return Vector2.zero;
	}

	void OnDrawGizmosSelected(){
		var anchorPoint = getAnchorPoint();

		var center = transform.position;
		var up = transform.TransformVector(new Vector3(0.0f, textSettings.textHeight, 0.0f));
		var right = transform.TransformVector(new Vector3(-textSettings.textWidth, 0.0f, 0.0f));

		var corner = center - up * anchorPoint.y - right * anchorPoint.x;
		var a = corner;
		var b = a + right;
		var c = b + up;
		var d = a + up;

		var oldColor = Gizmos.color;
		Gizmos.color = Color.yellow;
		Gizmos.DrawLine(a, b);
		Gizmos.DrawLine(b, c);
		Gizmos.DrawLine(c, d);
		Gizmos.DrawLine(a, d);
		Gizmos.color = oldColor;
	}

	// Update is called once per frame
	void LateUpdate(){
		if (isDirty)
			rebuildText();
	}
}
