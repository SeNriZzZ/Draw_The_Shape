using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Random = UnityEngine.Random;


public static class Extensions
{
    public static Color RandomColor()
    {
        return new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
    }
    
    public static void SetToParent(this Transform transform, Transform parent)
    {
        transform.parent = parent;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }
    
    public static Texture2D CreateReadableTexture(this Sprite sprite)
    {
        var source = sprite.texture;
        var renderTex = RenderTexture.GetTemporary(
            source.width,
            source.height,
            0,
            RenderTextureFormat.Default,
            RenderTextureReadWrite.Linear
        );
        Graphics.Blit(source, renderTex);

        var previous = RenderTexture.active;
        RenderTexture.active = renderTex;

        var readableTex = new Texture2D((int) sprite.rect.width, (int) sprite.rect.height);
        readableTex.ReadPixels(sprite.rect, 0, 0);
        readableTex.Apply();

        RenderTexture.active = previous;
        RenderTexture.ReleaseTemporary(renderTex);

        return readableTex;
    }

    
    public static RectTransform GetRectTransform(this Component component)
    {
        return component.GetComponent<RectTransform>();
    }

    public static Color WithAlpha(this Color color, float alpha)
    {
        color.a = alpha;
        return color;
    }
    
    public static bool RandomBool(float probability = 0.5f)
    {
        probability = Mathf.Clamp(probability, 0f, 1f);
        return Random.Range(0f, 1f) < probability;
    }
    
    public static int RandomSign(float probability = 0.5f)
    {
        probability = Mathf.Clamp(probability, 0f, 1f);
        return Random.Range(0f, 1f) < probability ? 1 : -1;
    }
    
    public static int RandomRangeWithout(int min, int max, int exclusion)
    {
        if (exclusion < min || exclusion > max)
            return Random.Range(min, max);

        var random = Random.Range(min, max - 1);

        if (random >= exclusion)
            random++;

        return random;
    }

    
    public static Texture2D MyResize(this Texture2D sourceTexture, int newWidth, int newHeight)
    {
        // Crop
        var startWidth = sourceTexture.width;
        var startHeight = sourceTexture.height;

        var aspectRatio = ((float) newWidth) / newHeight;

        var preferedWidth = 0;
        var preferedHeight = 0;

        var xOffset = 0;
        var yOffset = 0;

        if ((aspectRatio * startHeight) < startWidth)
        {
            preferedWidth = (int) (startHeight * aspectRatio);
            preferedHeight = startHeight;
            xOffset = (startWidth - preferedWidth) / 2;
        }
        else
        {
            preferedHeight = (int) (startWidth / aspectRatio);
            preferedWidth = startWidth;
            yOffset = (startHeight - preferedHeight) / 2;
        }

        var pixels = sourceTexture.GetPixels(xOffset, yOffset, preferedWidth, preferedHeight, 0);
        var _tex = new Texture2D(preferedWidth, preferedHeight);
        _tex.SetPixels(pixels);
        _tex.Apply();

        // Resize
        _tex.filterMode = FilterMode.Point;

        var renderTexture = RenderTexture.GetTemporary(newWidth, newHeight);
        renderTexture.filterMode = FilterMode.Point;

        RenderTexture.active = renderTexture;
        Graphics.Blit(_tex, renderTexture);

        var newTexture = new Texture2D(newWidth, newHeight);
        newTexture.ReadPixels(new Rect(0, 0, newWidth, newHeight), 0, 0);
        newTexture.Apply();

        RenderTexture.active = null;
        return newTexture;
    }
    
    public static void SetInactive(this Component component)
    {
        component.gameObject.SetActive(false);
    }
    
    public static void SetActive(this Component component)
    {
        component.gameObject.SetActive(true);
    }
    
    public static void SetInactive(this GameObject gameObject)
    {
        gameObject.SetActive(false);
    }
    
    public static void SetActive(this GameObject gameObject)
    {
        gameObject.SetActive(true);
    }

    public static RectTransform SetLeft(this RectTransform rectTransform, float left)
    {
        rectTransform.offsetMin = new Vector2(left, rectTransform.offsetMin.y);
        return rectTransform;
    }

    
    public static RectTransform SetRight(this RectTransform rectTransform, float right)
    {
        rectTransform.offsetMax = new Vector2(-right, rectTransform.offsetMax.y);
        return rectTransform;
    }

    
    public static RectTransform SetTop(this RectTransform rectTransform, float top)
    {
        rectTransform.offsetMax = new Vector2(rectTransform.offsetMax.x, -top);
        return rectTransform;
    }

    
    public static RectTransform SetBottom(this RectTransform rectTransform, float bottom)
    {
        rectTransform.offsetMin = new Vector2(rectTransform.offsetMin.x, bottom);
        return rectTransform;
    }

    
    public static void DestroyAllChild(this Transform transform)
    {
        foreach (Transform child in transform)
            UnityEngine.Object.Destroy(child.gameObject);
    }
    
    public static int GetParentsCount(this Transform transform)
    {
        var count = 0;
        var currentTransform = transform;

        while (currentTransform.parent != null)
        {
            count++;
            currentTransform = currentTransform.parent;
        }

        return count;
    }
    
    public static void DrawLine(Vector3 from, Vector3 to, float width = 0)
    {
        var count = Mathf.CeilToInt(width);

        if (count == 0)
            return;

        if (count == 1)
        {
            Gizmos.DrawLine(from, to);
            return;
        }

        var camera = Camera.current;
        if (camera == null)
        {
            Debug.LogError("Camera.current is null");
            return;
        }

        var scp1 = camera.WorldToScreenPoint(from);
        var scp2 = camera.WorldToScreenPoint(to);

        var v1 = (scp2 - scp1).normalized;
        var n = Vector3.Cross(v1, Vector3.forward);

        for (int i = 0; i < count; i++)
        {
            var o = 0.99f * n * width * ((float) i / (count - 1) - 0.5f);
            var origin = camera.ScreenToWorldPoint(scp1 + o);
            var destiny = camera.ScreenToWorldPoint(scp2 + o);
            Gizmos.DrawLine(origin, destiny);
        }
    }
    
    public static bool Includes(this LayerMask mask, int layer)
    {
        return (mask.value & 1 << layer) > 0;
    }
    
    public static bool Excludes(this LayerMask mask, int layer)
    {
        return !mask.Includes(layer);
    }

    public static IEnumerator WaitAll(this MonoBehaviour mono, params IEnumerator[] ienumerators)
    {
        return ienumerators.Select(mono.StartCoroutine).ToArray().GetEnumerator();
    }
    
    public static T GetRandom<T>(this IEnumerable<T> collection)
    {
        var count = collection.Count();
        if (count == 0)
            return default(T);

        return collection.ElementAt(Random.Range(0, count));
    }
}