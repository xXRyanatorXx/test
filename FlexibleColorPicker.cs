using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x020000DB RID: 219
public class FlexibleColorPicker : MonoBehaviour
{
	// Token: 0x1700007F RID: 127
	// (get) Token: 0x0600049E RID: 1182 RVA: 0x0002771D File Offset: 0x0002591D
	private FlexibleColorPicker.AdvancedSettings AS
	{
		get
		{
			return this.advancedSettings;
		}
	}

	// Token: 0x17000080 RID: 128
	// (get) Token: 0x0600049F RID: 1183 RVA: 0x00027725 File Offset: 0x00025925
	// (set) Token: 0x060004A0 RID: 1184 RVA: 0x00027741 File Offset: 0x00025941
	public Color color
	{
		get
		{
			if (this.bufferedColor == null)
			{
				return this.startingColor;
			}
			return this.bufferedColor.color;
		}
		set
		{
			if (this.bufferedColor == null)
			{
				this.startingColor = value;
				return;
			}
			this.bufferedColor.Set(value);
			this.UpdateMarkers();
			this.UpdateTextures();
			this.UpdateHex();
			this.typeUpdate = true;
		}
	}

	// Token: 0x060004A1 RID: 1185 RVA: 0x00027778 File Offset: 0x00025978
	public Color GetColor()
	{
		return this.color;
	}

	// Token: 0x060004A2 RID: 1186 RVA: 0x00027780 File Offset: 0x00025980
	public void SetColor(Color color)
	{
		this.color = color;
	}

	// Token: 0x060004A3 RID: 1187 RVA: 0x0002778C File Offset: 0x0002598C
	public Color GetColorFullAlpha()
	{
		Color color = this.color;
		color.a = 1f;
		return color;
	}

	// Token: 0x060004A4 RID: 1188 RVA: 0x000277AD File Offset: 0x000259AD
	public void SetColorNoAlpha(Color color)
	{
		color.a = this.color.a;
		this.color = color;
	}

	// Token: 0x060004A5 RID: 1189 RVA: 0x000277C8 File Offset: 0x000259C8
	private void Start()
	{
		this.bufferedColor = new FlexibleColorPicker.BufferedColor(this.startingColor);
		this.canvas = base.GetComponentInParent<Canvas>();
	}

	// Token: 0x060004A6 RID: 1190 RVA: 0x000277E8 File Offset: 0x000259E8
	private void OnEnable()
	{
		if (this.bufferedColor == null)
		{
			this.bufferedColor = new FlexibleColorPicker.BufferedColor(this.startingColor);
		}
		if (this.multiInstance && !this.materialsSeperated)
		{
			this.SeperateMaterials();
			this.materialsSeperated = true;
		}
		this.triggeredStaticMode = this.staticMode;
		this.UpdateTextures();
		this.MakeModeOptions();
		this.UpdateMarkers();
	}

	// Token: 0x060004A7 RID: 1191 RVA: 0x0002784C File Offset: 0x00025A4C
	private void Update()
	{
		this.typeUpdate = false;
		if (this.lastUpdatedMode != this.mode)
		{
			this.ChangeMode(this.mode);
		}
		if (this.staticMode != this.triggeredStaticMode)
		{
			this.UpdateTextures();
			this.triggeredStaticMode = this.staticMode;
		}
		if (this.multiInstance && !this.materialsSeperated)
		{
			this.SeperateMaterials();
			this.materialsSeperated = true;
		}
	}

	// Token: 0x060004A8 RID: 1192 RVA: 0x000278B8 File Offset: 0x00025AB8
	public void SetPointerFocus(int i)
	{
		if (i < 0 || i >= this.pickers.Length)
		{
			string str = "No picker image available of type ";
			FlexibleColorPicker.PickerType pickerType = (FlexibleColorPicker.PickerType)i;
			Debug.LogWarning(str + pickerType.ToString() + ". Did you assign all the picker images in the editor?");
		}
		else
		{
			this.focusedPicker = this.pickers[i];
		}
		this.focusedPickerType = (FlexibleColorPicker.PickerType)i;
	}

	// Token: 0x060004A9 RID: 1193 RVA: 0x00027914 File Offset: 0x00025B14
	public void PointerUpdate(BaseEventData e)
	{
		Vector2 normalizedPointerPosition = FlexibleColorPicker.GetNormalizedPointerPosition(this.canvas, this.focusedPicker.image.rectTransform, e);
		this.bufferedColor = this.PickColor(this.bufferedColor, this.focusedPickerType, normalizedPointerPosition);
		this.UpdateMarkers();
		this.UpdateTextures();
		this.typeUpdate = true;
		this.UpdateHex();
	}

	// Token: 0x060004AA RID: 1194 RVA: 0x00027970 File Offset: 0x00025B70
	public void TypeHex(string input)
	{
		this.TypeHex(input, false);
		this.UpdateTextures();
		this.UpdateMarkers();
	}

	// Token: 0x060004AB RID: 1195 RVA: 0x00027986 File Offset: 0x00025B86
	public void FinishTypeHex(string input)
	{
		this.TypeHex(input, true);
		this.UpdateTextures();
		this.UpdateMarkers();
	}

	// Token: 0x060004AC RID: 1196 RVA: 0x0002799C File Offset: 0x00025B9C
	public void ChangeMode(int newMode)
	{
		this.ChangeMode((FlexibleColorPicker.MainPickingMode)newMode);
	}

	// Token: 0x060004AD RID: 1197 RVA: 0x000279A5 File Offset: 0x00025BA5
	public void ChangeMode(FlexibleColorPicker.MainPickingMode mode)
	{
		this.mode = mode;
		this.triggeredStaticMode = false;
		this.UpdateTextures();
		this.UpdateMarkers();
		this.UpdateMode(mode);
	}

	// Token: 0x060004AE RID: 1198 RVA: 0x000279C8 File Offset: 0x00025BC8
	private void SeperateMaterials()
	{
		for (int i = 0; i < this.pickers.Length; i++)
		{
			FlexibleColorPicker.Picker picker = this.pickers[i];
			if (this.IsPickerAvailable(i) & picker.dynamicMaterial != null)
			{
				Material material = new Material(picker.dynamicMaterial);
				picker.dynamicMaterial = material;
				this.pickers[i] = picker;
				if (!this.staticMode)
				{
					picker.image.material = material;
				}
			}
		}
	}

	// Token: 0x060004AF RID: 1199 RVA: 0x00027A40 File Offset: 0x00025C40
	private FlexibleColorPicker.BufferedColor PickColor(FlexibleColorPicker.BufferedColor color, FlexibleColorPicker.PickerType type, Vector2 v)
	{
		if (type == FlexibleColorPicker.PickerType.Main)
		{
			return this.PickColorMain(color, v);
		}
		if (type - FlexibleColorPicker.PickerType.Preview > 1)
		{
			return this.PickColor1D(color, type, v);
		}
		return color;
	}

	// Token: 0x060004B0 RID: 1200 RVA: 0x00027A61 File Offset: 0x00025C61
	private FlexibleColorPicker.BufferedColor PickColorMain(FlexibleColorPicker.BufferedColor color, Vector2 v)
	{
		return this.PickColorMain(color, this.mode, v);
	}

	// Token: 0x060004B1 RID: 1201 RVA: 0x00027A74 File Offset: 0x00025C74
	private FlexibleColorPicker.BufferedColor PickColor1D(FlexibleColorPicker.BufferedColor color, FlexibleColorPicker.PickerType type, Vector2 v)
	{
		float value = FlexibleColorPicker.IsHorizontal(this.pickers[(int)type]) ? v.x : v.y;
		return this.PickColor1D(color, type, value);
	}

	// Token: 0x060004B2 RID: 1202 RVA: 0x00027AAC File Offset: 0x00025CAC
	private FlexibleColorPicker.BufferedColor PickColorMain(FlexibleColorPicker.BufferedColor color, FlexibleColorPicker.MainPickingMode mode, Vector2 v)
	{
		switch (mode)
		{
		case FlexibleColorPicker.MainPickingMode.HS:
			return this.PickColor2D(color, FlexibleColorPicker.PickerType.H, v.x, FlexibleColorPicker.PickerType.S, v.y);
		case FlexibleColorPicker.MainPickingMode.HV:
			return this.PickColor2D(color, FlexibleColorPicker.PickerType.H, v.x, FlexibleColorPicker.PickerType.V, v.y);
		case FlexibleColorPicker.MainPickingMode.SH:
			return this.PickColor2D(color, FlexibleColorPicker.PickerType.S, v.x, FlexibleColorPicker.PickerType.H, v.y);
		case FlexibleColorPicker.MainPickingMode.SV:
			return this.PickColor2D(color, FlexibleColorPicker.PickerType.S, v.x, FlexibleColorPicker.PickerType.V, v.y);
		case FlexibleColorPicker.MainPickingMode.VH:
			return this.PickColor2D(color, FlexibleColorPicker.PickerType.V, v.x, FlexibleColorPicker.PickerType.H, v.y);
		case FlexibleColorPicker.MainPickingMode.VS:
			return this.PickColor2D(color, FlexibleColorPicker.PickerType.V, v.x, FlexibleColorPicker.PickerType.S, v.y);
		default:
			return this.bufferedColor;
		}
	}

	// Token: 0x060004B3 RID: 1203 RVA: 0x00027B66 File Offset: 0x00025D66
	private FlexibleColorPicker.BufferedColor PickColor2D(FlexibleColorPicker.BufferedColor color, FlexibleColorPicker.PickerType type1, float value1, FlexibleColorPicker.PickerType type2, float value2)
	{
		color = this.PickColor1D(color, type1, value1);
		color = this.PickColor1D(color, type2, value2);
		return color;
	}

	// Token: 0x060004B4 RID: 1204 RVA: 0x00027B84 File Offset: 0x00025D84
	private FlexibleColorPicker.BufferedColor PickColor1D(FlexibleColorPicker.BufferedColor color, FlexibleColorPicker.PickerType type, float value)
	{
		switch (type)
		{
		case FlexibleColorPicker.PickerType.R:
			return color.PickR(Mathf.Lerp(this.AS.r.min, this.AS.r.max, value));
		case FlexibleColorPicker.PickerType.G:
			return color.PickG(Mathf.Lerp(this.AS.g.min, this.AS.g.max, value));
		case FlexibleColorPicker.PickerType.B:
			return color.PickB(Mathf.Lerp(this.AS.b.min, this.AS.b.max, value));
		case FlexibleColorPicker.PickerType.H:
			return color.PickH(Mathf.Lerp(this.AS.h.min, this.AS.h.max, value) * 5.9999f);
		case FlexibleColorPicker.PickerType.S:
			return color.PickS(Mathf.Lerp(this.AS.s.min, this.AS.s.max, value));
		case FlexibleColorPicker.PickerType.V:
			return color.PickV(Mathf.Lerp(this.AS.v.min, this.AS.v.max, value));
		case FlexibleColorPicker.PickerType.A:
			return color.PickA(Mathf.Lerp(this.AS.a.min, this.AS.a.max, value));
		default:
			throw new Exception("Picker type " + type.ToString() + " is not associated with a single color value.");
		}
	}

	// Token: 0x060004B5 RID: 1205 RVA: 0x00027D1C File Offset: 0x00025F1C
	public void CreateRandomString()
	{
		int num = 9 - 1;
		string text = "";
		string[] array = new string[]
		{
			"a",
			"b",
			"c",
			"d",
			"e",
			"f",
			"g",
			"h",
			"i",
			"j",
			"k",
			"l",
			"m",
			"n",
			"o",
			"p",
			"q",
			"r",
			"s",
			"t",
			"u",
			"v",
			"w",
			"x",
			"y",
			"z",
			"1",
			"2",
			"3",
			"4",
			"5",
			"6",
			"7",
			"8",
			"9",
			"0"
		};
		for (int i = 0; i <= num; i++)
		{
			text += array[UnityEngine.Random.Range(0, array.Length)];
		}
		this.hexInput.text = text;
		this.FinishTypeHex(text);
	}

	// Token: 0x060004B6 RID: 1206 RVA: 0x00027EA8 File Offset: 0x000260A8
	private void UpdateMarkers()
	{
		for (int i = 0; i < this.pickers.Length; i++)
		{
			if (this.IsPickerAvailable(i))
			{
				FlexibleColorPicker.PickerType type = (FlexibleColorPicker.PickerType)i;
				Vector2 value = this.GetValue(type);
				this.UpdateMarker(this.pickers[i], type, value);
			}
		}
	}

	// Token: 0x060004B7 RID: 1207 RVA: 0x00027EF0 File Offset: 0x000260F0
	private void UpdateMarker(FlexibleColorPicker.Picker picker, FlexibleColorPicker.PickerType type, Vector2 v)
	{
		if (type != FlexibleColorPicker.PickerType.Main)
		{
			if (type - FlexibleColorPicker.PickerType.Preview > 1)
			{
				bool flag = FlexibleColorPicker.IsHorizontal(picker);
				this.SetMarker(picker.image, v, flag, !flag);
			}
			return;
		}
		this.SetMarker(picker.image, v, true, true);
	}

	// Token: 0x060004B8 RID: 1208 RVA: 0x00027F34 File Offset: 0x00026134
	private void SetMarker(Image picker, Vector2 v, bool setX, bool setY)
	{
		RectTransform rectTransform = null;
		RectTransform rectTransform2 = null;
		if (setX && setY)
		{
			rectTransform = this.GetMarker(picker, null);
		}
		else if (setX)
		{
			rectTransform = this.GetMarker(picker, "hor");
			rectTransform2 = this.GetMarker(picker, "ver");
		}
		else if (setY)
		{
			rectTransform = this.GetMarker(picker, "ver");
			rectTransform2 = this.GetMarker(picker, "hor");
		}
		if (rectTransform2 != null)
		{
			rectTransform2.gameObject.SetActive(false);
		}
		if (rectTransform == null)
		{
			return;
		}
		rectTransform.gameObject.SetActive(true);
		RectTransform rectTransform3 = picker.rectTransform;
		Vector2 size = rectTransform3.rect.size;
		Vector2 v2 = rectTransform.localPosition;
		if (setX)
		{
			v2.x = (v.x - rectTransform3.pivot.x) * size.x;
		}
		if (setY)
		{
			v2.y = (v.y - rectTransform3.pivot.y) * size.y;
		}
		rectTransform.localPosition = v2;
	}

	// Token: 0x060004B9 RID: 1209 RVA: 0x00028038 File Offset: 0x00026238
	private RectTransform GetMarker(Image picker, string search)
	{
		for (int i = 0; i < picker.transform.childCount; i++)
		{
			RectTransform component = picker.transform.GetChild(i).GetComponent<RectTransform>();
			string text = component.name.ToLower();
			if (text.Contains("marker") & (string.IsNullOrEmpty(search) || text.Contains(search)))
			{
				return component;
			}
		}
		return null;
	}

	// Token: 0x060004BA RID: 1210 RVA: 0x0002809C File Offset: 0x0002629C
	private Vector2 GetValue(FlexibleColorPicker.PickerType type)
	{
		if (type == FlexibleColorPicker.PickerType.Main)
		{
			return this.GetValue(this.mode);
		}
		if (type - FlexibleColorPicker.PickerType.Preview > 1)
		{
			float value1D = this.GetValue1D(type);
			return new Vector2(value1D, value1D);
		}
		return Vector2.zero;
	}

	// Token: 0x060004BB RID: 1211 RVA: 0x000280CC File Offset: 0x000262CC
	private float GetValue1D(FlexibleColorPicker.PickerType type)
	{
		switch (type)
		{
		case FlexibleColorPicker.PickerType.R:
			return Mathf.InverseLerp(this.AS.r.min, this.AS.r.max, this.bufferedColor.r);
		case FlexibleColorPicker.PickerType.G:
			return Mathf.InverseLerp(this.AS.g.min, this.AS.g.max, this.bufferedColor.g);
		case FlexibleColorPicker.PickerType.B:
			return Mathf.InverseLerp(this.AS.b.min, this.AS.b.max, this.bufferedColor.b);
		case FlexibleColorPicker.PickerType.H:
			return Mathf.InverseLerp(this.AS.h.min, this.AS.h.max, this.bufferedColor.h / 5.9999f);
		case FlexibleColorPicker.PickerType.S:
			return Mathf.InverseLerp(this.AS.s.min, this.AS.s.max, this.bufferedColor.s);
		case FlexibleColorPicker.PickerType.V:
			return Mathf.InverseLerp(this.AS.v.min, this.AS.v.max, this.bufferedColor.v);
		case FlexibleColorPicker.PickerType.A:
			return Mathf.InverseLerp(this.AS.a.min, this.AS.a.max, this.bufferedColor.a);
		default:
			throw new Exception("Picker type " + type.ToString() + " is not associated with a single color value.");
		}
	}

	// Token: 0x060004BC RID: 1212 RVA: 0x00028280 File Offset: 0x00026480
	private Vector2 GetValue(FlexibleColorPicker.MainPickingMode mode)
	{
		switch (mode)
		{
		case FlexibleColorPicker.MainPickingMode.HS:
			return new Vector2(this.GetValue1D(FlexibleColorPicker.PickerType.H), this.GetValue1D(FlexibleColorPicker.PickerType.S));
		case FlexibleColorPicker.MainPickingMode.HV:
			return new Vector2(this.GetValue1D(FlexibleColorPicker.PickerType.H), this.GetValue1D(FlexibleColorPicker.PickerType.V));
		case FlexibleColorPicker.MainPickingMode.SH:
			return new Vector2(this.GetValue1D(FlexibleColorPicker.PickerType.S), this.GetValue1D(FlexibleColorPicker.PickerType.H));
		case FlexibleColorPicker.MainPickingMode.SV:
			return new Vector2(this.GetValue1D(FlexibleColorPicker.PickerType.S), this.GetValue1D(FlexibleColorPicker.PickerType.V));
		case FlexibleColorPicker.MainPickingMode.VH:
			return new Vector2(this.GetValue1D(FlexibleColorPicker.PickerType.V), this.GetValue1D(FlexibleColorPicker.PickerType.H));
		case FlexibleColorPicker.MainPickingMode.VS:
			return new Vector2(this.GetValue1D(FlexibleColorPicker.PickerType.V), this.GetValue1D(FlexibleColorPicker.PickerType.S));
		default:
			throw new Exception("Unkown main picking mode: " + mode.ToString());
		}
	}

	// Token: 0x060004BD RID: 1213 RVA: 0x00028344 File Offset: 0x00026544
	private void UpdateTextures()
	{
		foreach (object obj in Enum.GetValues(typeof(FlexibleColorPicker.PickerType)))
		{
			FlexibleColorPicker.PickerType pickerType = (FlexibleColorPicker.PickerType)obj;
			if (this.staticMode || this.AS.Get((int)pickerType).overrideStatic)
			{
				this.UpdateStatic(pickerType);
			}
			else
			{
				this.UpdateDynamic(pickerType);
			}
		}
	}

	// Token: 0x060004BE RID: 1214 RVA: 0x000283CC File Offset: 0x000265CC
	private void UpdateStatic(FlexibleColorPicker.PickerType type)
	{
		if (!this.IsPickerAvailable(type))
		{
			return;
		}
		FlexibleColorPicker.Picker picker = this.pickers[(int)type];
		bool flag = FlexibleColorPicker.IsHorizontal(picker);
		Sprite sprite = flag ? picker.staticSpriteHor : picker.staticSpriteVer;
		if (sprite == null)
		{
			sprite = (flag ? picker.staticSpriteVer : picker.staticSpriteHor);
		}
		picker.image.sprite = sprite;
		picker.image.material = null;
		picker.image.color = Color.white;
		Color color = this.color;
		if (type == FlexibleColorPicker.PickerType.Main)
		{
			picker.image.sprite = this.staticSpriteMain[(int)this.mode];
			return;
		}
		if (type == FlexibleColorPicker.PickerType.Preview)
		{
			color.a = 1f;
			picker.image.color = color;
			return;
		}
		if (type != FlexibleColorPicker.PickerType.PreviewAlpha)
		{
			return;
		}
		picker.image.color = color;
	}

	// Token: 0x060004BF RID: 1215 RVA: 0x000284A0 File Offset: 0x000266A0
	private void UpdateDynamic(FlexibleColorPicker.PickerType type)
	{
		if (!this.IsPickerAvailable(type))
		{
			return;
		}
		FlexibleColorPicker.Picker picker = this.pickers[(int)type];
		if (picker.dynamicMaterial == null)
		{
			return;
		}
		Material dynamicMaterial = picker.dynamicMaterial;
		picker.image.material = dynamicMaterial;
		picker.image.color = Color.white;
		picker.image.sprite = picker.dynamicSprite;
		FlexibleColorPicker.BufferedColor bufferedColor = this.bufferedColor;
		bool flag = FlexibleColorPicker.IsAlphaType(type);
		dynamicMaterial.SetInt("_Mode", this.GetGradientMode(type));
		Color color = this.PickColor(bufferedColor, type, Vector2.zero).color;
		Color color2 = this.PickColor(bufferedColor, type, Vector2.one).color;
		if (!flag)
		{
			color = new Color(color.r, color.g, color.b);
			color2 = new Color(color2.r, color2.g, color2.b);
		}
		dynamicMaterial.SetColor("_Color1", color);
		dynamicMaterial.SetColor("_Color2", color2);
		if (type == FlexibleColorPicker.PickerType.Main)
		{
			dynamicMaterial.SetInt("_DoubleMode", (int)this.mode);
		}
		dynamicMaterial.SetVector("_HSV", new Vector4(bufferedColor.h / 5.9999f, bufferedColor.s, bufferedColor.v, flag ? bufferedColor.a : 1f));
		dynamicMaterial.SetVector("_HSV_MIN", new Vector4(this.AS.h.min, this.AS.s.min, this.AS.v.min));
		dynamicMaterial.SetVector("_HSV_MAX", new Vector4(this.AS.h.max, this.AS.s.max, this.AS.v.max));
	}

	// Token: 0x060004C0 RID: 1216 RVA: 0x00028670 File Offset: 0x00026870
	private int GetGradientMode(FlexibleColorPicker.PickerType type)
	{
		int num = FlexibleColorPicker.IsHorizontal(this.pickers[(int)type]) ? 0 : 1;
		if (type == FlexibleColorPicker.PickerType.Main)
		{
			return 2;
		}
		if (type != FlexibleColorPicker.PickerType.H)
		{
			return num;
		}
		return 3 + num;
	}

	// Token: 0x060004C1 RID: 1217 RVA: 0x000286A5 File Offset: 0x000268A5
	private bool IsPickerAvailable(FlexibleColorPicker.PickerType type)
	{
		return this.IsPickerAvailable((int)type);
	}

	// Token: 0x060004C2 RID: 1218 RVA: 0x000286B0 File Offset: 0x000268B0
	private bool IsPickerAvailable(int index)
	{
		if (index < 0 || index >= this.pickers.Length)
		{
			return false;
		}
		FlexibleColorPicker.Picker picker = this.pickers[index];
		return !(picker.image == null) && picker.image.gameObject.activeInHierarchy;
	}

	// Token: 0x060004C3 RID: 1219 RVA: 0x00028700 File Offset: 0x00026900
	private void UpdateHex()
	{
		if (this.hexInput == null || !this.hexInput.gameObject.activeInHierarchy)
		{
			return;
		}
		this.hexInput.text = "#" + ColorUtility.ToHtmlStringRGB(this.color);
	}

	// Token: 0x060004C4 RID: 1220 RVA: 0x00028750 File Offset: 0x00026950
	private void TypeHex(string input, bool finish)
	{
		if (this.typeUpdate)
		{
			return;
		}
		this.typeUpdate = true;
		string sanitizedHex = FlexibleColorPicker.GetSanitizedHex(input, finish);
		string sanitizedHex2 = FlexibleColorPicker.GetSanitizedHex(input, true);
		int caretPosition = this.hexInput.caretPosition;
		this.hexInput.text = sanitizedHex;
		if (this.hexInput.caretPosition == 0)
		{
			this.hexInput.caretPosition = 1;
		}
		else if (sanitizedHex.Length == 2)
		{
			this.hexInput.caretPosition = 2;
		}
		else if (input.Length > sanitizedHex.Length && caretPosition < input.Length)
		{
			this.hexInput.caretPosition = caretPosition - input.Length + sanitizedHex.Length;
		}
		Color color;
		ColorUtility.TryParseHtmlString(sanitizedHex2, out color);
		this.bufferedColor.Set(color);
		this.UpdateMarkers();
		this.UpdateTextures();
	}

	// Token: 0x060004C5 RID: 1221 RVA: 0x00028818 File Offset: 0x00026A18
	private void MakeModeOptions()
	{
		if (this.modeDropdown == null || !this.modeDropdown.gameObject.activeInHierarchy)
		{
			return;
		}
		this.modeDropdown.ClearOptions();
		List<string> list = new List<string>();
		foreach (object obj in Enum.GetValues(typeof(FlexibleColorPicker.MainPickingMode)))
		{
			list.Add(((FlexibleColorPicker.MainPickingMode)obj).ToString());
		}
		this.modeDropdown.AddOptions(list);
		this.UpdateMode(this.mode);
	}

	// Token: 0x060004C6 RID: 1222 RVA: 0x000288D0 File Offset: 0x00026AD0
	private void UpdateMode(FlexibleColorPicker.MainPickingMode mode)
	{
		this.lastUpdatedMode = mode;
		if (this.modeDropdown == null || !this.modeDropdown.gameObject.activeInHierarchy)
		{
			return;
		}
		this.modeDropdown.value = (int)mode;
	}

	// Token: 0x060004C7 RID: 1223 RVA: 0x00028906 File Offset: 0x00026B06
	private static bool IsPreviewType(FlexibleColorPicker.PickerType type)
	{
		return type == FlexibleColorPicker.PickerType.Preview || type == FlexibleColorPicker.PickerType.PreviewAlpha;
	}

	// Token: 0x060004C8 RID: 1224 RVA: 0x00028918 File Offset: 0x00026B18
	private static bool IsAlphaType(FlexibleColorPicker.PickerType type)
	{
		return type == FlexibleColorPicker.PickerType.A || type == FlexibleColorPicker.PickerType.PreviewAlpha;
	}

	// Token: 0x060004C9 RID: 1225 RVA: 0x0002892C File Offset: 0x00026B2C
	private static bool IsHorizontal(FlexibleColorPicker.Picker p)
	{
		Vector2 size = p.image.rectTransform.rect.size;
		return size.x >= size.y;
	}

	// Token: 0x060004CA RID: 1226 RVA: 0x00028964 File Offset: 0x00026B64
	public static string GetSanitizedHex(string input, bool full)
	{
		if (string.IsNullOrEmpty(input))
		{
			return "#";
		}
		List<char> list = new List<char>();
		list.Add('#');
		int num = 0;
		char[] array = input.ToCharArray();
		while (list.Count < 7)
		{
			if (num >= input.Length)
			{
				break;
			}
			char c = char.ToUpper(array[num++]);
			if (FlexibleColorPicker.IsValidHexChar(c))
			{
				list.Add(c);
			}
		}
		while (full && list.Count < 7)
		{
			list.Insert(1, '0');
		}
		return new string(list.ToArray());
	}

	// Token: 0x060004CB RID: 1227 RVA: 0x000289E8 File Offset: 0x00026BE8
	private static bool IsValidHexChar(char c)
	{
		return char.IsNumber(c) | (c >= 'A' & c <= 'F');
	}

	// Token: 0x060004CC RID: 1228 RVA: 0x00028A02 File Offset: 0x00026C02
	public static Color ParseHex(string input)
	{
		return FlexibleColorPicker.ParseHex(input, Color.black);
	}

	// Token: 0x060004CD RID: 1229 RVA: 0x00028A10 File Offset: 0x00026C10
	public static Color ParseHex(string input, Color defaultColor)
	{
		Color result;
		if (ColorUtility.TryParseHtmlString(FlexibleColorPicker.GetSanitizedHex(input, true), out result))
		{
			return result;
		}
		return defaultColor;
	}

	// Token: 0x060004CE RID: 1230 RVA: 0x00028A30 File Offset: 0x00026C30
	private static Vector2 GetNormalizedPointerPosition(Canvas canvas, RectTransform rect, BaseEventData e)
	{
		switch (canvas.renderMode)
		{
		case RenderMode.ScreenSpaceOverlay:
			return FlexibleColorPicker.GetNormScreenSpace(rect, e);
		case RenderMode.ScreenSpaceCamera:
			if (canvas.worldCamera == null)
			{
				return FlexibleColorPicker.GetNormScreenSpace(rect, e);
			}
			return FlexibleColorPicker.GetNormWorldSpace(canvas, rect, e);
		case RenderMode.WorldSpace:
			if (canvas.worldCamera == null)
			{
				Debug.LogError("FCP in world space render mode requires an event camera to be set up on the parent canvas!");
				return Vector2.zero;
			}
			return FlexibleColorPicker.GetNormWorldSpace(canvas, rect, e);
		default:
			return Vector2.zero;
		}
	}

	// Token: 0x060004CF RID: 1231 RVA: 0x00028AAC File Offset: 0x00026CAC
	private static Vector2 GetNormScreenSpace(RectTransform rect, BaseEventData e)
	{
		Vector2 position = ((PointerEventData)e).position;
		Vector2 vector = rect.worldToLocalMatrix.MultiplyPoint(position);
		float x = Mathf.Clamp01(vector.x / rect.rect.size.x + rect.pivot.x);
		float y = Mathf.Clamp01(vector.y / rect.rect.size.y + rect.pivot.y);
		return new Vector2(x, y);
	}

	// Token: 0x060004D0 RID: 1232 RVA: 0x00028B40 File Offset: 0x00026D40
	private static Vector2 GetNormWorldSpace(Canvas canvas, RectTransform rect, BaseEventData e)
	{
		Vector2 position = ((PointerEventData)e).position;
		Ray ray = canvas.worldCamera.ScreenPointToRay(position);
		Plane plane = new Plane(canvas.transform.forward, canvas.transform.position);
		float d;
		plane.Raycast(ray, out d);
		Vector3 point = ray.origin + d * ray.direction;
		Vector2 vector = rect.worldToLocalMatrix.MultiplyPoint(point);
		float x = Mathf.Clamp01(vector.x / rect.rect.size.x + rect.pivot.x);
		float y = Mathf.Clamp01(vector.y / rect.rect.size.y + rect.pivot.y);
		return new Vector2(x, y);
	}

	// Token: 0x060004D1 RID: 1233 RVA: 0x00028C28 File Offset: 0x00026E28
	public static Color HSVToRGB(Vector3 hsv)
	{
		return FlexibleColorPicker.HSVToRGB(hsv.x, hsv.y, hsv.z);
	}

	// Token: 0x060004D2 RID: 1234 RVA: 0x00028C44 File Offset: 0x00026E44
	public static Color HSVToRGB(float h, float s, float v)
	{
		float num = s * v;
		float num2 = v - num;
		float num3 = num * (1f - Mathf.Abs(h % 2f - 1f)) + num2;
		num += num2;
		switch (Mathf.FloorToInt(h % 6f))
		{
		case 0:
			return new Color(num, num3, num2);
		case 1:
			return new Color(num3, num, num2);
		case 2:
			return new Color(num2, num, num3);
		case 3:
			return new Color(num2, num3, num);
		case 4:
			return new Color(num3, num2, num);
		case 5:
			return new Color(num, num2, num3);
		default:
			return Color.black;
		}
	}

	// Token: 0x060004D3 RID: 1235 RVA: 0x00028CE4 File Offset: 0x00026EE4
	public static Vector3 RGBToHSV(Color color)
	{
		float r = color.r;
		float g = color.g;
		float b = color.b;
		return FlexibleColorPicker.RGBToHSV(r, g, b);
	}

	// Token: 0x060004D4 RID: 1236 RVA: 0x00028D0C File Offset: 0x00026F0C
	public static Vector3 RGBToHSV(float r, float g, float b)
	{
		float num = Mathf.Max(new float[]
		{
			r,
			g,
			b
		});
		float num2 = Mathf.Min(new float[]
		{
			r,
			g,
			b
		});
		float num3 = num - num2;
		float x = 0f;
		if (num3 > 0f)
		{
			if (r >= b && r >= g)
			{
				x = Mathf.Repeat((g - b) / num3, 6f);
			}
			else if (g >= r && g >= b)
			{
				x = (b - r) / num3 + 2f;
			}
			else if (b >= r && b >= g)
			{
				x = (r - g) / num3 + 4f;
			}
		}
		float y = (num == 0f) ? 0f : (num3 / num);
		float z = num;
		return new Vector3(x, y, z);
	}

	// Token: 0x04000662 RID: 1634
	[Tooltip("Connections to the FCP's picker images, this should not be adjusted unless in advanced use cases.")]
	public FlexibleColorPicker.Picker[] pickers;

	// Token: 0x04000663 RID: 1635
	[Tooltip("Connection to the FCP's hexadecimal input field.")]
	public TMP_InputField hexInput;

	// Token: 0x04000664 RID: 1636
	[Tooltip("Connection to the FCP's mode dropdown menu.")]
	public Dropdown modeDropdown;

	// Token: 0x04000665 RID: 1637
	private Canvas canvas;

	// Token: 0x04000666 RID: 1638
	[Tooltip("The (starting) 2D picking mode, i.e. the 2 color values that can be picked with the large square picker.")]
	public FlexibleColorPicker.MainPickingMode mode;

	// Token: 0x04000667 RID: 1639
	[Tooltip("Sprites to be used in static mode on the main picker, one for each 2D mode.")]
	public Sprite[] staticSpriteMain;

	// Token: 0x04000668 RID: 1640
	private FlexibleColorPicker.BufferedColor bufferedColor;

	// Token: 0x04000669 RID: 1641
	private FlexibleColorPicker.Picker focusedPicker;

	// Token: 0x0400066A RID: 1642
	private FlexibleColorPicker.PickerType focusedPickerType;

	// Token: 0x0400066B RID: 1643
	private FlexibleColorPicker.MainPickingMode lastUpdatedMode;

	// Token: 0x0400066C RID: 1644
	private bool typeUpdate;

	// Token: 0x0400066D RID: 1645
	private bool triggeredStaticMode;

	// Token: 0x0400066E RID: 1646
	private bool materialsSeperated;

	// Token: 0x0400066F RID: 1647
	[Tooltip("Color set to the color picker on Start(). Before the start function, the standard public color variable is redirected to this value, so it may be changed at run time.")]
	public Color startingColor = Color.white;

	// Token: 0x04000670 RID: 1648
	[Tooltip("Use static mode: picker images are replaced by static images in stead of adaptive Unity shaders.")]
	public bool staticMode;

	// Token: 0x04000671 RID: 1649
	[Tooltip("Make sure FCP seperates its picker materials so that the dynamic mode works consistently, even when multiple FPCs are active at the same time. Turning this off yields a slight performance boost.")]
	public bool multiInstance = true;

	// Token: 0x04000672 RID: 1650
	private const float HUE_LOOP = 5.9999f;

	// Token: 0x04000673 RID: 1651
	private const string SHADER_MODE = "_Mode";

	// Token: 0x04000674 RID: 1652
	private const string SHADER_C1 = "_Color1";

	// Token: 0x04000675 RID: 1653
	private const string SHADER_C2 = "_Color2";

	// Token: 0x04000676 RID: 1654
	private const string SHADER_DOUBLE_MODE = "_DoubleMode";

	// Token: 0x04000677 RID: 1655
	private const string SHADER_HSV = "_HSV";

	// Token: 0x04000678 RID: 1656
	private const string SHADER_HSV_MIN = "_HSV_MIN";

	// Token: 0x04000679 RID: 1657
	private const string SHADER_HSV_MAX = "_HSV_MAX";

	// Token: 0x0400067A RID: 1658
	[Tooltip("More specific settings for color picker. Changes are not applied immediately, but require an FCP update to trigger.")]
	public FlexibleColorPicker.AdvancedSettings advancedSettings;

	// Token: 0x020000DC RID: 220
	[Serializable]
	public struct Picker
	{
		// Token: 0x0400067B RID: 1659
		public Image image;

		// Token: 0x0400067C RID: 1660
		public Sprite dynamicSprite;

		// Token: 0x0400067D RID: 1661
		public Sprite staticSpriteHor;

		// Token: 0x0400067E RID: 1662
		public Sprite staticSpriteVer;

		// Token: 0x0400067F RID: 1663
		public Material dynamicMaterial;
	}

	// Token: 0x020000DD RID: 221
	private enum PickerType
	{
		// Token: 0x04000681 RID: 1665
		Main,
		// Token: 0x04000682 RID: 1666
		R,
		// Token: 0x04000683 RID: 1667
		G,
		// Token: 0x04000684 RID: 1668
		B,
		// Token: 0x04000685 RID: 1669
		H,
		// Token: 0x04000686 RID: 1670
		S,
		// Token: 0x04000687 RID: 1671
		V,
		// Token: 0x04000688 RID: 1672
		A,
		// Token: 0x04000689 RID: 1673
		Preview,
		// Token: 0x0400068A RID: 1674
		PreviewAlpha
	}

	// Token: 0x020000DE RID: 222
	public enum MainPickingMode
	{
		// Token: 0x0400068C RID: 1676
		HS,
		// Token: 0x0400068D RID: 1677
		HV,
		// Token: 0x0400068E RID: 1678
		SH,
		// Token: 0x0400068F RID: 1679
		SV,
		// Token: 0x04000690 RID: 1680
		VH,
		// Token: 0x04000691 RID: 1681
		VS
	}

	// Token: 0x020000DF RID: 223
	[Serializable]
	public class AdvancedSettings
	{
		// Token: 0x060004D6 RID: 1238 RVA: 0x00028DDC File Offset: 0x00026FDC
		public FlexibleColorPicker.AdvancedSettings.PSettings Get(int i)
		{
			if (i <= 0 | i > 7)
			{
				return new FlexibleColorPicker.AdvancedSettings.PSettings();
			}
			return (new FlexibleColorPicker.AdvancedSettings.PSettings[]
			{
				this.r,
				this.g,
				this.b,
				this.h,
				this.s,
				this.v,
				this.a
			})[i - 1];
		}

		// Token: 0x04000692 RID: 1682
		public FlexibleColorPicker.AdvancedSettings.PSettings r;

		// Token: 0x04000693 RID: 1683
		public FlexibleColorPicker.AdvancedSettings.PSettings g;

		// Token: 0x04000694 RID: 1684
		public FlexibleColorPicker.AdvancedSettings.PSettings b;

		// Token: 0x04000695 RID: 1685
		public FlexibleColorPicker.AdvancedSettings.PSettings h;

		// Token: 0x04000696 RID: 1686
		public FlexibleColorPicker.AdvancedSettings.PSettings s;

		// Token: 0x04000697 RID: 1687
		public FlexibleColorPicker.AdvancedSettings.PSettings v;

		// Token: 0x04000698 RID: 1688
		public FlexibleColorPicker.AdvancedSettings.PSettings a;

		// Token: 0x020000E0 RID: 224
		[Serializable]
		public class PSettings
		{
			// Token: 0x04000699 RID: 1689
			[Tooltip("Normalized minimum for this color value, for restricting the slider range")]
			[Range(0f, 1f)]
			public float min;

			// Token: 0x0400069A RID: 1690
			[Tooltip("Normalized maximum for this color value, for restricting the slider range")]
			[Range(0f, 1f)]
			public float max = 1f;

			// Token: 0x0400069B RID: 1691
			[Tooltip("Make the picker associated with this value act static, even in a dynamic color picker setup")]
			public bool overrideStatic;
		}
	}

	// Token: 0x020000E1 RID: 225
	[Serializable]
	private class BufferedColor
	{
		// Token: 0x17000081 RID: 129
		// (get) Token: 0x060004D9 RID: 1241 RVA: 0x00028E59 File Offset: 0x00027059
		public float r
		{
			get
			{
				return this.color.r;
			}
		}

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x060004DA RID: 1242 RVA: 0x00028E66 File Offset: 0x00027066
		public float g
		{
			get
			{
				return this.color.g;
			}
		}

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x060004DB RID: 1243 RVA: 0x00028E73 File Offset: 0x00027073
		public float b
		{
			get
			{
				return this.color.b;
			}
		}

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x060004DC RID: 1244 RVA: 0x00028E80 File Offset: 0x00027080
		public float a
		{
			get
			{
				return this.color.a;
			}
		}

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x060004DD RID: 1245 RVA: 0x00028E8D File Offset: 0x0002708D
		public float h
		{
			get
			{
				return this.bufferedHue;
			}
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x060004DE RID: 1246 RVA: 0x00028E95 File Offset: 0x00027095
		public float s
		{
			get
			{
				return this.bufferedSaturation;
			}
		}

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x060004DF RID: 1247 RVA: 0x00028E9D File Offset: 0x0002709D
		public float v
		{
			get
			{
				return FlexibleColorPicker.RGBToHSV(this.color).z;
			}
		}

		// Token: 0x060004E0 RID: 1248 RVA: 0x00028EAF File Offset: 0x000270AF
		public BufferedColor()
		{
			this.bufferedHue = 0f;
			this.bufferedSaturation = 0f;
			this.color = Color.black;
		}

		// Token: 0x060004E1 RID: 1249 RVA: 0x00028ED8 File Offset: 0x000270D8
		public BufferedColor(Color color) : this()
		{
			this.Set(color);
		}

		// Token: 0x060004E2 RID: 1250 RVA: 0x00028EE7 File Offset: 0x000270E7
		public BufferedColor(Color color, float hue, float sat) : this(color)
		{
			this.bufferedHue = hue;
			this.bufferedSaturation = sat;
		}

		// Token: 0x060004E3 RID: 1251 RVA: 0x00028EFE File Offset: 0x000270FE
		public BufferedColor(Color color, FlexibleColorPicker.BufferedColor source) : this(color, source.bufferedHue, source.bufferedSaturation)
		{
			this.Set(color);
		}

		// Token: 0x060004E4 RID: 1252 RVA: 0x00028F1A File Offset: 0x0002711A
		public void Set(Color color)
		{
			this.Set(color, this.bufferedHue, this.bufferedSaturation);
		}

		// Token: 0x060004E5 RID: 1253 RVA: 0x00028F30 File Offset: 0x00027130
		public void Set(Color color, float bufferedHue, float bufferedSaturation)
		{
			this.color = color;
			Vector3 vector = FlexibleColorPicker.RGBToHSV(color);
			if (vector.y == 0f || vector.z == 0f)
			{
				this.bufferedHue = bufferedHue;
			}
			else
			{
				this.bufferedHue = vector.x;
			}
			if (vector.z == 0f)
			{
				this.bufferedSaturation = bufferedSaturation;
				return;
			}
			this.bufferedSaturation = vector.y;
		}

		// Token: 0x060004E6 RID: 1254 RVA: 0x00028FA4 File Offset: 0x000271A4
		public FlexibleColorPicker.BufferedColor PickR(float value)
		{
			Color color = this.color;
			color.r = value;
			return new FlexibleColorPicker.BufferedColor(color, this);
		}

		// Token: 0x060004E7 RID: 1255 RVA: 0x00028FC8 File Offset: 0x000271C8
		public FlexibleColorPicker.BufferedColor PickG(float value)
		{
			Color color = this.color;
			color.g = value;
			return new FlexibleColorPicker.BufferedColor(color, this);
		}

		// Token: 0x060004E8 RID: 1256 RVA: 0x00028FEC File Offset: 0x000271EC
		public FlexibleColorPicker.BufferedColor PickB(float value)
		{
			Color color = this.color;
			color.b = value;
			return new FlexibleColorPicker.BufferedColor(color, this);
		}

		// Token: 0x060004E9 RID: 1257 RVA: 0x00029010 File Offset: 0x00027210
		public FlexibleColorPicker.BufferedColor PickA(float value)
		{
			Color color = this.color;
			color.a = value;
			return new FlexibleColorPicker.BufferedColor(color, this);
		}

		// Token: 0x060004EA RID: 1258 RVA: 0x00029034 File Offset: 0x00027234
		public FlexibleColorPicker.BufferedColor PickH(float value)
		{
			Vector3 vector = FlexibleColorPicker.RGBToHSV(this.color);
			Color color = FlexibleColorPicker.HSVToRGB(value, vector.y, vector.z);
			color.a = this.color.a;
			return new FlexibleColorPicker.BufferedColor(color, value, this.bufferedSaturation);
		}

		// Token: 0x060004EB RID: 1259 RVA: 0x00029080 File Offset: 0x00027280
		public FlexibleColorPicker.BufferedColor PickS(float value)
		{
			Vector3 vector = FlexibleColorPicker.RGBToHSV(this.color);
			Color color = FlexibleColorPicker.HSVToRGB(this.bufferedHue, value, vector.z);
			color.a = this.color.a;
			return new FlexibleColorPicker.BufferedColor(color, this.bufferedHue, value);
		}

		// Token: 0x060004EC RID: 1260 RVA: 0x000290CC File Offset: 0x000272CC
		public FlexibleColorPicker.BufferedColor PickV(float value)
		{
			Color color = FlexibleColorPicker.HSVToRGB(this.bufferedHue, this.bufferedSaturation, value);
			color.a = this.color.a;
			return new FlexibleColorPicker.BufferedColor(color, this.bufferedHue, this.bufferedSaturation);
		}

		// Token: 0x0400069C RID: 1692
		public Color color;

		// Token: 0x0400069D RID: 1693
		private float bufferedHue;

		// Token: 0x0400069E RID: 1694
		private float bufferedSaturation;
	}
}
