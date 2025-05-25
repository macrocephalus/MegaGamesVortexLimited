import Player from './Player.js'; // Import the Player class

// Basic Phaser 3 game configuration
const config = {
    type: Phaser.AUTO,
    width: 1024,
    height: 768,
    backgroundColor: '#6495ED', // Cornflower Blue
    physics: {
        default: 'arcade',
        arcade: {
            gravity: { y: 0 }, // No global gravity for planes
            // debug: true // Optional: set to true for debugging physics bodies
        }
    },
    scene: {
        preload: preload,
        create: create,
        update: update
    }
};

// Create a new Phaser game instance
const game = new Phaser.Game(config);

// This object will hold our player instance
let player;

// Preload assets
function preload() {
    console.log("Preloading assets...");
    // Path relative to index.html, which is in the 'js' directory
    this.load.image('player_plane', '../MegaGamesVortexLimited/MegaGamesVortexLimitedContent/result1.png');
}

// Create game objects
function create() {
    console.log("Creating game objects...");
    // this.add.text(config.width / 2, config.height / 2, 'Phaser Game Booted!', { fontSize: '32px', fill: '#fff' }).setOrigin(0.5);

    // Player initial position (based on Plane.cs for 'Left == true' player)
    const initialPlayerX = 50;
    const initialPlayerY = config.height - 30; // 30 is the 'earth' offset

    player = new Player(this, initialPlayerX, initialPlayerY, 'player_plane');
    
    this.add.text(10, 10, 'Use A/D to rotate, W to thrust (initial setup)', { fontSize: '16px', fill: '#fff' });
}

// Game loop
function update() {
    if (player) {
        player.update(); // Cursors removed from player update call
    }
}
