﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FXManager : MonoBehaviourSingleton<FXManager>
{
    bool ValueReached(float from, float to, float current)
    {
        return from < to ? current >= to : current <= to;
    }

    #region Alpha Lerp
    #region Material
    public void StartAlphaLerp(Material material, float from, float to, float duration, Action onEnd = null)
    {
        StartCoroutine(LerpMaterialAlpha(material, from, to, duration, onEnd));
    }

    public void StartAlphaLerp0To1(Material material, float duration, Action onEnd = null)
    {
        StartCoroutine(LerpMaterialAlpha(material, 0f, 1f, duration, onEnd));
    }

    public void StartAlphaLerp1To0(Material material, float duration, Action onEnd = null)
    {
        StartCoroutine(LerpMaterialAlpha(material, 1f, 0f, duration, onEnd));
    }

    IEnumerator LerpMaterialAlpha(Material material, float from, float to, float duration, Action onEnd)
    {
        float currentAlpha = from;

        while (!ValueReached(from, to, currentAlpha))
        {
            if (material)
            {
                float step = 1f / (duration / Time.deltaTime);
                if (from > to) step *= -1f;
                currentAlpha += step;

                Color newColor = material.color;
                newColor.a = currentAlpha;
                material.color = newColor;

                yield return null;
            }
            else yield break;
        }

        onEnd?.Invoke();
    }
    #endregion

    #region Image
    public void StartAlphaLerp(Image image, float from, float to, float duration, Action onEnd = null)
    {
        StartCoroutine(LerpImageAlpha(image, from, to, duration, onEnd));
    }

    public void StartAlphaLerp0To1(Image image, float duration, Action onEnd = null)
    {
        StartCoroutine(LerpImageAlpha(image, 0f, 1f, duration, onEnd));
    }

    public void StartAlphaLerp1To0(Image image, float duration, Action onEnd = null)
    {
        StartCoroutine(LerpImageAlpha(image, 1f, 0f, duration, onEnd));
    }

    IEnumerator LerpImageAlpha(Image image, float from, float to, float duration, Action onEnd)
    {
        float currentAlpha = from;

        while (!ValueReached(from, to, currentAlpha))
        {
            if (image)
            {
                float step = 1f / (duration / Time.deltaTime);
                if (from > to) step *= -1f;
                currentAlpha += step;

                Color newColor = image.color;
                newColor.a = currentAlpha;
                image.color = newColor;

                yield return null;
            }
            else yield break;
        }

        onEnd?.Invoke();
    }
    #endregion

    #region Sprite Renderer
    public void StartAlphaLerp(SpriteRenderer sr, float from, float to, float duration, Action onEnd = null)
    {
        StartCoroutine(LerpSRAlpha(sr, from, to, duration, onEnd));
    }

    public void StartAlphaLerp0To1(SpriteRenderer sr, float duration, Action onEnd = null)
    {
        StartCoroutine(LerpSRAlpha(sr, 0f, 1f, duration, onEnd));
    }

    public void StartAlphaLerp1To0(SpriteRenderer sr, float duration, Action onEnd = null)
    {
        StartCoroutine(LerpSRAlpha(sr, 1f, 0f, duration, onEnd));
    }

    IEnumerator LerpSRAlpha(SpriteRenderer SR, float from, float to, float duration, Action onEnd)
    {
        float currentAlpha = from;

        while (!ValueReached(from, to, currentAlpha))
        {
            if (SR)
            {
                float step = 1f / (duration / Time.deltaTime);
                if (from > to) step *= -1f;
                currentAlpha += step;

                Color newColor = SR.color;
                newColor.a = currentAlpha;
                SR.color = newColor;

                yield return null;
            }
            else yield break;
        }

        onEnd?.Invoke();
    }
    #endregion
    #endregion

    #region Camera Shake
    public void StartCameraShake(float shakePointsAmount, float shakeMagnitude, float shakeSpeed, Action onEnd = null)
    {
        StartCoroutine(ShakeCamera(shakePointsAmount, shakeMagnitude, shakeSpeed, onEnd));
    }

    IEnumerator ShakeCamera(float shakePointsAmount, float shakeMagnitude, float shakeSpeed, Action onEnd)
    {
        Transform camera = Camera.main.transform;
        float positionZ = camera.position.z;

        List<Vector2> points = new List<Vector2>();

        Vector2 newPoint = new Vector2(UnityEngine.Random.Range(-shakeMagnitude, shakeMagnitude), UnityEngine.Random.Range(-shakeMagnitude, shakeMagnitude));
        points.Add(newPoint);
        for (int i = 0; i < shakePointsAmount - 1; i++)
        {
            newPoint.x = newPoint.x > 0f ? UnityEngine.Random.Range(-shakeMagnitude, 0f) : UnityEngine.Random.Range(0f, shakeMagnitude);
            newPoint.y = newPoint.y > 0f ? UnityEngine.Random.Range(-shakeMagnitude, 0f) : UnityEngine.Random.Range(0f, shakeMagnitude);
            points.Add(newPoint);
        }

        Vector3 position;
        Vector2 a;
        Vector2 b;
        for (int i = 0; i < shakePointsAmount + 1; i++)
        {
            a = camera.position;
            b = i < shakePointsAmount ? points[i] : Vector2.zero;

            float journeyLength = Vector2.Distance(a, b);
            float fractionToMove = shakeSpeed * Time.deltaTime;
            while ((Vector2)camera.position != b)
            {
                float distanceCovered = Vector2.Distance(a, camera.position);
                float fractionMoved = distanceCovered / journeyLength;

                position = camera.position;
                position = Vector2.Lerp(a, b, fractionMoved + fractionToMove);
                position.z = positionZ;
                camera.position = position;

                yield return new WaitForSeconds(Time.deltaTime);
            }
        }

        onEnd?.Invoke();
    }
    #endregion
}