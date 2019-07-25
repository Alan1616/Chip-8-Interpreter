# Chip-8-Interpreter
Simple Chip-8 virtual machine interpreter/emulator.  
Works as console based on commands.  
Load your ROM file by command LoadRom "filename" - note that i check only  
if file exist, so try to load only c8 programs otherwise you will crash.  
Then you can specify your Clock rate E.g SetClockRate 1000.  
NOTE that default clock rate is 600 which is probably good for  
80% of c8 ROMs you can find. Then you can specify your color scheme.
Do it by DisplayMode [default|fallout|blue|red] default is standard black and white.  
BuiltInFontFile.txt file is for users who know about chip-8 font and want to modify it   
to their liking. If you mess up just delete the file and it will be generated again.  
NOTE: if you add more font beyond 80 lines it won't matter anyway cause      
no program uses more than 16 characters and if you get past 511 lines you  
will crash. Each 5 lines stand for 1 character.   


If you want to build it yourself you need to drop libraries content into your 
bin/debug and bin/release files. 

If you want to change SLD to another Library or something it should be easy ( i hope :))
as Architecture library contains only Chip-8 implementation, so you can write your
own solution around it!

Credit to r/EmuDev/ https://www.reddit.com/r/EmuDev/comments/c1arfm/chip8_emulator_ball_dosnt_move_in_pongpong2/  
for help in finding 2 pesky hard to track bugs.  

Maybe i will add SCHP-48/MegaChip support in the future (if i find good reference).  
For now enjoy photorealistic Chip-8 games!  

