using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;


public class KnightPTest
{
    private GameObject knightObj;
    private KnightEnemy knight;
    private Rigidbody2D rb;
    private Animator animator;
    private GameObject attackZoneObj;
    private DetectionZone attackZone;
    private TouchingDir touchingDir;
    private Damageable damageable;

    [UnitySetUp]
    public IEnumerator Setup()
    {
        knightObj = new GameObject("KnightEnemy");

        rb = knightObj.AddComponent<Rigidbody2D>();
        knightObj.AddComponent<CapsuleCollider2D>();

        animator = knightObj.AddComponent<Animator>();
        touchingDir = knightObj.AddComponent<TouchingDir>();
        damageable = knightObj.AddComponent<Damageable>();
        knight = knightObj.AddComponent<KnightEnemy>();

        attackZoneObj = new GameObject("AttackZone");
        attackZone = attackZoneObj.AddComponent<DetectionZone>();
        attackZone.detectedColliders = new List<Collider2D>();

        typeof(KnightEnemy)
            .GetField("attackZone", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.SetValue(knight, attackZone);

        // alap transform
        knightObj.transform.localScale = Vector3.one;

        yield return null;
    }

    [UnityTearDown]
    public IEnumerator Teardown()
    {
        Object.Destroy(knightObj);
        Object.Destroy(attackZoneObj);
        yield return null;
    }

    // --- TEST 1: FlipDirection flipeli a sprite-ot és az irányt ---
    [UnityTest]

    
    public IEnumerator FlipDirection_ChangesDirectionAndScale()
    {
        knight.WalkDirection = KnightEnemy.WalkingDirection.Right;
        Vector3 originalScale = knightObj.transform.localScale;

        typeof(KnightEnemy)
            .GetMethod("FlipDirection", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.Invoke(knight, null);

        Assert.AreEqual(KnightEnemy.WalkingDirection.Left, knight.WalkDirection);
        Assert.AreEqual(-originalScale.x, knightObj.transform.localScale.x);
        yield return null;
    }

    // --- TEST 2: OnHit ad sebességet (knockback) ---
    [UnityTest]
    public IEnumerator OnHit_AppliesKnockback()
    {
        Vector2 knock = new Vector2(3f, 2f);
        knight.OnHit(10, knock);

        yield return null; // physics update után ellenőrizzük

        Assert.Greater(rb.velocity.x, 0);
        Assert.Greater(rb.velocity.y, 0);
    }

    // --- TEST 3: HasTarget true, ha a DetectionZone tartalmaz collidert ---
    [UnityTest]
    public IEnumerator HasTarget_True_WhenColliderDetected()
    {
        // Üres lista → nincs target
        attackZone.detectedColliders.Clear();

        typeof(KnightEnemy)
            .GetMethod("Update", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.Invoke(knight, null);

        Assert.IsFalse(knight.HasTarget);

        // Adunk neki 1 collidert
        var dummy = new GameObject("DummyTarget");
        var col = dummy.AddComponent<BoxCollider2D>();
        attackZone.detectedColliders.Add(col);

        // újra meghívjuk az Update-et reflectionnel
        typeof(KnightEnemy)
            .GetMethod("Update", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.Invoke(knight, null);

        Assert.IsTrue(knight.HasTarget);

        Object.Destroy(dummy);
        yield return null;
    }

    // --- TEST 4: OnCliffDetected flipel, ha grounded ---
    [UnityTest]
    public IEnumerator OnCliffDetected_Flips_WhenGrounded()
    {
        knight.WalkDirection = KnightEnemy.WalkingDirection.Right;

        typeof(TouchingDir)
            .GetProperty("IsGrounded")
            ?.SetValue(touchingDir, true);

        knight.OnCliffDetected();

        Assert.AreEqual(KnightEnemy.WalkingDirection.Left, knight.WalkDirection);
        yield return null;
    }
}
