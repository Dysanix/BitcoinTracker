   M S B A M L   `   `   `  ����I  EBitcoinTracker, Version=1.4.3.0, Culture=neutral, PublicKeyToken=null/clr-namespace:BitcoinTrackerBitcoinTracker  5   	   !    BitcoinTracker.WalletWidget   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   .localclr-namespace:BitcoinTracker     5   	   H DMahApps.Metro, Version=1.5.0.0, Culture=neutral, PublicKeyToken=null;Controls,http://metro.mahapps.com/winfx/xaml/controls  5   	     � Title$  Wallet��5	   	    � SizeToContent$ WidthAndHeight=�6    � WindowStartupLocation$ CenterScreen=�67    � Topmost .  � 
ResizeMode$ NoResize=�6k    � WindowStyle$ 
ToolWindow=�6�   (  "MahApps.Metro.Controls.MetroWindow   LeftWindowCommands 5
      +  %MahApps.Metro.Controls.WindowCommands  5   
   .@�5      �� -   5         ��Name$ btnSettings��5      $��Center=�6*   $��Center=�6G   $��20��6b   .��5      ��  �� Orientation$ 
Horizontal=�6   .+�5      �� $��img/cogwheel.png��6   5      5      5      .��5      � .+�5   
   �� $ 
Horizontal=�6   $��5,5,5,0q�6.   .+�5      �� -   5        	 ��Name$	 imgPulse��5      
 J� SnapsToDevicePixels
 .   �� EdgeMode$ Unspecified=�6C   $��img/greendot.png��6h   $
��Left=�6�   $��10��6�   $	��Top=�6�   $��10��6�    J� Opacity$ 0Q�6�    J� Effect 5      -  'System.Windows.Media.Effects.BlurEffect  5         Radius$ 3Q�6!      RenderingBias$ Quality=�6,   5      �� 5      -   5         ��Name$ txtInfo��5      $��5,-5,0,0q�6)   $��14(�6;   $��Center=�6I   $	��Top=�6f   
���  ��$
��Bold"�6�   5      �� 5   
   .+�5      �� -   5         ��Name$ lblWalletEUR��5      $��Center=�6*   $��Center=�6N   $��10,10,10,10q�6k   $��Center=�6�   $��Center=�6�   $��30(�6�   5      5      5       