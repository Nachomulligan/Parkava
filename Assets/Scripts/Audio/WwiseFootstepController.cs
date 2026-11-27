using UnityEngine;

/// <summary>
/// Posts the Wwise footstep event at a speed-based interval and selects the
/// Footsteps switch from the surface currently supporting the player.
/// </summary>
[RequireComponent(typeof(PlayerMovement))]
public class WwiseFootstepController : MonoBehaviour
{
    private const uint FootstepsSwitchGroupId = 2385628198;
    private const uint DirtSwitchId = 2195636714;
    private const uint GrassSwitchId = 4248645337;
    private const uint GravelSwitchId = 2185786256;
    private const uint WaterSwitchId = 2654748154;

    [Header("Wwise")]
    [SerializeField] private AK.Wwise.Event footstepEvent;

    [Header("Step Timing")]
    [SerializeField, Min(0f)] private float minimumPlanarSpeed = 0.5f;
    [SerializeField, Min(0.01f)] private float referenceSpeed = 8f;
    [SerializeField, Min(0.01f)] private float stepIntervalAtReferenceSpeed = 0.45f;
    [SerializeField, Min(0.01f)] private float minimumStepInterval = 0.16f;
    [SerializeField, Min(0.01f)] private float maximumStepInterval = 0.65f;
    [SerializeField, Min(0f)] private float groundedGracePeriod = 0.12f;

    [Header("Surface Detection")]
    [SerializeField] private string waterLayerName = "Water";

    private PlayerMovement playerMovement;
    private float stepTimer;
    private float lastGroundedTime = float.NegativeInfinity;
    private int waterLayer;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        waterLayer = LayerMask.NameToLayer(waterLayerName);
    }

    private void Update()
    {
        if (footstepEvent == null)
        {
            stepTimer = 0f;
            return;
        }

        bool hasGroundContact = playerMovement.IsGrounded && playerMovement.GroundCollider != null;
        if (hasGroundContact)
            lastGroundedTime = Time.time;

        if (!hasGroundContact)
        {
            // Ignore very short contact flickers on uneven terrain. Do not post while airborne.
            if (Time.time - lastGroundedTime > groundedGracePeriod)
                stepTimer = 0f;
            return;
        }

        if (playerMovement.PlanarSpeed < minimumPlanarSpeed)
        {
            stepTimer = 0f;
            return;
        }

        stepTimer += Time.deltaTime;
        float interval = Mathf.Clamp(
            stepIntervalAtReferenceSpeed * referenceSpeed / playerMovement.PlanarSpeed,
            minimumStepInterval,
            maximumStepInterval);

        if (stepTimer < interval)
            return;

        stepTimer %= interval;
        PostFootstep();
    }

    private void PostFootstep()
    {
        if (!TryGetSwitchId(playerMovement.GroundCollider, playerMovement.GroundContactPoint, out uint switchId))
            return;

        AkSoundEngine.SetSwitch(FootstepsSwitchGroupId, switchId, gameObject);
        footstepEvent.Post(gameObject);
    }

    private bool TryGetSwitchId(Collider surfaceCollider, Vector3 contactPoint, out uint switchId)
    {
        if (waterLayer >= 0 && surfaceCollider.gameObject.layer == waterLayer)
        {
            switchId = WaterSwitchId;
            return true;
        }

        if (surfaceCollider is TerrainCollider terrainCollider)
            return TryGetTerrainSwitchId(terrainCollider, contactPoint, out switchId);

        // Non-terrain ground uses its layer name as a simple surface convention.
        string layerName = LayerMask.LayerToName(surfaceCollider.gameObject.layer);
        return TryMapSurfaceName(layerName, out switchId);
    }

    private bool TryGetTerrainSwitchId(TerrainCollider terrainCollider, Vector3 contactPoint, out uint switchId)
    {
        TerrainData terrainData = terrainCollider.terrainData;
        if (terrainData == null || terrainData.terrainLayers == null || terrainData.terrainLayers.Length == 0)
        {
            switchId = DirtSwitchId;
            return true;
        }

        Vector3 localPoint = contactPoint - terrainCollider.transform.position;
        float normalizedX = Mathf.Clamp01(localPoint.x / terrainData.size.x);
        float normalizedZ = Mathf.Clamp01(localPoint.z / terrainData.size.z);
        int mapX = Mathf.Clamp(Mathf.RoundToInt(normalizedX * (terrainData.alphamapWidth - 1)), 0, terrainData.alphamapWidth - 1);
        int mapY = Mathf.Clamp(Mathf.RoundToInt(normalizedZ * (terrainData.alphamapHeight - 1)), 0, terrainData.alphamapHeight - 1);
        float[,,] weights = terrainData.GetAlphamaps(mapX, mapY, 1, 1);

        int dominantLayer = 0;
        for (int i = 1; i < terrainData.terrainLayers.Length; i++)
        {
            if (weights[0, 0, i] > weights[0, 0, dominantLayer])
                dominantLayer = i;
        }

        string terrainLayerName = terrainData.terrainLayers[dominantLayer].name;
        return TryMapSurfaceName(terrainLayerName, out switchId);
    }

    private bool TryMapSurfaceName(string surfaceName, out uint switchId)
    {
        if (string.IsNullOrEmpty(surfaceName))
        {
            switchId = DirtSwitchId;
            return true;
        }

        string normalizedName = surfaceName.ToLowerInvariant();
        if (normalizedName.Contains("grass"))
            switchId = GrassSwitchId;
        else if (normalizedName.Contains("road") || normalizedName.Contains("gravel"))
            switchId = GravelSwitchId;
        else if (normalizedName.Contains("water"))
            switchId = WaterSwitchId;
        else if (normalizedName.Contains("ground") || normalizedName.Contains("dirt"))
            switchId = DirtSwitchId;
        else
            switchId = DirtSwitchId;

        return true;
    }
}
