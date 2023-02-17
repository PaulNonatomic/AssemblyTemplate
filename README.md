# Project Background
This started out as a little project to simplify and speed up the process of creating a new assembly as they follow as pretty consistent pattern. 

To achieve this I built a simple process of digesting json files to produce file structures in Unity. This process it seems has further application in the generation of any commonly used file structure like that of creating new packages, so I've expanded the project to automate the generation of a new packages file structure and required files.

# Assembly Template
To create a new assembly right click in the project window and select `Create > Assembly Structure`.

![aLDUPw13gG](https://user-images.githubusercontent.com/4581647/219513182-effa0607-0ffb-4b86-88a1-9fd47172bf1b.png)

Complete the form in the Assembly Template window and select `Generate` to create a new folder within the selected directory named and populated with the details you enter in the Assembly Template window.

![Unity_1V9hVQNZR3](https://user-images.githubusercontent.com/4581647/219513782-28533047-0e59-42ef-a289-450ea2af98da.png)

The produced file structure is inclusive of all the typical files you would expect to see in a new assembly including all assembly definition files.

Should you wish to customise this file structure you can do so by duplicating the BasicAssemblyTemplate.json file and editing it to your liking. You can then set the `Template Path` field of the Assembly Template window to point to your new json template.

# Package Template
To create a new package right click in the project window and select `Create > New Package`.

![Unity_1Fp6aI0jh4](https://user-images.githubusercontent.com/4581647/219514581-65f16a2b-3284-4879-8e89-5669cc23328b.png)

Complete the form in the Package Template window and select `Generate` to create a new package within the packages directory named and populated with the details you enter in the Package Template window.

![Unity_d8S6UpjXF5](https://user-images.githubusercontent.com/4581647/219514874-838f323e-928c-42b1-bb2c-d433c274e8dc.png)

Should you wish to customise this file structure you can do so by duplicating the NewPackageTemplate.json file and editing it to your liking. You can then set the `Template Path` field of the Package Template window to point to your new json template.
