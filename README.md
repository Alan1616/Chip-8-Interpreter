# Chip-8-Interpreter
Simple Chip-8 virtual machine interpreter/emulator implemented in C#.  
SDL used as graphic and I/O.  
Works as console based on commands.  
Commands you can use:  
Help - displays commands list.  
LoadRom - loads rom from specified directory.  
Run - runs previously loaded ROM.    
SetClockRate [200-2000] - specify your clock rate.    
DisplayMode [default|fallout|blue|red] - choose color scheme.  

NOTE: default clock rate is 600 which is probably good for   
80% of c8 ROMs you can find.  


BuiltInFontFile.txt file is for users who know about chip-8 font and want to modify it.  
In case you mess up just delete the file and it will be generated again.    
NOTE: if you add more font beyond 80 lines it won't matter anyway cause        
no program uses more than 16 characters and if you get past 511 lines you    
will crash. Each 5 lines stand for 1 character.     


If you want to build it yourself you need to drop libraries content into your  
bin/debug and bin/release files.   

If you want to change SLD to another Library or something it should be easy (i hope :))   
as Architecture library contains only Chip-8 implementation, so you can write your  
own solution around it.   
 

