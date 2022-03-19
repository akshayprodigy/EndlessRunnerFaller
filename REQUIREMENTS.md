# Mechanics 

## Player 

The player should be able to move left and right and fall when it doesn't have any platform below him. 

When the character is out of screen, either on the top or the bottom, the game ends. 

## Platforms 

The platforms should spawn from the bottom of the screen and go up. All of them must have a hole big enough for the character to fall. Once they exit the screen through the top, they should stop existing (either destroy, pool or deactivate them). 

# Requirements 

- On the main menu, I should see the current highscore and when pressing play, change to the game scene. 
- On the game scene, the game functionality should be similar to the one in the gif. 
- When the character is out of screen, either because he couldn't fall fast enough or he fell before the next platform was available, the player should lose. 
- Every time the character goes down a floor, his score should increase. 
- The platforms should be randomly generated. No two playthroughs should have the same pattern. 
- When losing, a menu showing me my last score, my current highscore and the buttons: "Restart" and "Go back to main menu" should appear. The buttons should take the user back to the home scene and restart the level. 
- When I press escape, the game should pause, and I should have a menu with the option "Continue" and "Go back to main menu". The buttons should either continue the game or take the user back to the home scene. 
- When surpassing my highscore, this should change on the menus. 
- The platform spawn rate and speed should increase the longer the game lasts. 
- When I surpass my highscore, this should persist,  and I should be able to see it in the UIs when I launch the game again. 
- I should be able to change the initial spawn rate in an easy to modify way (scriptable object, config file, etc)


## Bonus points 

- Add some details to make the game scene prettier (lighting effects or particles, be creative). 
- The longer the game lasts, the faster the platforms should move (and the spawn rate should also become smaller) 
- Have some smooth transitions between level transitions (fading/effects/etc). 
- When surpassing my highscore, a new visual element should appear in the lost screen UI informing me of my achievement (like a text saying "You have a new highscore"). 
- When I'm about to surpass my highscore, a visual element should inform me of it (a different platform color, a particle effect, etc.). 
- Don't use PlayerPrefs for highscore persistence. 
- Don't use OnBecameInvisible for destroying the player. 
- When restarting the game, don't reload the scene. 
- Write [automated tests](https://docs.unity3d.com/Packages/com.unity.test-framework@1.1/manual/index.html). 