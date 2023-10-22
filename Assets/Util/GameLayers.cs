using UnityEngine;

public static class GameLayers
{
    #region Layer ID's
    public const int Environment = 6;

    public const int PlayerEntity = 8;
    public const int PlayerProjectiles = 9;
    public const int PlayerProjectilesNoInterCollision = 10;

    public const int EnemyEntities = 12;
    public const int EnemyEntitiesCriticalZone = 13;
    public const int EnemyProjectiles = 14;
    public const int EnemyProjectilesNoInterCollision = 15;

    public const int NeutralEntities = 17;
    public const int NeutralEntitiesCriticalZone = 18;
    #endregion


    #region Layermasks
    public static int EntityCriticalMask => AsBitmask(EnemyEntitiesCriticalZone);
    public static int EntityMask => AsBitmask(EnemyEntitiesCriticalZone);


    #endregion

    public static int AsBitmask(params int[] layers)
    {
        int mask = 0;
        foreach (var layer in layers)
            mask |= 1 << layer;
        return mask;
    }


    /*
    what layers do I need?

    forrendering:
        -fpsrig

    forgameplay/physics:
        -playercolliders
        -playerprojectiles
        -playerprojectilesnointercollision
        -environment

        -enemyprojectiles
        -enemyprojectilesnointercollision

        -neutralentities
        -neutralentitiescriticalzone

        -hostileentities
        -hostileentitiescriticalzone
    */
}
