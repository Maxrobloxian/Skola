public static class PlayerSettings
{
    // Scripts
    public static PlayerInventory playerInventory;

    // Health and protection
    public static float maxHealth = 20;
    public static float maxProtection = 0;

    // Movement
    public static float walkSpeed = 5;
    public const float sprintMultiplier = 2;
    public const float crouchMultiplier = .5f;
    public const float crawlMultiplier = .3f;

    public static float flySprintMultiplier = 3;

    public static float jumpPower = 5f;
    public static float jumpCooldown = .1f;

    // Character size
    public const float walkingHeight = 1.8f;
    public const float crouchingHeight = 1.5f;
    public const float crawlingHeight = .6f;

    // Player inventory
    public static int selectedSlot = 0;
    public static int inventoryRows = 4;
    public static int inventoryRowLength = 9;
    public static int offhandRowLength = 1;
    public static int hotbarRowLength = 9;

    // Player interaction
    public static int interactionRange = 5;

    internal static void AddData(PlayerInventory playerInventory)
    {
        PlayerSettings.playerInventory = playerInventory;
    }
}