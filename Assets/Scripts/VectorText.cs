using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class VectorText{
	class LetterData{
		public float[] floats = null;
		public int[] ints = null;
		public Vector2 getVertex(int index, float scale){
			var offset = index * 2;
			return new Vector2(floats[offset] * scale, floats[offset + 1] * scale);
		}
		public LetterData(float[] floats_, int[] ints_){
			floats = floats_;
			ints = ints_;
		}
	}

	static Dictionary<char, LetterData> letters = null;
	const float charScale = 10.0f;//well, damn it.

	public delegate void LetterDelegate<Data>(Vector2 a, Vector2 b, Data data);

	public static bool processLetter<Data>(char letter, LetterDelegate<Data> callback, Data data){
		if (callback == null)
			throw new System.ArgumentNullException();
		initLetters();

		if (!letters.ContainsKey(letter))
			return false;

		var curLetterData = letters[letter];

		for (int i = 0; i < curLetterData.ints.Length; i+= 2){
			int idxa = curLetterData.ints[i];
			int idxb = curLetterData.ints[i+1];

			callback(curLetterData.getVertex(idxa, charScale), curLetterData.getVertex(idxb, charScale), data);
		}

		return true;
	}

	static void initLetters(){
		if (letters != null)
			return;
		letters = new Dictionary<char, LetterData>();
		letters[' '] = new LetterData(new float[]{}, new int[]{});
		letters['!'] = new LetterData(new float[]{0.04f, 0.02f, 0.05f, 0.03f, 0.06f, 0.02f, 0.05f, 0.01f, 0.04f, 0.18f, 0.05f, 0.19f, 0.06f, 0.18f, 0.05f, 0.05f}, new int[]{0, 1, 1, 2, 2, 3, 3, 0, 4, 5, 5, 6, 6, 7, 7, 4});
		letters['"'] = new LetterData(new float[]{0.03f, 0.17f, 0.04f, 0.18f, 0.05f, 0.17f, 0.03f, 0.15f, 0.05f, 0.17f, 0.06f, 0.18f, 0.07f, 0.17f, 0.05f, 0.15f}, new int[]{0, 1, 1, 2, 2, 3, 3, 0, 4, 5, 5, 6, 6, 7, 7, 4});
		letters['#'] = new LetterData(new float[]{0.04f, 0.05f, 0.04f, 0.14f, 0.02f, 0.1f, 0.08f, 0.13f, 0.06f, 0.15f, 0.06f, 0.06f, 0.02f, 0.07f, 0.08f, 0.1f}, new int[]{0, 1, 2, 3, 4, 5, 6, 7});
		letters['%'] = new LetterData(new float[]{0.02f, 0.16f, 0.03f, 0.17f, 0.04f, 0.16f, 0.03f, 0.15f, 0.08f, 0.17f, 0.02f, 0.03f, 0.06f, 0.04f, 0.07f, 0.05f, 0.08f, 0.04f, 0.07f, 0.03f}, new int[]{0, 1, 1, 2, 2, 3, 3, 0, 4, 5, 6, 7, 7, 8, 8, 9, 9, 6});
		letters['\''] = new LetterData(new float[]{0.04f, 0.17f, 0.05f, 0.18f, 0.06f, 0.17f, 0.04f, 0.15f}, new int[]{0, 1, 1, 2, 2, 3, 3, 0});
		letters['('] = new LetterData(new float[]{0.04f, 0.03f, 0.04f, 0.17f, 0.06f, 0.19f, 0.06f, 0.01f}, new int[]{0, 1, 1, 2, 3, 0});
		letters[')'] = new LetterData(new float[]{0.04f, 0.19f, 0.06f, 0.17f, 0.06f, 0.03f, 0.04f, 0.01f}, new int[]{0, 1, 1, 2, 2, 3});
		letters['*'] = new LetterData(new float[]{0.06f, 0.08f, 0.04f, 0.12f, 0.06f, 0.12f, 0.04f, 0.08f, 0.07f, 0.1f, 0.03f, 0.1f}, new int[]{0, 1, 2, 3, 4, 5});
		letters['+'] = new LetterData(new float[]{0.02f, 0.1f, 0.08f, 0.1f, 0.05f, 0.13f, 0.05f, 0.07f}, new int[]{0, 1, 2, 3});
		letters[','] = new LetterData(new float[]{0.04f, 0.02f, 0.05f, 0.03f, 0.06f, 0.02f, 0.04f, 0.0f}, new int[]{0, 1, 1, 2, 3, 0, 3, 2});
		letters['-'] = new LetterData(new float[]{0.08f, 0.1f, 0.02f, 0.1f}, new int[]{0, 1});
		letters['.'] = new LetterData(new float[]{0.04f, 0.02f, 0.05f, 0.03f, 0.06f, 0.02f, 0.05f, 0.01f}, new int[]{0, 1, 1, 2, 2, 3, 3, 0});
		letters['/'] = new LetterData(new float[]{0.09f, 0.19f, 0.01f, 0.01f}, new int[]{0, 1});
		letters['0'] = new LetterData(new float[]{0.01f, 0.19f, 0.09f, 0.19f, 0.01f, 0.01f, 0.09f, 0.01f, 0.01f, 0.01f, 0.09f, 0.19f}, new int[]{0, 1, 2, 0, 1, 3, 3, 2, 4, 5});
		letters['1'] = new LetterData(new float[]{0.09f, 0.19f, 0.09f, 0.01f}, new int[]{0, 1});
		letters['2'] = new LetterData(new float[]{0.01f, 0.19f, 0.09f, 0.19f, 0.01f, 0.01f, 0.09f, 0.01f, 0.09f, 0.1f, 0.01f, 0.1f}, new int[]{0, 1, 2, 3, 1, 4, 4, 5, 2, 5});
		letters['3'] = new LetterData(new float[]{0.01f, 0.19f, 0.09f, 0.19f, 0.09f, 0.01f, 0.01f, 0.01f, 0.01f, 0.1f, 0.09f, 0.1f}, new int[]{0, 1, 1, 2, 2, 3, 4, 5});
		letters['4'] = new LetterData(new float[]{0.01f, 0.19f, 0.01f, 0.1f, 0.09f, 0.19f, 0.09f, 0.01f, 0.09f, 0.1f}, new int[]{0, 1, 2, 3, 1, 4});
		letters['5'] = new LetterData(new float[]{0.01f, 0.1f, 0.01f, 0.19f, 0.09f, 0.19f, 0.09f, 0.1f, 0.09f, 0.01f, 0.01f, 0.01f}, new int[]{0, 1, 1, 2, 3, 0, 3, 4, 4, 5});
		letters['6'] = new LetterData(new float[]{0.01f, 0.01f, 0.01f, 0.19f, 0.09f, 0.19f, 0.09f, 0.1f, 0.09f, 0.01f, 0.01f, 0.1f}, new int[]{0, 1, 1, 2, 3, 4, 4, 0, 5, 3});
		letters['7'] = new LetterData(new float[]{0.01f, 0.19f, 0.09f, 0.19f, 0.09f, 0.01f}, new int[]{0, 1, 1, 2});
		letters['8'] = new LetterData(new float[]{0.01f, 0.01f, 0.01f, 0.19f, 0.09f, 0.19f, 0.09f, 0.01f, 0.01f, 0.1f, 0.09f, 0.1f}, new int[]{0, 1, 1, 2, 2, 3, 3, 0, 4, 5});
		letters['9'] = new LetterData(new float[]{0.01f, 0.1f, 0.01f, 0.19f, 0.09f, 0.19f, 0.09f, 0.01f, 0.09f, 0.1f, 0.01f, 0.01f}, new int[]{0, 1, 1, 2, 2, 3, 4, 0, 3, 5});
		letters[':'] = new LetterData(new float[]{0.04f, 0.14f, 0.05f, 0.15f, 0.06f, 0.14f, 0.05f, 0.13f, 0.04f, 0.02f, 0.05f, 0.03f, 0.06f, 0.02f, 0.05f, 0.01f}, new int[]{0, 1, 1, 2, 2, 3, 3, 0, 4, 5, 5, 6, 6, 7, 7, 4});
		letters[';'] = new LetterData(new float[]{0.04f, 0.14f, 0.05f, 0.15f, 0.06f, 0.14f, 0.05f, 0.13f, 0.04f, 0.02f, 0.05f, 0.03f, 0.06f, 0.02f, 0.04f, 0.0f}, new int[]{0, 1, 1, 2, 2, 3, 3, 0, 4, 5, 5, 6, 6, 7, 7, 4});
		letters['<'] = new LetterData(new float[]{0.08f, 0.07f, 0.02f, 0.1f, 0.08f, 0.13f}, new int[]{0, 1, 1, 2});
		letters['='] = new LetterData(new float[]{0.02f, 0.12f, 0.08f, 0.12f, 0.02f, 0.08f, 0.08f, 0.08f}, new int[]{0, 1, 2, 3});
		letters['>'] = new LetterData(new float[]{0.02f, 0.13f, 0.08f, 0.1f, 0.02f, 0.07f}, new int[]{0, 1, 1, 2});
		letters['?'] = new LetterData(new float[]{0.04f, 0.02f, 0.05f, 0.03f, 0.06f, 0.02f, 0.05f, 0.01f, 0.05f, 0.05f, 0.05f, 0.08f, 0.09f, 0.12f, 0.09f, 0.17f, 0.07f, 0.19f, 0.03f, 0.19f, 0.01f, 0.17f}, new int[]{0, 1, 1, 2, 2, 3, 3, 0, 4, 5, 5, 6, 6, 7, 7, 8, 8, 9, 9, 10});
		letters['@'] = new LetterData(new float[]{0.01f, 0.17f, 0.03f, 0.19f, 0.07f, 0.19f, 0.01f, 0.03f, 0.09f, 0.17f, 0.03f, 0.01f, 0.07f, 0.01f, 0.09f, 0.03f, 0.09f, 0.1f, 0.07f, 0.08f, 0.05f, 0.08f, 0.03f, 0.1f, 0.03f, 0.12f, 0.05f, 0.14f}, new int[]{0, 1, 1, 2, 3, 0, 2, 4, 5, 3, 6, 5, 7, 6, 4, 8, 8, 9, 9, 10, 10, 11, 11, 12, 12, 13});
		letters['A'] = new LetterData(new float[]{0.01f, 0.01f, 0.05f, 0.19f, 0.09f, 0.01f, 0.03f, 0.1f, 0.07f, 0.1f}, new int[]{0, 1, 1, 2, 3, 4});
		letters['B'] = new LetterData(new float[]{0.01f, 0.01f, 0.01f, 0.19f, 0.07f, 0.19f, 0.09f, 0.03f, 0.07f, 0.01f, 0.09f, 0.08f, 0.07f, 0.1f, 0.01f, 0.1f, 0.09f, 0.17f, 0.09f, 0.12f}, new int[]{0, 1, 1, 2, 3, 4, 4, 0, 5, 3, 6, 5, 7, 6, 2, 8, 8, 9, 6, 9});
		letters['C'] = new LetterData(new float[]{0.01f, 0.03f, 0.01f, 0.17f, 0.03f, 0.19f, 0.07f, 0.01f, 0.03f, 0.01f, 0.09f, 0.03f, 0.07f, 0.19f, 0.09f, 0.17f}, new int[]{0, 1, 1, 2, 3, 4, 4, 0, 5, 3, 2, 6, 6, 7});
		letters['D'] = new LetterData(new float[]{0.01f, 0.01f, 0.01f, 0.19f, 0.07f, 0.19f, 0.09f, 0.17f, 0.09f, 0.03f, 0.07f, 0.01f}, new int[]{0, 1, 1, 2, 3, 4, 2, 3, 4, 5, 0, 5});
		letters['E'] = new LetterData(new float[]{0.01f, 0.01f, 0.01f, 0.19f, 0.09f, 0.19f, 0.06f, 0.1f, 0.01f, 0.1f, 0.09f, 0.01f}, new int[]{0, 1, 1, 2, 3, 4, 5, 0});
		letters['F'] = new LetterData(new float[]{0.01f, 0.01f, 0.01f, 0.19f, 0.09f, 0.19f, 0.06f, 0.1f, 0.01f, 0.1f}, new int[]{0, 1, 1, 2, 3, 4});
		letters['G'] = new LetterData(new float[]{0.01f, 0.03f, 0.01f, 0.17f, 0.03f, 0.19f, 0.07f, 0.19f, 0.09f, 0.17f, 0.03f, 0.01f, 0.07f, 0.01f, 0.09f, 0.03f, 0.09f, 0.06f, 0.07f, 0.06f}, new int[]{0, 1, 1, 2, 3, 4, 2, 3, 5, 0, 6, 5, 7, 6, 8, 7, 9, 8});
		letters['H'] = new LetterData(new float[]{0.01f, 0.1f, 0.09f, 0.1f, 0.09f, 0.19f, 0.09f, 0.01f, 0.01f, 0.19f, 0.01f, 0.01f}, new int[]{0, 1, 2, 3, 4, 5});
		letters['I'] = new LetterData(new float[]{0.03f, 0.01f, 0.07f, 0.01f, 0.05f, 0.19f, 0.05f, 0.01f, 0.03f, 0.19f, 0.07f, 0.19f}, new int[]{0, 1, 2, 3, 4, 5});
		letters['J'] = new LetterData(new float[]{0.05f, 0.01f, 0.03f, 0.03f, 0.03f, 0.19f, 0.07f, 0.19f, 0.07f, 0.19f, 0.07f, 0.03f}, new int[]{0, 1, 2, 3, 4, 5, 5, 0});
		letters['K'] = new LetterData(new float[]{0.01f, 0.01f, 0.01f, 0.19f, 0.01f, 0.1f, 0.09f, 0.19f, 0.09f, 0.01f}, new int[]{0, 1, 2, 3, 4, 2});
		letters['L'] = new LetterData(new float[]{0.01f, 0.01f, 0.01f, 0.19f, 0.09f, 0.01f}, new int[]{0, 1, 2, 0});
		letters['M'] = new LetterData(new float[]{0.01f, 0.01f, 0.01f, 0.19f, 0.05f, 0.1f, 0.09f, 0.19f, 0.09f, 0.01f}, new int[]{0, 1, 1, 2, 2, 3, 3, 4});
		letters['N'] = new LetterData(new float[]{0.01f, 0.01f, 0.01f, 0.19f, 0.09f, 0.01f, 0.09f, 0.19f}, new int[]{0, 1, 2, 3, 1, 2});
		letters['O'] = new LetterData(new float[]{0.01f, 0.03f, 0.01f, 0.17f, 0.07f, 0.19f, 0.09f, 0.17f, 0.03f, 0.19f, 0.09f, 0.03f, 0.07f, 0.01f, 0.03f, 0.01f}, new int[]{0, 1, 2, 3, 4, 2, 1, 4, 3, 5, 5, 6, 7, 0, 6, 7});
		letters['P'] = new LetterData(new float[]{0.01f, 0.01f, 0.01f, 0.19f, 0.07f, 0.19f, 0.09f, 0.17f, 0.09f, 0.12f, 0.07f, 0.1f, 0.01f, 0.1f}, new int[]{0, 1, 1, 2, 3, 4, 2, 3, 4, 5, 5, 6});
		letters['Q'] = new LetterData(new float[]{0.01f, 0.03f, 0.01f, 0.17f, 0.03f, 0.19f, 0.07f, 0.19f, 0.09f, 0.17f, 0.09f, 0.04f, 0.03f, 0.01f, 0.06f, 0.01f, 0.06f, 0.04f, 0.09f, 0.01f}, new int[]{0, 1, 1, 2, 3, 4, 2, 3, 4, 5, 6, 0, 7, 6, 5, 7, 8, 9});
		letters['R'] = new LetterData(new float[]{0.01f, 0.01f, 0.01f, 0.19f, 0.07f, 0.19f, 0.09f, 0.17f, 0.09f, 0.12f, 0.07f, 0.1f, 0.09f, 0.01f, 0.05f, 0.1f, 0.01f, 0.1f}, new int[]{0, 1, 1, 2, 3, 4, 2, 3, 4, 5, 6, 7, 5, 8});
		letters['S'] = new LetterData(new float[]{0.01f, 0.12f, 0.01f, 0.17f, 0.03f, 0.19f, 0.07f, 0.19f, 0.09f, 0.17f, 0.03f, 0.1f, 0.07f, 0.1f, 0.09f, 0.08f, 0.09f, 0.03f, 0.07f, 0.01f, 0.03f, 0.01f, 0.01f, 0.03f}, new int[]{0, 1, 1, 2, 3, 4, 2, 3, 5, 0, 6, 5, 7, 6, 8, 7, 9, 8, 10, 9, 11, 10});
		letters['T'] = new LetterData(new float[]{0.01f, 0.19f, 0.09f, 0.19f, 0.05f, 0.19f, 0.05f, 0.01f}, new int[]{0, 1, 2, 3});
		letters['U'] = new LetterData(new float[]{0.01f, 0.03f, 0.01f, 0.19f, 0.09f, 0.03f, 0.07f, 0.01f, 0.03f, 0.01f, 0.09f, 0.19f}, new int[]{0, 1, 2, 3, 3, 4, 4, 0, 5, 2});
		letters['V'] = new LetterData(new float[]{0.05f, 0.01f, 0.01f, 0.19f, 0.09f, 0.19f}, new int[]{0, 1, 2, 0});
		letters['W'] = new LetterData(new float[]{0.01f, 0.01f, 0.01f, 0.19f, 0.09f, 0.19f, 0.09f, 0.01f, 0.05f, 0.06f}, new int[]{0, 1, 2, 3, 3, 4, 4, 0});
		letters['X'] = new LetterData(new float[]{0.09f, 0.01f, 0.01f, 0.19f, 0.01f, 0.01f, 0.09f, 0.19f}, new int[]{0, 1, 2, 3});
		letters['Y'] = new LetterData(new float[]{0.05f, 0.1f, 0.01f, 0.19f, 0.05f, 0.01f, 0.09f, 0.19f}, new int[]{0, 1, 0, 2, 3, 0});
		letters['Z'] = new LetterData(new float[]{0.09f, 0.19f, 0.01f, 0.19f, 0.09f, 0.01f, 0.01f, 0.01f}, new int[]{0, 1, 2, 3, 3, 0});
		letters['['] = new LetterData(new float[]{0.04f, 0.01f, 0.04f, 0.19f, 0.06f, 0.19f, 0.06f, 0.01f}, new int[]{0, 1, 1, 2, 3, 0});
		letters['\\'] = new LetterData(new float[]{0.09f, 0.01f, 0.01f, 0.19f}, new int[]{0, 1});
		letters[']'] = new LetterData(new float[]{0.04f, 0.19f, 0.06f, 0.19f, 0.06f, 0.01f, 0.04f, 0.01f}, new int[]{0, 1, 1, 2, 2, 3});
		letters['^'] = new LetterData(new float[]{0.03f, 0.17f, 0.05f, 0.19f, 0.07f, 0.17f}, new int[]{0, 1, 1, 2});
		letters['_'] = new LetterData(new float[]{0.09f, 0.01f, 0.01f, 0.01f}, new int[]{0, 1});
		letters['`'] = new LetterData(new float[]{0.06f, 0.15f, 0.04f, 0.17f, 0.05f, 0.18f, 0.06f, 0.17f}, new int[]{0, 1, 1, 2, 2, 3, 3, 0});
		letters['a'] = new LetterData(new float[]{0.08f, 0.03f, 0.06f, 0.01f, 0.02f, 0.12f, 0.04f, 0.14f, 0.06f, 0.14f, 0.08f, 0.12f, 0.09f, 0.01f, 0.04f, 0.01f, 0.02f, 0.03f, 0.02f, 0.07f, 0.04f, 0.09f, 0.06f, 0.09f, 0.08f, 0.07f}, new int[]{0, 1, 2, 3, 4, 5, 3, 4, 5, 0, 0, 6, 1, 7, 7, 8, 8, 9, 9, 10, 10, 11, 11, 12});
		letters['b'] = new LetterData(new float[]{0.02f, 0.01f, 0.02f, 0.14f, 0.08f, 0.03f, 0.08f, 0.07f, 0.06f, 0.01f, 0.04f, 0.01f, 0.06f, 0.09f, 0.04f, 0.09f, 0.02f, 0.07f, 0.02f, 0.03f}, new int[]{0, 1, 2, 3, 4, 2, 5, 4, 3, 6, 6, 7, 7, 8, 9, 5});
		letters['c'] = new LetterData(new float[]{0.02f, 0.03f, 0.02f, 0.08f, 0.04f, 0.1f, 0.06f, 0.1f, 0.04f, 0.01f, 0.08f, 0.08f, 0.06f, 0.01f, 0.08f, 0.03f}, new int[]{0, 1, 2, 3, 1, 2, 4, 0, 3, 5, 6, 4, 7, 6});
		letters['d'] = new LetterData(new float[]{0.02f, 0.03f, 0.02f, 0.07f, 0.04f, 0.09f, 0.06f, 0.01f, 0.04f, 0.01f, 0.06f, 0.09f, 0.08f, 0.07f, 0.08f, 0.03f, 0.08f, 0.01f, 0.08f, 0.14f}, new int[]{0, 1, 1, 2, 3, 4, 4, 0, 2, 5, 5, 6, 7, 3, 8, 9});
		letters['e'] = new LetterData(new float[]{0.04f, 0.01f, 0.02f, 0.03f, 0.02f, 0.08f, 0.04f, 0.1f, 0.06f, 0.1f, 0.06f, 0.01f, 0.08f, 0.03f, 0.08f, 0.08f, 0.08f, 0.06f, 0.02f, 0.06f}, new int[]{0, 1, 2, 3, 3, 4, 1, 2, 5, 0, 6, 5, 4, 7, 7, 8, 8, 9});
		letters['f'] = new LetterData(new float[]{0.09f, 0.12f, 0.07f, 0.14f, 0.03f, 0.1f, 0.07f, 0.1f, 0.05f, 0.12f, 0.05f, 0.01f}, new int[]{0, 1, 2, 3, 4, 5, 1, 4});
		letters['g'] = new LetterData(new float[]{0.04f, 0.01f, 0.02f, 0.03f, 0.06f, 0.01f, 0.08f, 0.08f, 0.08f, 0.03f, 0.06f, 0.1f, 0.04f, 0.1f, 0.02f, 0.08f, 0.02f, 0.06f, 0.04f, 0.04f, 0.06f, 0.04f, 0.08f, 0.06f}, new int[]{0, 1, 2, 0, 3, 4, 4, 2, 5, 3, 6, 5, 7, 6, 8, 7, 9, 8, 10, 9, 11, 10});
		letters['h'] = new LetterData(new float[]{0.02f, 0.01f, 0.02f, 0.14f, 0.06f, 0.1f, 0.08f, 0.08f, 0.04f, 0.1f, 0.02f, 0.08f, 0.08f, 0.01f}, new int[]{0, 1, 2, 3, 4, 2, 5, 4, 3, 6});
		letters['i'] = new LetterData(new float[]{0.05f, 0.01f, 0.05f, 0.1f, 0.05f, 0.13f, 0.06f, 0.12f, 0.04f, 0.12f, 0.05f, 0.11f}, new int[]{0, 1, 2, 3, 4, 2, 5, 4, 3, 5});
		letters['j'] = new LetterData(new float[]{0.06f, 0.12f, 0.07f, 0.13f, 0.08f, 0.12f, 0.07f, 0.11f, 0.07f, 0.1f, 0.07f, 0.03f, 0.05f, 0.01f, 0.03f, 0.03f}, new int[]{0, 1, 1, 2, 2, 3, 3, 0, 4, 5, 5, 6, 6, 7});
		letters['k'] = new LetterData(new float[]{0.02f, 0.01f, 0.02f, 0.14f, 0.02f, 0.06f, 0.08f, 0.01f, 0.08f, 0.1f}, new int[]{0, 1, 2, 3, 4, 2});
		letters['l'] = new LetterData(new float[]{0.02f, 0.01f, 0.02f, 0.14f}, new int[]{0, 1});
		letters['m'] = new LetterData(new float[]{0.02f, 0.01f, 0.02f, 0.1f, 0.08f, 0.08f, 0.06f, 0.1f, 0.08f, 0.01f, 0.05f, 0.09f, 0.05f, 0.01f, 0.04f, 0.1f, 0.02f, 0.08f}, new int[]{0, 1, 2, 3, 4, 2, 5, 6, 7, 8, 5, 3, 7, 5});
		letters['n'] = new LetterData(new float[]{0.02f, 0.01f, 0.02f, 0.1f, 0.06f, 0.1f, 0.04f, 0.1f, 0.08f, 0.08f, 0.08f, 0.01f, 0.02f, 0.08f}, new int[]{0, 1, 2, 3, 4, 2, 5, 4, 3, 6});
		letters['o'] = new LetterData(new float[]{0.02f, 0.03f, 0.02f, 0.08f, 0.04f, 0.1f, 0.06f, 0.01f, 0.04f, 0.01f, 0.08f, 0.03f, 0.08f, 0.08f, 0.06f, 0.1f}, new int[]{0, 1, 1, 2, 3, 4, 4, 0, 5, 3, 6, 5, 7, 6, 2, 7});
		letters['p'] = new LetterData(new float[]{0.02f, 0.1f, 0.02f, 0.01f, 0.04f, 0.1f, 0.06f, 0.1f, 0.02f, 0.08f, 0.04f, 0.02f, 0.02f, 0.04f, 0.08f, 0.08f, 0.08f, 0.04f, 0.06f, 0.02f}, new int[]{0, 1, 2, 3, 4, 2, 5, 6, 3, 7, 7, 8, 8, 9, 5, 9});
		letters['q'] = new LetterData(new float[]{0.02f, 0.04f, 0.02f, 0.08f, 0.04f, 0.1f, 0.06f, 0.02f, 0.04f, 0.02f, 0.08f, 0.04f, 0.06f, 0.1f, 0.08f, 0.08f, 0.08f, 0.1f, 0.08f, 0.01f}, new int[]{0, 1, 1, 2, 3, 4, 4, 0, 5, 3, 2, 6, 6, 7, 8, 9});
		letters['r'] = new LetterData(new float[]{0.02f, 0.01f, 0.02f, 0.1f, 0.02f, 0.08f, 0.04f, 0.1f, 0.06f, 0.1f, 0.08f, 0.08f}, new int[]{0, 1, 2, 3, 4, 5, 3, 4});
		letters['s'] = new LetterData(new float[]{0.02f, 0.07f, 0.02f, 0.08f, 0.04f, 0.1f, 0.06f, 0.1f, 0.03f, 0.06f, 0.07f, 0.06f, 0.08f, 0.08f, 0.08f, 0.05f, 0.08f, 0.03f, 0.06f, 0.01f, 0.04f, 0.01f, 0.02f, 0.03f}, new int[]{0, 1, 2, 3, 1, 2, 4, 5, 3, 6, 0, 4, 5, 7, 7, 8, 8, 9, 9, 10, 10, 11});
		letters['t'] = new LetterData(new float[]{0.05f, 0.03f, 0.05f, 0.14f, 0.03f, 0.1f, 0.07f, 0.1f, 0.09f, 0.03f, 0.07f, 0.01f}, new int[]{0, 1, 2, 3, 4, 5, 5, 0});
		letters['u'] = new LetterData(new float[]{0.02f, 0.03f, 0.02f, 0.1f, 0.08f, 0.03f, 0.06f, 0.01f, 0.04f, 0.01f, 0.08f, 0.1f, 0.08f, 0.01f}, new int[]{0, 1, 2, 3, 3, 4, 4, 0, 5, 6});
		letters['v'] = new LetterData(new float[]{0.05f, 0.01f, 0.02f, 0.1f, 0.08f, 0.1f}, new int[]{0, 1, 2, 0});
		letters['w'] = new LetterData(new float[]{0.03f, 0.01f, 0.02f, 0.1f, 0.08f, 0.1f, 0.07f, 0.01f, 0.05f, 0.04f}, new int[]{0, 1, 2, 3, 3, 4, 4, 0});
		letters['x'] = new LetterData(new float[]{0.08f, 0.01f, 0.02f, 0.1f, 0.08f, 0.1f, 0.02f, 0.01f}, new int[]{0, 1, 2, 3});
		letters['y'] = new LetterData(new float[]{0.05f, 0.06f, 0.05f, 0.01f, 0.02f, 0.1f, 0.08f, 0.1f}, new int[]{0, 1, 0, 2, 3, 0});
		letters['z'] = new LetterData(new float[]{0.08f, 0.1f, 0.02f, 0.1f, 0.08f, 0.01f, 0.02f, 0.01f}, new int[]{0, 1, 2, 3, 3, 0});
		letters['{'] = new LetterData(new float[]{0.04f, 0.17f, 0.06f, 0.19f, 0.04f, 0.09f, 0.03f, 0.1f, 0.04f, 0.11f, 0.04f, 0.03f, 0.06f, 0.01f}, new int[]{0, 1, 2, 3, 3, 4, 4, 0, 5, 2, 6, 5});
		letters['|'] = new LetterData(new float[]{0.05f, 0.01f, 0.05f, 0.19f}, new int[]{0, 1});
		letters['}'] = new LetterData(new float[]{0.07f, 0.1f, 0.06f, 0.09f, 0.06f, 0.11f, 0.04f, 0.19f, 0.06f, 0.17f, 0.06f, 0.03f, 0.04f, 0.01f}, new int[]{0, 1, 2, 0, 3, 4, 4, 2, 1, 5, 5, 6});
		letters['~'] = new LetterData(new float[]{0.04f, 0.17f, 0.03f, 0.16f, 0.07f, 0.16f, 0.06f, 0.15f}, new int[]{0, 1, 2, 3, 3, 0});
		letters['\x007f'] = new LetterData(new float[]{0.01f, 0.01f, 0.01f, 0.19f, 0.09f, 0.19f, 0.09f, 0.01f}, new int[]{0, 1, 1, 2, 2, 3, 3, 0, 1, 3, 2, 0});
	}
}