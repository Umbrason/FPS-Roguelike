using System.Collections.Generic;
using UnityEngine;

public static class Hittests
{
    public struct LayerMaskRadiusPair
    {
        public LayerMaskRadiusPair(int layerMask, float radius)
        {
            LayerMask = layerMask;
            Radius = radius;
        }

        public int LayerMask { get; set; }
        public float Radius { get; set; }
    }

    public static bool GenerousSpherecast(Ray ray, out RaycastHit hit, params LayerMaskRadiusPair[] pairs)
    {
        foreach (var pair in pairs)
        {
            var hasHit = Physics.SphereCast(ray: ray,
                                            radius: pair.Radius,
                                            hitInfo: out hit,
                                            maxDistance: Mathf.Infinity,
                                            layerMask: pair.LayerMask);
            if (hasHit) return true;
        }
        hit = default;
        return false;
    }


    public static bool GenerousSpherecastAll(Ray ray, out RaycastHit[] hits, params LayerMaskRadiusPair[] pairs)
    {
        List<RaycastHit> hitList = new();
        foreach (var pair in pairs)
        {
            hits = Physics.SphereCastAll(ray: ray,
                                         radius: pair.Radius,
                                         maxDistance: Mathf.Infinity,
                                         layerMask: pair.LayerMask);
            hitList.AddRange(hits);
        }
        hits = hitList.ToArray();
        return hits.Length > 0;
    }


}