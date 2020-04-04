// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: NetworkMsg.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
/// <summary>Holder for reflection information generated from NetworkMsg.proto</summary>
public static partial class NetworkMsgReflection {

  #region Descriptor
  /// <summary>File descriptor for NetworkMsg.proto</summary>
  public static pbr::FileDescriptor Descriptor {
    get { return descriptor; }
  }
  private static pbr::FileDescriptor descriptor;

  static NetworkMsgReflection() {
    byte[] descriptorData = global::System.Convert.FromBase64String(
        string.Concat(
          "ChBOZXR3b3JrTXNnLnByb3RvIkoKCk5ldHdvcmtNc2cSHwoKY2xpZW50SW5m",
          "bxgBIAEoCzILLkNsaWVudEluZm8SGwoIcG9zaXRpb24YAiABKAsyCS5Qb3Np",
          "dGlvbiIdCgpDbGllbnRJbmZvEg8KB3JvYm90SWQYASABKAUiKwoIUG9zaXRp",
          "b24SCQoBeBgBIAEoBRIJCgF5GAIgASgFEgkKAXoYAyABKAViBnByb3RvMw=="));
    descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
        new pbr::FileDescriptor[] { },
        new pbr::GeneratedClrTypeInfo(null, new pbr::GeneratedClrTypeInfo[] {
          new pbr::GeneratedClrTypeInfo(typeof(global::NetworkMsg), global::NetworkMsg.Parser, new[]{ "ClientInfo", "Position" }, null, null, null),
          new pbr::GeneratedClrTypeInfo(typeof(global::ClientInfo), global::ClientInfo.Parser, new[]{ "RobotId" }, null, null, null),
          new pbr::GeneratedClrTypeInfo(typeof(global::Position), global::Position.Parser, new[]{ "X", "Y", "Z" }, null, null, null)
        }));
  }
  #endregion

}
#region Messages
public sealed partial class NetworkMsg : pb::IMessage<NetworkMsg> {
  private static readonly pb::MessageParser<NetworkMsg> _parser = new pb::MessageParser<NetworkMsg>(() => new NetworkMsg());
  private pb::UnknownFieldSet _unknownFields;
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public static pb::MessageParser<NetworkMsg> Parser { get { return _parser; } }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public static pbr::MessageDescriptor Descriptor {
    get { return global::NetworkMsgReflection.Descriptor.MessageTypes[0]; }
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  pbr::MessageDescriptor pb::IMessage.Descriptor {
    get { return Descriptor; }
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public NetworkMsg() {
    OnConstruction();
  }

  partial void OnConstruction();

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public NetworkMsg(NetworkMsg other) : this() {
    clientInfo_ = other.clientInfo_ != null ? other.clientInfo_.Clone() : null;
    position_ = other.position_ != null ? other.position_.Clone() : null;
    _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public NetworkMsg Clone() {
    return new NetworkMsg(this);
  }

  /// <summary>Field number for the "clientInfo" field.</summary>
  public const int ClientInfoFieldNumber = 1;
  private global::ClientInfo clientInfo_;
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public global::ClientInfo ClientInfo {
    get { return clientInfo_; }
    set {
      clientInfo_ = value;
    }
  }

  /// <summary>Field number for the "position" field.</summary>
  public const int PositionFieldNumber = 2;
  private global::Position position_;
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public global::Position Position {
    get { return position_; }
    set {
      position_ = value;
    }
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public override bool Equals(object other) {
    return Equals(other as NetworkMsg);
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public bool Equals(NetworkMsg other) {
    if (ReferenceEquals(other, null)) {
      return false;
    }
    if (ReferenceEquals(other, this)) {
      return true;
    }
    if (!object.Equals(ClientInfo, other.ClientInfo)) return false;
    if (!object.Equals(Position, other.Position)) return false;
    return Equals(_unknownFields, other._unknownFields);
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public override int GetHashCode() {
    int hash = 1;
    if (clientInfo_ != null) hash ^= ClientInfo.GetHashCode();
    if (position_ != null) hash ^= Position.GetHashCode();
    if (_unknownFields != null) {
      hash ^= _unknownFields.GetHashCode();
    }
    return hash;
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public override string ToString() {
    return pb::JsonFormatter.ToDiagnosticString(this);
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public void WriteTo(pb::CodedOutputStream output) {
    if (clientInfo_ != null) {
      output.WriteRawTag(10);
      output.WriteMessage(ClientInfo);
    }
    if (position_ != null) {
      output.WriteRawTag(18);
      output.WriteMessage(Position);
    }
    if (_unknownFields != null) {
      _unknownFields.WriteTo(output);
    }
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public int CalculateSize() {
    int size = 0;
    if (clientInfo_ != null) {
      size += 1 + pb::CodedOutputStream.ComputeMessageSize(ClientInfo);
    }
    if (position_ != null) {
      size += 1 + pb::CodedOutputStream.ComputeMessageSize(Position);
    }
    if (_unknownFields != null) {
      size += _unknownFields.CalculateSize();
    }
    return size;
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public void MergeFrom(NetworkMsg other) {
    if (other == null) {
      return;
    }
    if (other.clientInfo_ != null) {
      if (clientInfo_ == null) {
        clientInfo_ = new global::ClientInfo();
      }
      ClientInfo.MergeFrom(other.ClientInfo);
    }
    if (other.position_ != null) {
      if (position_ == null) {
        position_ = new global::Position();
      }
      Position.MergeFrom(other.Position);
    }
    _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public void MergeFrom(pb::CodedInputStream input) {
    uint tag;
    while ((tag = input.ReadTag()) != 0) {
      switch(tag) {
        default:
          _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
          break;
        case 10: {
          if (clientInfo_ == null) {
            clientInfo_ = new global::ClientInfo();
          }
          input.ReadMessage(clientInfo_);
          break;
        }
        case 18: {
          if (position_ == null) {
            position_ = new global::Position();
          }
          input.ReadMessage(position_);
          break;
        }
      }
    }
  }

}

public sealed partial class ClientInfo : pb::IMessage<ClientInfo> {
  private static readonly pb::MessageParser<ClientInfo> _parser = new pb::MessageParser<ClientInfo>(() => new ClientInfo());
  private pb::UnknownFieldSet _unknownFields;
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public static pb::MessageParser<ClientInfo> Parser { get { return _parser; } }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public static pbr::MessageDescriptor Descriptor {
    get { return global::NetworkMsgReflection.Descriptor.MessageTypes[1]; }
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  pbr::MessageDescriptor pb::IMessage.Descriptor {
    get { return Descriptor; }
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public ClientInfo() {
    OnConstruction();
  }

  partial void OnConstruction();

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public ClientInfo(ClientInfo other) : this() {
    robotId_ = other.robotId_;
    _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public ClientInfo Clone() {
    return new ClientInfo(this);
  }

  /// <summary>Field number for the "robotId" field.</summary>
  public const int RobotIdFieldNumber = 1;
  private int robotId_;
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public int RobotId {
    get { return robotId_; }
    set {
      robotId_ = value;
    }
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public override bool Equals(object other) {
    return Equals(other as ClientInfo);
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public bool Equals(ClientInfo other) {
    if (ReferenceEquals(other, null)) {
      return false;
    }
    if (ReferenceEquals(other, this)) {
      return true;
    }
    if (RobotId != other.RobotId) return false;
    return Equals(_unknownFields, other._unknownFields);
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public override int GetHashCode() {
    int hash = 1;
    if (RobotId != 0) hash ^= RobotId.GetHashCode();
    if (_unknownFields != null) {
      hash ^= _unknownFields.GetHashCode();
    }
    return hash;
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public override string ToString() {
    return pb::JsonFormatter.ToDiagnosticString(this);
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public void WriteTo(pb::CodedOutputStream output) {
    if (RobotId != 0) {
      output.WriteRawTag(8);
      output.WriteInt32(RobotId);
    }
    if (_unknownFields != null) {
      _unknownFields.WriteTo(output);
    }
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public int CalculateSize() {
    int size = 0;
    if (RobotId != 0) {
      size += 1 + pb::CodedOutputStream.ComputeInt32Size(RobotId);
    }
    if (_unknownFields != null) {
      size += _unknownFields.CalculateSize();
    }
    return size;
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public void MergeFrom(ClientInfo other) {
    if (other == null) {
      return;
    }
    if (other.RobotId != 0) {
      RobotId = other.RobotId;
    }
    _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public void MergeFrom(pb::CodedInputStream input) {
    uint tag;
    while ((tag = input.ReadTag()) != 0) {
      switch(tag) {
        default:
          _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
          break;
        case 8: {
          RobotId = input.ReadInt32();
          break;
        }
      }
    }
  }

}

public sealed partial class Position : pb::IMessage<Position> {
  private static readonly pb::MessageParser<Position> _parser = new pb::MessageParser<Position>(() => new Position());
  private pb::UnknownFieldSet _unknownFields;
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public static pb::MessageParser<Position> Parser { get { return _parser; } }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public static pbr::MessageDescriptor Descriptor {
    get { return global::NetworkMsgReflection.Descriptor.MessageTypes[2]; }
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  pbr::MessageDescriptor pb::IMessage.Descriptor {
    get { return Descriptor; }
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public Position() {
    OnConstruction();
  }

  partial void OnConstruction();

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public Position(Position other) : this() {
    x_ = other.x_;
    y_ = other.y_;
    z_ = other.z_;
    _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public Position Clone() {
    return new Position(this);
  }

  /// <summary>Field number for the "x" field.</summary>
  public const int XFieldNumber = 1;
  private int x_;
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public int X {
    get { return x_; }
    set {
      x_ = value;
    }
  }

  /// <summary>Field number for the "y" field.</summary>
  public const int YFieldNumber = 2;
  private int y_;
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public int Y {
    get { return y_; }
    set {
      y_ = value;
    }
  }

  /// <summary>Field number for the "z" field.</summary>
  public const int ZFieldNumber = 3;
  private int z_;
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public int Z {
    get { return z_; }
    set {
      z_ = value;
    }
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public override bool Equals(object other) {
    return Equals(other as Position);
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public bool Equals(Position other) {
    if (ReferenceEquals(other, null)) {
      return false;
    }
    if (ReferenceEquals(other, this)) {
      return true;
    }
    if (X != other.X) return false;
    if (Y != other.Y) return false;
    if (Z != other.Z) return false;
    return Equals(_unknownFields, other._unknownFields);
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public override int GetHashCode() {
    int hash = 1;
    if (X != 0) hash ^= X.GetHashCode();
    if (Y != 0) hash ^= Y.GetHashCode();
    if (Z != 0) hash ^= Z.GetHashCode();
    if (_unknownFields != null) {
      hash ^= _unknownFields.GetHashCode();
    }
    return hash;
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public override string ToString() {
    return pb::JsonFormatter.ToDiagnosticString(this);
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public void WriteTo(pb::CodedOutputStream output) {
    if (X != 0) {
      output.WriteRawTag(8);
      output.WriteInt32(X);
    }
    if (Y != 0) {
      output.WriteRawTag(16);
      output.WriteInt32(Y);
    }
    if (Z != 0) {
      output.WriteRawTag(24);
      output.WriteInt32(Z);
    }
    if (_unknownFields != null) {
      _unknownFields.WriteTo(output);
    }
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public int CalculateSize() {
    int size = 0;
    if (X != 0) {
      size += 1 + pb::CodedOutputStream.ComputeInt32Size(X);
    }
    if (Y != 0) {
      size += 1 + pb::CodedOutputStream.ComputeInt32Size(Y);
    }
    if (Z != 0) {
      size += 1 + pb::CodedOutputStream.ComputeInt32Size(Z);
    }
    if (_unknownFields != null) {
      size += _unknownFields.CalculateSize();
    }
    return size;
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public void MergeFrom(Position other) {
    if (other == null) {
      return;
    }
    if (other.X != 0) {
      X = other.X;
    }
    if (other.Y != 0) {
      Y = other.Y;
    }
    if (other.Z != 0) {
      Z = other.Z;
    }
    _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public void MergeFrom(pb::CodedInputStream input) {
    uint tag;
    while ((tag = input.ReadTag()) != 0) {
      switch(tag) {
        default:
          _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
          break;
        case 8: {
          X = input.ReadInt32();
          break;
        }
        case 16: {
          Y = input.ReadInt32();
          break;
        }
        case 24: {
          Z = input.ReadInt32();
          break;
        }
      }
    }
  }

}

#endregion


#endregion Designer generated code