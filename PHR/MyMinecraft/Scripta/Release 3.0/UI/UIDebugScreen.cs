using TMPro;
using UnityEngine;

public class UIDebugScreen : MonoBehaviour
{
    [Header("Left")]
    [SerializeField] TextMeshProUGUI version;
    [SerializeField] TextMeshProUGUI fps;
    [SerializeField] TextMeshProUGUI playerPosition;
    [SerializeField] TextMeshProUGUI inChunkPosition;
    [SerializeField] TextMeshProUGUI chunkPosition;
    [SerializeField] TextMeshProUGUI direction;
    [SerializeField] TextMeshProUGUI seed;

    [Header("Right")]
    [SerializeField] TextMeshProUGUI memory;
    [SerializeField] TextMeshProUGUI cpu;
    [SerializeField] TextMeshProUGUI gpu;
    [SerializeField] TextMeshProUGUI[] targetBlock;

    // Left
    internal void SetVersion(string versionType, string version) { ChangeText(this.version, $"MyMinecraft: {versionType} {version}"); }
    internal void SetFPS(float fps) { if (fps != -1) ChangeText(this.fps, $"FPS: {fps:0.}"); else ChangeText(this.fps, $"FPS: Loading"); }
    internal void SetPlayerPos(Vector3Int realPos) { ChangeText(playerPosition, $"Player: {realPos.x},{realPos.y},{realPos.z}"); }
    internal void SetInChunkPos(Vector3Int inChunkPos) { ChangeText(inChunkPosition, $"Chunk relative: {inChunkPos}"); }
    internal void SetChunkPos(Vector2Int worlPos) { ChangeText(chunkPosition, $"Chunk position: {worlPos}"); }
    internal void SetDirection(string direction) { ChangeText(this.direction, $"Direction: {direction}"); }
    internal void SetSeed(Vector2Int seed) { ChangeText(this.seed, $"World seed: {seed}"); }

    // Right
    internal void SetMemory(float allocated, float reserved) { ChangeText(this.memory, $"Memory: {allocated:0.}/{reserved:0.}MB"); }
    internal void SetCPU(string cpu) { ChangeText(this.cpu, $"CPU: {cpu}"); }
    internal void SetGPU(string gpu) { ChangeText(this.gpu, $"GPU: {gpu}"); }
    internal void SetTargetBlock(BlockType blockType, Vector3Int blockPos)
    {
        targetBlock[0].gameObject.SetActive(true);
        targetBlock[1].gameObject.SetActive(true);

        ChangeText(targetBlock[0], $"Targeted block: {blockPos}");
        ChangeText(targetBlock[1], $"Block name: {blockType}");
    }
    internal void SetTargetBlock() { targetBlock[0].gameObject.SetActive(false); targetBlock[1].gameObject.SetActive(false); }

    void ChangeText(TextMeshProUGUI textMeshProUGUI, string text)
    {
        textMeshProUGUI.text = $"<mark=#FFFFFF60>{text}</mark>";
    }
}