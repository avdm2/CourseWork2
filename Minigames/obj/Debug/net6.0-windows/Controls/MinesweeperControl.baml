   M S B A M L   `   `   `  ����D  @Minigames, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:Minigames.ModelsMinigames.Models  5   	   (    "Minigames.Models.MinesweeperWindow   5   	   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   0localclr-namespace:Minigames.Models     5   	     � Title$  Minesweeper��5   	   $	��600��6   $	��800��6*    � 
ResizeMode$ NoResize=�5	   	    � 	Resources 5
         ButtonStyle �� 
TargetType �� Property �� Value��%0  &	        �� 5   
   " ��6$   .�5      ��  � ��$	 150��6&   �� 5       � ��$ 50��6'   �� 5       � ��$ 18(�6)   �� 5       � ��$
 Bold"�6+   �� 5       � ��$ 5q�6'   �� 5       � ��
 ��ح�6+   �� 5       � ��
 ��  �6+   5      5      .��5      �  � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   �� $��0��6    �� Orientation$ 
Horizontal=�6"   $��Center=�6;   $��Center=�6X   $��5q�6s   .+�5      �� -   5        $��Beginner��5      #��[  6D   �� 5      -   5        $��Intermediate��5      #��[  6L   �� 5      -   5        $��Expert��5      #��[  6@   5       �� 5"   
   -   5         ��Name$ 
FlagsLabel��5"      $��0��6   $��Flags: 0��61   $��10q�6D   $
��Left=�6P   $	��Top=�6k   �� 5$   
   -   5        $��MinesweeperGrid��5$   &   $��1��6   $��5q�6=   �� 5%   
   -   5        $��2��5%      $��Close��6   $��Center=�6.   $��Center=�6K   #��[  6   5&      5'      