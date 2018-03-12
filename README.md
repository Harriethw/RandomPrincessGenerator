RandomPrincessGenerator

Narrative text generator in Unity

Inspired by Tracery (https://github.com/galaxykate/tracery) and other generative text tools, I wanted to create my own tool or library for generating short character descriptions.

--Resources

The Resources folder contains dictionaries of words in txt files. Mostly used from http://scrapmaker.com/home

--Pixel Character Builder

This is a fantastic free tool from the Unity asset store for customising your own pixel characters and randomly generating them https://assetstore.unity.com/packages/tools/sprite-management/pixel-character-builder-89154

--TextGenerator

Gets the text files from Resources, adds them to arrays. On Generate, picks random words from the arrays, then adds them to a random choice of story structure. Calls the pixel character tool to generate a random pixel character
