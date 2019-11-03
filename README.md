Unity Define Manager
==================

Easily manage global and platform-specific defines in Unity.

![](https://raw.githubusercontent.com/caneva20/UnityDefineManager/master/screenshot.png)

## Defines
**CSharp**: These are applied for all Runtime scripts

**Editor**: These are applied for all Editor scripts

**Platform Defines**: These are specific to the targeted platform, and Editor.

## Install
Use [UpmGitExtension](https://github.com/mob-sakai/UpmGitExtension) (Recommended/Easier)

**Or**

Find `Packages/manifest.json` in your project and edit it to look like this:
```json
{
  "dependencies": {
    "caneva20.unitydefinemanager": "https://github.com/caneva20/UnityDefineManager.git#0.1.1-preview",
    ...
  },
}
```

## Quick Start

**Editor**

- Open `Window/Unity Define Manager`

**Scripting**
```CSharp
//Add new Runtime Define
GlobalDefineUtility.AddDefine(Compiler.CSharp, "RUNTIME_DEFINE_NAME");

//Add new Editor Define
GlobalDefineUtility.AddDefine(Compiler.Editor, "EDITOR_DEFINE_NAME");



//Remove new Runtime Define
GlobalDefineUtility.RemoveDefine(Compiler.CSharp, "RUNTIME_DEFINE_NAME");

//Remove new Editor Define
GlobalDefineUtility.RemoveDefine(Compiler.Editor, "EDITOR_DEFINE_NAME");
```
