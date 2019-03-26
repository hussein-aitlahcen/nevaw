using Google.Protobuf;
using Google.Protobuf.Reflection;
using System;
using System.Diagnostics;

namespace Ankama.Cube.Protocols.FightPreparationProtocol
{
	public sealed class ForceMatchmakingAgainstAICmd : IMessage<ForceMatchmakingAgainstAICmd>, IMessage, IEquatable<ForceMatchmakingAgainstAICmd>, IDeepCloneable<ForceMatchmakingAgainstAICmd>, ICustomDiagnosticMessage
	{
		private static readonly MessageParser<ForceMatchmakingAgainstAICmd> _parser = new MessageParser<ForceMatchmakingAgainstAICmd>((Func<ForceMatchmakingAgainstAICmd>)(() => new ForceMatchmakingAgainstAICmd()));

		private UnknownFieldSet _unknownFields;

		[DebuggerNonUserCode]
		public static MessageParser<ForceMatchmakingAgainstAICmd> Parser => _parser;

		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor => FightPreparationProtocolReflection.Descriptor.get_MessageTypes()[4];

		[DebuggerNonUserCode]
		MessageDescriptor Descriptor => Descriptor;

		[DebuggerNonUserCode]
		public ForceMatchmakingAgainstAICmd()
		{
		}

		[DebuggerNonUserCode]
		public ForceMatchmakingAgainstAICmd(ForceMatchmakingAgainstAICmd other)
			: this()
		{
			_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		[DebuggerNonUserCode]
		public ForceMatchmakingAgainstAICmd Clone()
		{
			return new ForceMatchmakingAgainstAICmd(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as ForceMatchmakingAgainstAICmd);
		}

		[DebuggerNonUserCode]
		public bool Equals(ForceMatchmakingAgainstAICmd other)
		{
			if (other == null)
			{
				return false;
			}
			if (other == this)
			{
				return true;
			}
			return object.Equals(_unknownFields, other._unknownFields);
		}

		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (_unknownFields != null)
			{
				num ^= ((object)_unknownFields).GetHashCode();
			}
			return num;
		}

		[DebuggerNonUserCode]
		public override string ToString()
		{
			return JsonFormatter.ToDiagnosticString(this);
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (_unknownFields != null)
			{
				_unknownFields.WriteTo(output);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (_unknownFields != null)
			{
				num += _unknownFields.CalculateSize();
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(ForceMatchmakingAgainstAICmd other)
		{
			if (other != null)
			{
				_unknownFields = UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
			}
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0)
			{
				_unknownFields = UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
			}
		}

		public string ToDiagnosticString()
		{
			return "ForceMatchmakingAgainstAICmd";
		}
	}
}
