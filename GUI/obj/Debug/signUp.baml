   M S B A M L   `   `   `  ����>  :GUI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:GUIGUI  5   	       
GUI.signUp   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   #localclr-namespace:GUI     5   	     � Title$  signUp��5   	   $	��500��6   $	��750��6%   .��5	      � ��5
   
   �� 5       �� ImageSource$ pictures/sign_up.jpg��6   .+�5   
   �� -   5         ��Name$ id_label��5      $��ID��6"   $
��Left=�6/   $��	87,63,0,0q�6J   $	��Top=�6]   $��24��6u   $��64��6�   #��� ��6�   ��6�   Z� 6�   �� 5   
   -   5        $ 
pass_label��5      $��password��6$   $
��Left=�67   $��
87,100,0,0q�6R   $	��Top=�6f   $��30��6~   $��87��6�   #��� ��6�   �� 5   
   -   5        $ 
name_label��5      $
��name��6$   $
��Left=�63   $��
87,141,0,0q�6N   $	��Top=�6b   $��30��6z   $��87��6�   #��� ��6�   �� 5   
   -   5        $ email_label��5      $��email��6%   $
��Left=�65   $��
87,194,0,0q�6P   $	��Top=�6d   $��30��6|   $��87��6�   #��� ��6�   �� 5   
   -   5         ��Name$ 
id_textBox��5      $
��Left=�6&   $��23��6A   $��
192,64,0,0q�6M    �� TextWrapping$
 Wrap=�6a   $	��Top=�6u   $	��120��6�   �� 5   
   -   5        $ pass_textBox��5      $
��Left=�6(   $��23��6C   $��192,100,0,0q�6O   $
 Wrap=�6d   $	��Top=�6x   $	��120��6�   �� 5   
   -   5        $ name_textBox��5      $
��Left=�6(   $��23��6C   $��192,141,0,0q�6O   $
 Wrap=�6d   $	��Top=�6x   $	��120��6�   �� 5   
   -   5        $ email_textBox��5      $
��Left=�6)   $��23��6D   $��192,194,0,0q�6P   $
 Wrap=�6e   $	��Top=�6y   $	��120��6�   �� 5   
   
���   �$��1q�6%   $
��Left=�69   $	��130��6T   $��
87,269,0,0q�6a   $	��Top=�6u   $	��308��6�   ���	#FFC1C1B06�   �� 5   
   $
��Left=�6   $��71��6,   $��100,290,0,0q�68   $	��Top=�6M   $��57��6e   ��5      �� 5      $ pictures/avatar1.jpg��6   5      �� 5   
   -	   5         ��Name$ canvas1��5      $
��Left=�6"   $��71��6=   $��247,290,0,0q�6I   $	��Top=�6^   $��57��6v   ��5      �� 5      $ pictures/avatar3.jpg��6   5      �� 5    
   -
   5        $ canvas2��5       $
��Left=�6"   $��71��6=   $��174,290,0,0q�6I   $	��Top=�6^   $��57��6v   ��5!      �� 5"      $ pictures/avatar2.png��6   5$      �� 5%   
   $
��Left=�6   $��71��6,   $��323,290,0,0q�68   $	��Top=�6M   $��57��6e   ��5&      �� 5'      $ pictures/avatar4.jpg��6   5)      �� 5*   
   -   5        $ 	MainLabel��5*      $)��#Please enter your personal details:��6%   $
��Left=�6S   $��	58,15,0,0q�6n   $	��Top=�6�   $��35��6�   $	��286��6�   $
��Bold"�6�   $��14(�6�   ���	#FFF1ECEC6�   �� 5+   
   -   5        $ MainLabel_Copy��5+      $��please choose an avatar��6*   $
��Left=�6L   $��
87,241,0,0q�6g   $	��Top=�6{   $��35��6�   $	��286��6�   $
��Bold"�6�   $��14(�6�   ���	#FFCF49496�   �� 5,   
   -   5         ��Name$ confirm_button��5,      $��confirm��6)   $
��Left=�6;   $��441,412,0,0q�6V   $	��Top=�6k   $	��100��6�   $��30��6�   	� 5-   
   -   5         	�Name$ avatar1��5-      $��avatar1��6'   $
��Left=�69   $��100,374,0,0q�6T   $	��Top=�6i   6�   	� 5.   
   -   5        $ avatar2��5.      $��avatar2��6'   $
��Left=�69   $��175,374,0,0q�6T   $	��Top=�6i   6�   	� 5/   
   -   5        $ avatar3��5/      $��avatar3��6'   $
��Left=�69   $��248,374,0,0q�6T   $	��Top=�6i   6�   	� 50   
   -   5        $ avatar4��50      $��avatar4��6'   $
��Left=�69   $��324,374,0,0q�6T   $	��Top=�6i   6�   �� 51   
   -   5        $ home_button��51      $
��Home��6&   $
��Left=�65   $��
523,20,0,0q�6P   $	��Top=�6d   $	��100��6|   $��30��6�   53      54      