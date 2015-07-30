using UnityEngine;
using System.Collections;
using System;

[Flags]
public enum ABILITY_ELEMENT {
	NONE = 0x0,
	KINETIC = 0x1,
	FIRE = 0x2,
	WATER = 0x4,
	AIR = 0x8,
	EARTH = 0x16,
	LIGHT = 0x32,
	DARK = 0x64,
	ELECTRIC = 0x128,
	ICE = 0x256
}

public enum ABILITY_TYPE {
	NONE = 0,
	ACTIVE,
	PASSIVE,
	UNIT_TYPE,
	RACE
}

public enum EFFECT_TYPE {
	NONE = 0,
	ACTIVATION,
	RESOLUTION,
	PASSIVE
}

public enum TERRAIN_TYPE {
	NONE = 0,
	PLAINS,
	WASTELAND,
	DESERT,
	QUAGMIRE,
	DUST_BOWL,
	RIVER
}
