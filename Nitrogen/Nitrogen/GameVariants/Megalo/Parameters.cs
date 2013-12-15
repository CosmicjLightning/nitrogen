﻿using Nitrogen.GameVariants.Megalo.Definitions;
using Nitrogen.GameVariants.Megalo.ParameterTypes;
using Nitrogen.IO;
using System;
using System.Collections.Generic;

namespace Nitrogen.GameVariants.Megalo
{
	public sealed class Parameters
		: List<Parameter>
	{
		private static Dictionary<ParameterType, Type> ParameterTypes = new Dictionary<ParameterType, Type>
		{
			{ ParameterType.None, null },
			{ ParameterType.Integer, typeof(IntegerValue) },
			{ ParameterType.Float, typeof(FloatValue) },
		};

		internal void Serialize (BitStream s, Definition definition)
		{
			if ( s.State == StreamState.Read )
				Clear();

			if ( s.State == StreamState.Write && definition.Parameters.Count != Count )
				throw new Exception(string.Format("Parameter count mismatch for {0}", definition.Name ?? definition.Opcode.ToString()));

			for ( int i = 0; i < definition.Parameters.Count; i++ )
			{
				if ( s.State == StreamState.Write )
				{
					if ( definition.Parameters[i].ParameterType != this[i].ParameterType )
					{
						throw new Exception(string.Format(
							"Parameter type mismatch; expected {0} but got {1} (index: {2})",
							definition.Parameters[i].ParameterType,
							this[i].ParameterType,
							i
						));
					}

					s.SerializeObject(this[i]);
				}
				else if ( s.State == StreamState.Read )
				{
					var type = definition.Parameters[i].ParameterType;

					if ( !ParameterTypes.ContainsKey(type) )
						throw new Exception("Unhandled parameter type: " + type);

					if ( ParameterTypes[type] != null )
					{
						var param = Activator.CreateInstance(ParameterTypes[type]) as Parameter;
						this.Add(param);
						s.SerializeObject(param);
					}
				}
			}
		}
	}
}