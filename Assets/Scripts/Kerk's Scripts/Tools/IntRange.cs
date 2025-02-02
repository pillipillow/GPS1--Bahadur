﻿using System;

[Serializable]
public class IntRange
{
	public int m_Min;
	public int m_Max;

	public IntRange(int min, int max)
	{
		m_Min = min;
		m_Max = max;
	}

	public int Random
	{
		get { return UnityEngine.Random.Range (m_Min, m_Max); }
	}

	public int Min
	{
		get { return m_Min; }
	}

	public int Max
	{
		get { return m_Max; }
	}
}
