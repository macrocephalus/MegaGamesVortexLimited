export default class Player extends Phaser.GameObjects.Sprite {
    constructor(scene, x, y, textureKey) {
        super(scene, x, y, textureKey);

        scene.add.existing(this);
        scene.physics.add.existing(this); // Enable physics

        this.originalTexture = textureKey;
        this.thrustPower = 200; // Acceleration power when W is pressed
        this.rotationSpeed = 0.07; // Adjusted for potentially faster updates
        this.isAlive = true;
        this.earthOffset = 30;

        this.setOrigin(0.5, 0.5);
        this.setRotation(0);

        // Physics properties
        this.body.setDrag(100); // Natural deceleration
        this.body.setMaxVelocity(300); // Max speed for the plane
        this.body.setCollideWorldBounds(true); // Prevent going off-screen (for now)
                                            // World wrapping will be handled later like in C#

        console.log('Player created with physics:', this.originalTexture);

        // Define keys for this player (Player 1: A, W, D)
        this.keyA = scene.input.keyboard.addKey(Phaser.Input.Keyboard.KeyCodes.A);
        this.keyD = scene.input.keyboard.addKey(Phaser.Input.Keyboard.KeyCodes.D);
        this.keyW = scene.input.keyboard.addKey(Phaser.Input.Keyboard.KeyCodes.W);
    }

    update() {
        if (!this.isAlive) {
            this.body.setVelocity(0,0); // Stop if not alive
            return;
        }

        // Rotation
        if (this.keyA.isDown) {
            this.rotation -= this.rotationSpeed;
        } else if (this.keyD.isDown) {
            this.rotation += this.rotationSpeed;
        }

        // Thrust
        if (this.keyW.isDown) {
            // Accelerate in the direction of rotation
            // The velocityFromRotation function sets the x/y velocity components
            this.scene.physics.velocityFromRotation(this.rotation, this.body.velocity.length() + this.thrustPower, this.body.velocity);
        } else {
            // If W is not pressed, drag will slow it down.
            // If we need more explicit deceleration similar to C#'s fallen() or speed reduction:
            // this.body.setAcceleration(0,0); // Or apply a counter-force
        }
        
        // Keep rotation within 0 to 2*PI
        if (this.rotation > Math.PI * 2) {
            this.rotation -= Math.PI * 2;
        } else if (this.rotation < 0) {
            this.rotation += Math.PI * 2;
        }
        
        // Sync sprite angle with physics body rotation if necessary (usually direct rotation of sprite is fine)
        // this.setAngle(Phaser.Math.RadToDeg(this.rotation)); // Phaser sprites often use degrees for setAngle
    }
}
