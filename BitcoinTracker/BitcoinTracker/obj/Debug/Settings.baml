   M S B A M L   `   `   `  ����I  EBitcoinTracker, Version=1.4.3.0, Culture=neutral, PublicKeyToken=null/clr-namespace:BitcoinTrackerBitcoinTracker  5   	       BitcoinTracker.Settings   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   .localclr-namespace:BitcoinTracker     5   	   H DMahApps.Metro, Version=1.5.0.0, Culture=neutral, PublicKeyToken=null;Controls,http://metro.mahapps.com/winfx/xaml/controls  5   	      WindowBackgroundBrush#��[  5   	     � Title$  Settings��5
   	   $��210.916��6   $	��400��6+    � WindowStyle$ 
ToolWindow=�67    � Topmost .  � WindowStartupLocation$ Manual=�6_    � 
ResizeMode$ NoResize=�6~   (  "MahApps.Metro.Controls.MetroWindow   RightWindowCommands 5      +  %MahApps.Metro.Controls.WindowCommands  5   
   .@�5      �� -   5         ��Name$ btnMute��5      $��Center=�6&   $��Center=�6C   $��20��6^   .��5      ��  �� Orientation$ 
Horizontal=�6   .+�5      �� -   5         ��Name$ btnMuteImage��5      $��img/mute.png��62   5      5      5      .��5      � .+�5   
   �� -   5        	 ��Name$	 SettingsTabControl��5      .@�5      �� $��General��6   +  %MahApps.Metro.Controls.ControlsHelper
   HeaderFontSize$
 16Q�6'   .��5      � .+�5      �� -   5         ��Name$ comboCurrency��5      $��Center=�66   $��Stretch=�6X   $��75,72,203,0q�6v   $	��Top=�6�   �� 5      -   5        $ comboAPI��5      $��Center=�61   $��Stretch=�6S   $��75,10,203,0q�6q   $	��Top=�6�   �� 5      -   5         ��Name$ label��5      $��	Currency:��6+   $
��Left=�6?   $��	10,72,0,0q�6Z   $	��Top=�6m   �� 5      -   5        $ btnSave��5      $
��Save��6.   $��Center=�6=   $��0,0,0,10q�6Z   $��Bottom=�6l   $��75��6�   6�   �� 5      -   5        $ 
label_Copy��5      $��Refresh Interval:��60   $
��Left=�6L   $��
198,56,0,0q�6g   $	��Top=�6{   �� 5      -	   5        $ lblInterval��5      $��1.5 sec��61   $��Center=�6C   $��297,56,10,0q�6g   $	��Top=�6|   �� 5      -
   5         ��Name$ sliderInterval��5       �� IsSnapToTickEnabled . $��Stretch=�6P   $��198,79,10,0q�6n   $	��Top=�6�   $��19��6�    � Maximum$ 60000Q�6�    � Minimum$
 1500Q�6�    � SmallChange$	 100Q�6�    � LargeChange$
 1000Q�6�    �� TickFrequency$	 100Q�6  �� 5      -   5        $ label_Copy1��5      $��	Bitcoins:��61   $��Right=�6E   $
��Left=�6h   $��	10,41,0,0q�6�   $	��Top=�6�   $��60��6�   "  BitcoinTracker.DoubleTextBox  5       -   5          Name$ txtBitcoins��5    *   "   BitcoinTracker.DoubleTextBox   PreviewTextInput$" txtBitcoins_PreviewTextInput��6a   $��Center=�6?   $��Stretch=�6�   $��26��6�   $��75,41,203,0q�6�    �� TextWrapping$
 Wrap=�6�   $��0��6�   $	��Top=�6�   �� 5!      -   5         ��Name$ chckTopMost��5!       d� 	IsChecked$
 TrueY�64   $��Stay in foreground��6E   $
��Left=�6b   $��
198,10,0,0q�6}   $	��Top=�6�   $	��180��6�   �� 5"      -   5        $ chckTaskbar��5"      $��Show in taskbar��64   $
��Left=�6N   $��
198,33,0,0q�6i   $	��Top=�6}   $	��180��6�   �� 5#      -   5        $ label_Copy3��5#      $��Right=�61   $
��API:��6T   $
��Left=�6c   $��	10,10,0,0q�6~   $	��Top=�6�   $��60��6�   5$      5%      �� 5&      $��Alerts��6   $
 16Q�6&   .��5'      � .+�5(      �� -   5        $ label1��5(      $4��.Notify me when my wallet reaches a value of...��6,   $��Center=�6e   $��Center=�6�   $
��Left=�6�   $��10,0,0,0q�6�   $	��Top=�6�   $	��368��6�   $��36��6�   �� 5)      -   5        $ lblHigherThan��5)      $��Higher than:��63   $��Right=�6J   $
��Left=�6m   $��	74,41,0,0q�6�   $	��Top=�6�   $	��119��6�   
��� � �6�   �� 5*      -   5        $ lblLowerThan��5*      $��Lower than:��62   $��Right=�6H   $
��Left=�6k   $��	74,72,0,0q�6�   $	��Top=�6�   $	��119��6�   
���  ��6�     5+      -   5        $ 
doubleHigh��5+   *      GotFocus$ DoubleHighLow_OnGotFocus��6a      	LostFocus$ DoubleHighLow_OnLostFocus��6�   $��Center=�6?   $
��Left=�6�   $��
193,41,0,0q�6�   $
 Wrap=�6�   $��0��6�   $	��Top=�6�   $	��110��6  $��26��6    5,      -   5        $ 	doubleLow��5,   *   $ DoubleHighLow_OnGotFocus��6`   $ DoubleHighLow_OnLostFocus��6�   $��Center=�6>   $
��Left=�6�   $��
193,72,0,0q�6�   $
 Wrap=�6�   $��0��6�   $	��Top=�6�   $	��110��6  $��26��6  �� 5-      -   5        $ label1_Copy��5-      $��(Autosaves)��61   $��Center=�6G   $��Center=�6i   $��Stretch=�6�   $��	10,0,10,0q�6�   $��Bottom=�6�   $��36��6�   5/      50      �� 51      $��Updates��6   $
 16Q�6'   .��52      � .+�53      �� -   5        $ btnUpdateDownload��53      $��Download Update��69   {�.  $��Center=�6e   $��10q�6�   $��Bottom=�6�   $	��112��6�   �� 54      -   5        $ btnUpdateVirusScan��54      $��
Virus Scan��6:   {�.  $��Center=�6a   $��308,0,10,10q�6~   $��Bottom=�6�   $��70��6�   �� 55      $��Disabled=�6#   $
��Auto=�6L   $��10,36,10,42q�6o   .��56      �� -   5         ��Name$ stckUpdateLog��56   %   $ Vertical=�6<   57      �� 58      -   5        $ lblUpdateStatus��58      $#��BitcoinTracker is up-to-date!��65   $��Center=�6]   $��Center=�6   $
��Left=�6�   $��10,0,0,0q�6�   $	��Top=�6�   $	��368��6�   $��36��6�   �� 59      -   5        $ chckCheckForUpdates��59      $��Check periodically��6<   $
��Left=�6Y   $��
10,119,0,0q�6t   $	��Top=�6�   $	��123��6�   6�   5:      5;      �� 5<      $��Credits��6   $
 16Q�6'   .��5=      � .+�5>      �� $��Disabled=�6#   $
��Auto=�6L   .��5?      �� $ Vertical=�6%   .+�5@      �� -   5         ��Name$ 
lblCredits��5@   (   $
��Wrap=�6<    �� TextAlignment$ Center=�6P   $��Stretch=�6g   $��
10,10,10,0q�6�   $��Stretch=�6�   $��Segoe UI)�6�   5A      5B      5C      5D      5E      5F      5G      