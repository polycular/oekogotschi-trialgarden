# oekogotschi #

A location-based AR game about sustainability.

This project is kindly supported by [netidee](https://www.netidee.at) an initiative by the [Austrian Internet Foundation IPA](http://www.nic.at/ipa)  

## Structure ##
The project consits of serveral modules, which helped us to create the Oekogotschi in the garden of the ORF Salzburg. 

    .
    ├── _gamar_server
    ├── _gamar_unity_integration
	|   ├── _docs
	|	└── _Assets
    ├── _snipnogotchi
	|   ├── _Assets
	|	|	├── _Project
	|	|	├── _SharedAssets
	|	|	└── _ToolbAR
	|	└── _ProjectSettings
    └── README.md

### GamAR ###
*GamAR* is the webserver managing the game app and the several augmented reality (AR) stations. 
There is a *GamAR Unity Integreation* with a sample unity project that shows, how to connect the webserver with a game. 

### SnipnoGotchi ###
*SnipnoGotchi* is a sample unity project, which shows how to build an AR game and structure multiple games. It is best to keep things seperated and store each indiviual game with all its assets inside a folder (e.g. *Project*). *SharedAssets* should be the place to share assets between individual games/scenes e.g. the main GUI of a project. *ToolbAR* contains our own libraries which can be used in all projects (Math Libs, AR helpers for QCAR, ...). For the project to run you need to add the [Qualcomm Vuforia Unity Extension v3](https://developer.vuforia.com/downloads/sdk)

Prerequisites:

- Unity 4.6.x
- Qualcomm Vuforia Unity Extension 3.5.x

## Licence ##

This project is licensed under the Apache 2 license, quoted below.

Copyright 2015, Polycular OG

Licensed under the Apache License, Version 2.0 (the "License");
you may not use these files except in compliance with the License.
You may obtain a copy of the License at

[http://www.apache.org/licenses/LICENSE-2.0](http://www.apache.org/licenses/LICENSE-2.0)

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.