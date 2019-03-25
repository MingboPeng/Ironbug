# Ironbug
This is a set of tools that developed in C# for the Ladybug Tools

![Image of Ladybug_ImageViewer](https://github.com/MingboPeng/Ironbug/blob/master/doc/Icon/1x/Ironbug.png) 

I am excited to write to you the introduction of a new member of Ladybug family: Ironbug. As many of you know that I have worked on a couple of components for Ladybug toolset in the past, and I have been working on bringing OpenStudio’s full HVAC modeling capability to the Grasshopper since the beginning of this year. In the meantime, I am also working on converting Ladybug [+] and Honeybee [+] Python core library to C# API to extend the Ladybug’s development ecosystem, and take advantages of C# language and tooling.

Ironbug is basically a group of tools that are written in C#. This project was called HoneybeeSharp when I first started the ReadAnnualResultsIII for Honeybee, and later changed to Ironbug when the ImageViewer was added. This year, a set of the HVAC components have been added to this group to broaden the current Honeybee energy modeling’s ability.


## Currently includes:

#### 1.Ladybug_ImageViewer 
![Image of Ladybug_ImageViewer](https://github.com/MingboPeng/Ironbug/blob/master/doc/Icon/Ladybug_Viewer_24.png) 

Preview images directly in Grasshopper, and supports the HDR image (Radiance image) and other common image formats. The ImageViewer also allows you to generate an animated gif image when there are more than one image connected.
In the recent update (DEC 2018), extracting radiance values (illuminance, luminance, radiation) has been added to 

#### 2.Honeybee_ReadAnnualResultsIII
![Image of Honeybee_ReadAnnualResultsIII](https://github.com/MingboPeng/Ironbug/blob/master/doc/Icon/Ironbug.HVAC/24h/ReadAnnualResultsIII.png) 

ReadAnnualResultsIII has the same function as ReadAnnualResultsII, but 20x faster!

#### 3.Ladybug.SortByLayers
![Image of Honeybee_SortByLayers](https://github.com/MingboPeng/Ironbug/blob/master/doc/Icon/Ironbug.HVAC/24h/SortByLayers.png) 

Sort and group the Rhino geometries based on its layer information. It is very useful when modeling a complex simulation scenario with several materials or zone types, etc.

#### 4.Honeybee.HVAC (WIP)
![Image of Ironbug](https://raw.githubusercontent.com/MingboPeng/Ironbug/master/doc/Icon/Ironbug.HVAC.PNG) 

A set of 80 components (by Aug 4, 2018) for creating highly customizable HVAC systems including: Air handling unit, Water loops (hot water, chilled water, condensing water), and VRF.

## Installation
1. Download the [Ironbug.zip (1.4 MB)](https://github.com/MingboPeng/Ironbug/releases) and unblock the file.

![Image of Ironbug](https://github.com/MingboPeng/Ironbug/blob/master/doc/GIF/unblock.gif) 

2. Unzip to `C:\Ironbug`, and open 00_installer.gh in Grasshopper.
![image of folder](https://discourse.ladybug.tools/uploads/default/original/2X/5/5fe511542effa4a87dd90c7a2a435bfc6474eba9.png)

3. Find the OpenStudio folder path on your computer, and set toggle to true to install.
![image of installer](https://github.com/MingboPeng/Ironbug/blob/master/doc/GIF/installer.gif)


4. Restart the Grasshopper, and enjoy!

