# TheyAreBillionsBackup
The game They Are Billions is notoriously difficult and severely punishes the player for any mistake they make. The game also has no concept of rolling back to a savegame. You can continue a game if you exit, but you cannot make separate saves to roll back to in the case that you make a fatal mistake. 

This made learning the game a bit difficult, so I spent 20 minutes and built a command-line based savegame backup. This will monitor your savegame folder and any time it detects changes, it will copy the new save into a separate folder with a timestamp. To "load" a savegame, simply copy your desired timestamped savegame over top of the original in the game save folder (rename it to match the original), and reload the game.
