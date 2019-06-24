# Chip-8-Interpreter
Simple Chip-8 virtual machine interpreter/emulator.  
Works in console based on commands.  
First load your ROM file by command LoadRom "filename" - note that i check only  
if file exist, so try to load only c8 programs otherwise you will crash.  
Then you can specify your Clock rate by E.g SetClockRate 1000.  
NOTE that default clock rate is 600 which is probably good for  
70% of c8 ROMs you can find. Then you can specify your color scheme. Only 2 schemes available atm.  
Do it by FalloutMode [on/off] off is default - black and white. On mode is grey and green.  
Don't delete BuiltInFontFile.txt or you will have no built in font (o rly?). This file  
is for users that know about chip-8 font and want to modify it for theier liking.  
NOTE: if you add more font beyond 80 lines it won't matter anyway cause  
no program uses more than 16 characters and if you get past 511 lines you  
may run into troubles with running your program, so modify only first 80 lines
of file. Each 5 lines stands for 1 character.
If you want to build it yourself you need to drop libraries content into your 
bin/debug and bin/release files you also need Costura.Fody 3.3.3v nuget package.  

Credit to r/EmuDev/ https://www.reddit.com/r/EmuDev/comments/c1arfm/chip8_emulator_ball_dosnt_move_in_pongpong2/  
for help in finding 2 pesky hard to track bugs.  


Maybe i will add super chip support in the future :).  
For now enjoy photorealistic Chip-8 games!  

