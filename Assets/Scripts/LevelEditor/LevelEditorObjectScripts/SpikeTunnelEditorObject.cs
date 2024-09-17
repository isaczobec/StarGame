using UnityEngine;

public class SpikeTunnelEditorObject : LevelEditorObject {
    private const string tunnelGapRef = "Tunnel gap";
    private const string spikeSpacingRef = "Spike spacing";
    [SerializeField] private CurveSpikeGenerator curveSpikeGenerator; // the curve spike generator that this object uses


    protected override void OnObjectSetup() {
        curveSpikeGenerator.SetSpikeDistance(editorObjectData.GetSetting<float>(spikeSpacingRef));
        curveSpikeGenerator.SetSpikeGap(editorObjectData.GetSetting<float>(tunnelGapRef));
        UpdateSpikeTunnelNodePositions();
    }

    public override void OnNodeMoved(EditorObjectNode node) {
        UpdateSpikeTunnelNodePositions();
    }

    /// <summary>
    /// Updates the spike tunnel based on the nodes.
    /// </summary>
    private void UpdateSpikeTunnelNodePositions() {
        Vector2[] nodePositions = new Vector2[nodes.Count];
        for (int i = 0; i < nodes.Count; i++) {
            nodePositions[i] = nodes[i].relativePosition;
        }
        curveSpikeGenerator.ClearSplines();
        curveSpikeGenerator.CreateSplineCurveFromNodePositions(nodePositions,transform.position);
        curveSpikeGenerator.GenerateAppropriateSpikes(destroyOld: true);
    }

    public override void OnSettingChanged<T>(string settingName, T value) {
        if (value.GetType() == typeof(float)) {
            if (settingName == tunnelGapRef) curveSpikeGenerator.SetSpikeGap((float)(object)value);
            else if (settingName == spikeSpacingRef) curveSpikeGenerator.SetSpikeDistance((float)(object)value);
            UpdateSpikeTunnelNodePositions();
        }
    }



}