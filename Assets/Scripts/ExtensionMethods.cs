using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class ExtensionMethods
{
    #region Reset Transform
    /// <summary>
/// Resets the position, rotation, and scale of a Transform based on optional parameters.
/// If a parameter is not provided, it defaults to specific values:
/// - Position: Vector3.zero (0, 0, 0)
/// - Rotation: Quaternion.identity (no rotation)
/// - Scale: Vector3.one (1, 1, 1)
/// </summary>
/// <param name="targetTransform">The Transform to reset.</param>
/// <param name="position">Optional new position. Defaults to Vector3.zero if null.</param>
/// <param name="rotation">Optional new rotation. Defaults to Quaternion.identity if null.</param>
/// <param name="scale">Optional new scale. Defaults to Vector3.one if null.</param>
public static void ResetTransform(
    this Transform targetTransform,
    Vector3? position = null,
    Quaternion? rotation = null,
    Vector3? scale = null)
{
    targetTransform.position = position ?? Vector3.zero;
    targetTransform.rotation = rotation ?? Quaternion.identity;
    targetTransform.localScale = scale ?? Vector3.one;
}
    #endregion

    #region Modify Components
    /// <summary>
    /// Creates a new Vector3 with specific components modified (x, y, z).
    /// Any component not provided retains its current value.
    /// </summary>
    /// <param name="vector">The original Vector3 to modify.</param>
    /// <param name="x">Optional new x value. If null, the current x value is kept.</param>
    /// <param name="y">Optional new y value. If null, the current y value is kept.</param>
    /// <param name="z">Optional new z value. If null, the current z value is kept.</param>
    /// <returns>A new Vector3 with the modified components.</returns>
    public static Vector3 WithModifiedComponents(
        this Vector3 vector,
        float? x = null,
        float? y = null,
        float? z = null)
    {
        x ??= vector.x;
        y ??= vector.y;
        z ??= vector.z;
        return new Vector3(x.Value, y.Value, z.Value);
    }
    #endregion

    #region Get or Add Component
    /// <summary>
    /// Retrieves a component of type <typeparamref name="T"/> from the specified GameObject.
    /// If the component does not exist, it adds and returns a new one.
    /// </summary>
    /// <typeparam name="T">The type of the component to retrieve or add.</typeparam>
    /// <param name="gameObject">The GameObject to search for the component.</param>
    /// <returns>The existing or newly added component of type <typeparamref name="T"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the provided GameObject is null.</exception>
    public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
    {
        if (gameObject == null)
            throw new ArgumentNullException(nameof(gameObject), "GameObject cannot be null.");

        return gameObject.GetComponent<T>() ?? gameObject.AddComponent<T>();
    }
    #endregion

    #region Set Transparency
    /// <summary>
    /// Sets the transparency (alpha) value of a SpriteRenderer's color.
    /// </summary>
    /// <param name="spriteRenderer">The SpriteRenderer whose transparency needs to be adjusted.</param>
    /// <param name="transparency">The desired transparency value, clamped between 0 (fully transparent) and 1 (fully opaque).</param>
    public static void SetTransparency(this SpriteRenderer spriteRenderer, float transparency)
    {
        if (spriteRenderer == null)
        {
            Debug.LogWarning("SpriteRenderer is null. Transparency not set.");
            return;
        }

        Color currentColor = spriteRenderer.color;
        currentColor.a = Mathf.Clamp01(transparency);
        spriteRenderer.color = currentColor;
    }
    #endregion

    #region Set Transparency of UI elements
    /// <summary>
    /// Sets the transparency (alpha value) of a UI element's color.
    /// </summary>
    /// <param name="graphic">The UI Graphic (e.g., Image, Text, etc.) to modify.</param>
    /// <param name="alpha">The alpha transparency value (0 = fully transparent, 1 = fully opaque).</param>
    /// <exception cref="ArgumentNullException">Thrown if the graphic is null.</exception>
    public static void SetTransparency(this UnityEngine.UI.Graphic graphic, float alpha)
    {
        if (graphic == null)
            throw new ArgumentNullException(nameof(graphic), "Graphic cannot be null.");

        alpha = Mathf.Clamp01(alpha); // Ensure alpha is within [0, 1].
        Color color = graphic.color;
        color.a = alpha;
        graphic.color = color;
    }
    #endregion
}