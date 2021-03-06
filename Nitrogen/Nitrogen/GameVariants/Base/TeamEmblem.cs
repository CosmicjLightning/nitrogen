﻿using Nitrogen.Enums;
using Nitrogen.IO;
using Nitrogen.Utilities;
using System;
using System.Diagnostics.Contracts;

namespace Nitrogen.GameVariants.Base
{
	public sealed class TeamEmblem
		: ISerializable<BitStream>
	{
		private bool
			_unknownFlag,
			_backgroundToggle,
			_foregroundToggle;

		private byte
			_foreground,
			_background,
			_primaryColor,
			_secondaryColor,
			_bgColor;

		public bool ForegroundToggle
		{
			get { return _foregroundToggle; }
			set { _foregroundToggle = value; }
		}

		public byte Foreground
		{
			get { return _foreground; }
			set { _foreground = value; }
		}

		public byte Background
		{
			get { return _foreground; }
			set { _foreground = value; }
		}

		public EmblemColor PrimaryColor
		{
			get { return (EmblemColor) _primaryColor; }
			set
			{
				Contract.Requires(value.IsDefined());
				_primaryColor = (byte) value;
			}
		}

		public EmblemColor SecondaryColor
		{
			get { return (EmblemColor) _secondaryColor; }
			set
			{
				Contract.Requires(value.IsDefined());
				_secondaryColor = (byte) value;
			}
		}

		public EmblemColor BackgroundColor
		{
			get { return (EmblemColor) _bgColor; }
			set
			{
				Contract.Requires(value.IsDefined());
				_bgColor = (byte) value;
			}
		}

		#region ISerializable<BitStream> Members

		void ISerializable<BitStream>.SerializeObject (BitStream s)
		{
			s.Stream(ref _foreground);
			s.Stream(ref _background);
			s.Stream(ref _unknownFlag);
			s.Stream(ref _backgroundToggle);
			s.Stream(ref _foregroundToggle);
			s.Stream(ref _primaryColor, bits: 6);
			s.Stream(ref _secondaryColor, bits: 6);
			s.Stream(ref _bgColor, bits: 6);
		}

		#endregion
	}
}
