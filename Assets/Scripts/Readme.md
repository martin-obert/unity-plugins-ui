# UI Package
Contains generic repeaters for rendering lists, grids and other UI element containers. Abstracts data binding and enforces reactive programming.

# Prerequisites
UniTask: from git url
```https://github.com/Cysharp/UniTask.git?path=src/UniTask/Assets/Plugins/UniTask```

UniRx: release unitypackage
[https://github.com/neuecc/UniRx/releases/download/7.1.0/UniRx.unitypackage](https://github.com/neuecc/UniRx/releases/download/7.1.0/UniRx.unitypackage)

# Features

## Forms
- wrpas around Unity inputs and extends their validation in context with form
- forms can be serialized (saved) to JSON by default, but you can also impl custom serializer for Web Requests

### Input validatio
- each form input type (text, toggle, checkbox) can be extended with validation (scriptable object)
- forms integrate validations and emit events on form state (valid/invalid)

## Repeaters
- similar to Forms repeaters, renders a sequence of items from data source

## Data Sources
- primarly used for repeaters
- inherit from PlaceholderDataSource for adhoc data source. Otherwise use Custom Script for binding data to repeater

## Placeholders
- skeletons in UI [see article](https://uxdesign.cc/what-you-should-know-about-skeleton-screens-a820c45a571a)
- early implementation, nothing much special

