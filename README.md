# Unity Package - Template
 Template repository, from which Unity plugins can be created to support UPM. Also contains CI/CD for Verdaccio

[Package Readme](Assets/Scripts/Readme.md)

# Creating new Package

## Update package.json

- name 

## Setup GitActions

### Repository Secrets

- NPM_REGISTRY_IP = Verdaccion VM IP
- NPM_AUTH_TOKEN = Generated for each user when logged in (found in .npmrc file)

### main.yaml

- ```package-name: <same as package.json "name">```

# Following could be README template heading

# Unity Package
by Martin Obert

Created for as Open Licence, no Royalties or whatsoever. Just install and enjoy.


## Unity installation
Packages are self hosted, so you don't have to manually deal with dependencies and can fully utilize Unity UPM.

update your Unity project ```manifest.json``` with section:

```
"scopedRegistries": [
    {
      "name": "Obert",
      "url": "http://34.159.136.206/",
      "scopes": [
        "com.obert"
      ]
    }
  ]
```

If you still prefer the git installation of separate packages, you could use direct github link like: ```https://github.com/martin-obert/<repository-name>.git?path=Assets/Scripts```.
*Important part is the ```?path=Assets/Scripts``` so that Unity reaches for actual package.json file*
