   M S B A M L   `   `   `  ����G  CWindowMirror, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:WindowMirrorWindowMirror  5   	       WindowMirror.MainWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   ,localclr-namespace:WindowMirror     5   	   -   5          � Title$  WindowMirror��5	   	   $	��301��6   $	��356��6+    � Topmost .  � 
ResizeMode$ NoResize=�6F   $y�Visible=�6\   |�.  ���	#FF394653 � AllowsTransparency .  � WindowStyle$
 None=�6�    � Icon$' !resources/windows-regular-240.png��6�   ��5
      �� 5   
    �� Color$ 	#FF394653��6   .��5      � $	��301��6   $��Bottom=�6    � RowDefinitions 5   
   �� 5      $	��31* �6   �� 5      $	��42* �6   �� 5      $
��228* �6    � ColumnDefinitions 5   
   �� 5      .+�5   
   �� -   5        	 ��Name$	 windowsComboBox��5      $
��Left=�6,   $��	10,17,0,0q�6G   $	��Top=�6Z   $��99��6r   $��22��6}   $��1��6�   ��5      �� 5      .R�5      �� ��6    �� *MainWindowTitle+5      5      �� 5   
   -   5        $	 displaysComboBox��5      $
��Left=�6-   $��
114,17,0,0q�6H   $	��Top=�6\   $��99��6t   $��22��6   $��1��6�   ��5"      �� 5#      .R�5$      �� ��6    �� *
DeviceName+5%      5'      �� 5(   
   -   5        
 ��Name$
 captureButton��5(      $
��Left=�6(   $��	218,6,0,0q�6C   $	��Top=�6V   $	��128��6n   $��33��6z   ���	#FF2BA9F1$��1��6�   .��5)      �� $��25��6   $��76��6%    �� Orientation$ 
Horizontal=�60   .+�5*      �� $&�� /resources/camera-regular-60.png��6   $��26��6B   $��0q�6N   �� 5+      -   5         ��Name$ label��5+      $��Capture��6'   $��
-3,-1,0,-1q�69   
�������$��Bottom=�6`   $��Center=�6{   $��27��6�   $��52��6�   5,      5.      �� 5/   
   -   5         ��Name$ previewRectangle��5/      ���	#FF171717$
��Left=�6?   $	��218��6Z   
���   �$	��Top=�6v   $	��336��6�   $��10,0,0,0q�6�   $��2��6�   �� 51   
   -   5         ��Name$ previewImage��51      $
��Left=�6&   $	��218��6A   $��10,0,0,0q�6N   $��2��6`   $	��Top=�6m   $	��336��6�   $
��Fill=�6�   �� 53   
   -   5        $
 popOutButton��53      $�� ��6'   $
��Left=�62   $��	306,0,0,0q�6M   $	��Top=�6`   $��40��6x   $��40��6�   $��2��6�   ��6�   Z� ��6�   Z� ��54      �� 55       �� ImageSource$5 /resources/right-top-arrow-circle-regular-60.png��6    j� Stretch$ Uniform=�6[   57      �� 58   
   -	   5        $ windowsLabel��58      $��Window��6&   $
��Left=�67   $	��Top=�6R   $��19��6j   $��99��6v   $��11(�6�   $��10,0,0,0q�6�   
�������$��0q�6�   $��Bottom=�6�    J� RenderTransformOrigin$ 0.505,0.632&�6�   $��1��6  �� 59   
   -
   5        $ windowsLabel_Copy��59      $��Display��6+   $
��Left=�6=   $��	114,0,0,0q�6X   $��19��6k   $��93��6w   $��11(�6�   
�������$	��Top=�6�   $��0q�6�   $��Bottom=�6�   $��1��6�   �� 5:   
   $
��Left=�6   $��31��6/   $��	10,17,0,0q�6;   $	��Top=�6N   $	��336��6f   $��2��6r   ���	#FF1717176�   �� 5;   
   -   5        $ label1��5;      $��Window Mirror��6    $
��Left=�68   $��29,0,0,0q�6S   $	��Top=�6e   $��99��6}   
�������6�   �� 5<   
   -   5        $ image��5<      $
��Left=�6   $��24��6:   $	��Top=�6F   $��24��6^   $'��!resources/windows-regular-240.png��6i   $��5,2,0,0q�6�   �� 5=   
   -   5        $
 closeButton��5=      $�� ��6&   $��Right=�61   $	��Top=�6M   $��24��6e   $��24��6p   ���	#FF8F8F8F$��Center=�6�   $��Center=�6�   $��Disabled=�6�   $��0q�6$   � UseLayoutRounding .  $��
ExtraLight"�6J  $��20(�6|  $��	Condensed&�6�  ��6|   Z� ��5>      �� 5?      $  resources/x-regular-60.png��6   5A      �� 5B   
   -   5        $
 minimizeButton��5B      $�� ��6)   $��Right=�64   $	��Top=�6P   $��24��6h   $��24��6s   ���	#FF8F8F8F$��Center=�6�   $��Center=�6�   $��Disabled=�6�   $��0q�6'   .  $��
ExtraLight"�6M  $��20(�6�  $��	Condensed&�6�  $��0,0,29,0q�6�  ��6   Z� ��5C      �� 5D      $$ resources/minus-regular-60.png��6   5F      �� 5G   
   -   5         ��Name$ 
errorPanel��5G      $��Center=�6%   $��26��6B   $��
114,0,52,5q�6N   $��Bottom=�6b   $	��190��6}   .+�5H      �� -   5        $ 	errorIcon��5H      $��26��6'   $��26��63   $"��resources/error-solid-60.png��6>   �� 5I      -   5        $ errorMessage��5I      $��ERROR��6*    �� Left$ 26��6:   $��26��6K   $	��148��6W   
�������$��Center=�6v   $��14(�6�   $��	Tw Cen MT)�6�   $��0q�6�   5J      5K      5L      