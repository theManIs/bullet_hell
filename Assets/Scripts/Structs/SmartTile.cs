using System;
using Unity.Netcode;
using UnityEngine;

[Serializable]
public struct SmartTile : INetworkSerializable, IEquatable<SmartTile>
{
    public int SpriteFrequencyIndex;
    public Vector3Int TilePosition;
    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref SpriteFrequencyIndex);
        serializer.SerializeValue(ref TilePosition);
    }

    public bool Equals(SmartTile other)
    {
        return SpriteFrequencyIndex == other.SpriteFrequencyIndex && TilePosition.Equals(other.TilePosition);
    }

    public override bool Equals(object obj)
    {
        return obj is SmartTile other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(SpriteFrequencyIndex, TilePosition);
    }
}
