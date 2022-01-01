using UnityEditor;
using UnityEditor.Rendering;

namespace Thicckitty
{
    [CustomPropertyDrawer(typeof(RangedEnemyTypeData))]
    public class RangedEnemyTypePropertyDrawer : ACustomPropertyDrawer
    {
        protected override int DisplayOrGetPropertiesInField(SerializedProperty property, bool draw)
        {
            SerializedProperty projectilePrefab = property.FindPropertyRelative("projectilePrefab");
            SerializedProperty projectileLaunchPosition = property.FindPropertyRelative("projectileLaunchPosition");
            SerializedProperty projectileAdditionalForce = property.FindPropertyRelative("projectileAdditionalForce");
            SerializedProperty hasUpMotion = property.FindPropertyRelative("hasUpMotion");
            SerializedProperty upRotation = property.FindPropertyRelative("upRotation");
            
            int numProperties = GetNumberOfPropertiesInFields(projectilePrefab, projectileLaunchPosition,
                projectileAdditionalForce, hasUpMotion) + 1;
            if (draw)
            {
                DisplayLabelField("Projectile Settings");
                DisplayField(projectilePrefab, "Prefab");
                DisplayField(projectileLaunchPosition, "Launch Position");
                DisplayField(projectileAdditionalForce, "Additional Force");
                DisplayField(hasUpMotion, "Has Up Motion");
            }

            if (hasUpMotion.boolValue)
            {
                numProperties += GetNumberOfPropertiesInField(upRotation);
                if (draw)
                {
                    DisplayField(upRotation, "Up Rotation");
                }
            }

            numProperties++;
            if (draw)
            {
                DisplaySeparator();
            }
            
            SerializedProperty hasMaxRange = property.FindPropertyRelative("hasMaxRange");
            SerializedProperty maxRange = property.FindPropertyRelative("maxRange");
            if (draw)
            {
                DisplayField(hasMaxRange, "Has Max Range");
            }

            numProperties += GetNumberOfPropertiesInField(hasMaxRange);
            if (hasMaxRange.boolValue)
            {
                numProperties += GetNumberOfPropertiesInField(maxRange);
                if (draw)
                {
                    DisplayField(maxRange, "Max Range");
                }
            }

            SerializedProperty mustSeeTarget = property.FindPropertyRelative("mustSeeTarget");
            SerializedProperty minTargetViewThreshold = property.FindPropertyRelative("minTargetViewThreshold");
            if (draw)
            {
                DisplayField(mustSeeTarget, "Must See Target");
            }

            numProperties += GetNumberOfPropertiesInField(mustSeeTarget);
            if (mustSeeTarget.boolValue)
            {
                numProperties += GetNumberOfPropertiesInField(minTargetViewThreshold);
                if (draw)
                {
                    DisplayField(minTargetViewThreshold, "Min Target View Threshold");
                }
            }

            SerializedProperty minCooldownSeconds = property.FindPropertyRelative("minCooldownSeconds");
            SerializedProperty maxCooldownSeconds = property.FindPropertyRelative("maxCooldownSeconds");
            numProperties += GetNumberOfPropertiesInFields(minCooldownSeconds, maxCooldownSeconds);
            if (draw)
            {
                DisplayField(minCooldownSeconds, "Min Cooldown Seconds");
                DisplayField(maxCooldownSeconds, "Max Cooldown Seconds");
            }

            SerializedProperty constantNumberOfProjectiles = property.FindPropertyRelative("constantNumberOfProjectiles");
            SerializedProperty minProjectilesToShoot = property.FindPropertyRelative("minProjectilesToShoot");
            SerializedProperty maxProjectilesToShoot = property.FindPropertyRelative("maxProjectilesToShoot");
            SerializedProperty numProjectilesToShoot = property.FindPropertyRelative("numProjectilesToShoot");
            numProperties += GetNumberOfPropertiesInField(constantNumberOfProjectiles);
            if (draw)
            {
                DisplayField(constantNumberOfProjectiles, "Constant Number Of Projectiles");
            }

            if (constantNumberOfProjectiles.boolValue)
            {
                numProperties += GetNumberOfPropertiesInField(numProjectilesToShoot);
                if (draw)
                {
                    DisplayField(numProjectilesToShoot, "Num Projectiles");
                }
            }
            else
            {
                numProperties += GetNumberOfPropertiesInFields(minProjectilesToShoot, maxProjectilesToShoot);
                if (draw)
                {
                    DisplayField(minProjectilesToShoot, "Min Projectiles");
                    DisplayField(maxProjectilesToShoot, "Max Projectiles");
                }
            }
                
            SerializedProperty constantBetweenShotsCooldown = property.FindPropertyRelative("constantNumberOfProjectiles");
            SerializedProperty minBetweenShotsCooldown = property.FindPropertyRelative("minBetweenShotsCooldown");
            SerializedProperty maxBetweenShotsCooldown = property.FindPropertyRelative("maxBetweenShotsCooldown");
            SerializedProperty betweenShotsCooldown = property.FindPropertyRelative("betweenShotsCooldown");
            numProperties += GetNumberOfPropertiesInField(constantBetweenShotsCooldown);
            if (draw)
            {
                DisplayField(constantBetweenShotsCooldown, "Constant Between Shots Cooldown");
            }

            if (constantNumberOfProjectiles.boolValue)
            {
                numProperties += GetNumberOfPropertiesInField(betweenShotsCooldown);
                if (draw)
                {
                    DisplayField(betweenShotsCooldown, "Between Shots Cooldown");
                }
            }
            else
            {
                numProperties += GetNumberOfPropertiesInFields(minBetweenShotsCooldown, maxBetweenShotsCooldown);
                if (draw)
                {
                    DisplayField(minBetweenShotsCooldown, "Min Between Shots Cooldown");
                    DisplayField(maxBetweenShotsCooldown, "Max Between Shots Cooldown");
                }
            }
            return numProperties;
        }
    }
}