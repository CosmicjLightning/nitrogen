﻿using Nitrogen.IO;
using Nitrogen.Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Nitrogen.GameVariants.Megalo
{
	public sealed class MegaloData
		: ISerializable<BitStream>
	{
		private List<UserDefinedTraits> _megaloTraits;
		private List<UserDefinedOption> _userDefinedOptions;
		private StringTable _megaloStrings, _localizedName, _localizedDescription, _introDescription, _categoryName;
		private sbyte _baseGameTypeNameStringIndex, _baseGameTypeIconIndex, _baseGameTypeCategoryIndex;
		private MapPermissions _mapPermissions;
		private PlayerRatingParameters _ratingParameters;
		private GlobalGameOptions _globalGameOptions;
		private List<MapLoadout> _mapLoadouts;
		private int[] _disabledOptions, _hiddenOptions;
		private int _disabledUserDefinedOptions, _hiddenUserDefinedOptions;
		private List<Condition> _conditions;

		public MegaloData()
		{
			_megaloTraits = new List<UserDefinedTraits>();
			_userDefinedOptions = new List<UserDefinedOption>();
			_baseGameTypeNameStringIndex = -1;
			_baseGameTypeCategoryIndex = -1;
			_mapPermissions = new MapPermissions();
			_ratingParameters = new PlayerRatingParameters();
			_globalGameOptions = new GlobalGameOptions();
			_mapLoadouts = new List<MapLoadout>();
			_disabledOptions = new int[26];
			_hiddenOptions = new int[26];
			_megaloStrings = new StringTable();
			_localizedName = new StringTable();
			_localizedDescription = new StringTable();
			_introDescription = new StringTable();
			_categoryName = new StringTable();
			_conditions = new List<Condition>();
		}

		#region Properties

		/// <summary>
		/// Gets or sets the trait sets which can be referenced by the script.
		/// 
		/// A maximum of 16 traits are available.
		/// </summary>
		public List<UserDefinedTraits> Traits
		{
			get { return _megaloTraits; }
			set
			{
				Contract.Requires<ArgumentNullException>(value != null);
				Contract.Requires(value.Count <= 16);

				_megaloTraits = value;
			}
		}

		/// <summary>
		/// Gets or sets the user-defined game variant options.
		/// 
		/// A maximum of 16 user-defined options are available.
		/// </summary>
		public List<UserDefinedOption> UserDefinedOptions
		{
			get { return _userDefinedOptions; }
			set
			{
				Contract.Requires<ArgumentNullException>(value != null);
				_userDefinedOptions = value;
			}
		}

		/// <summary>
		/// Gets or sets the string table containing strings which can be referenced by the script.
		/// </summary>
		public StringTable Strings
		{
			get { return _megaloStrings; }
			set
			{
				Contract.Requires<ArgumentNullException>(value != null);
				_megaloStrings = value;
			}
		}

		/// <summary>
		/// Gets or sets the localized variant name.
		/// </summary>
		public LocalizedString Name
		{
			get { return _localizedName.Count > 0 ? _localizedName[0] : null; }

			set
			{
				if ( _localizedName.Count == 0 )
					_localizedName.Add(value);
				else
					_localizedName[0] = value;
			}
		}

		/// <summary>
		/// Gets or sets the localized variant description.
		/// </summary>
		public LocalizedString Description
		{
			get { return _localizedDescription.Count > 0 ? _localizedDescription[0] : null; }

			set
			{
				if ( _localizedDescription.Count == 0 )
					_localizedDescription.Add(value);
				else
					_localizedDescription[0] = value;
			}
		}

		/// <summary>
		/// Gets or sets the category name.
		/// </summary>
		public LocalizedString CategoryName
		{
			get { return _categoryName.Count > 0 ? _categoryName[0] : null; }

			set
			{
				if ( _categoryName.Count == 0 )
					_categoryName.Add(value);
				else
					_categoryName[0] = value;
			}
		}

		#endregion

		#region ISerializable<BitStream> Members

		void ISerializable<BitStream>.SerializeObject (BitStream s)
		{
			s.SerializeObjects(_megaloTraits, countBitLength: 5);
			s.SerializeObjects(_userDefinedOptions, countBitLength: 5);
			_megaloStrings.Serialize(s, offsetBitLength: 16, lengthBitLength: 16, countBitLength: 8);
			s.Stream(ref _baseGameTypeNameStringIndex);
			_localizedName.Serialize(s, offsetBitLength: 11, lengthBitLength: 11, countBitLength: 1);
			_localizedDescription.Serialize(s, offsetBitLength: 13, lengthBitLength: 13, countBitLength: 1);
			_introDescription.Serialize(s, offsetBitLength: 13, lengthBitLength: 13, countBitLength: 1);
			_categoryName.Serialize(s, offsetBitLength: 10, lengthBitLength: 10, countBitLength: 1);
			s.StreamPlusOne(ref _baseGameTypeIconIndex, 5);
			s.StreamPlusOne(ref _baseGameTypeCategoryIndex, 5);
			s.SerializeObject(_mapPermissions);
			s.SerializeObject(_ratingParameters);
			s.SerializeObject(_globalGameOptions);
			s.SerializeObjects(_mapLoadouts);
			s.Stream(_disabledOptions);
			s.Stream(_hiddenOptions);
			s.Stream(ref _disabledUserDefinedOptions);
			s.Stream(ref _hiddenUserDefinedOptions);
			s.SerializeObjects(_conditions, countBitLength: 10);
			

			/*ushort count = Register<ushort>("Count", n: 11);
			for (var i = 0; i < count; i++)
			{
				Group("Action[" + i + "]", () =>
				{
					byte opcode = Register<byte>("Opcode");
					if (opcode > 0)
					{
						Group("Parameters", () =>
						{
							var action = GameRegistry.Halo4.MegaloActionsDatabase.GetDefinition(opcode);
							Import<DefinitionParameters>(new Context { { "Definition", action } });
						});
					}
				});
			}*/

			/*byte count = Register<byte>("Count");
			for (var i = 0; i < count; i++)
			{
				Group("Trigger[" + i + "]", () =>
				{
					var enumType = Register<byte>("EnumType", n: 3);
					Register<byte>("Type", n: 4);

					switch (enumType)
					{
						case 5:
							bool isFilterIndexNull = Register<bool>("IsFilterIndexNull");
							if (!isFilterIndexNull)
								Register<byte>("FilterIndex", n: 4);
							break;

						case 6:
							Register<bool>("GameObjectType");

							bool isGameObjectFilterIndexNull = Register<bool>("IsGameObjectFilterIndexNull");
							if (!isGameObjectFilterIndexNull)
								Register<byte>("GameObjectFilterIndex", n: 2);
							break;
					}

					Register<uint>("ConditionIndex", n: 10);
					Register<uint>("ConditionCount", n: 10);
					Register<uint>("ActionIndex", n: 11);
					Register<uint>("ActionCount", n: 11);
					Register<byte>("FrameUpdateFrequency");
					Register<byte>("FrameUpdateOffset");
				});
			}*/
			/* 
				{ "Conditions", DefineConditions },
				{ "Actions", DefineActions },
				{ "Triggers", DefineTriggers },
				{ "CustomStats", DefineCustomStats },
				{ "Variables", DefineVariables },
				{ "Widgets", DefineHudWidgets },
				{ "Events", DefineEvents },
				{ "RequiredObjectTypes", DefineRequiredObjectTypes },
				{ "ObjectFilters", DefineObjectFilters },
				{ "CandySpawnerFilters", DefineCandySpawnersFilters },*/
		}

		#endregion
	}
}
