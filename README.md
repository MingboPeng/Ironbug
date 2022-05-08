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



# Installation

**Prerequisite**: Ironbug v1.0 and above only works with the new Ladybug Tools (not the legacy version). It is suggested to use the [Pollination installer](https://www.pollination.cloud/grasshopper-plugin) to install the Ladybug Tools, which comes with all compatible libraries and engines.

1. Download the Ironbug [single-click installer](https://github.com/MingboPeng/Ironbug/releases) and execute it with admin privilege.

2. The installer will auto-select the valid Ladybug Tools' installation folder, and install the Ironbug to its `grasshopper\ironbug` folder.
![image](https://user-images.githubusercontent.com/9031066/167290376-b49e9995-48c8-4829-8bfd-df3412744470.png)

3. Finish the installation and enjoy!

Note: installing the Ironbug with the new single-click installer will automatically remove the old Ironbug (if any) from `C:\Ironbug`.

## Installation Issues:

### 1. Ironbug can't find the OpenStudio:
Ironbug only uses the OpenStudio that is installed to the Ladybug Tools installation folder. If you use the Pollination installer to install Ladybug Tools, the OpenStudio is installed to `C:\Program Files\ladybug_tools\openstudio`. The easiest way to fix this issue is to use the [Pollination installer](https://www.pollination.cloud/grasshopper-plugin) to install the LBT plugins.

### 2. Single-click installer can't find the Ladybug Tools' installation folder
It is likely you installed the LBT from Food4Rhino, or installed to LBT to a non-standard location that Ironbug's installer can't find it. The following two directories are checked by the installer to find the Ladybug Tools' installation folder:
    
    1. C:\Program Files\ladybug_tools\
    2. C:\Users\USERNAME\ladybug_tools\

The easiest way to fix this issue is to use the [Pollination installer](https://www.pollination.cloud/grasshopper-plugin) to install the LBT plugins.

If you really have to install the Ladybug Tools to a customized directory, for example `D:\ladybug_tools`, you will have to manually point the Ironbug's installer the correct Ladybug Tools folder. In this case, the installation path will be `D:\ladybug_tools\grasshopper\ironbug`

### More issues?
Post an issue here: https://github.com/MingboPeng/Ironbug/issues/new


