using System;

// An enum containing all possible types of content. Mostly for convenience and to make sure no wrong values are selected
[Serializable]
public enum ContentType {
	text,
	audio,
	pictures,
	special,
	keypad,
	research
}
