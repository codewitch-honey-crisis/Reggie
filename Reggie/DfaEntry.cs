using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;

namespace Reggie
{
	/// <summary>
	/// This is an internal class that helps the code serializer know how to serialize DFA entries
	/// </summary>
	class DfaEntryConverter : TypeConverter
	{
		// we only need to convert to an InstanceDescriptor
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			if (typeof(InstanceDescriptor) == destinationType)
				return true;
			return base.CanConvertTo(context, destinationType);
		}
		// we return an InstanceDescriptor so the serializer can read it to figure out what code to generate
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (typeof(InstanceDescriptor) == destinationType)
			{
				// basically what we're doing is reporting that the constructor contains all the necessary
				// parameters for initializing an instance of this object in the specified state
				var dte = (DfaEntry)value;
				return new InstanceDescriptor(typeof(DfaEntry).GetConstructor(new Type[] { typeof(int), typeof(DfaTransitionEntry[]) }), new object[] { dte.AcceptSymbolId, dte.Transitions });
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}
	}
	/// <summary>
	/// Represents an entry in a DFA state table
	/// </summary>
	[TypeConverter(typeof(DfaEntryConverter))]
#if FALIB
	public 
#endif
	struct DfaEntry
	{
		/// <summary>
		/// Constructs a new instance of the DFA state table with the specified parameters
		/// </summary>
		/// <param name="acceptSymbolId">The symbolId to accept or -1 for non-accepting</param>
		/// <param name="transitions">The transition entries</param>
		public DfaEntry(int acceptSymbolId, DfaTransitionEntry[] transitions)
		{
			AcceptSymbolId = acceptSymbolId;
			Transitions = transitions;
		}
		/// <summary>
		/// Indicates the accept symbol's id or -1 for non-accepting
		/// </summary>
		public int AcceptSymbolId;
		/// <summary>
		/// Indicates the transition entries
		/// </summary>
		public DfaTransitionEntry[] Transitions;
	}
	/// <summary>
	/// This is an internal class that helps the code serializer serialize a DfaTransitionEntry
	/// </summary>
	class DfaTransitionEntryConverter : TypeConverter
	{
		// we only need to convert to an InstanceDescriptor
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			if (typeof(InstanceDescriptor) == destinationType)
				return true;
			return base.CanConvertTo(context, destinationType);
		}
		// report the constructor of the class so the serializer knows which call to serialize
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (typeof(InstanceDescriptor) == destinationType)
			{
				var dte = (DfaTransitionEntry)value;
				return new InstanceDescriptor(typeof(DfaTransitionEntry).GetConstructor(new Type[] { typeof(int[]), typeof(int) }), new object[] { dte.PackedRanges, dte.Destination });
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}
	}
	/// <summary>
	/// Indicates a transition entry in the DFA state table
	/// </summary>
	[TypeConverter(typeof(DfaTransitionEntryConverter))]
	public struct DfaTransitionEntry
	{
		/// <summary>
		/// Constructs a DFA transition entry with the specified parameters
		/// </summary>
		/// <param name="transitions">Packed character range pairs as a flat array</param>
		/// <param name="destination">The destination state id</param>
		public DfaTransitionEntry(int[] transitions, int destination)
		{
			PackedRanges = transitions;
			Destination = destination;
		}
		/// <summary>
		/// Indicates the packed range characters. Each range is specified by two array entries, first and last in that order.
		/// </summary>
		public int[] PackedRanges;
		/// <summary>
		/// Indicates the destination state id
		/// </summary>
		public int Destination;
	}
}
