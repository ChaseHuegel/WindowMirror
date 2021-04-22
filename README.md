# WindowMirror
.NET app that casts a live capture of a window or monitor or any dimensions within the window. Works for windows out of focus or in the background. 

Trello https://trello.com/b/UE9rUCM8/window-mirror-app

This tool gives you an overlay that you can use for multitasking, watching videos, or monitoring your workspace - it was inspired by the pop-out video feature in the Opera browser. This is intended to alleviate issues with limited screen space to help productivity. I'm writing it to make my day job easier, allowing me to maintain a live view of important areas of the software I use to monitor time sensitive feeds instead of layering 6 windows just right to where I can see their individual important parts.

# MVP
The minimal functions needed for the app to do what its intended is a control panel with the ability to select a display or monitor to capture, a button to start the capture, preview panel of what is being captured, and the ability to pop out a borderless always-on-top viewer of the live feed that can be moved anywhere on your desktop.

## Control panel
The control panel is always on top and draggable from anywhere on the window for easy of use. It pulls lists of valid windows and displays for capturing, and has a preview area where a live view will be streamed.

![74d4e4b99444bf6759d47b0169257548](https://user-images.githubusercontent.com/14932139/115769875-0e870780-a37a-11eb-97c7-bc56cbdad0c3.gif)

# More features
There is more functionality planned after the MVP is completed. The app will allow the user to use the preview to change dimensions of the capture, for example selecting Outlook as your capture window then selecting a region around your inbox so that you can have a live feed of your inbox. The app will also be allowed to run in the task tray and implement shortcuts for quick access, like the ability to define a dimension by dragging a bounding box similar to the Windows Snipping Tool, Gyazo, and other snapshot tools. For QoL the control panel should also be resizeable itself and add the option to minimize the preview area, turning the panel into a simple toolbar.
